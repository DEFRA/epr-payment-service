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
            DateTime today,
            IReadOnlySet<(string OrganisationId, string SubsidiaryId)> newlyAddedSubsidiaries)
        {
            ArgumentNullException.ThrowIfNull(latest);
            ArgumentNullException.ThrowIfNull(producer);
            ArgumentNullException.ThrowIfNull(lifecycle);
            ArgumentNullException.ThrowIfNull(newlyAddedSubsidiaries);

            var deadline = latest.SubmissionPeriodWindow.DeadlineDate;

            var latestSubmittedOnOrAfterDeadline =
                (lifecycle.LatestSubmittedForApprovalDate ?? today).Date >= deadline.Date;
            var firstSubmissionWasLate = lifecycle.FirstSubmittedForApprovalDate is DateTime firstApproval
                                          && firstApproval.Date >= deadline.Date;
            var neverYetSubmittedForApproval = lifecycle.FirstSubmittedForApprovalDate is null;
            var isLateFeeApplicable = firstSubmissionWasLate || (neverYetSubmittedForApproval && latestSubmittedOnOrAfterDeadline);
            var isResubmissionAfterDeadline = !neverYetSubmittedForApproval && latestSubmittedOnOrAfterDeadline;

            return new ProducerRegistrationFeesRequestDto
            {
                ProducerType = producer.OrganisationSize,
                NumberOfSubsidiaries = producer.Subsidiaries.Count,
                NumberOfLateSubsidiaries = CountLateSubsidiaries(producer, isLateFeeApplicable, isResubmissionAfterDeadline, newlyAddedSubsidiaries),
                NoOfSubsidiariesOnlineMarketplace = producer.Subsidiaries.Count(s => s.IsOnlineMarketplace),
                NoOfSubsidiariesClosedLoopRecycling = producer.Subsidiaries.Count(s => s.IsClosedLoopRecycling),
                IsProducerOnlineMarketplace = producer.IsOnlineMarketplace,
                IsClosedLoopRecycling = producer.IsClosedLoopRecycling,
                IsLateFeeApplicable = isLateFeeApplicable,
                Regulator = latest.RegulatorNation,
                ApplicationReferenceNumber = latest.ApplicationReferenceNumber,
                SubmissionDate = lifecycle.CalcDate,
            };
        }

        public static ComplianceSchemeFeesRequestDto BuildComplianceSchemeRequest(
            RegistrationSubmissionData latest,
            SubmissionLifecycle lifecycle,
            DateTime today,
            IReadOnlySet<(string OrganisationId, string SubsidiaryId)> newlyAddedSubsidiaries)
        {
            ArgumentNullException.ThrowIfNull(latest);
            ArgumentNullException.ThrowIfNull(lifecycle);
            ArgumentNullException.ThrowIfNull(newlyAddedSubsidiaries);

            var deadline = latest.SubmissionPeriodWindow.DeadlineDate;

            var submissionLevelLate = (lifecycle.LatestSubmittedForApprovalDate ?? today).Date >= deadline.Date;
            var isOriginalCsoLate = lifecycle.FirstSubmittedForApprovalDate is DateTime firstApproval
                                    && firstApproval.Date >= deadline.Date;
            var noFirstSubmission = lifecycle.FirstSubmittedForApprovalDate is null;
            var isResubmissionAfterDeadline = !noFirstSubmission && submissionLevelLate;

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
                    .Select(p => MapMember(p, isOriginalCsoLate, noFirstSubmission, submissionLevelLate, isResubmissionAfterDeadline, newlyAddedSubsidiaries))
                    .ToList(),
            };
        }

        private static ComplianceSchemeMemberDto MapMember(
            RegistrationSubmissionProducer producer,
            bool isOriginalCsoLate,
            bool noFirstSubmission,
            bool submissionLevelLate,
            bool isResubmissionAfterDeadline,
            IReadOnlySet<(string OrganisationId, string SubsidiaryId)> newlyAddedSubsidiaries)
        {
            var memberIsLate =
                isOriginalCsoLate
                || (noFirstSubmission && submissionLevelLate)
                || (submissionLevelLate && producer.IsNewJoiner);

            return new ComplianceSchemeMemberDto
            {
                MemberId = producer.OrganisationId,
                MemberType = producer.OrganisationSize,
                IsOnlineMarketplace = producer.IsOnlineMarketplace,
                IsClosedLoopRecycling = producer.IsClosedLoopRecycling,
                NumberOfSubsidiaries = producer.Subsidiaries.Count,
                NumberOfLateSubsidiaries = CountLateSubsidiaries(producer, memberIsLate, isResubmissionAfterDeadline, newlyAddedSubsidiaries),
                NoOfSubsidiariesOnlineMarketplace = producer.Subsidiaries.Count(s => s.IsOnlineMarketplace),
                NoOfSubsidiariesClosedLoopRecycling = producer.Subsidiaries.Count(s => s.IsClosedLoopRecycling),
                IsLateFeeApplicable = memberIsLate,
            };
        }

        private static int CountLateSubsidiaries(
            RegistrationSubmissionProducer producer,
            bool isLateFeeApplicable,
            bool isResubmissionAfterDeadline,
            IReadOnlySet<(string OrganisationId, string SubsidiaryId)> newlyAddedSubsidiaries)
        {
            // Rule 1 — the whole submission is late: every subsidiary attracts the sub-late-fee.
            if (isLateFeeApplicable)
            {
                return producer.Subsidiaries.Count;
            }

            // Rule 2 — on-time initial submission but the current cycle is a resubmission on/after
            // the deadline: only subsidiaries not present in any prior approved on-time cycle count.
            if (isResubmissionAfterDeadline)
            {
                return producer.Subsidiaries.Count(s =>
                    newlyAddedSubsidiaries.Contains((producer.OrganisationId, s.SubsidiaryId)));
            }

            return 0;
        }
    }
}
