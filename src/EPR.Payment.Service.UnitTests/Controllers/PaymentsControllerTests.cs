using AutoFixture;
using EPR.Payment.Service.Common.Dtos.Request;
using EPR.Payment.Service.Controllers;
using EPR.Payment.Service.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EPR.Payment.Service.UnitTests.Controllers
{
    [TestClass]
    public class PaymentsControllerTests
    {
        private readonly IFixture _fixture;
        private readonly PaymentsController _controller;
        private readonly Mock<IPaymentsService> _paymentsServiceMock;

        public PaymentsControllerTests()
        {
            _fixture = new Fixture();
            _paymentsServiceMock = new Mock<IPaymentsService>();
            _controller = new PaymentsController(_paymentsServiceMock.Object);
        }

        [TestMethod]
        public async Task InsertPaymentStatus_ValidInput_ShouldReturnOkWithGuid()
        {
            // Arrange
            var request = _fixture.Build<PaymentStatusInsertRequestDto>().Create();

            var expectedResult = new Guid();

            _paymentsServiceMock.Setup(service => service.InsertPaymentStatusAsync(request))
                .ReturnsAsync(expectedResult);

            //Act
            var result = await _controller.InsertPaymentStatus(request);

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
            var result = await _controller.InsertPaymentStatus(request);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task InsertPaymentStatus_ServiceThrowsException_ShouldReturnInternalServerError()
        {
            // Arrange
            var request = new PaymentStatusInsertRequestDto();

            _paymentsServiceMock.Setup(service => service.InsertPaymentStatusAsync(request))
                               .ThrowsAsync(new Exception("Test Exception"));

            // Act
            var result = await _controller.InsertPaymentStatus(request);

            // Assert
            result.Result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }

        [TestMethod]
        public async Task InsertPaymentStatus_ArgumentExceptionThrow_ShouldReturnBadRequest()
        {
            // Arrange
            var request = new PaymentStatusInsertRequestDto();

            _paymentsServiceMock.Setup(service => service.InsertPaymentStatusAsync(request))
                               .ThrowsAsync(new ArgumentException("Test Exception"));

            // Act
            var result = await _controller.InsertPaymentStatus(request);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task UpdatePaymentStatus_ValidInput_ShouldReturnNoContentResult()
        {
            // Arrange
            var Id = new Guid();
            var request = _fixture.Build<PaymentStatusUpdateRequestDto>().Create();

            //Act
            var result = await _controller.UpdatePaymentStatus(Id, request);

            //Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [TestMethod]
        public async Task UpdatePaymentStatus_InvalidInput_ShouldReturnBadRequest()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new PaymentStatusUpdateRequestDto();
            _controller!.ModelState.AddModelError("Regulator", "Regulator is required");

            // Act
            var result = await _controller.UpdatePaymentStatus(id, request);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task UpdatePaymentStatus_ArgumentExceptionThrow_ShouldReturnBadRequest()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new PaymentStatusUpdateRequestDto();

            _paymentsServiceMock.Setup(service => service.UpdatePaymentStatusAsync(id, request))
                               .ThrowsAsync(new ArgumentException("Test Exception"));

            // Act
            var result = await _controller.UpdatePaymentStatus(id, request);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task UpdatePaymentStatus_ServiceThrowsException_ShouldReturnInternalServerError()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new PaymentStatusUpdateRequestDto();

            _paymentsServiceMock.Setup(service => service.UpdatePaymentStatusAsync(id, request))
                               .ThrowsAsync(new Exception("Test Exception"));

            // Act
            var result = await _controller.UpdatePaymentStatus(id, request);

            // Assert
            result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
    }
}