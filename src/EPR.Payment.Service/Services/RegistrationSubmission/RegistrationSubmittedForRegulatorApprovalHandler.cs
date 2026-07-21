using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationSubmission;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationSubmission;
using EPR.Payment.Service.Common.Services.Interfaces.RegistrationSubmission;
using Microsoft.Extensions.Logging;

namespace EPR.Payment.Service.Services.RegistrationSubmission
{
    public class RegistrationSubmittedForRegulatorApprovalHandler : IRegistrationSubmittedForRegulatorApprovalHandler
    {
        public const string EventName = "SubmittedForRegulatorApproval";

        private readonly IRegistrationSubmissionDataEventRepository _repository;
        private readonly ILogger<RegistrationSubmittedForRegulatorApprovalHandler> _logger;

        public RegistrationSubmittedForRegulatorApprovalHandler(
            IRegistrationSubmissionDataEventRepository repository,
            ILogger<RegistrationSubmittedForRegulatorApprovalHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
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
        }
    }
}
