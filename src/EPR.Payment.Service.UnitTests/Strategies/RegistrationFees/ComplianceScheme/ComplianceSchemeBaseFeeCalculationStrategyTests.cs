using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
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
    public class ComplianceSchemeBaseFeeCalculationStrategyTests
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
            Action act = () => new ComplianceSchemeBaseFeeCalculationStrategy(nullRepository!);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'feesRepository')");
        }

        [TestMethod]
        public void Constructor_WhenFeesRepositoryIsNotNull_ShouldInitializeComplianceSchemeBaseFeeCalculationStrategy()
        {
            // Arrange
            var feesRepositoryMock = _fixture.Create<Mock<IComplianceSchemeFeesRepository>>();

            // Act
            var strategy = new ComplianceSchemeBaseFeeCalculationStrategy(feesRepositoryMock.Object);

            // Assert
            using (new AssertionScope())
            {
                strategy.Should().NotBeNull();
                strategy.Should().BeAssignableTo<IComplianceSchemeBaseFeeCalculationStrategy<RegulatorType, decimal>>();
            }
        }

        [TestMethod]
        [AutoMoqData]
        public async Task CalculateFeeAsync_WhenValidRegulator_ReturnsBaseFee(
            [Frozen] Mock<IComplianceSchemeFeesRepository> feesRepositoryMock,
            ComplianceSchemeBaseFeeCalculationStrategy strategy)
        {
            // Arrange
            var regulator = RegulatorType.Create("GB-ENG");

            feesRepositoryMock.Setup(repo => repo.GetBaseFeeAsync(regulator, It.IsAny<CancellationToken>()))
                .ReturnsAsync(1380400m); // £13,804 in pence

            // Act
            var result = await strategy.CalculateFeeAsync(regulator, CancellationToken.None);

            // Assert
            result.Should().Be(1380400m); // £13,804 in pence
        }

        [TestMethod]
        [AutoMoqData]
        public async Task CalculateFeeAsync_WhenBaseFeeIsZero_ThrowsKeyNotFoundException(
            [Frozen] Mock<IComplianceSchemeFeesRepository> feesRepositoryMock,
            ComplianceSchemeBaseFeeCalculationStrategy strategy)
        {
            // Arrange
            var regulator = RegulatorType.Create("GB-ENG");

            feesRepositoryMock.Setup(repo => repo.GetBaseFeeAsync(regulator, It.IsAny<CancellationToken>()))
                .ReturnsAsync(0m); // Simulate a scenario where the fee is zero

            // Act
            Func<Task> act = async () => await strategy.CalculateFeeAsync(regulator, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage(string.Format(ComplianceSchemeFeeCalculationExceptions.InvalidRegulatorError, regulator.Value));
        }
    }
}