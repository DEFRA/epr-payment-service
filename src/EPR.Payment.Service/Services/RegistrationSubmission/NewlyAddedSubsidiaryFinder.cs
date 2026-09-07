using EPR.Payment.Service.Common.Constants;
using EPR.Payment.Service.Common.Data.DataModels;

namespace EPR.Payment.Service.Services.RegistrationSubmission
{
    // Determines which (OrganisationId, SubsidiaryId) pairs on the current registration cycle
    // are "newly added" — meaning they were not present in any prior cycle for the same
    // submissionId that was both accepted by the regulator AND submitted before the deadline.
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
                if (!IsApprovedAndSubmittedOnTime(cycle, deadlineDate))
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

        private static bool IsApprovedAndSubmittedOnTime(RegistrationSubmissionData cycle, DateTime deadlineDate)
        {
            bool accepted = false;
            bool submittedOnTime = false;

            foreach (var evt in cycle.Events)
            {
                if (!accepted && evt.EventName == RegistrationEventNames.AcceptedByRegulator)
                {
                    accepted = true;
                }
                else if (!submittedOnTime
                         && evt.EventName == RegistrationEventNames.SubmittedForRegulatorApproval
                         && evt.EventDate.Date <= deadlineDate)
                {
                    submittedOnTime = true;
                }

                if (accepted && submittedOnTime)
                {
                    return true;
                }
            }

            return accepted && submittedOnTime;
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
