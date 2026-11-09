using Azure;
using CsvHelper;
using EPR.Payment.Service.Common.Data.DataModels.Migration;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.Migration;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationSubmission;
using EPR.Payment.Service.Common.Logging;

namespace EPR.Payment.Service.Services.Migration.RegistrationHistory
{
    public interface IRegistrationHistorySubmissionReplayer
    {
        Task<SubmissionOutcome> ReplayAsync(Guid runId, SubmissionReplayPlan plan, CancellationToken cancellationToken);
    }

    /// <summary>
    /// Replays one submission through the live handlers in its own DI scope and DB transaction, so a
    /// failure leaves nothing behind for that submission. A file whose CSV has subsidiary rows without a
    /// producer row is skipped with its events and the rest of the submission is migrated; the submission
    /// fails only when no file can be migrated.
    /// </summary>
    public class RegistrationHistorySubmissionReplayer : IRegistrationHistorySubmissionReplayer
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IRegistrationReplayHandlerFactory _handlerFactory;
        private readonly TimeProvider _timeProvider;
        private readonly ILogger<RegistrationHistorySubmissionReplayer> _logger;

        public RegistrationHistorySubmissionReplayer(
            IServiceScopeFactory scopeFactory,
            IRegistrationReplayHandlerFactory handlerFactory,
            TimeProvider timeProvider,
            ILogger<RegistrationHistorySubmissionReplayer> logger)
        {
            _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
            _handlerFactory = handlerFactory ?? throw new ArgumentNullException(nameof(handlerFactory));
            _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<SubmissionOutcome> ReplayAsync(Guid runId, SubmissionReplayPlan plan, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(plan);

            await using var scope = _scopeFactory.CreateAsyncScope();
            var provider = scope.ServiceProvider;
            var repository = provider.GetRequiredService<IRegistrationHistoryMigrationRepository>();

            if (await repository.SubmissionExistsAsync(plan.SubmissionId, cancellationToken))
            {
                return new SubmissionOutcome(SubmissionMigrationStatus.NotRequired, MigrationReasons.AlreadyCaptured);
            }

            var clock = new ReplayTimeProvider();
            var handlers = _handlerFactory.Create(provider, clock);
            var dataIdsByFile = new Dictionary<string, Guid>(StringComparer.OrdinalIgnoreCase);
            var skippedFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var skippedFileWarnings = new List<string>();
            var eventsReplayed = 0;
            var eventsSkipped = plan.EventsSkipped;

            try
            {
                await using var transaction = await repository.BeginTransactionAsync(cancellationToken);

                foreach (var step in plan.Steps)
                {
                    clock.SetUtcNow(step.ReplayTs);

                    using var stepScope = _logger.AddScopedData(new Dictionary<string, object>
                    {
                        ["MigrationFileId"] = step.FileId,
                        ["MigrationReplayStep"] = step.GetType().Name,
                        ["MigrationReplayTs"] = step.ReplayTs,
                    });

                    // The live handlers attach events to the latest declared file, so a skipped file's events are skipped too.
                    if (step is not SubmittedStep && skippedFiles.Contains(step.FileId))
                    {
                        eventsSkipped++;
                        continue;
                    }

                    switch (step)
                    {
                        case SubmittedStep submitted:
                            Guid dataId;
                            try
                            {
                                dataId = await handlers.SubmissionData.HandleAsync(submitted.Request, cancellationToken);
                            }
                            catch (InvalidOperationException ex) when (IsMissingProducerRow(ex))
                            {
                                // Thrown while the CSV is mapped, before anything is added, so the transaction carries on.
                                _logger.LogWarning(
                                    "Registration history migration skipped file {FileId} of SubmissionId {SubmissionId}: {Error}",
                                    submitted.FileId,
                                    plan.SubmissionId,
                                    ex.Message);
                                skippedFiles.Add(submitted.FileId);
                                skippedFileWarnings.Add($"{MigrationReasons.CsvInvalid}: file {submitted.FileId} skipped - {ex.Message}");
                                break;
                            }

                            dataIdsByFile[submitted.FileId] = dataId;
                            await repository.AddMigratedEntityAsync(new MigratedEntity
                            {
                                RunId = runId,
                                SubmissionId = plan.SubmissionId,
                                FileId = submitted.FileId,
                                RegistrationSubmissionDataId = dataId,
                                CreatedAt = _timeProvider.GetUtcNow(),
                            }, cancellationToken);
                            break;
                        case ApplicationSubmittedStep applicationSubmitted:
                            await handlers.ApplicationSubmitted.HandleAsync(applicationSubmitted.Request, cancellationToken);
                            eventsReplayed++;
                            break;
                        case DecisionStep decision:
                            await handlers.Decision.HandleAsync(decision.Request, cancellationToken);
                            eventsReplayed++;
                            break;
                    }
                }

                if (dataIdsByFile.Count == 0)
                {
                    // Nothing migratable; the transaction is disposed without committing.
                    return new SubmissionOutcome(
                        SubmissionMigrationStatus.Failed,
                        MigrationReasons.CsvInvalid,
                        string.Join(" ", skippedFileWarnings),
                        plan.Warnings,
                        EventsSkipped: eventsSkipped);
                }

                var warnings = plan.Warnings
                    .Concat(skippedFileWarnings)
                    .Concat(await VerifyEventPairingAsync(provider, plan, dataIdsByFile, cancellationToken))
                    .ToList();

                await transaction.CommitAsync(cancellationToken);

                return new SubmissionOutcome(
                    SubmissionMigrationStatus.Succeeded,
                    Warnings: warnings,
                    FilesMigrated: dataIdsByFile.Count,
                    EventsReplayed: eventsReplayed,
                    EventsSkipped: eventsSkipped);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                var reason = Classify(ex);
                _logger.LogError(
                    ex,
                    "Registration history migration failed for SubmissionId {SubmissionId} with reason {Reason}; its changes were rolled back.",
                    plan.SubmissionId,
                    reason);

                return new SubmissionOutcome(SubmissionMigrationStatus.Failed, reason, ex.Message, plan.Warnings, EventsSkipped: plan.EventsSkipped);
            }
        }

        private async Task<IReadOnlyList<string>> VerifyEventPairingAsync(
            IServiceProvider provider,
            SubmissionReplayPlan plan,
            IReadOnlyDictionary<string, Guid> dataIdsByFile,
            CancellationToken cancellationToken)
        {
            var records = await provider.GetRequiredService<IRegistrationSubmissionDataRepository>()
                .GetAllForSubmissionAsync(plan.SubmissionId, cancellationToken);

            var mismatches = new List<string>();
            foreach (var (fileId, dataId) in dataIdsByFile)
            {
                var expected = plan.Steps.Count(s => s is not SubmittedStep && string.Equals(s.FileId, fileId, StringComparison.OrdinalIgnoreCase));
                var actual = records.FirstOrDefault(r => r.Id == dataId)?.Events.Count ?? 0;

                if (expected != actual)
                {
                    _logger.LogWarning(
                        "Registration history PairingMismatch for SubmissionId {SubmissionId} file {FileId}: expected {Expected} events, recorded {Actual}.",
                        plan.SubmissionId,
                        fileId,
                        expected,
                        actual);
                    mismatches.Add($"PairingMismatch: file {fileId} expected {expected} events, recorded {actual}.");
                }
            }

            return mismatches;
        }

        private static string Classify(Exception ex) => ex switch
        {
            RequestFailedException { Status: 404 } => MigrationReasons.BlobNotFound,
            CsvHelperException => MigrationReasons.CsvInvalid,
            _ when IsMissingProducerRow(ex) => MigrationReasons.CsvInvalid,
            _ => MigrationReasons.HandlerError,
        };

        // RegistrationSubmissionDataHandler's error for an OrganisationId with subsidiary rows but no producer row.
        private static bool IsMissingProducerRow(Exception ex) =>
            ex is InvalidOperationException && ex.Message.StartsWith("No producer row", StringComparison.Ordinal);
    }
}
