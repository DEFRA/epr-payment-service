using AutoFixture;
using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Dtos.Request;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Controllers;
using EPR.Payment.Service.Services.Interfaces;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EPR.Payment.Service.UnitTests.Controllers
{
    [TestClass]
    public class PaymentsControllerTests
    {
        private IFixture _fixture = null!;
        private PaymentsController _controller = null!;
        private Mock<IPaymentsService> _paymentsServiceMock = null!;
        private CancellationToken _cancellationToken;

        [TestInitialize]
        public void TestInitialize()
        {
            _fixture = new Fixture();
            _paymentsServiceMock = new Mock<IPaymentsService>();
            _controller = new PaymentsController(_paymentsServiceMock.Object);
            _cancellationToken = new CancellationToken();
        }

        [TestMethod]
        [AutoMoqData]
        public async Task InsertPaymentStatus_ValidInput_ShouldReturnOkWithGuid([Frozen] Guid expectedResult)
        {
            // Arrange
            var request = _fixture.Build<PaymentStatusInsertRequestDto>().Create();

            _paymentsServiceMock.Setup(service => service.InsertPaymentStatusAsync(request, _cancellationToken))
                .ReturnsAsync(expectedResult);

            //Act
            var result = await _controller.InsertPaymentStatus(request, _cancellationToken);

            //Assert
            result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(expectedResult);
        }

        [TestMethod]
        public async Task InsertPaymentStatus_InValidInput_ShouldReturnBadRequest()
        {
            // Arrange
            var request = new PaymentStatusInsertRequestDto();
            _controller!.ModelState.AddModelError("Regulator", "Regulator is required");

            // Act
            var result = await _controller.InsertPaymentStatus(request, _cancellationToken);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task InsertPaymentStatus_ServiceThrowsException_ShouldReturnInternalServerError()
        {
            // Arrange
            var request = new PaymentStatusInsertRequestDto();

            _paymentsServiceMock.Setup(service => service.InsertPaymentStatusAsync(request, _cancellationToken))
                               .ThrowsAsync(new Exception("Test Exception"));

            // Act
            var result = await _controller.InsertPaymentStatus(request, _cancellationToken);

            // Assert
            result.Result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }

        [TestMethod]
        public async Task InsertPaymentStatus_ArgumentExceptionThrow_ShouldReturnBadRequest()
        {
            // Arrange
            var request = new PaymentStatusInsertRequestDto();

            _paymentsServiceMock.Setup(service => service.InsertPaymentStatusAsync(request, _cancellationToken))
                               .ThrowsAsync(new ArgumentException("Test Exception"));

            // Act
            var result = await _controller.InsertPaymentStatus(request, _cancellationToken);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task InsertPaymentStatus_ThrowsValidationException_ShouldReturnBadRequest()
        {
            // Arrange

            var request = _fixture.Build<PaymentStatusInsertRequestDto>().Create();

            var validationException = new ValidationException("Validation error");
            _paymentsServiceMock.Setup(s => s.InsertPaymentStatusAsync(It.IsAny<PaymentStatusInsertRequestDto>(), _cancellationToken)).ThrowsAsync(validationException); 

            // Act
            var result = await _controller.InsertPaymentStatus(request, _cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                result.Result.Should().BeOfType<BadRequestObjectResult>();

                var badRequestResult = result.Result as BadRequestObjectResult;
                badRequestResult.Should().NotBeNull();
            }
        }

        [TestMethod]
        [AutoMoqData]
        public async Task UpdatePaymentStatus_ValidInput_ShouldReturnNoContentResult([Frozen] Guid id)
        {
            // Arrange
            var request = _fixture.Build<PaymentStatusUpdateRequestDto>().Create();

            //Act
            var result = await _controller.UpdatePaymentStatus(id, request, _cancellationToken);

            //Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [TestMethod]
        [AutoMoqData]
        public async Task UpdatePaymentStatus_InvalidInput_ShouldReturnBadRequest([Frozen] Guid id)
        {
            // Arrange
            var request = new PaymentStatusUpdateRequestDto();
            _controller!.ModelState.AddModelError("Regulator", "Regulator is required");

            // Act
            var result = await _controller.UpdatePaymentStatus(id, request, _cancellationToken);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        [AutoMoqData]
        public async Task UpdatePaymentStatus_ArgumentExceptionThrow_ShouldReturnBadRequest([Frozen] Guid id)
        {
            // Arrange
            var request = new PaymentStatusUpdateRequestDto();

            _paymentsServiceMock.Setup(service => service.UpdatePaymentStatusAsync(id, request, _cancellationToken))
                               .ThrowsAsync(new ArgumentException("Test Exception"));

            // Act
            var result = await _controller.UpdatePaymentStatus(id, request, _cancellationToken);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        [AutoMoqData]
        public async Task UpdatePaymentStatus_ServiceThrowsException_ShouldReturnInternalServerError([Frozen] Guid id)
        {
            // Arrange
            var request = new PaymentStatusUpdateRequestDto();

            _paymentsServiceMock.Setup(service => service.UpdatePaymentStatusAsync(id, request, _cancellationToken))
                               .ThrowsAsync(new Exception("Test Exception"));

            // Act
            var result = await _controller.UpdatePaymentStatus(id, request, _cancellationToken);

            // Assert
            result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }

        [TestMethod]
        [AutoMoqData]
        public async Task UpdatePaymentStatus_ThrowsValidationException_ShouldReturnBadRequest([Frozen] Guid id)
        {
            // Arrange
            var request = new PaymentStatusUpdateRequestDto();

            var validationException = new ValidationException("Validation error");
            _paymentsServiceMock.Setup(s => s.UpdatePaymentStatusAsync(id, request, _cancellationToken)).ThrowsAsync(validationException);


            // Act
            var result = await _controller.UpdatePaymentStatus(id, request, _cancellationToken);

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