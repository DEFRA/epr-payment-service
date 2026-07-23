using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationSubmission;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationSubmission;
using EPR.Payment.Service.Common.Services.Interfaces.RegistrationSubmission;
using Microsoft.Extensions.Logging;

namespace EPR.Payment.Service.Services.RegistrationSubmission
{
    public class RegulatorRegistrationDecisionHandler : IRegulatorRegistrationDecisionHandler
    {
        private readonly IRegistrationSubmissionDataEventRepository _repository;
        private readonly ILogger<RegulatorRegistrationDecisionHandler> _logger;

        public RegulatorRegistrationDecisionHandler(
            IRegistrationSubmissionDataEventRepository repository,
            ILogger<RegulatorRegistrationDecisionHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task HandleAsync(RegulatorRegistrationDecisionRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            using var logScope = _logger.BeginScope(new Dictionary<string, object>
            {
                ["SubmissionId"] = request.SubmissionId,
                ["EventName"] = request.EventName,
                ["DecisionDate"] = request.DecisionDate,
            });

            var newId = await _repository.AddEventForLatestSubmissionAsync(
                request.SubmissionId,
                request.EventName,
                request.DecisionDate,
                cancellationToken);

            if (newId is null)
            {
                _logger.LogWarning(
                    "No RegistrationSubmissionData row found for SubmissionId {SubmissionId} when recording {EventName}; abandoning message so Service Bus retries once the upstream fees-calculation message has been processed.",
                    request.SubmissionId,
                    request.EventName);

                throw new SubmissionDataNotFoundForEventException(request.SubmissionId, request.EventName);
            }

            _logger.LogInformation(
                "Recorded {EventName} event {EventId} against RegistrationSubmissionData for SubmissionId {SubmissionId}.",
                request.EventName,
                newId,
                request.SubmissionId);
        }
    }
}
