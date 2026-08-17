using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationSubmission;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.Producer;
using EPR.Payment.Service.Services.Interfaces.Payments;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.Producer;
using EPR.Payment.Service.Services.Interfaces.RegistrationSubmission;
using Microsoft.Extensions.Logging;

namespace EPR.Payment.Service.Services.RegistrationSubmission
{
    public class ProducerFeeBySubmissionService : IProducerFeeBySubmissionService
    {
        private readonly IRegistrationSubmissionDataRepository _repository;
        private readonly IRegistrationFeeSnapshotRepository _snapshotRepository;
        private readonly IProducerFeesCalculatorService _calculatorService;
        private readonly IPaymentsService _paymentsService;
        private readonly TimeProvider _timeProvider;
        private readonly ILogger<ProducerFeeBySubmissionService> _logger;

        public ProducerFeeBySubmissionService(
            IRegistrationSubmissionDataRepository repository,
            IRegistrationFeeSnapshotRepository snapshotRepository,
            IProducerFeesCalculatorService calculatorService,
            IPaymentsService paymentsService,
            TimeProvider timeProvider,
            ILogger<ProducerFeeBySubmissionService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _snapshotRepository = snapshotRepository ?? throw new ArgumentNullException(nameof(snapshotRepository));
            _calculatorService = calculatorService ?? throw new ArgumentNullException(nameof(calculatorService));
            _paymentsService = paymentsService ?? throw new ArgumentNullException(nameof(paymentsService));
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

            var snapshot = await _snapshotRepository.GetByRegistrationSubmissionDataIdAsync(latestRecord.Id, cancellationToken);
            if (snapshot is not null)
            {
                var snapshotResponse = RegistrationFeeSnapshotProjector.ToProducerResponse(snapshot);
                snapshotResponse.PreviousPayment = await _paymentsService.GetPreviousPaymentsByReferenceAsync(latestRecord.ApplicationReferenceNumber, cancellationToken);
                snapshotResponse.OutstandingPayment = snapshotResponse.TotalFee - snapshotResponse.PreviousPayment;
                snapshotResponse.RegistrationBlobName = latestRecord.RegistrationBlobName;
                return snapshotResponse;
            }

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
            var request = RegistrationFeeRequestBuilder.BuildProducerRequest(latestRecord, producer, submissionLifecycle, nowUtc);

            _logger.LogInformation(
                "Calculating producer fee for SubmissionId {SubmissionId}; calcDate={CalcDate}.",
                submissionId,
                submissionLifecycle.CalcDate);

            var response = await _calculatorService.CalculateFeesAsync(request, cancellationToken);
            if (response is not null)
            {
                response.RegistrationBlobName = latestRecord.RegistrationBlobName;
            }

            return response;
        }
    }
}
