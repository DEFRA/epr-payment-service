using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees;
using EPR.Payment.Service.Strategies.RegistrationFees.ComplianceScheme;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;

namespace EPR.Payment.Service.UnitTests.Strategies.RegistrationFees.ComplianceScheme
{
    [TestClass]
    public class CSOnlineMarketCalculationStrategyTests
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

            // Act
            Action act = () => new CSOnlineMarketCalculationStrategy(nullRepository!);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'feesRepository')");
        }

        [TestMethod]
        public void Constructor_WhenFeesRepositoryIsNotNull_ShouldInitializeOnlineMarketCalculationStrategy()
        {
            // Arrange
            var feesRepositoryMock = _fixture.Create<Mock<IComplianceSchemeFeesRepository>>();

            // Act
            var strategy = new CSOnlineMarketCalculationStrategy(feesRepositoryMock.Object);

            // Assert
            using (new AssertionScope())
            {
                strategy.Should().NotBeNull();
                strategy.Should().BeAssignableTo<IFeeCalculationStrategy<ComplianceSchemeMemberWithRegulatorDto, decimal>>();
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeeAsync_WhenOnlineMarketplaceIsTrueMarketWithValidRegulator_ReturnsOnlineMarketFee(
            [Frozen] Mock<IComplianceSchemeFeesRepository> feesRepositoryMock,
            CSOnlineMarketCalculationStrategy strategy)
        {
            // Arrange
            var request = new ComplianceSchemeMemberWithRegulatorDto
            {
                IsOnlineMarketplace = true,
                Regulator = RegulatorType.GBEng,
                MemberType = "Small",
                SubmissionDate = DateTime.UtcNow.Date
            };

            feesRepositoryMock.Setup(repo => repo.GetOnlineMarketFeeAsync(request.Regulator, request.SubmissionDate, It.IsAny<CancellationToken>()))
                .ReturnsAsync(257900m);

            // Act
            var result = await strategy.CalculateFeeAsync(request, CancellationToken.None);

            // Assert
            result.Should().Be(257900m);
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeeAsync_WhenOnlineMarketplaceIsFalse_ReturnsZeroFee(
            CSOnlineMarketCalculationStrategy strategy)
        {
            // Arrange
            var request = new ComplianceSchemeMemberWithRegulatorDto
            {
                IsOnlineMarketplace = false,
                Regulator = RegulatorType.GBEng,
                MemberType = "Small",
                SubmissionDate = DateTime.UtcNow.Date
            };

            // Act
            var result = await strategy.CalculateFeeAsync(request, CancellationToken.None);

            // Assert
            result.Should().Be(0m);
        }
    }
}
