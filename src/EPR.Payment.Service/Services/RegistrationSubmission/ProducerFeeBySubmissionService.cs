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

            var snapshotRecords = await _repository.GetAllForSubmissionAsync(submissionId, cancellationToken);
            if (snapshotRecords.Count == 0)
            {
                _logger.LogInformation(
                    "Skipping producer fee calculation for SubmissionId {SubmissionId}: snapshot is empty.",
                    submissionId);
                return null;
            }

            var nowUtc = _timeProvider.GetUtcNow().UtcDateTime;
            var submissionLifecycle = SubmissionLifecycleAnalyser.Analyse(snapshotRecords, nowUtc);

            if (submissionLifecycle.LatestNonRejected is null)
            {
                _logger.LogInformation(
                    "Skipping producer fee calculation for SubmissionId {SubmissionId}: every snapshot row was rejected by the regulator.",
                    submissionId);
                return null;
            }

            var latestRecord = submissionLifecycle.LatestNonRejected;

            if (latestRecord.Producers.Count == 0)
            {
                _logger.LogInformation(
                    "Latest RegistrationSubmissionData for SubmissionId {SubmissionId} has no producer rows.",
                    submissionId);
                return null;
            }

            if (latestRecord.Producers.Count > 1)
            {
                _logger.LogWarning(
                    "Direct-producer submission {SubmissionId} unexpectedly has {ProducerCount} producer rows; using the first.",
                    submissionId,
                    latestRecord.Producers.Count);
            }

            var producer = latestRecord.Producers.First();
            var submissionDeadline = latestRecord.SubmissionPeriodWindow.DeadlineDate;

            var latestSubmittedOnOrAfterDeadline =
                (submissionLifecycle.LatestSubmittedForApprovalDate ?? nowUtc).Date >= submissionDeadline.Date;
            var firstSubmissionWasLate = submissionLifecycle.FirstSubmittedForApprovalDate is DateTime firstApproval
                                          && firstApproval.Date >= submissionDeadline.Date;
            var neverYetSubmittedForApproval = submissionLifecycle.FirstSubmittedForApprovalDate is null;

            var request = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = producer.OrganisationSize,
                NumberOfSubsidiaries = producer.Subsidiaries.Count,
                NoOfSubsidiariesOnlineMarketplace = producer.Subsidiaries.Count(s => s.IsOnlineMarketplace),
                NoOfSubsidiariesClosedLoopRecycling = producer.Subsidiaries.Count(s => s.IsClosedLoopRecycling),
                IsProducerOnlineMarketplace = producer.IsOnlineMarketplace,
                IsClosedLoopRecycling = producer.IsClosedLoopRecycling,
                IsLateFeeApplicable = firstSubmissionWasLate || (neverYetSubmittedForApproval && latestSubmittedOnOrAfterDeadline),
                Regulator = latestRecord.RegulatorNation,
                ApplicationReferenceNumber = latestRecord.ApplicationReferenceNumber,
                SubmissionDate = submissionLifecycle.CalcDate,
            };

            _logger.LogInformation(
                "Calculating producer fee for SubmissionId {SubmissionId}; calcDate={CalcDate}, latestSubmittedOnOrAfterDeadline={LatestLate}, firstSubmissionWasLate={FirstLate}.",
                submissionId,
                submissionLifecycle.CalcDate,
                latestSubmittedOnOrAfterDeadline,
                firstSubmissionWasLate);

            var response = await _calculatorService.CalculateFeesAsync(request, cancellationToken);
            if (response is not null)
            {
                response.RegistrationBlobName = latestRecord.RegistrationBlobName;
            }

            return response;
        }
    }
}
