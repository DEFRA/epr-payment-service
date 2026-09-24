using EPR.Payment.Service.Common.Data.DataModels;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Services.RegistrationSubmission;
using FluentAssertions;
using FluentAssertions.Execution;

namespace EPR.Payment.Service.UnitTests.Services.RegistrationSubmission
{
    [TestClass]
    public class RegistrationFeeRequestBuilderTests
    {
        private static readonly DateTime Today = new(2026, 7, 24, 0, 0, 0, DateTimeKind.Utc);
        private static readonly DateTime Deadline = new(2026, 4, 1, 0, 0, 0, DateTimeKind.Utc);
        private static readonly IReadOnlySet<(string OrganisationId, string SubsidiaryId)> EmptyNewlyAdded =
            new HashSet<(string OrganisationId, string SubsidiaryId)>();

        [TestMethod]
        public void BuildProducerRequest_MapsCoreFields()
        {
            var rsd = ProducerRsd();
            var producer = rsd.Producers.First();
            var lifecycle = new SubmissionLifecycle(rsd, rsd, null, null, Today);

            var request = RegistrationFeeRequestBuilder.BuildProducerRequest(rsd, producer, lifecycle, Today, EmptyNewlyAdded);

            using (new AssertionScope())
            {
                request.ProducerType.Should().Be("Large");
                request.Regulator.Should().Be("GB-ENG");
                request.ApplicationReferenceNumber.Should().Be("PEPR2601111");
                request.SubmissionDate.Should().Be(Today);
                request.IsLateFeeApplicable.Should().BeTrue();
            }
        }

        [TestMethod]
        public void BuildProducerRequest_OnTimeFirstSubmission_IsLateFeeFalse()
        {
            var rsd = ProducerRsd();
            var producer = rsd.Producers.First();
            var onTime = Deadline.AddDays(-10);
            var lifecycle = new SubmissionLifecycle(rsd, rsd, onTime, onTime, onTime);

            var request = RegistrationFeeRequestBuilder.BuildProducerRequest(rsd, producer, lifecycle, Today, EmptyNewlyAdded);

            request.IsLateFeeApplicable.Should().BeFalse();
        }

        [TestMethod]
        public void BuildComplianceSchemeRequest_MapsMembersAndFlags()
        {
            var rsd = ComplianceSchemeRsd();
            var lifecycle = new SubmissionLifecycle(rsd, rsd, null, null, Today);

            var request = RegistrationFeeRequestBuilder.BuildComplianceSchemeRequest(rsd, lifecycle, Today, EmptyNewlyAdded);

            using (new AssertionScope())
            {
                request.Regulator.Should().Be("GB-ENG");
                request.ApplicationReferenceNumber.Should().Be("PEPR2601111");
                request.SubmissionDate.Should().Be(Today);
                request.IncludeRegistrationFee.Should().BeTrue();
                request.ComplianceSchemeMembers.Should().HaveCount(2);
                request.ComplianceSchemeMembers.Should().OnlyContain(m => m.IsLateFeeApplicable);
            }
        }

        [TestMethod]
        public void BuildProducerRequest_Rule1_SubmissionLate_CountsAllSubsAsLate()
        {
            // First submission is late → IsLateFeeApplicable = true → all subs.
            var rsd = ProducerRsdWithSubs("S1", "S2", "S3");
            var producer = rsd.Producers.First();
            var lateFirstSubmit = Deadline.AddDays(5);
            var lifecycle = new SubmissionLifecycle(rsd, rsd, lateFirstSubmit, lateFirstSubmit, lateFirstSubmit);

            var request = RegistrationFeeRequestBuilder.BuildProducerRequest(rsd, producer, lifecycle, Today, EmptyNewlyAdded);

            using (new AssertionScope())
            {
                request.IsLateFeeApplicable.Should().BeTrue();
                request.NumberOfLateSubsidiaries.Should().Be(3);
            }
        }

        [TestMethod]
        public void BuildProducerRequest_Rule2_OnTimeFirstResubmissionAfterDeadline_CountsOnlyNewlyAdded()
        {
            var rsd = ProducerRsdWithSubs("S1", "S2", "S3");
            var producer = rsd.Producers.First();
            var onTimeFirstSubmit = Deadline.AddDays(-10);
            // Latest resubmission not-yet-submitted → LatestSubmittedForApprovalDate null; Today is after deadline.
            var lifecycle = new SubmissionLifecycle(rsd, rsd, onTimeFirstSubmit, null, onTimeFirstSubmit);
            var newlyAdded = new HashSet<(string, string)> { ("ORG-1", "S2") };

            var request = RegistrationFeeRequestBuilder.BuildProducerRequest(rsd, producer, lifecycle, Today, newlyAdded);

            using (new AssertionScope())
            {
                request.IsLateFeeApplicable.Should().BeFalse();
                request.NumberOfLateSubsidiaries.Should().Be(1);
            }
        }

        [TestMethod]
        public void BuildProducerRequest_OnTimeResubmission_NumberOfLateSubsidiariesIsZero()
        {
            var rsd = ProducerRsdWithSubs("S1", "S2");
            var producer = rsd.Producers.First();
            var onTime = Deadline.AddDays(-10);
            var lifecycle = new SubmissionLifecycle(rsd, rsd, onTime, onTime, onTime);

            var request = RegistrationFeeRequestBuilder.BuildProducerRequest(rsd, producer, lifecycle, Today, EmptyNewlyAdded);

            using (new AssertionScope())
            {
                request.IsLateFeeApplicable.Should().BeFalse();
                request.NumberOfLateSubsidiaries.Should().Be(0);
            }
        }

        [TestMethod]
        public void BuildComplianceSchemeRequest_NumberOfLateSubsidiariesIsScopedPerMember()
        {
            var rsd = ComplianceSchemeRsdWithSubs(
                ("MEM-A", new[] { "SA1", "SA2" }),
                ("MEM-B", new[] { "SB1" }));
            var lateFirstSubmit = Deadline.AddDays(5);
            var lifecycle = new SubmissionLifecycle(rsd, rsd, lateFirstSubmit, lateFirstSubmit, lateFirstSubmit);
            // Rule 1: submission is late → all subs of each member count.
            var newlyAdded = new HashSet<(string, string)>(); // unused under Rule 1

            var request = RegistrationFeeRequestBuilder.BuildComplianceSchemeRequest(rsd, lifecycle, Today, newlyAdded);

            using (new AssertionScope())
            {
                request.ComplianceSchemeMembers.Should().HaveCount(2);
                request.ComplianceSchemeMembers[0].NumberOfLateSubsidiaries.Should().Be(2);
                request.ComplianceSchemeMembers[1].NumberOfLateSubsidiaries.Should().Be(1);
            }
        }

        [TestMethod]
        public void BuildComplianceSchemeRequest_Rule2_ResubmissionAfterDeadline_CountsNewlyAddedScopedPerMember()
        {
            var rsd = ComplianceSchemeRsdWithSubs(
                ("MEM-A", new[] { "SA1", "SA2" }),
                ("MEM-B", new[] { "SB1" }));
            var onTimeFirstSubmit = Deadline.AddDays(-10);
            var lifecycle = new SubmissionLifecycle(rsd, rsd, onTimeFirstSubmit, null, onTimeFirstSubmit);
            var newlyAdded = new HashSet<(string, string)>
            {
                ("MEM-A", "SA2"),  // newly added on MEM-A
                ("MEM-B", "SB1"),  // newly added on MEM-B
            };

            var request = RegistrationFeeRequestBuilder.BuildComplianceSchemeRequest(rsd, lifecycle, Today, newlyAdded);

            using (new AssertionScope())
            {
                request.ComplianceSchemeMembers[0].NumberOfLateSubsidiaries.Should().Be(1);
                request.ComplianceSchemeMembers[1].NumberOfLateSubsidiaries.Should().Be(1);
            }
        }

        [TestMethod]
        public void BuildComplianceSchemeRequest_NewJoinerDuringLateResubmission_IsLateOnlyForTheNewJoiner()
        {
            // The scheme's own original submission was on time (isOriginalCsoLate = false), so the
            // scheme-wide Rule 1/Rule 2 branches of memberIsLate don't fire for anyone. This
            // resubmission (filed after the deadline) adds a brand-new member alongside one that was
            // already part of the scheme from that earlier on-time cycle - only the new joiner should
            // read as late; the existing member's own late-fee status must be unaffected by it.
            var rsd = ComplianceSchemeRsdWithSubs(
                ("MEM-EXISTING", new[] { "S1" }),
                ("MEM-NEW-JOINER", new[] { "S2" }));
            rsd.Producers.Single(p => p.OrganisationId == "MEM-NEW-JOINER").IsNewJoiner = true;

            var onTimeFirstSubmit = Deadline.AddDays(-10);
            var lateResubmit = Deadline.AddDays(5);
            var lifecycle = new SubmissionLifecycle(rsd, rsd, onTimeFirstSubmit, lateResubmit, lateResubmit);
            var newlyAdded = new HashSet<(string, string)>(); // MEM-EXISTING's own subsidiary was already present, not newly added

            var request = RegistrationFeeRequestBuilder.BuildComplianceSchemeRequest(rsd, lifecycle, Today, newlyAdded);

            var existingMember = request.ComplianceSchemeMembers.Single(m => m.MemberId == "MEM-EXISTING");
            var newJoinerMember = request.ComplianceSchemeMembers.Single(m => m.MemberId == "MEM-NEW-JOINER");

            using (new AssertionScope())
            {
                existingMember.IsLateFeeApplicable.Should().BeFalse("the scheme's first submission was on time and this member isn't a new joiner");
                existingMember.NumberOfLateSubsidiaries.Should().Be(0, "this member's subsidiary was already present in the earlier on-time cycle");

                newJoinerMember.IsLateFeeApplicable.Should().BeTrue("this member joined the scheme during a resubmission filed after the deadline");
                newJoinerMember.NumberOfLateSubsidiaries.Should().Be(1, "IsLateFeeApplicable true triggers Rule 1 - every subsidiary of a late member counts");
            }
        }

        [TestMethod]
        public void BuildComplianceSchemeRequest_NewJoinerViaOnTimeResubmission_IsNotLate()
        {
            // Companion to the test above: IsNewJoiner only matters in combination with
            // submissionLevelLate (memberIsLate's third disjunct is `submissionLevelLate &&
            // producer.IsNewJoiner`) - a new joiner added via a resubmission that is itself still
            // on time must not be treated as late just because they're new.
            var rsd = ComplianceSchemeRsdWithSubs(
                ("MEM-EXISTING", new[] { "S1" }),
                ("MEM-NEW-JOINER", new[] { "S2" }));
            rsd.Producers.Single(p => p.OrganisationId == "MEM-NEW-JOINER").IsNewJoiner = true;

            var onTimeFirstSubmit = Deadline.AddDays(-10);
            var onTimeResubmit = Deadline.AddDays(-3);
            var lifecycle = new SubmissionLifecycle(rsd, rsd, onTimeFirstSubmit, onTimeResubmit, onTimeResubmit);
            var newlyAdded = new HashSet<(string, string)>();

            var request = RegistrationFeeRequestBuilder.BuildComplianceSchemeRequest(rsd, lifecycle, Today, newlyAdded);

            var newJoinerMember = request.ComplianceSchemeMembers.Single(m => m.MemberId == "MEM-NEW-JOINER");

            using (new AssertionScope())
            {
                newJoinerMember.IsLateFeeApplicable.Should().BeFalse("the resubmission that added this new joiner was itself still before the deadline");
                newJoinerMember.NumberOfLateSubsidiaries.Should().Be(0, "not late, and not a resubmission-after-deadline either - Rule 2 doesn't apply");
            }
        }

        [DataTestMethod]
        [DataRow("Rule1: first submission late", true)]
        [DataRow("Rule2: on-time first submission, resubmitted after deadline", false)]
        public void BuildProducerRequest_And_BuildComplianceSchemeRequest_AgreeOnLateFeeSemantics(
            string scenario, bool firstSubmissionLate)
        {
            _ = scenario;
            var firstSubmittedDate = firstSubmissionLate ? Deadline.AddDays(5) : Deadline.AddDays(-10);
            var latestSubmittedDate = firstSubmissionLate ? firstSubmittedDate : (DateTime?)null;
            var newlyAdded = new HashSet<(string, string)> { ("ORG-1", "S2") };

            var producerRsd = ProducerRsdWithSubs("S1", "S2", "S3");
            var producerLifecycle = new SubmissionLifecycle(producerRsd, producerRsd, firstSubmittedDate, latestSubmittedDate, firstSubmittedDate);
            var producerRequest = RegistrationFeeRequestBuilder.BuildProducerRequest(
                producerRsd, producerRsd.Producers.First(), producerLifecycle, Today, newlyAdded);

            var csRsd = ComplianceSchemeRsdWithSubs(("ORG-1", new[] { "S1", "S2", "S3" }));
            var csLifecycle = new SubmissionLifecycle(csRsd, csRsd, firstSubmittedDate, latestSubmittedDate, firstSubmittedDate);
            var csRequest = RegistrationFeeRequestBuilder.BuildComplianceSchemeRequest(csRsd, csLifecycle, Today, newlyAdded);
            var csMember = csRequest.ComplianceSchemeMembers.Single();

            using (new AssertionScope())
            {
                csMember.IsLateFeeApplicable.Should().Be(
                    producerRequest.IsLateFeeApplicable,
                    "a single-member compliance scheme and an equivalent direct producer must reach the same late-fee decision for the same lifecycle history");
                csMember.NumberOfLateSubsidiaries.Should().Be(
                    producerRequest.NumberOfLateSubsidiaries,
                    "the two paths must count late subsidiaries identically given the same newly-added set");
            }
        }

        [TestMethod]
        public void BuildComplianceSchemeRequest_CsoSmallProducerWindow_IncludeRegistrationFeeFalse()
        {
            var rsd = ComplianceSchemeRsd();
            rsd.SubmissionPeriodWindow.WindowType = "CsoSmallProducer";
            var lifecycle = new SubmissionLifecycle(rsd, rsd, null, null, Today);

            var request = RegistrationFeeRequestBuilder.BuildComplianceSchemeRequest(rsd, lifecycle, Today, EmptyNewlyAdded);

            request.IncludeRegistrationFee.Should().BeFalse();
        }

        private static RegistrationSubmissionData ProducerRsd() => new()
        {
            Id = Guid.NewGuid(),
            SubmissionId = Guid.NewGuid(),
            RegistrationBlobName = "blob",
            RegulatorNation = "GB-ENG",
            ApplicationReferenceNumber = "PEPR2601111",
            Producers = new List<RegistrationSubmissionProducer>
            {
                new() { OrganisationId = "ORG-1", OrganisationSize = "Large", Subsidiaries = new List<RegistrationSubmissionSubsidiary>() },
            },
            SubmissionPeriodWindow = new SubmissionPeriod
            {
                Id = 1,
                WindowType = "LargeProducer",
                DeadlineDate = Deadline,
                OpeningDate = Deadline.AddMonths(-6),
                ClosingDate = Deadline.AddMonths(6),
                RegistrationYear = 2026,
            },
        };

        private static RegistrationSubmissionData ComplianceSchemeRsd()
        {
            var rsd = ProducerRsd();
            rsd.ComplianceSchemeId = Guid.NewGuid();
            rsd.SubmissionPeriodWindow.WindowType = "CsoLargeProducer";
            rsd.Producers = new List<RegistrationSubmissionProducer>
            {
                new() { OrganisationId = "ORG-1", OrganisationSize = "Large", Subsidiaries = new List<RegistrationSubmissionSubsidiary>() },
                new() { OrganisationId = "ORG-2", OrganisationSize = "Small", Subsidiaries = new List<RegistrationSubmissionSubsidiary>() },
            };
            return rsd;
        }

        private static RegistrationSubmissionData ProducerRsdWithSubs(params string[] subsidiaryIds)
        {
            var rsd = ProducerRsd();
            rsd.Producers.First().Subsidiaries = subsidiaryIds
                .Select(id => new RegistrationSubmissionSubsidiary { Id = Guid.NewGuid(), SubsidiaryId = id })
                .ToList();
            return rsd;
        }

        private static RegistrationSubmissionData ComplianceSchemeRsdWithSubs(params (string OrgId, string[] Subs)[] members)
        {
            var rsd = ComplianceSchemeRsd();
            rsd.Producers = members.Select(m => new RegistrationSubmissionProducer
            {
                Id = Guid.NewGuid(),
                OrganisationId = m.OrgId,
                OrganisationSize = "Large",
                Subsidiaries = m.Subs.Select(s => new RegistrationSubmissionSubsidiary
                {
                    Id = Guid.NewGuid(),
                    SubsidiaryId = s,
                }).ToList(),
            }).ToList();
            return rsd;
        }
    }
}
