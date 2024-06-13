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
        private readonly PaymentsController _controller;
        private readonly Mock<IPaymentsService> _paymentsServiceMock;

        public PaymentsControllerTests()
        {
            _paymentsServiceMock = new Mock<IPaymentsService>();
            _controller = new PaymentsController(_paymentsServiceMock.Object);
        }

        [TestMethod]
        public async Task InsertPaymentStatus_ReturnOk()
        {
            // Arrange
            var paymentId = "123";
            var status = new PaymentStatusInsertRequestDto { Status = "Inserted" };

            //Act
            var result = await _controller.InsertPaymentStatus(paymentId, status);

            //Assert
            result.Should().BeOfType<OkResult>();
        }

        [TestMethod]
        public async Task InsertPaymentStatus_ServiceThrowsException_ReturnsInternalServerError()
        {
            // Arrange
            var paymentId = "123";
            var status = new PaymentStatusInsertRequestDto { };

            _paymentsServiceMock.Setup(service => service.InsertPaymentStatusAsync(paymentId, status))
                               .ThrowsAsync(new Exception("Test Exception"));

            // Act
            var result = await _controller.InsertPaymentStatus(paymentId, status);

            // Assert
            result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
    }
}