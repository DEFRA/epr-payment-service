using EPR.Payment.Service.Common.Constants;
using EPR.Payment.Service.Common.Data.DataModels;

namespace EPR.Payment.Service.Services.RegistrationSubmission
{
    public static class SubmissionLifecycleAnalyser
    {
        public static SubmissionLifecycle Analyse(
            IReadOnlyList<RegistrationSubmissionData> records,
            DateTime today)
        {
            ArgumentNullException.ThrowIfNull(records);

            var nonRejected = records
                .Where(r => !r.Events.Any(e => e.EventName == RegistrationEventNames.RejectedByRegulator))
                .ToList();

            var first = nonRejected.FirstOrDefault();
            var latest = nonRejected.LastOrDefault();

            var firstSubmittedDate = LatestSubmittedForApprovalDate(first);
            var latestSubmittedDate = LatestSubmittedForApprovalDate(latest);

            return new SubmissionLifecycle(
                FirstNonRejected: first,
                LatestNonRejected: latest,
                FirstSubmittedForApprovalDate: firstSubmittedDate,
                LatestSubmittedForApprovalDate: latestSubmittedDate,
                CalcDate: firstSubmittedDate ?? today);
        }

        private static DateTime? LatestSubmittedForApprovalDate(RegistrationSubmissionData? record) =>
            record?.Events
                .Where(e => e.EventName == RegistrationEventNames.SubmittedForRegulatorApproval)
                .OrderByDescending(e => e.EventDate)
                .Select(e => (DateTime?)e.EventDate)
                .FirstOrDefault();
    }
}
