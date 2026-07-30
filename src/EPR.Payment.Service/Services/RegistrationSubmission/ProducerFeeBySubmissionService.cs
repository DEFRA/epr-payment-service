using EPR.Payment.Service.Common.Data.DataModels;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationSubmission;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.Producer;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.Producer;
using EPR.Payment.Service.Services.Interfaces.RegistrationSubmission;
using Microsoft.Extensions.Logging;

namespace EPR.Payment.Service.Services.RegistrationSubmission
{
    public class ProducerFeeBySubmissionService : IProducerFeeBySubmissionService
    {
        private readonly IRegistrationSubmissionDataRepository _repository;
        private readonly IProducerFeesCalculatorService _calculatorService;
        private readonly TimeProvider _timeProvider;
        private readonly ILogger<ProducerFeeBySubmissionService> _logger;

        public ProducerFeeBySubmissionService(
            IRegistrationSubmissionDataRepository repository,
            IProducerFeesCalculatorService calculatorService,
            TimeProvider timeProvider,
            ILogger<ProducerFeeBySubmissionService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _calculatorService = calculatorService ?? throw new ArgumentNullException(nameof(calculatorService));
            _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<RegistrationFeesResponseDto?> GetFeesAsync(Guid submissionId, CancellationToken cancellationToken)
        {
            using var logScope = _logger.BeginScope(new Dictionary<string, object>
            {
                ["SubmissionId"] = submissionId,
            });

            var records = await _repository.GetAllForSubmissionAsync(submissionId, cancellationToken);
            if (records.Count == 0)
            {
                _logger.LogInformation("No RegistrationSubmissionData found for SubmissionId {SubmissionId}.", submissionId);
                return null;
            }

            var today = _timeProvider.GetUtcNow().UtcDateTime;
            var lifecycle = SubmissionLifecycleAnalyser.Analyse(records, today);

            if (lifecycle.LatestNonRejected is null)
            {
                _logger.LogInformation(
                    "All RegistrationSubmissionData rows for SubmissionId {SubmissionId} have been rejected.",
                    submissionId);
                return null;
            }

            var latest = lifecycle.LatestNonRejected;

            if (latest.Producers.Count == 0)
            {
                _logger.LogInformation(
                    "Latest RegistrationSubmissionData for SubmissionId {SubmissionId} has no producer rows.",
                    submissionId);
                return null;
            }

            if (latest.Producers.Count > 1)
            {
                _logger.LogWarning(
                    "Direct-producer submission {SubmissionId} unexpectedly has {ProducerCount} producer rows; using the first.",
                    submissionId,
                    latest.Producers.Count);
            }

            var producer = latest.Producers.First();
            var deadline = latest.SubmissionPeriodWindow.DeadlineDate;

            var submissionLevelLate = IsOnOrAfterDeadline(lifecycle.LatestSubmittedForApprovalDate, deadline, today);
            var isOriginalLate = lifecycle.FirstSubmittedForApprovalDate is DateTime firstApproval
                                 && firstApproval.Date >= deadline.Date;
            var noFirstSubmission = lifecycle.FirstSubmittedForApprovalDate is null;

            var request = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = producer.OrganisationSize,
                NumberOfSubsidiaries = producer.Subsidiaries.Count,
                NoOfSubsidiariesOnlineMarketplace = producer.Subsidiaries.Count(s => s.IsOnlineMarketplace),
                NoOfSubsidiariesClosedLoopRecycling = producer.Subsidiaries.Count(s => s.IsClosedLoopRecycling),
                IsProducerOnlineMarketplace = producer.IsOnlineMarketplace,
                IsClosedLoopRecycling = producer.IsClosedLoopRecycling,
                IsLateFeeApplicable = isOriginalLate || (noFirstSubmission && submissionLevelLate),
                Regulator = latest.RegulatorNation,
                ApplicationReferenceNumber = latest.ApplicationReferenceNumber,
                SubmissionDate = lifecycle.CalcDate,
            };

            _logger.LogInformation(
                "Calculating producer fees for SubmissionId {SubmissionId} (calcDate={CalcDate}, submissionLevelLate={SubmissionLevelLate}, isOriginalLate={IsOriginalLate}).",
                submissionId,
                lifecycle.CalcDate,
                submissionLevelLate,
                isOriginalLate);

            return await _calculatorService.CalculateFeesAsync(request, cancellationToken);
        }

        private static bool IsOnOrAfterDeadline(DateTime? candidate, DateTime deadline, DateTime today) =>
            (candidate ?? today).Date >= deadline.Date;
    }
}
