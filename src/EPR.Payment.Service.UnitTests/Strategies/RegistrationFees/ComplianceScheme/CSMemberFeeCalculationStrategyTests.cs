using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Strategies.RegistrationFees.ComplianceScheme;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;

namespace EPR.Payment.Service.UnitTests.Strategies.RegistrationFees.ComplianceScheme
{
    [TestClass]
    public class CSMemberFeeCalculationStrategyTests
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
            // Arrange
            IComplianceSchemeFeesRepository? nullRepository = null;

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => new CSMemberFeeCalculationStrategy(nullRepository!));
        }

        [TestMethod]
        public void Constructor_WhenFeesRepositoryIsNotNull_ShouldInitializeComplianceSchemeMemberCalculationStrategy()
        {
            // Arrange
            var feesRepositoryMock = _fixture.Create<Mock<IComplianceSchemeFeesRepository>>();

            // Act
            var strategy = new CSMemberFeeCalculationStrategy(feesRepositoryMock.Object);

            // Assert
            using (new AssertionScope())
            {
                strategy.Should().NotBeNull();
                strategy.Should().BeAssignableTo<ICSMemberFeeCalculationStrategy<ComplianceSchemeMemberWithRegulatorDto, decimal>>();
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeeAsync_WhenValidRequest_ReturnsMemberFee(
            [Frozen] Mock<IComplianceSchemeFeesRepository> feesRepositoryMock,
            CSMemberFeeCalculationStrategy strategy)
        {
            // Arrange
            var request = new ComplianceSchemeMemberWithRegulatorDto { MemberType = "Large", Regulator = RegulatorType.GBEng, SubmissionDate = DateTime.UtcNow };

            feesRepositoryMock.Setup(repo => repo.GetMemberFeeAsync(request.MemberType, request.Regulator, request.SubmissionDate, It.IsAny<CancellationToken>()))
                .ReturnsAsync(165800m); 

            // Act
            var result = await strategy.CalculateFeeAsync(request, CancellationToken.None);

            // Assert
            result.Should().Be(165800m); 
        }
    }
}
