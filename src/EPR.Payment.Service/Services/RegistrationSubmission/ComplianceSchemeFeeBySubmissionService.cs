using EPR.Payment.Service.Common.Constants;
using EPR.Payment.Service.Common.Data.DataModels;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationSubmission;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Services.Interfaces.RegistrationSubmission;
using Microsoft.Extensions.Logging;

namespace EPR.Payment.Service.Services.RegistrationSubmission
{
    public class ComplianceSchemeFeeBySubmissionService : IComplianceSchemeFeeBySubmissionService
    {
        private const string CsoSmallProducerWindowType = "CsoSmallProducer";

        private readonly IRegistrationSubmissionDataRepository _repository;
        private readonly IComplianceSchemeCalculatorService _calculatorService;
        private readonly TimeProvider _timeProvider;
        private readonly ILogger<ComplianceSchemeFeeBySubmissionService> _logger;

        public ComplianceSchemeFeeBySubmissionService(
            IRegistrationSubmissionDataRepository repository,
            IComplianceSchemeCalculatorService calculatorService,
            TimeProvider timeProvider,
            ILogger<ComplianceSchemeFeeBySubmissionService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _calculatorService = calculatorService ?? throw new ArgumentNullException(nameof(calculatorService));
            _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ComplianceSchemeFeesResponseDto?> GetFeesAsync(Guid submissionId, CancellationToken cancellationToken)
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
            var deadline = latest.SubmissionPeriodWindow.DeadlineDate;

            var submissionLevelLate = IsOnOrAfterDeadline(lifecycle.LatestSubmittedForApprovalDate, deadline, today);
            var isOriginalCsoLate = lifecycle.FirstSubmittedForApprovalDate is DateTime firstApproval
                                    && firstApproval.Date >= deadline.Date;
            var noFirstSubmission = lifecycle.FirstSubmittedForApprovalDate is null;

            var request = new ComplianceSchemeFeesRequestDto
            {
                Regulator = latest.RegulatorNation,
                ApplicationReferenceNumber = latest.ApplicationReferenceNumber,
                SubmissionDate = lifecycle.CalcDate,
                IncludeRegistrationFee = !string.Equals(
                    latest.SubmissionPeriodWindow.WindowType,
                    CsoSmallProducerWindowType,
                    StringComparison.OrdinalIgnoreCase),
                ComplianceSchemeMembers = latest.Producers
                    .Select(p => MapMember(p, isOriginalCsoLate, noFirstSubmission, submissionLevelLate))
                    .ToList(),
            };

            _logger.LogInformation(
                "Calculating compliance-scheme fees for SubmissionId {SubmissionId} (calcDate={CalcDate}, submissionLevelLate={SubmissionLevelLate}, isOriginalCsoLate={IsOriginalCsoLate}, memberCount={MemberCount}).",
                submissionId,
                lifecycle.CalcDate,
                submissionLevelLate,
                isOriginalCsoLate,
                request.ComplianceSchemeMembers.Count);

            return await _calculatorService.CalculateFeesAsync(request, cancellationToken);
        }

        private static ComplianceSchemeMemberDto MapMember(
            RegistrationSubmissionProducer producer,
            bool isOriginalCsoLate,
            bool noFirstSubmission,
            bool submissionLevelLate)
        {
            return new ComplianceSchemeMemberDto
            {
                MemberId = producer.OrganisationId,
                MemberType = producer.OrganisationSize,
                IsOnlineMarketplace = producer.IsOnlineMarketplace,
                IsClosedLoopRecycling = producer.IsClosedLoopRecycling,
                NumberOfSubsidiaries = producer.Subsidiaries.Count,
                NoOfSubsidiariesOnlineMarketplace = producer.Subsidiaries.Count(s => s.IsOnlineMarketplace),
                NoOfSubsidiariesClosedLoopRecycling = producer.Subsidiaries.Count(s => s.IsClosedLoopRecycling),
                IsLateFeeApplicable =
                    isOriginalCsoLate
                    || (noFirstSubmission && submissionLevelLate)
                    || (submissionLevelLate && producer.IsNewJoiner),
            };
        }

        private static bool IsOnOrAfterDeadline(DateTime? candidate, DateTime deadline, DateTime today) =>
            (candidate ?? today).Date >= deadline.Date;
    }
}
