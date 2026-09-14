using EPR.Payment.Service.Common.Constants;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationSubmission;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationSubmission;
using EPR.Payment.Service.Common.Services.Interfaces.RegistrationSubmission;
using EPR.Payment.Service.Services.Interfaces.RegistrationSubmission;
using Microsoft.Extensions.Logging;

namespace EPR.Payment.Service.Services.RegistrationSubmission
{
    public class RegistrationSubmittedForRegulatorApprovalHandler : IRegistrationSubmittedForRegulatorApprovalHandler
    {
        public const string EventName = RegistrationEventNames.SubmittedForRegulatorApproval;

        private readonly IRegistrationSubmissionDataEventRepository _repository;
        private readonly IRegistrationSubmissionDataRepository _registrationSubmissionDataRepository;
        private readonly IRegistrationFeeSnapshotHandler _feeSnapshotHandler;
        private readonly TimeProvider _timeProvider;
        private readonly ILogger<RegistrationSubmittedForRegulatorApprovalHandler> _logger;

        public RegistrationSubmittedForRegulatorApprovalHandler(
            IRegistrationSubmissionDataEventRepository repository,
            IRegistrationSubmissionDataRepository registrationSubmissionDataRepository,
            IRegistrationFeeSnapshotHandler feeSnapshotHandler,
            TimeProvider timeProvider,
            ILogger<RegistrationSubmittedForRegulatorApprovalHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _registrationSubmissionDataRepository = registrationSubmissionDataRepository ?? throw new ArgumentNullException(nameof(registrationSubmissionDataRepository));
            _feeSnapshotHandler = feeSnapshotHandler ?? throw new ArgumentNullException(nameof(feeSnapshotHandler));
            _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task HandleAsync(RegistrationSubmittedForRegulatorApprovalRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            using var logScope = _logger.BeginScope(new Dictionary<string, object>
            {
                ["SubmissionId"] = request.SubmissionId,
                ["ApplicationReferenceNumber"] = request.ApplicationReferenceNumber,
                ["SubmissionDate"] = request.SubmissionDate,
            });

            var newId = await _repository.AddEventForLatestSubmissionAsync(
                request.SubmissionId,
                EventName,
                request.SubmissionDate,
                cancellationToken);

            if (newId is null)
            {
                _logger.LogWarning(
                    "No RegistrationSubmissionData row found for SubmissionId {SubmissionId}; abandoning message so Service Bus retries once the upstream fees-calculation message has been processed.",
                    request.SubmissionId);

                throw new SubmissionDataNotFoundForEventException(request.SubmissionId, EventName);
            }

            _logger.LogInformation(
                "Recorded {EventName} event {EventId} against RegistrationSubmissionData for SubmissionId {SubmissionId}.",
                EventName,
                newId,
                request.SubmissionId);

            var allRecords = await _registrationSubmissionDataRepository.GetAllForSubmissionAsync(request.SubmissionId, cancellationToken);
            if (allRecords.Count == 0)
            {
                _logger.LogWarning(
                    "Snapshot creation skipped: no RegistrationSubmissionData rows for SubmissionId {SubmissionId} after event recorded.",
                    request.SubmissionId);
                return;
            }

            var lifecycle = SubmissionLifecycleAnalyser.Analyse(allRecords, _timeProvider.GetUtcNow().UtcDateTime);
            if (lifecycle.LatestNonRejected is null)
            {
                _logger.LogWarning(
                    "Snapshot creation skipped: every RegistrationSubmissionData row for SubmissionId {SubmissionId} was rejected by the regulator.",
                    request.SubmissionId);
                return;
            }

            await _feeSnapshotHandler.HandleAsync(lifecycle.LatestNonRejected, request.SubmissionDate, lifecycle, cancellationToken);
        }
    }
}
