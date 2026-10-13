using System.Diagnostics;
using EPR.Payment.Service.Common.Data.DataModels.Migration;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.Migration;
using EPR.Payment.Service.Common.Logging;
using EPR.Payment.Service.Options;
using Microsoft.Extensions.Options;

namespace EPR.Payment.Service.Services.Migration.RegistrationHistory
{
    public interface IRegistrationHistoryMigrationRunner
    {
        Task RunAsync(CancellationToken cancellationToken);
    }

    /// <summary>
    /// One-off migration of historic Synapse registration submissions into the [registration] schema:
    /// stage the common-data dataset, replay each submission through the live handlers, then
    /// optionally clear down everything it created if any submission failed.
    /// </summary>
    public class RegistrationHistoryMigrationRunner : IRegistrationHistoryMigrationRunner
    {
        private readonly RegistrationHistoryMigrationOptions _options;
        private readonly CommonDataApiOptions _commonDataApiOptions;
        private readonly ICommonDataApiClient _client;
        private readonly IRegistrationHistoryMigrationRepository _repository;
        private readonly IRegistrationHistorySubmissionReplayer _replayer;
        private readonly TimeProvider _timeProvider;
        private readonly ILogger<RegistrationHistoryMigrationRunner> _logger;

        public RegistrationHistoryMigrationRunner(
            IOptions<RegistrationHistoryMigrationOptions> options,
            IOptions<CommonDataApiOptions> commonDataApiOptions,
            ICommonDataApiClient client,
            IRegistrationHistoryMigrationRepository repository,
            IRegistrationHistorySubmissionReplayer replayer,
            TimeProvider timeProvider,
            ILogger<RegistrationHistoryMigrationRunner> logger)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _commonDataApiOptions = commonDataApiOptions?.Value ?? throw new ArgumentNullException(nameof(commonDataApiOptions));
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _replayer = replayer ?? throw new ArgumentNullException(nameof(replayer));
            _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            if (!_options.Enabled)
            {
                _logger.LogInformation("Registration history migration is disabled.");
                return;
            }

            if (string.IsNullOrWhiteSpace(_options.RunKey) || string.IsNullOrWhiteSpace(_commonDataApiOptions.BaseUrl))
            {
                _logger.LogError("Registration history migration is enabled but RunKey or CommonDataApi:BaseUrl is not configured; not running.");
                return;
            }

            using var logScope = _logger.AddScopedData(new Dictionary<string, object> { ["MigrationRunKey"] = _options.RunKey });

            if (!await _repository.TryAcquireRunLockAsync(cancellationToken))
            {
                _logger.LogInformation("Registration history migration {RunKey} is being run by another instance; skipping.", _options.RunKey);
                return;
            }

            try
            {
                await RunLockedAsync(cancellationToken);
            }
            finally
            {
                await ReleaseRunLockAsync();
            }
        }

        private async Task RunLockedAsync(CancellationToken cancellationToken)
        {
            var run = await _repository.GetRunAsync(_options.RunKey, cancellationToken);
            if (run is not null && run.Status != MigrationRunStatus.Running)
            {
                _logger.LogInformation(
                    "Registration history migration SkippedAlreadyRun: RunKey {RunKey} finished with status {Status} at {CompletedAt}.",
                    _options.RunKey,
                    run.Status,
                    run.CompletedAt);
                return;
            }

            var resumed = run is not null;
            run ??= await _repository.CreateRunAsync(_options.RunKey, Environment.MachineName, _timeProvider.GetUtcNow(), cancellationToken);

            using var runScope = _logger.AddScopedData(new Dictionary<string, object>
            {
                ["MigrationRunId"] = run.Id,
                ["MigrationRunResumed"] = resumed,
            });
            _logger.LogInformation(
                "Registration history migration Started: RunKey {RunKey} RunId {RunId} Resumed {Resumed} ClearDownMigratedDataOnError {ClearDown}.",
                _options.RunKey,
                run.Id,
                resumed,
                _options.ClearDownMigratedDataOnError);

            var stopwatch = Stopwatch.StartNew();
            string? runError = null;

            try
            {
                if (run.StagingCompletedAt is null)
                {
                    await StageAsync(run, cancellationToken);
                }

                await ProcessPendingSubmissionsAsync(run, cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                _logger.LogWarning("Registration history migration {RunKey} interrupted by shutdown; it will resume on next startup.", _options.RunKey);
                throw;
            }
            catch (Exception ex)
            {
                runError = ex.ToString();
                _logger.LogError(ex, "Registration history migration {RunKey} failed at run level.", _options.RunKey);
            }

            await FinishAsync(run, runError, stopwatch.Elapsed, cancellationToken);
        }

        private async Task ReleaseRunLockAsync()
        {
            try
            {
                await _repository.ReleaseRunLockAsync(CancellationToken.None);
            }
            catch (Exception ex)
            {
                // The lock is session-scoped, so it is also released when the connection is closed or reset.
                _logger.LogWarning(ex, "Registration history migration could not explicitly release its run lock.");
            }
        }

        private async Task StageAsync(MigrationRun run, CancellationToken cancellationToken)
        {
            using var phaseScope = _logger.AddScopedData(new Dictionary<string, object> { ["MigrationPhase"] = "Staging" });

            var stopwatch = Stopwatch.StartNew();
            await _repository.ResetStagingAsync(run.Id, cancellationToken);

            var batch = new List<RegistrationHistoryStaging>(_options.StagingBatchSize);
            var rowCount = 0;

            await foreach (var record in _client.GetRegistrationHistoryAsync(cancellationToken))
            {
                batch.Add(ToStaging(run.Id, record));
                rowCount++;

                if (batch.Count >= _options.StagingBatchSize)
                {
                    await _repository.AddStagingBatchAsync(batch, cancellationToken);
                    batch = new List<RegistrationHistoryStaging>(_options.StagingBatchSize);
                }
            }

            if (batch.Count > 0)
            {
                await _repository.AddStagingBatchAsync(batch, cancellationToken);
            }

            var submissionCount = await _repository.CreatePendingSubmissionsAsync(run.Id, cancellationToken);

            run.StagedRowCount = rowCount;
            run.SubmissionCount = submissionCount;
            run.StagingCompletedAt = _timeProvider.GetUtcNow();
            await _repository.SaveRunAsync(run, cancellationToken);

            _logger.LogInformation(
                "Registration history migration StagingCompleted: {RowCount} rows for {SubmissionCount} submissions in {ElapsedMs} ms.",
                rowCount,
                submissionCount,
                stopwatch.ElapsedMilliseconds);
        }

        private async Task ProcessPendingSubmissionsAsync(MigrationRun run, CancellationToken cancellationToken)
        {
            using var phaseScope = _logger.AddScopedData(new Dictionary<string, object> { ["MigrationPhase"] = "Replay" });

            var periods = await _repository.GetSubmissionPeriodsAsync(cancellationToken);
            var pending = await _repository.GetPendingSubmissionsAsync(run.Id, cancellationToken);
            var processed = 0;

            foreach (var submission in pending)
            {
                cancellationToken.ThrowIfCancellationRequested();

                using var submissionScope = _logger.AddScopedData(new Dictionary<string, object> { ["SubmissionId"] = submission.SubmissionId });

                var rows = await _repository.GetStagingForSubmissionAsync(run.Id, submission.SubmissionId, cancellationToken);
                var planResult = RegistrationHistorySubmissionPlanner.Plan(rows, periods, _options.SubsidiaryLateFeeChargedFrom);

                var outcome = planResult.Outcome
                    ?? await _replayer.ReplayAsync(run.Id, planResult.Plan!, cancellationToken);

                await RecordOutcomeAsync(submission, outcome, cancellationToken);

                processed++;
                if (processed % Math.Max(1, _options.ProgressLogInterval) == 0 || processed == pending.Count)
                {
                    var counts = await _repository.GetStatusCountsAsync(run.Id, cancellationToken);
                    _logger.LogInformation(
                        "Registration history migration Progress: {Processed}/{Total} processed this session; Succeeded {Succeeded} Failed {Failed} NotRequired {NotRequired} Pending {Pending}.",
                        processed,
                        pending.Count,
                        counts.Succeeded,
                        counts.Failed,
                        counts.NotRequired,
                        counts.Pending);
                }
            }
        }

        private async Task RecordOutcomeAsync(RegistrationSubmissionMigration submission, SubmissionOutcome outcome, CancellationToken cancellationToken)
        {
            submission.Status = outcome.Status;
            submission.Reason = outcome.Reason;
            submission.Error = outcome.Error;
            submission.Warnings = outcome.Warnings is { Count: > 0 } ? string.Join(Environment.NewLine, outcome.Warnings) : null;
            submission.FilesMigrated = outcome.FilesMigrated;
            submission.EventsReplayed = outcome.EventsReplayed;
            submission.EventsSkipped = outcome.EventsSkipped;
            submission.ProcessedAt = _timeProvider.GetUtcNow();

            await _repository.UpdateSubmissionAsync(submission, cancellationToken);

            if (outcome.Status == SubmissionMigrationStatus.Failed)
            {
                _logger.LogWarning(
                    "Registration history migration SubmissionFailed: SubmissionId {SubmissionId} Reason {Reason} Error {Error}.",
                    submission.SubmissionId,
                    outcome.Reason,
                    outcome.Error);
            }
        }

        private async Task FinishAsync(MigrationRun run, string? runError, TimeSpan elapsed, CancellationToken cancellationToken)
        {
            using var phaseScope = _logger.AddScopedData(new Dictionary<string, object> { ["MigrationPhase"] = "Finish" });

            var counts = await _repository.GetStatusCountsAsync(run.Id, cancellationToken);
            var hasErrors = runError is not null || counts.Failed > 0;

            run.SucceededCount = counts.Succeeded;
            run.FailedCount = counts.Failed;
            run.NotRequiredCount = counts.NotRequired;
            run.ErrorMessage = runError;

            if (hasErrors && _options.ClearDownMigratedDataOnError)
            {
                _logger.LogWarning(
                    "Registration history migration ClearDownStarted: {Failed} failed submissions, run-level error {HasRunError}.",
                    counts.Failed,
                    runError is not null);

                var result = await _repository.ClearDownAsync(run.Id, cancellationToken);

                _logger.LogWarning(
                    "Registration history migration ClearDownCompleted: deleted {Deleted} RegistrationSubmissionData rows; unlinked payments {UnlinkedPaymentIds}.",
                    result.DeletedRegistrationSubmissionDataCount,
                    result.UnlinkedPaymentIds);

                run.ClearDownApplied = true;
                run.Status = MigrationRunStatus.ClearedDown;
            }
            else if (runError is not null)
            {
                run.Status = MigrationRunStatus.Failed;
            }
            else
            {
                run.Status = counts.Failed > 0 ? MigrationRunStatus.CompletedWithErrors : MigrationRunStatus.Completed;
            }

            run.CompletedAt = _timeProvider.GetUtcNow();
            await _repository.SaveRunAsync(run, cancellationToken);

            _logger.LogInformation(
                "Registration history migration Completed: Status {Status} Submissions {Total} Succeeded {Succeeded} Failed {Failed} NotRequired {NotRequired} Pending {Pending} Duration {Duration}.",
                run.Status,
                counts.Total,
                counts.Succeeded,
                counts.Failed,
                counts.NotRequired,
                counts.Pending,
                elapsed.ToString("g"));
        }

        private static RegistrationHistoryStaging ToStaging(Guid runId, RegistrationHistoryRecord record) => new()
        {
            RunId = runId,
            SubmissionId = record.SubmissionId,
            FileId = record.FileId,
            BlobName = record.BlobName,
            OrganisationId = record.OrganisationId,
            ComplianceSchemeId = record.ComplianceSchemeId,
            SubmissionPeriod = record.SubmissionPeriod,
            RegistrationJourney = record.RegistrationJourney,
            RegulatorNation = record.RegulatorNation,
            Roles = record.Roles,
            SubmissionFileCount = record.SubmissionFileCount,
            EventType = record.EventType,
            EventTypeOrder = record.EventTypeOrder,
            ReplayTs = record.ReplayTs,
            EventDate = record.EventDate,
            ApplicationReferenceNumber = record.ApplicationReferenceNumber,
            Decision = record.Decision,
        };
    }
}
