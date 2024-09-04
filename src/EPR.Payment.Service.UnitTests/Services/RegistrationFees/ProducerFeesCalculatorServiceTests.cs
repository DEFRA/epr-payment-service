using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees;
using EPR.Payment.Service.Services.RegistrationFees;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees;
using EPR.Payment.Service.Utilities.RegistrationFees.Interfaces;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace EPR.Payment.Service.UnitTests.Services.RegistrationFees
{
    [TestClass]
    public class ProducerFeesCalculatorServiceTests
    {
        private Mock<IBaseFeeCalculationStrategy<ProducerRegistrationFeesRequestDto>> _baseFeeCalculationStrategyMock = null!;
        private Mock<ISubsidiariesFeeCalculationStrategy<ProducerRegistrationFeesRequestDto>> _subsidiariesFeeCalculationStrategyMock = null!;
        private Mock<IValidator<ProducerRegistrationFeesRequestDto>> _validatorMock = null!;
        private Mock<IFeeBreakdownGenerator<ProducerRegistrationFeesRequestDto, RegistrationFeesResponseDto>> _feeBreakdownGeneratorMock = null!;
        private ProducerFeesCalculatorService? _calculatorService = null;

        [TestInitialize]
        public void TestInitialize()
        {
            _baseFeeCalculationStrategyMock = new Mock<IBaseFeeCalculationStrategy<ProducerRegistrationFeesRequestDto>>();
            _subsidiariesFeeCalculationStrategyMock = new Mock<ISubsidiariesFeeCalculationStrategy<ProducerRegistrationFeesRequestDto>>();
            _validatorMock = new Mock<IValidator<ProducerRegistrationFeesRequestDto>>();
            _feeBreakdownGeneratorMock = new Mock<IFeeBreakdownGenerator<ProducerRegistrationFeesRequestDto, RegistrationFeesResponseDto>>();

            _calculatorService = new ProducerFeesCalculatorService(
                _baseFeeCalculationStrategyMock.Object,
                _subsidiariesFeeCalculationStrategyMock.Object,
                _validatorMock.Object,
                _feeBreakdownGeneratorMock.Object
            );
        }

        [TestMethod]
        public void Constructor_WhenBaseFeeCalculationStrategyIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            IBaseFeeCalculationStrategy<ProducerRegistrationFeesRequestDto>? baseFeeCalculationStrategy = null;

            // Act
            Action act = () => new ProducerFeesCalculatorService(
                baseFeeCalculationStrategy!,
                _subsidiariesFeeCalculationStrategyMock.Object,
                _validatorMock.Object,
                _feeBreakdownGeneratorMock.Object);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'baseFeeCalculationStrategy')");
        }

        [TestMethod]
        public void Constructor_WhenSubsidiariesFeeCalculationStrategyIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            ISubsidiariesFeeCalculationStrategy<ProducerRegistrationFeesRequestDto>? subsidiariesFeeCalculationStrategy = null;

            // Act
            Action act = () => new ProducerFeesCalculatorService(
                _baseFeeCalculationStrategyMock.Object,
                subsidiariesFeeCalculationStrategy!,
                _validatorMock.Object,
                _feeBreakdownGeneratorMock.Object);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'subsidiariesFeeCalculationStrategy')");
        }

        [TestMethod]
        public void Constructor_WhenValidatorIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            IValidator<ProducerRegistrationFeesRequestDto>? validator = null;

            // Act
            Action act = () => new ProducerFeesCalculatorService(
                _baseFeeCalculationStrategyMock.Object,
                _subsidiariesFeeCalculationStrategyMock.Object,
                validator!,
                _feeBreakdownGeneratorMock.Object);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'validator')");
        }

        [TestMethod]
        public void Constructor_WhenFeeBreakdownGeneratorIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            IFeeBreakdownGenerator<ProducerRegistrationFeesRequestDto, RegistrationFeesResponseDto>? feeBreakdownGenerator = null;

            // Act
            Action act = () => new ProducerFeesCalculatorService(
                _baseFeeCalculationStrategyMock.Object,
                _subsidiariesFeeCalculationStrategyMock.Object,
                _validatorMock.Object,
                feeBreakdownGenerator!);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'feeBreakdownGenerator')");
        }

        [TestMethod]
        public void Constructor_WhenAllDependenciesAreNotNull_ShouldInitializeProducerFeesCalculatorService()
        {
            // Act
            var service = new ProducerFeesCalculatorService(
                _baseFeeCalculationStrategyMock.Object,
                _subsidiariesFeeCalculationStrategyMock.Object,
                _validatorMock.Object,
                _feeBreakdownGeneratorMock.Object);

            // Assert
            using (new AssertionScope())
            {
                service.Should().NotBeNull();
                service.Should().BeAssignableTo<IProducerFeesCalculatorService>();
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeesAsync_WhenLargeProducerWith50Subsidiaries_ReturnsCorrectFees(
            [Frozen] ProducerRegistrationFeesRequestDto request)
        {
            // Arrange
            request.ProducerType = "Large";
            request.NumberOfSubsidiaries = 50;
            request.Regulator = "GB-ENG";

            _baseFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(262000m); // £2,620 represented in pence

            _subsidiariesFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1536000m); // Total subsidiaries fee in pence

            _validatorMock.Setup(v => v.Validate(It.IsAny<ProducerRegistrationFeesRequestDto>()))
                .Returns(new ValidationResult());

            // Act
            var result = await _calculatorService!.CalculateFeesAsync(request, CancellationToken.None);

            // Assert
            _feeBreakdownGeneratorMock.Verify(g => g.GenerateFeeBreakdownAsync(result, request, CancellationToken.None), Times.Once);

            using (new AssertionScope())
            {
                result.BaseFee.Should().Be(262000m); // £2,620 represented in pence
                result.SubsidiariesFee.Should().Be(1536000m); // Total subsidiaries fee in pence
                result.TotalFee.Should().Be(1798000m); // Total fee in pence
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeesAsync_WhenLargeProducerWith10Subsidiaries_ReturnsCorrectFees(
            [Frozen] ProducerRegistrationFeesRequestDto request)
        {
            // Arrange
            request.ProducerType = "Large";
            request.NumberOfSubsidiaries = 10;
            request.Regulator = "GB-ENG";

            _baseFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(262000m); // £2,620 represented in pence

            _subsidiariesFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(558000m); // Total subsidiaries fee in pence

            _validatorMock.Setup(v => v.Validate(It.IsAny<ProducerRegistrationFeesRequestDto>()))
                .Returns(new ValidationResult());

            // Act
            var result = await _calculatorService!.CalculateFeesAsync(request, CancellationToken.None);

            // Assert
            _feeBreakdownGeneratorMock.Verify(g => g.GenerateFeeBreakdownAsync(result, request, CancellationToken.None), Times.Once);

            using (new AssertionScope())
            {
                result.BaseFee.Should().Be(262000m); // £2,620 represented in pence
                result.SubsidiariesFee.Should().Be(558000m); // Total subsidiaries fee in pence for 10 subsidiaries
                result.TotalFee.Should().Be(820000m); // Total fee in pence
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeesAsync_WhenLargeProducerWithNoBaseFeeAnd50Subsidiaries_ReturnsCorrectFees(
            [Frozen] ProducerRegistrationFeesRequestDto request)
        {
            // Arrange
            request.ProducerType = ""; // ProducerType is empty to indicate no base fee
            request.NumberOfSubsidiaries = 50;
            request.Regulator = "GB-ENG";

            _baseFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(0m); // No base fee for any ProducerType

            _subsidiariesFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1536000m); // Total subsidiaries fee in pence

            _validatorMock.Setup(v => v.Validate(It.IsAny<ProducerRegistrationFeesRequestDto>()))
                .Returns(new ValidationResult());

            // Act
            var result = await _calculatorService!.CalculateFeesAsync(request, CancellationToken.None);

            // Assert
            _feeBreakdownGeneratorMock.Verify(g => g.GenerateFeeBreakdownAsync(result, request, CancellationToken.None), Times.Once);

            using (new AssertionScope())
            {
                result.BaseFee.Should().Be(0m); // No base fee
                result.SubsidiariesFee.Should().Be(1536000m); // Total subsidiaries fee in pence
                result.TotalFee.Should().Be(1536000m); // Total fee in pence (since no base fee)
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeesAsync_WhenSmallProducerWith25Subsidiaries_ReturnsCorrectFees(
            [Frozen] ProducerRegistrationFeesRequestDto request)
        {
            // Arrange
            request.ProducerType = "Small";
            request.NumberOfSubsidiaries = 25;
            request.Regulator = "GB-ENG";

            _baseFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(121600m); // £1,216 represented in pence

            _subsidiariesFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1186000m); // Total subsidiaries fee in pence

            _validatorMock.Setup(v => v.Validate(It.IsAny<ProducerRegistrationFeesRequestDto>()))
                .Returns(new ValidationResult());

            // Act
            var result = await _calculatorService!.CalculateFeesAsync(request, CancellationToken.None);

            // Assert
            _feeBreakdownGeneratorMock.Verify(g => g.GenerateFeeBreakdownAsync(result, request, CancellationToken.None), Times.Once);

            using (new AssertionScope())
            {
                result.BaseFee.Should().Be(121600m); // £1,216 represented in pence
                result.SubsidiariesFee.Should().Be(1186000m); // Total subsidiaries fee in pence
                result.TotalFee.Should().Be(1307600m); // Total fee in pence
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeesAsync_WhenSmallProducerWith20Subsidiaries_ReturnsCorrectFees(
            [Frozen] ProducerRegistrationFeesRequestDto request)
        {
            // Arrange
            request.ProducerType = "Small";
            request.NumberOfSubsidiaries = 20;
            request.Regulator = "GB-ENG";

            _baseFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(121600m); // £1,216 represented in pence

            _subsidiariesFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1116000m); // Total subsidiaries fee in pence for 20 subsidiaries

            _validatorMock.Setup(v => v.Validate(It.IsAny<ProducerRegistrationFeesRequestDto>()))
                .Returns(new ValidationResult());

            // Act
            var result = await _calculatorService!.CalculateFeesAsync(request, CancellationToken.None);

            // Assert
            _feeBreakdownGeneratorMock.Verify(g => g.GenerateFeeBreakdownAsync(result, request, CancellationToken.None), Times.Once);

            using (new AssertionScope())
            {
                result.BaseFee.Should().Be(121600m); // £1,216 represented in pence
                result.SubsidiariesFee.Should().Be(1116000m); // Total subsidiaries fee in pence for 20 subsidiaries
                result.TotalFee.Should().Be(1237600m); // Total fee in pence
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeesAsync_WhenLargeProducerWithNoSubsidiaries_ReturnsBaseFeeOnly(
            [Frozen] ProducerRegistrationFeesRequestDto request)
        {
            // Arrange
            request.ProducerType = "Large";
            request.NumberOfSubsidiaries = 0;
            request.Regulator = "GB-ENG";

            _baseFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(262000m); // £2,620 represented in pence

            _validatorMock.Setup(v => v.Validate(It.IsAny<ProducerRegistrationFeesRequestDto>()))
                .Returns(new ValidationResult());

            // Act
            var result = await _calculatorService!.CalculateFeesAsync(request, CancellationToken.None);

            // Assert
            _feeBreakdownGeneratorMock.Verify(g => g.GenerateFeeBreakdownAsync(result, request, CancellationToken.None), Times.Once);

            using (new AssertionScope())
            {
                result.BaseFee.Should().Be(262000m); // £2,620 represented in pence
                result.SubsidiariesFee.Should().Be(0m); // No subsidiaries fee
                result.TotalFee.Should().Be(262000m); // Total fee in pence (base fee only)
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeesAsync_WhenInvalidRequest_ThrowsValidationException(
            [Frozen] ProducerRegistrationFeesRequestDto request)
        {
            // Arrange
            request.ProducerType = null!;
            request.NumberOfSubsidiaries = -1;
            request.Regulator = null!;

            var validationFailures = new[]
            {
                new ValidationFailure("ProducerType", "ProducerType is required"),
                new ValidationFailure("NumberOfSubsidiaries", "Number of Subsidiaries must be greater than or equal to 0"),
                new ValidationFailure("Regulator", "Regulator is required")
            };

            _validatorMock.Setup(v => v.Validate(It.IsAny<ProducerRegistrationFeesRequestDto>()))
                .Returns(new ValidationResult(validationFailures));

            // Act & Assert
            await Assert.ThrowsExceptionAsync<ValidationException>(async () =>
            {
                await _calculatorService!.CalculateFeesAsync(request, CancellationToken.None);
            });
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeesAsync_WhenArgumentExceptionOccurs_ThrowsInvalidOperationException(
            [Frozen] ProducerRegistrationFeesRequestDto request)
        {
            // Arrange
            _baseFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ArgumentException("Invalid argument"));

            _validatorMock.Setup(v => v.Validate(It.IsAny<ProducerRegistrationFeesRequestDto>()))
                .Returns(new ValidationResult());

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () =>
            {
                await _calculatorService!.CalculateFeesAsync(request, CancellationToken.None);
            });

            exception.Message.Should().Be("An error occurred while calculating fees.");
            exception.InnerException.Should().BeOfType<ArgumentException>();
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeesAsync_WhenNoExceptionOccurs_CallsGenerateFeeBreakdown(
            [Frozen] ProducerRegistrationFeesRequestDto request)
        {
            // Arrange
            request.ProducerType = "Large";
            request.NumberOfSubsidiaries = 10;
            request.Regulator = "GB-ENG";

            _baseFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(262000m); // £2,620 represented in pence

            _subsidiariesFeeCalculationStrategyMock.Setup(strategy => strategy.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(558000m); // Total subsidiaries fee in pence

            _validatorMock.Setup(v => v.Validate(It.IsAny<ProducerRegistrationFeesRequestDto>()))
                .Returns(new ValidationResult());

            // Act
            var result = await _calculatorService!.CalculateFeesAsync(request, CancellationToken.None);

            // Assert
            _feeBreakdownGeneratorMock.Verify(g => g.GenerateFeeBreakdownAsync(result, request, CancellationToken.None), Times.Once);
        }
    }
}
