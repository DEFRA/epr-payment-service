using Azure;
using CsvHelper;
using EPR.Payment.Service.Common.Data.DataModels.Migration;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.Migration;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationSubmission;

namespace EPR.Payment.Service.Services.Migration.RegistrationHistory
{
    public interface IRegistrationHistorySubmissionReplayer
    {
        Task<SubmissionOutcome> ReplayAsync(Guid runId, SubmissionReplayPlan plan, CancellationToken cancellationToken);
    }

    /// <summary>
    /// Replays one submission through the live handlers in its own DI scope and DB transaction, so a
    /// failure leaves nothing behind for that submission.
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
            var eventsReplayed = 0;

            try
            {
                await using var transaction = await repository.BeginTransactionAsync(cancellationToken);

                foreach (var step in plan.Steps)
                {
                    clock.SetUtcNow(step.ReplayTs);

                    switch (step)
                    {
                        case SubmittedStep submitted:
                            var dataId = await handlers.SubmissionData.HandleAsync(submitted.Request, cancellationToken);
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

                var warnings = plan.Warnings.Concat(await VerifyEventPairingAsync(provider, plan, dataIdsByFile, cancellationToken)).ToList();

                await transaction.CommitAsync(cancellationToken);

                return new SubmissionOutcome(
                    SubmissionMigrationStatus.Succeeded,
                    Warnings: warnings,
                    FilesMigrated: dataIdsByFile.Count,
                    EventsReplayed: eventsReplayed,
                    EventsSkipped: plan.EventsSkipped);
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
            InvalidOperationException when ex.Message.StartsWith("No producer row", StringComparison.Ordinal) => MigrationReasons.CsvInvalid,
            _ => MigrationReasons.HandlerError,
        };
    }
}
