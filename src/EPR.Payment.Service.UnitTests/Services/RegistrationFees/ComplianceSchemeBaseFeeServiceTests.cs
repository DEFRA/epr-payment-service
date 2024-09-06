using AutoFixture.MSTest;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Services.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees.ComplianceScheme;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace EPR.Payment.Service.UnitTests.Services.RegistrationFees
{
    [TestClass]
    public class ComplianceSchemeBaseFeeServiceTests
    {
        private Mock<IComplianceSchemeBaseFeeCalculationStrategy> _baseFeeCalculationStrategyMock = null!;
        private Mock<IValidator<string>> _regulatorValidatorMock = null!;
        private ComplianceSchemeBaseFeeService? _baseFeeService = null;

        [TestInitialize]
        public void TestInitialize()
        {
            _baseFeeCalculationStrategyMock = new Mock<IComplianceSchemeBaseFeeCalculationStrategy>();
            _regulatorValidatorMock = new Mock<IValidator<string>>();

            _baseFeeService = new ComplianceSchemeBaseFeeService(
                _baseFeeCalculationStrategyMock.Object,
                _regulatorValidatorMock.Object
            );
        }

        [TestMethod]
        public void Constructor_WhenBaseFeeCalculationStrategyIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            IComplianceSchemeBaseFeeCalculationStrategy? baseFeeCalculationStrategy = null;

            // Act
            Action act = () => new ComplianceSchemeBaseFeeService(
                baseFeeCalculationStrategy!,
                _regulatorValidatorMock.Object);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'baseFeeCalculationStrategy')");
        }

        [TestMethod]
        public void Constructor_WhenRegulatorValidatorIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            IValidator<string>? regulatorValidator = null;

            // Act
            Action act = () => new ComplianceSchemeBaseFeeService(
                _baseFeeCalculationStrategyMock.Object,
                regulatorValidator!);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'regulatorValidator')");
        }

        [TestMethod]
        public void Constructor_WhenAllDependenciesAreNotNull_ShouldInitializeComplianceSchemeBaseFeeService()
        {
            // Act
            var service = new ComplianceSchemeBaseFeeService(
                _baseFeeCalculationStrategyMock.Object,
                _regulatorValidatorMock.Object);

            // Assert
            using (new AssertionScope())
            {
                service.Should().NotBeNull();
                service.Should().BeAssignableTo<IComplianceSchemeBaseFeeService>();
            }
        }

        [TestMethod, AutoMoqData]
        public async Task GetComplianceSchemeBaseFeeAsync_WhenValidRegulator_ReturnsBaseFee(
            [Frozen] string regulator)
        {
            // Arrange
            _regulatorValidatorMock.Setup(v => v.Validate(It.IsAny<string>()))
                .Returns(new ValidationResult());

            _baseFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1380400m); // £13,804 in pence

            // Act
            var result = await _baseFeeService!.GetComplianceSchemeBaseFeeAsync(regulator, CancellationToken.None);

            // Assert
            result.Should().Be(1380400m); // £13,804 in pence
        }

        [TestMethod, AutoMoqData]
        public async Task GetComplianceSchemeBaseFeeAsync_WhenRegulatorIsNullOrEmpty_ThrowsArgumentException(
            [Frozen] string regulator)
        {
            // Arrange
            regulator = string.Empty;
            _regulatorValidatorMock.Setup(v => v.Validate(regulator))
                .Returns(new ValidationResult(new List<ValidationFailure> {
            new ValidationFailure("regulator", "Regulator is required")
            {
                Severity = Severity.Error // This should match the actual message format
            }
                }));

            // Act
            Func<Task> act = async () => await _baseFeeService!.GetComplianceSchemeBaseFeeAsync(regulator, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>()
                .WithMessage("Validation failed: \n -- regulator: Regulator is required Severity: Error");
        }

        [TestMethod, AutoMoqData]
        public async Task GetComplianceSchemeBaseFeeAsync_WhenBaseFeeCalculationThrowsArgumentException_ThrowsInvalidOperationException(
            [Frozen] string regulator)
        {
            // Arrange
            _regulatorValidatorMock.Setup(v => v.Validate(It.IsAny<string>()))
                .Returns(new ValidationResult());

            _baseFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ArgumentException("Invalid regulator"));

            // Act
            Func<Task> act = async () => await _baseFeeService!.GetComplianceSchemeBaseFeeAsync(regulator, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Error calculating compliance scheme base fee.");
        }

        [TestMethod, AutoMoqData]
        public async Task GetComplianceSchemeBaseFeeAsync_WhenNoExceptionOccurs_ReturnsExpectedFee(
            [Frozen] string regulator)
        {
            // Arrange
            _regulatorValidatorMock.Setup(v => v.Validate(It.IsAny<string>()))
                .Returns(new ValidationResult());

            _baseFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1380400m); // £13,804 in pence

            // Act
            var result = await _baseFeeService!.GetComplianceSchemeBaseFeeAsync(regulator, CancellationToken.None);

            // Assert
            result.Should().Be(1380400m); // £13,804 in pence
        }
    }
}
