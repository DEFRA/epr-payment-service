using AutoFixture;
using AutoFixture.AutoMoq;
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
    public class CSSubsidiaryLateFeeCalculationStrategyTests
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
            Assert.ThrowsException<ArgumentNullException>(() => new CSSubsidiaryLateFeeCalculationStrategy(nullRepository!));
        }

        [TestMethod]
        public void Constructor_WhenFeesRepositoryIsNotNull_ShouldInitializeSubsidiaryLateFeeCalculationStrategy()
        {
            // Arrange
            var feesRepositoryMock = _fixture.Create<Mock<IComplianceSchemeFeesRepository>>();

            // Act
            var strategy = new CSSubsidiaryLateFeeCalculationStrategy(feesRepositoryMock.Object);

            // Assert
            using (new AssertionScope())
            {
                strategy.Should().NotBeNull();
                strategy.Should().BeAssignableTo<IFeeCalculationStrategy<ComplianceSchemeLateFeeRequestDto, decimal>>();
            }
        }

        // Unlike the Producer strategy, CalculateFeeAsync here has no "is there anything to charge"
        // guard of its own - the caller (ComplianceSchemeCalculatorService.GetSubsidiaryLateFee) only
        // ever invokes this strategy after already checking at least one member has
        // NumberOfLateSubsidiaries > 0, so there is no "count is zero" case to exercise at this level.
        [TestMethod, AutoMoqData]
        public async Task CalculateFeeAsync_WithValidRegulator_ReturnsSubsidiaryLateFee(
            [Frozen] Mock<IComplianceSchemeFeesRepository> feesRepositoryMock,
            CSSubsidiaryLateFeeCalculationStrategy strategy)
        {
            // Arrange
            var request = new ComplianceSchemeLateFeeRequestDto
            {
                IsLateFeeApplicable = true,
                Regulator = RegulatorType.GBEng,
                SubmissionDate = DateTime.UtcNow,
            };

            feesRepositoryMock.Setup(repo => repo.GetSubsidiaryLateFeeAsync(request.Regulator, request.SubmissionDate, It.IsAny<CancellationToken>()))
                .ReturnsAsync(77200m);

            // Act
            var result = await strategy.CalculateFeeAsync(request, CancellationToken.None);

            // Assert
            result.Should().Be(77200m);
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeeAsync_PassesRegulatorAndSubmissionDateThrough_RegardlessOfIsLateFeeApplicable(
            [Frozen] Mock<IComplianceSchemeFeesRepository> feesRepositoryMock,
            CSSubsidiaryLateFeeCalculationStrategy strategy)
        {
            // Arrange - IsLateFeeApplicable is deliberately false here: the strategy itself doesn't
            // read that flag at all (only the caller's own gating decides whether to invoke it), so
            // the repository call - and its result - must still go through unconditionally.
            var request = new ComplianceSchemeLateFeeRequestDto
            {
                IsLateFeeApplicable = false,
                Regulator = RegulatorType.GBEng,
                SubmissionDate = DateTime.UtcNow,
            };

            feesRepositoryMock.Setup(repo => repo.GetSubsidiaryLateFeeAsync(request.Regulator, request.SubmissionDate, It.IsAny<CancellationToken>()))
                .ReturnsAsync(51200m);

            // Act
            var result = await strategy.CalculateFeeAsync(request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                result.Should().Be(51200m);
                feesRepositoryMock.Verify(repo => repo.GetSubsidiaryLateFeeAsync(request.Regulator, request.SubmissionDate, It.IsAny<CancellationToken>()), Times.Once);
            }
        }
    }
}
