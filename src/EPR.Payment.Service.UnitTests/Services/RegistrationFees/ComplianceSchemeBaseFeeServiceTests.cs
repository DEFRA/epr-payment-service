using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Services.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees.ComplianceScheme;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentValidation;
using Moq;

namespace EPR.Payment.Service.UnitTests.Services.RegistrationFees
{
    [TestClass]
    public class ComplianceSchemeBaseFeeServiceTests
    {
        private Mock<IComplianceSchemeBaseFeeCalculationStrategy> _baseFeeCalculationStrategyMock = null!;
        private ComplianceSchemeBaseFeeService? _baseFeeService = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _baseFeeCalculationStrategyMock = new Mock<IComplianceSchemeBaseFeeCalculationStrategy>();
            _baseFeeService = new ComplianceSchemeBaseFeeService(_baseFeeCalculationStrategyMock.Object);
        }

        [TestMethod]
        public void Constructor_WhenBaseFeeCalculationStrategyIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            IComplianceSchemeBaseFeeCalculationStrategy? baseFeeCalculationStrategy = null;

            // Act
            Action act = () => new ComplianceSchemeBaseFeeService(baseFeeCalculationStrategy!);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'baseFeeCalculationStrategy')");
        }

        [TestMethod]
        public void Constructor_WhenAllDependenciesAreNotNull_ShouldInitializeComplianceSchemeBaseFeeService()
        {
            // Act
            var service = new ComplianceSchemeBaseFeeService(_baseFeeCalculationStrategyMock.Object);

            // Assert
            using (new AssertionScope())
            {
                service.Should().NotBeNull();
                service.Should().BeAssignableTo<IComplianceSchemeBaseFeeService>();
            }
        }

        [TestMethod, AutoMoqData]
        public async Task GetComplianceSchemeBaseFeeAsync_WhenValidRegulator_ReturnsBaseFee(
            [Frozen] RegulatorType regulator)
        {
            // Arrange
            _baseFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<RegulatorType>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1380400m); // £13,804 in pence

            // Act
            var result = await _baseFeeService.GetComplianceSchemeBaseFeeAsync(regulator, CancellationToken.None);

            // Assert
            result.Should().Be(1380400m); // £13,804 in pence
        }

        [TestMethod, AutoMoqData]
        public async Task GetComplianceSchemeBaseFeeAsync_WhenBaseFeeCalculationThrowsKeyNotFoundException_ThrowsInvalidOperationException(
            [Frozen] RegulatorType regulator)
        {
            // Arrange
            _baseFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<RegulatorType>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new KeyNotFoundException("Regulator not found"));

            // Act
            Func<Task> act = async () => await _baseFeeService.GetComplianceSchemeBaseFeeAsync(regulator, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage(ComplianceSchemeFeeCalculationExceptions.BaseFeeCalculationInvalidOperation);
        }

        [TestMethod, AutoMoqData]
        public async Task GetComplianceSchemeBaseFeeAsync_WhenNoExceptionOccurs_ReturnsExpectedFee(
            [Frozen] RegulatorType regulator)
        {
            // Arrange
            _baseFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<RegulatorType>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1380400m); // £13,804 in pence

            // Act
            var result = await _baseFeeService.GetComplianceSchemeBaseFeeAsync(regulator, CancellationToken.None);

            // Assert
            result.Should().Be(1380400m); // £13,804 in pence
        }
    }
}