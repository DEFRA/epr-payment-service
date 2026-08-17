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

        [TestMethod]
        public void BuildProducerRequest_MapsCoreFields()
        {
            var rsd = ProducerRsd();
            var producer = rsd.Producers.First();
            var lifecycle = new SubmissionLifecycle(rsd, rsd, null, null, Today);

            var request = RegistrationFeeRequestBuilder.BuildProducerRequest(rsd, producer, lifecycle, Today);

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

            var request = RegistrationFeeRequestBuilder.BuildProducerRequest(rsd, producer, lifecycle, Today);

            request.IsLateFeeApplicable.Should().BeFalse();
        }

        [TestMethod]
        public void BuildComplianceSchemeRequest_MapsMembersAndFlags()
        {
            var rsd = ComplianceSchemeRsd();
            var lifecycle = new SubmissionLifecycle(rsd, rsd, null, null, Today);

            var request = RegistrationFeeRequestBuilder.BuildComplianceSchemeRequest(rsd, lifecycle, Today);

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
        public void BuildComplianceSchemeRequest_CsoSmallProducerWindow_IncludeRegistrationFeeFalse()
        {
            var rsd = ComplianceSchemeRsd();
            rsd.SubmissionPeriodWindow.WindowType = "CsoSmallProducer";
            var lifecycle = new SubmissionLifecycle(rsd, rsd, null, null, Today);

            var request = RegistrationFeeRequestBuilder.BuildComplianceSchemeRequest(rsd, lifecycle, Today);

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
    }
}
