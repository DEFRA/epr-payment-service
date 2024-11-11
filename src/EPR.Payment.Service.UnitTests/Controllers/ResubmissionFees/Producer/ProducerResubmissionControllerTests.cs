using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Dtos.Request.ResubmissionFees.Producer;
using EPR.Payment.Service.Common.Dtos.Response.ResubmissionFees.Producer;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Controllers.ResubmissionFees.Producer;
using EPR.Payment.Service.Services.Interfaces.ResubmissionFees.Producer;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EPR.Payment.Service.UnitTests.Controllers.ResubmissionFees.Producer
{
    [TestClass]
    public class ProducerResubmissionControllerTests
    {
        private ProducerResubmissionController _controller = null!;
        private Mock<IProducerResubmissionService> _producerResubmissionServiceMock = null!;
        private Mock<IValidator<ProducerResubmissionFeeRequestDto>> _validatorMock = null!;
        private CancellationToken _cancellationToken;

        [TestInitialize]
        public void TestInitialize()
        {
            _producerResubmissionServiceMock = new Mock<IProducerResubmissionService>();
            _validatorMock = new Mock<IValidator<ProducerResubmissionFeeRequestDto>>();
            _controller = new ProducerResubmissionController(
                _producerResubmissionServiceMock.Object,
                _validatorMock.Object
            );
            _cancellationToken = new CancellationToken();
        }

        [TestMethod]
        public void Constructor_WithValidArguments_ShouldInitializeCorrectly()
        {
            // Act
            var controller = new ProducerResubmissionController(
                _producerResubmissionServiceMock.Object,
                _validatorMock.Object
            );

            // Assert
            using (new AssertionScope())
            {
                controller.Should().NotBeNull();
                controller.Should().BeAssignableTo<ProducerResubmissionController>();
            }
        }

        [TestMethod]
        public void Constructor_WithNullProducerResubmissionService_ShouldThrowArgumentNullException()
        {
            // Act
            Action act = () => new ProducerResubmissionController(
                null!,
                _validatorMock.Object
            );

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("producerResubmissionService");
        }

        [TestMethod]
        public void Constructor_WithNullValidator_ShouldThrowArgumentNullException()
        {
            // Act
            Action act = () => new ProducerResubmissionController(
                _producerResubmissionServiceMock.Object,
                null!
            );

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("validator");
        }

        [TestMethod, AutoMoqData]
        public async Task GetResubmissionAsync_ServiceReturnsAResult_ShouldReturnOkResponse(
            [Frozen] ProducerResubmissionFeeRequestDto request,
            [Frozen] ProducerResubmissionFeeResponseDto expectedResponse)
        {
            // Arrange
            _validatorMock.Setup(v => v.Validate(request)).Returns(new ValidationResult());
            _producerResubmissionServiceMock
                .Setup(i => i.GetResubmissionFeeAsync(request, _cancellationToken))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetResubmissionAsync(request, _cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeOfType<OkObjectResult>();
                result.As<OkObjectResult>().Value.Should().BeEquivalentTo(expectedResponse);
            }
        }

        [TestMethod, AutoMoqData]
        public async Task GetResubmissionAsync_ServiceThrowsException_ShouldReturnInternalServerError(
            [Frozen] ProducerResubmissionFeeRequestDto request)
        {
            // Arrange
            _validatorMock.Setup(v => v.Validate(request)).Returns(new ValidationResult());
            var exception = new Exception("Test Exception");
            _producerResubmissionServiceMock
                .Setup(i => i.GetResubmissionFeeAsync(request, _cancellationToken))
                .ThrowsAsync(exception);

            // Act
            var result = await _controller.GetResubmissionAsync(request, _cancellationToken);

            // Assert
            result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }

        [TestMethod, AutoMoqData]
        public async Task GetResubmissionAsync_ValidationFails_ShouldReturnBadRequest(
            [Frozen] ProducerResubmissionFeeRequestDto request)
        {
            // Arrange
            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("Regulator", "Invalid regulator parameter.")
            };
            _validatorMock.Setup(v => v.Validate(request)).Returns(new ValidationResult(validationFailures));

            // Act
            var result = await _controller.GetResubmissionAsync(request, _cancellationToken);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var problemDetails = result.As<BadRequestObjectResult>().Value as ProblemDetails;
            problemDetails.Should().NotBeNull();
            problemDetails?.Title.Should().Be("Validation Error");
            problemDetails?.Detail.Should().Contain("Invalid regulator parameter.");
        }

        [TestMethod, AutoMoqData]
        public async Task GetResubmissionAsync_ServiceThrowsValidationException_ShouldReturnBadRequest(
            [Frozen] ProducerResubmissionFeeRequestDto request)
        {
            // Arrange
            _validatorMock.Setup(v => v.Validate(request)).Returns(new ValidationResult());
            _producerResubmissionServiceMock
                .Setup(s => s.GetResubmissionFeeAsync(request, _cancellationToken))
                .ThrowsAsync(new ValidationException("Validation error"));

            // Act
            var result = await _controller.GetResubmissionAsync(request, _cancellationToken);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var problemDetails = result.As<BadRequestObjectResult>().Value as ProblemDetails;
            problemDetails.Should().NotBeNull();
            problemDetails?.Title.Should().Be("Validation Error");
            problemDetails?.Detail.Should().Contain("Validation error");
        }

        [TestMethod, AutoMoqData]
        public async Task GetResubmissionAsync_ServiceThrowsArgumentException_ShouldReturnBadRequest(
        [Frozen] ProducerResubmissionFeeRequestDto request)
        {
            // Arrange
            _validatorMock.Setup(v => v.Validate(request)).Returns(new ValidationResult());
            _producerResubmissionServiceMock
                .Setup(s => s.GetResubmissionFeeAsync(request, _cancellationToken))
                .ThrowsAsync(new ArgumentException("Invalid input parameter."));

            // Act
            var result = await _controller.GetResubmissionAsync(request, _cancellationToken);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result.As<BadRequestObjectResult>();
            badRequestResult.Value.Should().Be("Invalid input parameter.");
        }

    }
}