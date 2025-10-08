using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.Producer;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Controllers.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Controllers.RegistrationFees.Producer;
using EPR.Payment.Service.Services.Interfaces.FeeItems;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.Producer;
using EPR.Payment.Service.Strategies.Interfaces.FeeItems;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EPR.Payment.Service.UnitTests.Controllers.RegistrationFees.Producer
{
    [TestClass]
    public class ProducerFeesControllerV2Tests
    {
        private IFixture _fixture = null!;
        private Mock<IProducerFeesCalculatorService> _producerFeesCalculatorServiceMock = null!;
        private Mock<IValidator<ProducerRegistrationFeesRequestDto>> _validatorMock = null!;
        private ProducerFeesController _controller = null!;
        private Mock<IFeeItemWriter> _feeSummaryWriterMock = null!;
        private Mock<IFeeItemProducerSaveRequestMapper> _mapperMock = null!;
        private Mock<IValidator<ProducerRegistrationFeesRequestV2Dto>> _validatorV2Mock = null!;
        [TestInitialize]
        public void TestInitialize()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _producerFeesCalculatorServiceMock = _fixture.Freeze<Mock<IProducerFeesCalculatorService>>();
            _validatorMock = _fixture.Freeze<Mock<IValidator<ProducerRegistrationFeesRequestDto>>>();
            _feeSummaryWriterMock = _fixture.Freeze<Mock<IFeeItemWriter>>();
            _mapperMock = _fixture.Freeze<Mock<IFeeItemProducerSaveRequestMapper>>();
            _validatorV2Mock = _fixture.Freeze<Mock<IValidator<ProducerRegistrationFeesRequestV2Dto>>>();
            _controller = new ProducerFeesController(
                    _producerFeesCalculatorServiceMock.Object,
                    _validatorMock.Object,
                    _feeSummaryWriterMock.Object,
                    _mapperMock.Object,
                    _validatorV2Mock.Object);
        }

        [TestMethod]
        public void Constructor_WithValidArguments_ShouldInitialize()
        {
            // Act
            var controller = new ProducerFeesController(
                _producerFeesCalculatorServiceMock.Object,
                _validatorMock.Object,
                _feeSummaryWriterMock.Object,
                _mapperMock.Object,
                _validatorV2Mock.Object);

            // Assert
            using (new AssertionScope())
            {
                controller.Should().NotBeNull();
                controller.Should().BeAssignableTo<ProducerFeesController>();
            }
        }

        [TestMethod]
        [AutoMoqData]
        public void Constructor_WhenProducerFeesCalculatorServiceIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            IProducerFeesCalculatorService? producerFeesCalculatorService = null;

            // Act
            Action act = () => new ProducerFeesController(
                producerFeesCalculatorService!,
                    _validatorMock.Object,
                    _feeSummaryWriterMock.Object,
                    _mapperMock.Object,
                    _validatorV2Mock.Object);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'producerFeesCalculatorService')");
        }

        [TestMethod]
        [AutoMoqData]
        public void Constructor_WhenValidatorIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            IValidator<ProducerRegistrationFeesRequestV2Dto>? _validator2 = null;

            // Act
            Action act = () => new ProducerFeesController(
                _producerFeesCalculatorServiceMock.Object,
                _validatorMock.Object,
                _feeSummaryWriterMock.Object,
                _mapperMock.Object,
                _validator2);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter '_validator2')");
        }

        [TestMethod]
        [AutoMoqData]
        public async Task CalculateFeesAsync_WhenValidRequest_ReturnsOkResultWithCalculatedFees(
            [Frozen] ProducerRegistrationFeesRequestV2Dto request,
            [Frozen] RegistrationFeesResponseDto response)
        {
            // Arrange
            _validatorV2Mock.Setup(v => v.Validate(It.IsAny<ProducerRegistrationFeesRequestV2Dto>()))
                .Returns(new ValidationResult());

            _producerFeesCalculatorServiceMock.Setup(s => s.CalculateFeesAsync(It.IsAny<ProducerRegistrationFeesRequestV2Dto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.CalculateFeesAsync(request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                result.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(response);
            }
        }

        [TestMethod]
        [AutoMoqData]
        public async Task CalculateFeesAsync_WhenRequestValidationFails_ReturnsBadRequestWithValidationErrorDetails(
            [Frozen] ProducerRegistrationFeesRequestV2Dto request)
        {
            // Arrange
            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("ProducerType", "ProducerType is invalid"),
                new ValidationFailure("Regulator", "Regulator is required")
            };

            _validatorV2Mock.Setup(v => v.Validate(It.IsAny<ProducerRegistrationFeesRequestV2Dto>()))
                .Returns(new ValidationResult(validationFailures));

            // Act
            var result = await _controller.CalculateFeesAsync(request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Which;
                var problemDetails = badRequestResult.Value.Should().BeOfType<ProblemDetails>().Which;
                problemDetails.Detail.Should().Be("ProducerType is invalid; Regulator is required");
            }
        }

        [TestMethod]
        [AutoMoqData]
        public async Task CalculateFeesAsync_WhenCalculationThrowsValidationException_ReturnsBadRequestWithValidationExceptionDetails(
            [Frozen] ProducerRegistrationFeesRequestV2Dto request)
        {
            // Arrange
            var exceptionMessage = "Validation failed";

            _validatorV2Mock.Setup(v => v.Validate(It.IsAny<ProducerRegistrationFeesRequestV2Dto>()))
                .Returns(new ValidationResult());

            _producerFeesCalculatorServiceMock.Setup(s => s.CalculateFeesAsync(It.IsAny<ProducerRegistrationFeesRequestV2Dto>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ValidationException(exceptionMessage));

            // Act
            var result = await _controller.CalculateFeesAsync(request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Which;
                var problemDetails = badRequestResult.Value.Should().BeOfType<ProblemDetails>().Which;
                problemDetails.Detail.Should().Be(exceptionMessage);
            }
        }

        [TestMethod]
        [AutoMoqData]
        public async Task CalculateFeesAsync_WhenCalculationThrowsArgumentException_ReturnsBadRequestWithArgumentExceptionDetails(
            [Frozen] ProducerRegistrationFeesRequestV2Dto request)
        {
            // Arrange
            var exceptionMessage = "Invalid argument";

            _validatorV2Mock.Setup(v => v.Validate(It.IsAny<ProducerRegistrationFeesRequestV2Dto>()))
                .Returns(new ValidationResult());

            _producerFeesCalculatorServiceMock.Setup(s => s.CalculateFeesAsync(It.IsAny<ProducerRegistrationFeesRequestV2Dto>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ArgumentException(exceptionMessage));

            // Act
            var result = await _controller.CalculateFeesAsync(request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Which;
                badRequestResult.Value.Should().Be(exceptionMessage);
            }
        }


        [TestMethod]
        [AutoMoqData]
        public async Task CalculateFeesAsync_WhenCalculationThrowsException_ShouldReturnInternalServerError(
              [Frozen] ProducerRegistrationFeesRequestV2Dto request)
        {
            // Arrange
            var exceptionMessage = "exception";
            _producerFeesCalculatorServiceMock.Setup(i => i.CalculateFeesAsync(It.IsAny<ProducerRegistrationFeesRequestV2Dto>(), It.IsAny<CancellationToken>()))
                               .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _controller.CalculateFeesAsync(request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.Result.Should().BeOfType<ObjectResult>().Which.Value.Should().Be($"{ProducerFeesCalculationExceptions.FeeCalculationError}: {exceptionMessage}");
            }

        }
    }
}