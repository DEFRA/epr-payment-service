using AutoFixture;
using AutoFixture.AutoMoq;
using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.Producer;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Controllers.RegistrationFees.Producer;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.Producer;
using EPR.Payment.Service.Services.Interfaces.RegistrationSubmission;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EPR.Payment.Service.UnitTests.Controllers.RegistrationFees.Producer
{
    [TestClass]
    public class ProducerFeesControllerTests
    {
        private IFixture _fixture = null!;
        private Mock<IProducerFeesCalculatorService> _producerFeesCalculatorServiceMock = null!;
        private Mock<IProducerFeeBySubmissionService> _feeBySubmissionServiceMock = null!;
        private Mock<IValidator<ProducerRegistrationFeesRequestDto>> _validatorMock = null!;
        private ProducerFeesController _controller = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _producerFeesCalculatorServiceMock = _fixture.Freeze<Mock<IProducerFeesCalculatorService>>();
            _feeBySubmissionServiceMock = _fixture.Freeze<Mock<IProducerFeeBySubmissionService>>();
            _validatorMock = _fixture.Freeze<Mock<IValidator<ProducerRegistrationFeesRequestDto>>>();
            _controller = new ProducerFeesController(
                _producerFeesCalculatorServiceMock.Object,
                _feeBySubmissionServiceMock.Object,
                _validatorMock.Object);
        }

        [TestMethod]
        [AutoMoqData]
        public void Constructor_WhenProducerFeesCalculatorServiceIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            IProducerFeesCalculatorService? producerFeesCalculatorService = null;

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(
                () => { var unused = new ProducerFeesController(producerFeesCalculatorService!, _feeBySubmissionServiceMock.Object, _validatorMock.Object); },
                "Value cannot be null. (Parameter 'producerFeesCalculatorService')");
        }

        [TestMethod]
        [AutoMoqData]
        public void Constructor_WhenFeeBySubmissionServiceIsNull_ThrowsArgumentNullException()
        {
            IProducerFeeBySubmissionService? feeBySubmissionService = null;

            Action act = () =>
            {
                var unused = new ProducerFeesController(_producerFeesCalculatorServiceMock.Object, feeBySubmissionService!, _validatorMock.Object);
            };

            act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'feeBySubmissionService')");
        }

        [TestMethod]
        [AutoMoqData]
        public void Constructor_WhenValidatorIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            IValidator<ProducerRegistrationFeesRequestDto>? validator = null;

            // Act
            Action act = () =>
            {
                var unused = new ProducerFeesController(_producerFeesCalculatorServiceMock.Object, _feeBySubmissionServiceMock.Object, validator!);
            };

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'validator')");
        }

        [TestMethod]
        public async Task GetFeesBySubmissionAsync_WhenServiceReturnsResponse_ReturnsOk()
        {
            var submissionId = Guid.NewGuid();
            var expected = new RegistrationFeesResponseDto
            {
                SubsidiariesFeeBreakdown = new SubsidiariesFeeBreakdown(),
            };
            _feeBySubmissionServiceMock
                .Setup(s => s.GetFeesAsync(submissionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            var result = await _controller.GetFeesBySubmissionAsync(submissionId, CancellationToken.None);

            using (new AssertionScope())
            {
                var ok = result.Result.Should().BeOfType<OkObjectResult>().Subject;
                ok.Value.Should().BeSameAs(expected);
            }
        }

        [TestMethod]
        public async Task GetFeesBySubmissionAsync_WhenServiceReturnsNull_ReturnsNotFound()
        {
            var submissionId = Guid.NewGuid();
            _feeBySubmissionServiceMock
                .Setup(s => s.GetFeesAsync(submissionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((RegistrationFeesResponseDto?)null);

            var result = await _controller.GetFeesBySubmissionAsync(submissionId, CancellationToken.None);

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        [AutoMoqData]
        public async Task CalculateFeesAsync_WhenValidRequest_ReturnsOkResultWithCalculatedFees(
            [Frozen] ProducerRegistrationFeesRequestDto request,
            [Frozen] RegistrationFeesResponseDto response)
        {
            // Arrange
            _validatorMock.Setup(v => v.Validate(It.IsAny<ProducerRegistrationFeesRequestDto>()))
                .Returns(new ValidationResult());

            _producerFeesCalculatorServiceMock.Setup(s => s.CalculateFeesAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
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
            [Frozen] ProducerRegistrationFeesRequestDto request)
        {
            // Arrange
            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("ProducerType", "ProducerType is invalid"),
                new ValidationFailure("Regulator", "Regulator is required")
            };

            _validatorMock.Setup(v => v.Validate(It.IsAny<ProducerRegistrationFeesRequestDto>()))
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
            [Frozen] ProducerRegistrationFeesRequestDto request)
        {
            // Arrange
            var exceptionMessage = "Validation failed";

            _validatorMock.Setup(v => v.Validate(It.IsAny<ProducerRegistrationFeesRequestDto>()))
                .Returns(new ValidationResult());

            _producerFeesCalculatorServiceMock.Setup(s => s.CalculateFeesAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
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
            [Frozen] ProducerRegistrationFeesRequestDto request)
        {
            // Arrange
            var exceptionMessage = "Invalid argument";

            _validatorMock.Setup(v => v.Validate(It.IsAny<ProducerRegistrationFeesRequestDto>()))
                .Returns(new ValidationResult());

            _producerFeesCalculatorServiceMock.Setup(s => s.CalculateFeesAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
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
              [Frozen] ProducerRegistrationFeesRequestDto request)
        {
            // Arrange
            var exceptionMessage = "exception";
            _producerFeesCalculatorServiceMock.Setup(i => i.CalculateFeesAsync(It.IsAny<ProducerRegistrationFeesRequestDto>(), It.IsAny<CancellationToken>()))
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