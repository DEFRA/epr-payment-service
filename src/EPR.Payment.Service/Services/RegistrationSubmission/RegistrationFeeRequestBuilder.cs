using EPR.Payment.Service.Common.Data.DataModels;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;

namespace EPR.Payment.Service.Services.RegistrationSubmission
{
    public static class RegistrationFeeRequestBuilder
    {
        private const string CsoSmallProducerWindowType = "CsoSmallProducer";

        public static ProducerRegistrationFeesRequestDto BuildProducerRequest(
            RegistrationSubmissionData latest,
            RegistrationSubmissionProducer producer,
            SubmissionLifecycle lifecycle,
            DateTime today)
        {
            ArgumentNullException.ThrowIfNull(latest);
            ArgumentNullException.ThrowIfNull(producer);
            ArgumentNullException.ThrowIfNull(lifecycle);

            var deadline = latest.SubmissionPeriodWindow.DeadlineDate;

            var latestSubmittedOnOrAfterDeadline =
                (lifecycle.LatestSubmittedForApprovalDate ?? today).Date >= deadline.Date;
            var firstSubmissionWasLate = lifecycle.FirstSubmittedForApprovalDate is DateTime firstApproval
                                          && firstApproval.Date >= deadline.Date;
            var neverYetSubmittedForApproval = lifecycle.FirstSubmittedForApprovalDate is null;

            return new ProducerRegistrationFeesRequestDto
            {
                ProducerType = producer.OrganisationSize,
                NumberOfSubsidiaries = producer.Subsidiaries.Count,
                NoOfSubsidiariesOnlineMarketplace = producer.Subsidiaries.Count(s => s.IsOnlineMarketplace),
                NoOfSubsidiariesClosedLoopRecycling = producer.Subsidiaries.Count(s => s.IsClosedLoopRecycling),
                IsProducerOnlineMarketplace = producer.IsOnlineMarketplace,
                IsClosedLoopRecycling = producer.IsClosedLoopRecycling,
                IsLateFeeApplicable = firstSubmissionWasLate || (neverYetSubmittedForApproval && latestSubmittedOnOrAfterDeadline),
                Regulator = latest.RegulatorNation,
                ApplicationReferenceNumber = latest.ApplicationReferenceNumber,
                SubmissionDate = lifecycle.CalcDate,
            };
        }

        public static ComplianceSchemeFeesRequestDto BuildComplianceSchemeRequest(
            RegistrationSubmissionData latest,
            SubmissionLifecycle lifecycle,
            DateTime today)
        {
            ArgumentNullException.ThrowIfNull(latest);
            ArgumentNullException.ThrowIfNull(lifecycle);

            var deadline = latest.SubmissionPeriodWindow.DeadlineDate;

            var submissionLevelLate = (lifecycle.LatestSubmittedForApprovalDate ?? today).Date >= deadline.Date;
            var isOriginalCsoLate = lifecycle.FirstSubmittedForApprovalDate is DateTime firstApproval
                                    && firstApproval.Date >= deadline.Date;
            var noFirstSubmission = lifecycle.FirstSubmittedForApprovalDate is null;

            return new ComplianceSchemeFeesRequestDto
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
    }
}
