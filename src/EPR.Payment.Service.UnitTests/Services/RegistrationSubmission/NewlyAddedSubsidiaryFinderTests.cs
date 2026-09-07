using EPR.Payment.Service.Common.Constants;
using EPR.Payment.Service.Common.Data.DataModels;
using EPR.Payment.Service.Services.RegistrationSubmission;
using FluentAssertions;
using FluentAssertions.Execution;

namespace EPR.Payment.Service.UnitTests.Services.RegistrationSubmission
{
    [TestClass]
    public class NewlyAddedSubsidiaryFinderTests
    {
        private static readonly DateTime Deadline = new(2026, 4, 1, 0, 0, 0, DateTimeKind.Utc);
        private static readonly DateTime OnTime = Deadline.AddDays(-10);
        private static readonly DateTime AfterDeadline = Deadline.AddDays(10);

        [TestMethod]
        public void Find_NullCurrentCycle_Throws()
        {
            Action act = () => NewlyAddedSubsidiaryFinder.Find(null!, Array.Empty<RegistrationSubmissionData>(), Deadline);
            act.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Find_NullAllCycles_Throws()
        {
            Action act = () => NewlyAddedSubsidiaryFinder.Find(NewCycle("cur"), null!, Deadline);
            act.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Find_NoPriorCycles_EveryCurrentSubIsNewlyAdded()
        {
            var current = NewCycle("cur", ("ORG-1", new[] { "S1", "S2" }));

            var result = NewlyAddedSubsidiaryFinder.Find(current, new[] { current }, Deadline);

            result.Should().BeEquivalentTo(new[]
            {
                ("ORG-1", "S1"),
                ("ORG-1", "S2"),
            });
        }

        [TestMethod]
        public void Find_PriorApprovedOnTime_SubsExistingInPriorAreNotNewlyAdded()
        {
            var prior = NewCycle("p1", ("ORG-1", new[] { "S1" }));
            AddAccepted(prior);
            AddSubmittedForApproval(prior, OnTime);

            var current = NewCycle("cur", ("ORG-1", new[] { "S1", "S2" }));

            var result = NewlyAddedSubsidiaryFinder.Find(current, new[] { prior, current }, Deadline);

            result.Should().BeEquivalentTo(new[] { ("ORG-1", "S2") });
        }

        [TestMethod]
        public void Find_PriorApprovedButLate_TreatsPriorSubsAsNewlyAdded()
        {
            var prior = NewCycle("p1", ("ORG-1", new[] { "S1" }));
            AddAccepted(prior);
            AddSubmittedForApproval(prior, AfterDeadline);

            var current = NewCycle("cur", ("ORG-1", new[] { "S1", "S2" }));

            var result = NewlyAddedSubsidiaryFinder.Find(current, new[] { prior, current }, Deadline);

            // prior was submitted late, so it does NOT establish a baseline; both current subs count as newly added.
            result.Should().BeEquivalentTo(new[]
            {
                ("ORG-1", "S1"),
                ("ORG-1", "S2"),
            });
        }

        [TestMethod]
        public void Find_PriorOnTimeButNotAccepted_TreatsPriorSubsAsNewlyAdded()
        {
            var prior = NewCycle("p1", ("ORG-1", new[] { "S1" }));
            AddSubmittedForApproval(prior, OnTime);
            // no AcceptedByRegulator event

            var current = NewCycle("cur", ("ORG-1", new[] { "S1", "S2" }));

            var result = NewlyAddedSubsidiaryFinder.Find(current, new[] { prior, current }, Deadline);

            result.Should().BeEquivalentTo(new[]
            {
                ("ORG-1", "S1"),
                ("ORG-1", "S2"),
            });
        }

        [TestMethod]
        public void Find_UnionAcrossPriorApprovedOnTimeCycles_RestoredSubIsNotNew()
        {
            // Cycle 1 (approved on-time) had S1, S2
            var cycle1 = NewCycle("p1", ("ORG-1", new[] { "S1", "S2" }));
            AddAccepted(cycle1);
            AddSubmittedForApproval(cycle1, OnTime);

            // Cycle 2 (approved on-time) removed S2
            var cycle2 = NewCycle("p2", ("ORG-1", new[] { "S1" }));
            AddAccepted(cycle2);
            AddSubmittedForApproval(cycle2, OnTime.AddDays(1));

            // Cycle 3 (current, after deadline) has S1, S2 (S2 restored), S3 (brand new)
            var current = NewCycle("cur", ("ORG-1", new[] { "S1", "S2", "S3" }));

            var result = NewlyAddedSubsidiaryFinder.Find(current, new[] { cycle1, cycle2, current }, Deadline);

            // S2 was in cycle-1 baseline, so it is NOT newly added even though absent from cycle-2.
            result.Should().BeEquivalentTo(new[] { ("ORG-1", "S3") });
        }

        [TestMethod]
        public void Find_CaseInsensitiveMatchOnSubsidiaryIdAndOrganisationId()
        {
            var prior = NewCycle("p1", ("ORG-1", new[] { "S1" }));
            AddAccepted(prior);
            AddSubmittedForApproval(prior, OnTime);

            var current = NewCycle("cur", ("org-1", new[] { "s1" }));

            var result = NewlyAddedSubsidiaryFinder.Find(current, new[] { prior, current }, Deadline);

            result.Should().BeEmpty();
        }

        [TestMethod]
        public void Find_MultipleProducers_ScopedByOrganisationId()
        {
            var prior = NewCycle("p1",
                ("ORG-A", new[] { "SA1" }),
                ("ORG-B", new[] { "SB1" }));
            AddAccepted(prior);
            AddSubmittedForApproval(prior, OnTime);

            // S-with-same-id-different-org test: ORG-A's SA1 is baseline; ORG-B's SA1 should count as newly added.
            var current = NewCycle("cur",
                ("ORG-A", new[] { "SA1" }),
                ("ORG-B", new[] { "SB1", "SA1" }));

            var result = NewlyAddedSubsidiaryFinder.Find(current, new[] { prior, current }, Deadline);

            result.Should().BeEquivalentTo(new[] { ("ORG-B", "SA1") });
        }

        [TestMethod]
        public void Find_CurrentCycleAppearsInAllCycles_IsSkippedFromBaseline()
        {
            // The current cycle would otherwise be its own baseline if not excluded — test this exclusion.
            var current = NewCycle("cur", ("ORG-1", new[] { "S1" }));
            AddAccepted(current);
            AddSubmittedForApproval(current, OnTime);

            var result = NewlyAddedSubsidiaryFinder.Find(current, new[] { current }, Deadline);

            result.Should().BeEquivalentTo(new[] { ("ORG-1", "S1") });
        }

        [TestMethod]
        public void Find_CurrentCycleHasNoSubsidiaries_ReturnsEmpty()
        {
            var current = NewCycle("cur", ("ORG-1", Array.Empty<string>()));

            var result = NewlyAddedSubsidiaryFinder.Find(current, new[] { current }, Deadline);

            result.Should().BeEmpty();
        }

        // Helpers -------------------------------------------------------------

        private static RegistrationSubmissionData NewCycle(string tag, params (string OrgId, string[] Subs)[] producers)
        {
            return new RegistrationSubmissionData
            {
                Id = Guid.NewGuid(),
                SubmissionId = Guid.NewGuid(),
                RegistrationBlobName = $"blob-{tag}",
                RegulatorNation = "GB-ENG",
                ApplicationReferenceNumber = $"REF-{tag}",
                Producers = producers.Select(p => new RegistrationSubmissionProducer
                {
                    Id = Guid.NewGuid(),
                    OrganisationId = p.OrgId,
                    OrganisationSize = "Large",
                    Subsidiaries = p.Subs.Select(s => new RegistrationSubmissionSubsidiary
                    {
                        Id = Guid.NewGuid(),
                        SubsidiaryId = s,
                    }).ToList(),
                }).ToList(),
                Events = new List<RegistrationSubmissionDataEvent>(),
            };
        }

        private static void AddAccepted(RegistrationSubmissionData cycle)
        {
            cycle.Events.Add(new RegistrationSubmissionDataEvent
            {
                Id = Guid.NewGuid(),
                EventName = RegistrationEventNames.AcceptedByRegulator,
                EventDate = DateTime.UtcNow,
            });
        }

        private static void AddSubmittedForApproval(RegistrationSubmissionData cycle, DateTime eventDate)
        {
            cycle.Events.Add(new RegistrationSubmissionDataEvent
            {
                Id = Guid.NewGuid(),
                EventName = RegistrationEventNames.SubmittedForRegulatorApproval,
                EventDate = eventDate,
            });
        }
    }
}
