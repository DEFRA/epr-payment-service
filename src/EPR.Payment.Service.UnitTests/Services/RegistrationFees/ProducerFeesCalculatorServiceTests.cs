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
        private Mock<IFeeCalculationStrategy<ProducerRegistrationFeesRequestDto>> _baseFeeCalculationStrategyMock = null!;
        private Mock<IFeeCalculationStrategy<ProducerRegistrationFeesRequestDto>> _subsidiariesFeeCalculationStrategyMock = null!;
        private Mock<IValidator<ProducerRegistrationFeesRequestDto>> _validatorMock = null!;
        private Mock<IFeeBreakdownGenerator<ProducerRegistrationFeesRequestDto, RegistrationFeesResponseDto>> _feeBreakdownGeneratorMock = null!;
        private IProducerFeesCalculatorService _calculatorService = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _baseFeeCalculationStrategyMock = new Mock<IFeeCalculationStrategy<ProducerRegistrationFeesRequestDto>>();
            _subsidiariesFeeCalculationStrategyMock = new Mock<IFeeCalculationStrategy<ProducerRegistrationFeesRequestDto>>();
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
        public void Constructor_ShouldThrowArgumentNullException_WhenBaseFeeCalculationStrategyIsNull()
        {
            // Arrange
            IFeeCalculationStrategy<ProducerRegistrationFeesRequestDto>? baseFeeCalculationStrategy = null;

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
        public void Constructor_ShouldThrowArgumentNullException_WhenSubsidiariesFeeCalculationStrategyIsNull()
        {
            // Arrange
            IFeeCalculationStrategy<ProducerRegistrationFeesRequestDto>? subsidiariesFeeCalculationStrategy = null;

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
        public void Constructor_ShouldThrowArgumentNullException_WhenValidatorIsNull()
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
        public void Constructor_ShouldThrowArgumentNullException_WhenFeeBreakdownGeneratorIsNull()
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
        public void Constructor_ShouldInitializeProducerFeesCalculatorService_WhenAllDependenciesAreNotNull()
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

        [TestMethod]
        [AutoMoqData]
        public async Task CalculateFeesAsync_LargeProducerWith50Subsidiaries_ReturnsCorrectFees(
            [Frozen] ProducerRegistrationFeesRequestDto request)
        {
            // Arrange
            request.ProducerType = "Large";
            request.NumberOfSubsidiaries = 50;
            request.Regulator = "GB-ENG";

            _baseFeeCalculationStrategyMock.Setup(s => s.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(262000m); // £2,620 represented in pence

            _subsidiariesFeeCalculationStrategyMock.Setup(s => s.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1536000m); // Total subsidiaries fee for 50 subsidiaries represented in pence

            _validatorMock.Setup(v => v.Validate(It.IsAny<ProducerRegistrationFeesRequestDto>()))
                .Returns(new ValidationResult());

            // Act
            var result = await _calculatorService.CalculateFeesAsync(request, CancellationToken.None);

            // Assert
            _feeBreakdownGeneratorMock.Verify(g => g.GenerateFeeBreakdownAsync(result, request, CancellationToken.None), Times.Once);

            using (new AssertionScope())
            {
                result.BaseFee.Should().Be(262000m); // £2,620 represented in pence
                result.SubsidiariesFee.Should().Be(1536000m); // Total subsidiaries fee in pence
                result.TotalFee.Should().Be(1798000m); // Total fee in pence
            }
        }

        [TestMethod]
        [AutoMoqData]
        public async Task CalculateFeesAsync_LargeProducerWith10Subsidiaries_ReturnsCorrectFees(
            [Frozen] ProducerRegistrationFeesRequestDto request)
        {
            // Arrange
            request.ProducerType = "Large";
            request.NumberOfSubsidiaries = 10;
            request.Regulator = "GB-ENG";

            _baseFeeCalculationStrategyMock.Setup(s => s.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(262000m); // £2,620 represented in pence

            _subsidiariesFeeCalculationStrategyMock.Setup(s => s.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(558000m); // 10 subsidiaries at £558 each represented in pence

            _validatorMock.Setup(v => v.Validate(It.IsAny<ProducerRegistrationFeesRequestDto>()))
                .Returns(new ValidationResult());

            // Act
            var result = await _calculatorService.CalculateFeesAsync(request, CancellationToken.None);

            // Assert
            _feeBreakdownGeneratorMock.Verify(g => g.GenerateFeeBreakdownAsync(result, request, CancellationToken.None), Times.Once);

            using (new AssertionScope())
            {
                result.BaseFee.Should().Be(262000m); // £2,620 represented in pence
                result.SubsidiariesFee.Should().Be(558000m); // Total subsidiaries fee in pence for 10 subsidiaries
                result.TotalFee.Should().Be(820000m); // Total fee in pence
            }
        }

        [TestMethod]
        [AutoMoqData]
        public async Task CalculateFeesAsync_LargeProducerWithNoBaseFeeAnd50Subsidiaries_ReturnsCorrectFees(
            [Frozen] ProducerRegistrationFeesRequestDto request)
        {
            // Arrange
            request.ProducerType = ""; // ProducerType is empty to indicate no base fee
            request.NumberOfSubsidiaries = 50;
            request.Regulator = "GB-ENG";

            _baseFeeCalculationStrategyMock.Setup(s => s.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(0m); // No base fee for any ProducerType

            _subsidiariesFeeCalculationStrategyMock.Setup(s => s.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1536000m); // Total subsidiaries fee for 50 subsidiaries represented in pence

            _validatorMock.Setup(v => v.Validate(It.IsAny<ProducerRegistrationFeesRequestDto>()))
                .Returns(new ValidationResult());

            // Act
            var result = await _calculatorService.CalculateFeesAsync(request, CancellationToken.None);

            // Assert
            _feeBreakdownGeneratorMock.Verify(g => g.GenerateFeeBreakdownAsync(result, request, CancellationToken.None), Times.Once);

            using (new AssertionScope())
            {
                result.BaseFee.Should().Be(0m); // No base fee
                result.SubsidiariesFee.Should().Be(1536000m); // Total subsidiaries fee in pence
                result.TotalFee.Should().Be(1536000m); // Total fee in pence (since no base fee)
            }
        }

        [TestMethod]
        [AutoMoqData]
        public async Task CalculateFeesAsync_SmallProducerWith25Subsidiaries_ReturnsCorrectFees(
            [Frozen] ProducerRegistrationFeesRequestDto request)
        {
            // Arrange
            request.ProducerType = "Small";
            request.NumberOfSubsidiaries = 25;
            request.Regulator = "GB-ENG";

            _baseFeeCalculationStrategyMock.Setup(s => s.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(121600m); // £1,216 represented in pence

            _subsidiariesFeeCalculationStrategyMock.Setup(s => s.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1186000m); // Total subsidiaries fee for 25 subsidiaries represented in pence

            _validatorMock.Setup(v => v.Validate(It.IsAny<ProducerRegistrationFeesRequestDto>()))
                .Returns(new ValidationResult());

            // Act
            var result = await _calculatorService.CalculateFeesAsync(request, CancellationToken.None);

            // Assert
            _feeBreakdownGeneratorMock.Verify(g => g.GenerateFeeBreakdownAsync(result, request, CancellationToken.None), Times.Once);

            using (new AssertionScope())
            {
                result.BaseFee.Should().Be(121600m); // £1,216 represented in pence
                result.SubsidiariesFee.Should().Be(1186000m); // Total subsidiaries fee in pence
                result.TotalFee.Should().Be(1307600m); // Total fee in pence
            }
        }

        [TestMethod]
        [AutoMoqData]
        public async Task CalculateFeesAsync_SmallProducerWith20Subsidiaries_ReturnsCorrectFees(
            [Frozen] ProducerRegistrationFeesRequestDto request)
        {
            // Arrange
            request.ProducerType = "Small";
            request.NumberOfSubsidiaries = 20;
            request.Regulator = "GB-ENG";

            _baseFeeCalculationStrategyMock.Setup(s => s.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(121600m); // £1,216 represented in pence

            _subsidiariesFeeCalculationStrategyMock.Setup(s => s.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(1116000m); // 20 subsidiaries at £558 each represented in pence

            _validatorMock.Setup(v => v.Validate(It.IsAny<ProducerRegistrationFeesRequestDto>()))
                .Returns(new ValidationResult());

            // Act
            var result = await _calculatorService.CalculateFeesAsync(request, CancellationToken.None);

            // Assert
            _feeBreakdownGeneratorMock.Verify(g => g.GenerateFeeBreakdownAsync(result, request, CancellationToken.None), Times.Once);

            using (new AssertionScope())
            {
                result.BaseFee.Should().Be(121600m); // £1,216 represented in pence
                result.SubsidiariesFee.Should().Be(1116000m); // Total subsidiaries fee in pence for 20 subsidiaries
                result.TotalFee.Should().Be(1237600m); // Total fee in pence
            }
        }

        [TestMethod]
        [AutoMoqData]
        public async Task CalculateFeesAsync_LargeProducerWithNoSubsidiaries_ReturnsBaseFeeOnly(
            [Frozen] ProducerRegistrationFeesRequestDto request)
        {
            // Arrange
            request.ProducerType = "Large";
            request.NumberOfSubsidiaries = 0;
            request.Regulator = "GB-ENG";

            _baseFeeCalculationStrategyMock.Setup(s => s.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(262000m); // £2,620 represented in pence

            _subsidiariesFeeCalculationStrategyMock.Setup(s => s.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(0m); // No subsidiaries fee

            _validatorMock.Setup(v => v.Validate(It.IsAny<ProducerRegistrationFeesRequestDto>()))
                .Returns(new ValidationResult());

            // Act
            var result = await _calculatorService.CalculateFeesAsync(request, CancellationToken.None);

            // Assert
            _feeBreakdownGeneratorMock.Verify(g => g.GenerateFeeBreakdownAsync(result, request, CancellationToken.None), Times.Once);

            using (new AssertionScope())
            {
                result.BaseFee.Should().Be(262000m); // £2,620 represented in pence
                result.SubsidiariesFee.Should().Be(0m); // No subsidiaries fee
                result.TotalFee.Should().Be(262000m); // Total fee in pence (base fee only)
            }
        }

        [TestMethod]
        [AutoMoqData]
        public async Task CalculateFeesAsync_InvalidRequest_ThrowsValidationException(
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
                await _calculatorService.CalculateFeesAsync(request, CancellationToken.None);
            });
        }

        [TestMethod]
        [AutoMoqData]
        public async Task CalculateFeesAsync_ThrowsInvalidOperationException_WhenArgumentExceptionOccurs(
            [Frozen] ProducerRegistrationFeesRequestDto request)
        {
            // Arrange
            _baseFeeCalculationStrategyMock.Setup(s => s.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ArgumentException("Invalid argument"));

            _validatorMock.Setup(v => v.Validate(It.IsAny<ProducerRegistrationFeesRequestDto>()))
                .Returns(new ValidationResult());

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () =>
            {
                await _calculatorService.CalculateFeesAsync(request, CancellationToken.None);
            });

            exception.Message.Should().Be("An error occurred while calculating fees.");
            exception.InnerException.Should().BeOfType<ArgumentException>();
        }

        [TestMethod]
        [AutoMoqData]
        public async Task CalculateFeesAsync_CallsGenerateFeeBreakdown_WhenNoExceptionOccurs(
            [Frozen] ProducerRegistrationFeesRequestDto request)
        {
            // Arrange
            request.ProducerType = "Large";
            request.NumberOfSubsidiaries = 10;
            request.Regulator = "GB-ENG";

            _baseFeeCalculationStrategyMock.Setup(s => s.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(262000m); // £2,620 represented in pence

            _subsidiariesFeeCalculationStrategyMock.Setup(s => s.CalculateFeeAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(558000m); // 10 subsidiaries at £558 each represented in pence

            _validatorMock.Setup(v => v.Validate(It.IsAny<ProducerRegistrationFeesRequestDto>()))
                .Returns(new ValidationResult());

            // Act
            var result = await _calculatorService.CalculateFeesAsync(request, CancellationToken.None);

            // Assert
            _feeBreakdownGeneratorMock.Verify(g => g.GenerateFeeBreakdownAsync(result, request, CancellationToken.None), Times.Once);
        }
    }
}