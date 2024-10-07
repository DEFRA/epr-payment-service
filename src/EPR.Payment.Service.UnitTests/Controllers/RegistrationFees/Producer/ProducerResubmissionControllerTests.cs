using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Dtos.Request.Payments;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Controllers.RegistrationFees.Producer;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.Producer;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EPR.Payment.Service.UnitTests.Controllers.RegistrationFees.Producer
{
    [TestClass]
    public class ProducerResubmissionControllerTests
    {
        private ProducerResubmissionController _controller = null!;
        private Mock<IProducerResubmissionService> _registrationFeesServiceMock = null!;
        private CancellationToken _cancellationToken;

        [TestInitialize]
        public void TestInitialize()
        {
            _registrationFeesServiceMock = new Mock<IProducerResubmissionService>();
            _controller = new ProducerResubmissionController(_registrationFeesServiceMock.Object);
            _cancellationToken = new CancellationToken();
        }

        [TestMethod]
        public void Constructor_WithValidArguments_ShouldInitializeCorrectly()
        {
            // Act
            var controller = new ProducerResubmissionController(
                _registrationFeesServiceMock.Object
            );

            // Assert
            using (new AssertionScope())
            {
                controller.Should().NotBeNull();
                controller.Should().BeAssignableTo<ProducerResubmissionController>();
            }
        }

        [TestMethod]
        public void Constructor_WithNullRegistrationFeesService_ShouldThrowArgumentNullException()
        {

            // Act
            Action act = () => new ProducerResubmissionController(
                null!
            );

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("producerResubmissionService");
        }

        [TestMethod, AutoMoqData]
        public async Task GetResubmissionAsync_ServiceReturnsAResult_ShouldReturnOkResponse(
            [Frozen] RegulatorDto producerResubmissionFeeRequestDto,
            [Frozen] decimal expectedAmount)
        {
            //Arrange
            _registrationFeesServiceMock.Setup(i => i.GetResubmissionAsync(producerResubmissionFeeRequestDto, _cancellationToken)).ReturnsAsync(expectedAmount);

            //Act
            var result = await _controller.GetResubmissionAsync(producerResubmissionFeeRequestDto, _cancellationToken);

            //Assert
            using (new AssertionScope())
            {
                result.Should().BeOfType<OkObjectResult>();
                result.As<OkObjectResult>().Should().NotBeNull();
            }
        }

        [TestMethod, AutoMoqData]
        public async Task GetResubmissionAsync_ServiceThrowsExceptionWithInnerException_ShouldReturnInternalServerError(
            [Frozen] RegulatorDto producerResubmissionFeeRequestDto)
        {
            // Arrange
            var innerExceptionMessage = "Inner exception message";
            var ex = new Exception("Outer exception", new Exception(innerExceptionMessage));
            _registrationFeesServiceMock.Setup(i => i.GetResubmissionAsync(producerResubmissionFeeRequestDto, _cancellationToken))
                               .ThrowsAsync(ex);

            // Act
            var result = await _controller.GetResubmissionAsync(producerResubmissionFeeRequestDto, _cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            }

        }

        [TestMethod, AutoMoqData]
        public async Task GetResubmissionAsync_ServiceThrowsExceptionWithoutInnerException_ShouldReturnInternalServerError(
            [Frozen] RegulatorDto producerResubmissionFeeRequestDto)
        {
            // Arrange
            var ex = new Exception("Outer exception");
            _registrationFeesServiceMock.Setup(i => i.GetResubmissionAsync(producerResubmissionFeeRequestDto, _cancellationToken))
                               .ThrowsAsync(ex);

            // Act
            var result = await _controller.GetResubmissionAsync(producerResubmissionFeeRequestDto, _cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            }

        }

        [TestMethod]
        public async Task GetResubmissionAsync_InValidRequest_ShouldReturnBadRequest()
        {
            // Arrange
            var request = new RegulatorDto();
            _controller!.ModelState.AddModelError("Regulator", "Regulator is required");

            // Act
            var result = await _controller.GetResubmissionAsync(request, _cancellationToken);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task GetResubmissionAsync_ArgumentExceptionThrow_ShouldReturnBadRequest()
        {
            // Arrange
            var request = new RegulatorDto();

            _registrationFeesServiceMock.Setup(service => service.GetResubmissionAsync(request, _cancellationToken))
                               .ThrowsAsync(new ArgumentException("Test Exception"));

            // Act
            var result = await _controller.GetResubmissionAsync(request, _cancellationToken);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod, AutoMoqData]
        public async Task GetResubmissionAsync_ThrowsValidationException_ShouldReturnBadRequest([Frozen] RegulatorDto request)
        {
            // Arrange
            _registrationFeesServiceMock.Setup(s => s.GetResubmissionAsync(It.IsAny<RegulatorDto>(), _cancellationToken)).ThrowsAsync(new ValidationException("Validation error"));

            // Act
            var result = await _controller.GetResubmissionAsync(request, _cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeOfType<BadRequestObjectResult>();

                var badRequestResult = result as BadRequestObjectResult;
                badRequestResult.Should().NotBeNull();
            }
        }
    }
}
