using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using EPR.Payment.Service.Strategies.Interfaces.Common;
using EPR.Payment.Service.Strategies.RegistrationFees.ComplianceScheme;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;

namespace EPR.Payment.Service.UnitTests.Strategies.RegistrationFees.ComplianceScheme
{
    [TestClass]
    public class CSClosedLoopRecyclingCalculationStrategyTests
    {
        private IFixture _fixture = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
        }

        [TestMethod]
        public void Constructor_WhenFeesRepositoryIsNull_ShouldThrowArgumentNullException()
        {
            IComplianceSchemeFeesRepository? nullRepository = null;

            Assert.ThrowsException<ArgumentNullException>(() => new CSClosedLoopRecyclingCalculationStrategy(nullRepository!));
        }

        [TestMethod]
        public void Constructor_WhenFeesRepositoryIsNotNull_ShouldInitializeStrategy()
        {
            var feesRepositoryMock = _fixture.Create<Mock<IComplianceSchemeFeesRepository>>();

            var strategy = new CSClosedLoopRecyclingCalculationStrategy(feesRepositoryMock.Object);

            using (new AssertionScope())
            {
                strategy.Should().NotBeNull();
                strategy.Should().BeAssignableTo<IFeeCalculationStrategy<ComplianceSchemeMemberWithRegulatorDto, decimal>>();
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeeAsync_WhenClosedLoopRecyclingTrueAndValidRegulator_ReturnsClosedLoopRecyclingFee(
            [Frozen] Mock<IComplianceSchemeFeesRepository> feesRepositoryMock,
            CSClosedLoopRecyclingCalculationStrategy strategy)
        {
            var request = new ComplianceSchemeMemberWithRegulatorDto
            {
                IsClosedLoopRecycling = true,
                Regulator = RegulatorType.GBEng,
                MemberType = "Large",
                SubmissionDate = DateTime.UtcNow
            };

            feesRepositoryMock.Setup(repo => repo.GetClosedLoopRecyclingFeeAsync(request.Regulator, request.SubmissionDate, It.IsAny<CancellationToken>()))
                .ReturnsAsync(254800m);

            var result = await strategy.CalculateFeeAsync(request, CancellationToken.None);

            result.Should().Be(254800m);
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeeAsync_WhenClosedLoopRecyclingFalse_ReturnsZeroFee(
            [Frozen] Mock<IComplianceSchemeFeesRepository> feesRepositoryMock,
            CSClosedLoopRecyclingCalculationStrategy strategy)
        {
            var request = new ComplianceSchemeMemberWithRegulatorDto
            {
                IsClosedLoopRecycling = false,
                Regulator = RegulatorType.GBEng,
                MemberType = "Large",
                SubmissionDate = DateTime.UtcNow
            };

            var result = await strategy.CalculateFeeAsync(request, CancellationToken.None);

            result.Should().Be(0m);
            feesRepositoryMock.Verify(r => r.GetClosedLoopRecyclingFeeAsync(It.IsAny<RegulatorType>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
