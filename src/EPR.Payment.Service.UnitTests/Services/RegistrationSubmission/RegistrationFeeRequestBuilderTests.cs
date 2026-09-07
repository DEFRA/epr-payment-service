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
