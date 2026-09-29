using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationSubmission;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Services.Interfaces.Payments;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Services.Interfaces.RegistrationSubmission;
using Microsoft.Extensions.Logging;

namespace EPR.Payment.Service.Services.RegistrationSubmission
{
    public class ComplianceSchemeFeeBySubmissionService : IComplianceSchemeFeeBySubmissionService
    {
        private readonly IRegistrationSubmissionDataRepository _repository;
        private readonly IRegistrationFeeSnapshotRepository _snapshotRepository;
        private readonly IComplianceSchemeCalculatorService _calculatorService;
        private readonly IPaymentsService _paymentsService;
        private readonly TimeProvider _timeProvider;
        private readonly ILogger<ComplianceSchemeFeeBySubmissionService> _logger;

        public ComplianceSchemeFeeBySubmissionService(
            IRegistrationSubmissionDataRepository repository,
            IRegistrationFeeSnapshotRepository snapshotRepository,
            IComplianceSchemeCalculatorService calculatorService,
            IPaymentsService paymentsService,
            TimeProvider timeProvider,
            ILogger<ComplianceSchemeFeeBySubmissionService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _snapshotRepository = snapshotRepository ?? throw new ArgumentNullException(nameof(snapshotRepository));
            _calculatorService = calculatorService ?? throw new ArgumentNullException(nameof(calculatorService));
            _paymentsService = paymentsService ?? throw new ArgumentNullException(nameof(paymentsService));
            _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ComplianceSchemeFeesResponseDto?> GetFeesAsync(Guid submissionId, bool requireSubmittedForApproval, CancellationToken cancellationToken)
        {
            using var logScope = _logger.BeginScope(new Dictionary<string, object>
            {
                ["SubmissionId"] = submissionId,
                ["RequireSubmittedForApproval"] = requireSubmittedForApproval,
            });

            var records = await _repository.GetAllForSubmissionAsync(submissionId, cancellationToken);
            if (records.Count == 0)
            {
                _logger.LogInformation("No RegistrationSubmissionData found for SubmissionId {SubmissionId}.", submissionId);
                return null;
            }

            var today = _timeProvider.GetUtcNow().UtcDateTime;
            var lifecycle = SubmissionLifecycleAnalyser.Analyse(records, today, requireSubmittedForApproval);

            if (lifecycle.LatestNonRejected is null)
            {
                _logger.LogInformation(
                    "No eligible RegistrationSubmissionData for SubmissionId {SubmissionId} (requireSubmittedForApproval={RequireSubmittedForApproval}).",
                    submissionId,
                    requireSubmittedForApproval);
                return null;
            }

            var latest = lifecycle.LatestNonRejected;

            var snapshot = await _snapshotRepository.GetByRegistrationSubmissionDataIdAsync(latest.Id, cancellationToken);
            if (snapshot is not null)
            {
                var snapshotResponse = RegistrationFeeSnapshotProjector.ToComplianceSchemeResponse(snapshot, latest.Producers);
                snapshotResponse.PreviousPayment = await _paymentsService.GetPreviousPaymentsByReferenceAsync(latest.ApplicationReferenceNumber, cancellationToken);
                snapshotResponse.OutstandingPayment = snapshotResponse.TotalFee - snapshotResponse.PreviousPayment;
                snapshotResponse.RegistrationBlobName = latest.RegistrationBlobName;
                return snapshotResponse;
            }

            var newlyAddedSubs = NewlyAddedSubsidiaryFinder.Find(
                latest,
                records,
                latest.SubmissionPeriodWindow.DeadlineDate);
            var request = RegistrationFeeRequestBuilder.BuildComplianceSchemeRequest(latest, lifecycle, today, newlyAddedSubs);

            _logger.LogInformation(
                "Calculating compliance-scheme fees for SubmissionId {SubmissionId} (calcDate={CalcDate}, memberCount={MemberCount}).",
                submissionId,
                lifecycle.CalcDate,
                request.ComplianceSchemeMembers.Count);

            var response = await _calculatorService.CalculateFeesAsync(request, cancellationToken);
            if (response is not null)
            {
                response.RegistrationBlobName = latest.RegistrationBlobName;
                EnrichMemberMetadata(response, latest.Producers);
            }

            return response;
        }

        private static void EnrichMemberMetadata(
            ComplianceSchemeFeesResponseDto response,
            ICollection<Common.Data.DataModels.RegistrationSubmissionProducer> producers)
        {
            if (response.ComplianceSchemeMembersWithFees.Count == 0 || producers.Count == 0)
            {
                return;
            }

            var producersByMemberId = producers
                .GroupBy(p => p.OrganisationId, StringComparer.Ordinal)
                .ToDictionary(g => g.Key, g => g.First(), StringComparer.Ordinal);

            foreach (var member in response.ComplianceSchemeMembersWithFees)
            {
                if (producersByMemberId.TryGetValue(member.MemberId, out var producer))
                {
                    member.MemberType = producer.OrganisationSize;
                    member.NumberOfSubsidiaries = producer.Subsidiaries.Count;
                }
            }
        }
    }
}
