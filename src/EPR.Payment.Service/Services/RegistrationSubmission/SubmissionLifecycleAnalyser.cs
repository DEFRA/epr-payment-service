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
            var todayUtc = DateTime.SpecifyKind(today, DateTimeKind.Utc);

            return new SubmissionLifecycle(
                FirstNonRejected: first,
                LatestNonRejected: latest,
                FirstSubmittedForApprovalDate: firstSubmittedDate,
                LatestSubmittedForApprovalDate: latestSubmittedDate,
                CalcDate: firstSubmittedDate ?? todayUtc);
        }

        // EventDate is persisted as SQL Server datetime2 (no timezone) and materialises with
        // Kind=Unspecified. Downstream fee validation requires strict UTC, so mark the value UTC
        // here — the stored value already represents UTC, just untagged.
        private static DateTime? LatestSubmittedForApprovalDate(RegistrationSubmissionData? record) =>
            record?.Events
                .Where(e => e.EventName == RegistrationEventNames.SubmittedForRegulatorApproval)
                .OrderByDescending(e => e.EventDate)
                .Select(e => (DateTime?)DateTime.SpecifyKind(e.EventDate, DateTimeKind.Utc))
                .FirstOrDefault();
    }
}
