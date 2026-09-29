using EPR.Payment.Service.Common.Constants;
using EPR.Payment.Service.Common.Data.DataModels;

namespace EPR.Payment.Service.Services.RegistrationSubmission
{
    // Determines which (OrganisationId, SubsidiaryId) pairs on the current registration cycle
    // are "newly added" — meaning they were not present in any prior cycle for the same
    // submissionId that had its application submitted on time and is still active (not
    // rejected or cancelled by the regulator). A queried cycle, or a cycle that has been
    // submitted on time but has not yet been terminally decided, both establish baseline.
    // The baseline is the union across ALL such prior cycles so that a subsidiary registered
    // once, removed later, and re-added is not treated as new.
    public static class NewlyAddedSubsidiaryFinder
    {
        public static IReadOnlySet<(string OrganisationId, string SubsidiaryId)> Find(
            RegistrationSubmissionData currentCycle,
            IReadOnlyList<RegistrationSubmissionData> allCyclesForSubmission,
            DateTime deadline)
        {
            ArgumentNullException.ThrowIfNull(currentCycle);
            ArgumentNullException.ThrowIfNull(allCyclesForSubmission);

            var comparer = SubsidiaryKeyComparer.Instance;
            var current = new HashSet<(string OrganisationId, string SubsidiaryId)>(comparer);
            foreach (var producer in currentCycle.Producers)
            {
                foreach (var subsidiary in producer.Subsidiaries)
                {
                    current.Add((producer.OrganisationId, subsidiary.SubsidiaryId));
                }
            }

            var deadlineDate = deadline.Date;
            var baseline = new HashSet<(string OrganisationId, string SubsidiaryId)>(comparer);
            foreach (var cycle in allCyclesForSubmission)
            {
                if (cycle.Id == currentCycle.Id)
                {
                    continue;
                }
                if (!IsApplicationSubmittedOnTimeAndActive(cycle, deadlineDate))
                {
                    continue;
                }

                foreach (var producer in cycle.Producers)
                {
                    foreach (var subsidiary in producer.Subsidiaries)
                    {
                        baseline.Add((producer.OrganisationId, subsidiary.SubsidiaryId));
                    }
                }
            }

            current.ExceptWith(baseline);
            return current;
        }

        private static bool IsApplicationSubmittedOnTimeAndActive(RegistrationSubmissionData cycle, DateTime deadlineDate)
        {
            bool submittedOnTime = false;

            foreach (var evt in cycle.Events)
            {
                if (evt.EventName == RegistrationEventNames.RejectedByRegulator
                    || evt.EventName == RegistrationEventNames.CancelledByRegulator)
                {
                    return false;
                }

                if (!submittedOnTime
                    && evt.EventName == RegistrationEventNames.SubmittedForRegulatorApproval
                    && evt.EventDate.Date <= deadlineDate)
                {
                    submittedOnTime = true;
                }
            }

            return submittedOnTime;
        }

        private sealed class SubsidiaryKeyComparer : IEqualityComparer<(string OrganisationId, string SubsidiaryId)>
        {
            public static readonly SubsidiaryKeyComparer Instance = new();

            public bool Equals((string OrganisationId, string SubsidiaryId) x, (string OrganisationId, string SubsidiaryId) y) =>
                string.Equals(x.OrganisationId, y.OrganisationId, StringComparison.OrdinalIgnoreCase)
                && string.Equals(x.SubsidiaryId, y.SubsidiaryId, StringComparison.OrdinalIgnoreCase);

            public int GetHashCode((string OrganisationId, string SubsidiaryId) obj) =>
                HashCode.Combine(
                    obj.OrganisationId?.ToUpperInvariant(),
                    obj.SubsidiaryId?.ToUpperInvariant());
        }
    }
}
