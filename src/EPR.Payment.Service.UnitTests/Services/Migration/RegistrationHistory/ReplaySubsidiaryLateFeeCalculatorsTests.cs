using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.Producer;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.Producer;
using EPR.Payment.Service.Services.Migration.RegistrationHistory;
using FluentAssertions;
using Moq;

namespace EPR.Payment.Service.UnitTests.Services.Migration.RegistrationHistory
{
    [TestClass]
    public class ReplaySubsidiaryLateFeeCalculatorsTests
    {
        private static readonly DateTime ChargedFrom = new(2026, 10, 5, 0, 0, 0, DateTimeKind.Utc);

        [TestMethod]
        [DataRow(-1, 0, DisplayName = "before charged-from: no late subsidiaries")]
        [DataRow(0, 2, DisplayName = "on charged-from: late subsidiaries kept")]
        public async Task Producer_LateSubsidiariesOnlyFromChargedFrom(int daysFromChargedFrom, int expectedLateSubsidiaries)
        {
            var inner = new Mock<IProducerFeesCalculatorService>();
            var expected = new RegistrationFeesResponseDto { SubsidiariesFeeBreakdown = new() };
            inner.Setup(c => c.CalculateFeesAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>())).ReturnsAsync(expected);
            var request = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = "Large",
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "PEPR001",
                NumberOfLateSubsidiaries = 2,
            };

            var response = await new ReplayProducerFeesCalculator(inner.Object, Clock(daysFromChargedFrom), ChargedFrom)
                .CalculateFeesAsync(request, CancellationToken.None);

            response.Should().BeSameAs(expected);
            inner.Verify(c => c.CalculateFeesAsync(
                It.Is<ProducerRegistrationFeesRequestDto>(r => r.NumberOfLateSubsidiaries == expectedLateSubsidiaries),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        [DataRow(-1, 0, DisplayName = "before charged-from: no late subsidiaries")]
        [DataRow(0, 3, DisplayName = "on charged-from: late subsidiaries kept")]
        public async Task ComplianceScheme_LateSubsidiariesOnlyFromChargedFrom(int daysFromChargedFrom, int expectedLateSubsidiaries)
        {
            var inner = new Mock<IComplianceSchemeCalculatorService>();
            var expected = new ComplianceSchemeFeesResponseDto();
            inner.Setup(c => c.CalculateFeesAsync(It.IsAny<ComplianceSchemeFeesRequestDto>(), It.IsAny<CancellationToken>())).ReturnsAsync(expected);
            var request = new ComplianceSchemeFeesRequestDto
            {
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "PEPR001",
                ComplianceSchemeMembers =
                {
                    new ComplianceSchemeMemberDto { MemberId = "1", MemberType = "Large", NumberOfLateSubsidiaries = 3 },
                    new ComplianceSchemeMemberDto { MemberId = "2", MemberType = "Small", NumberOfLateSubsidiaries = 3 },
                },
            };

            var response = await new ReplayComplianceSchemeCalculator(inner.Object, Clock(daysFromChargedFrom), ChargedFrom)
                .CalculateFeesAsync(request, CancellationToken.None);

            response.Should().BeSameAs(expected);
            inner.Verify(c => c.CalculateFeesAsync(
                It.Is<ComplianceSchemeFeesRequestDto>(r => r.ComplianceSchemeMembers.All(m => m.NumberOfLateSubsidiaries == expectedLateSubsidiaries)),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        private static ReplayTimeProvider Clock(int daysFromChargedFrom)
        {
            var clock = new ReplayTimeProvider();
            clock.SetUtcNow(ChargedFrom.AddDays(daysFromChargedFrom));
            return clock;
        }
    }
}
