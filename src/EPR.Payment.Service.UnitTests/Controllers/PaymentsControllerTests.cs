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
            var request = new PaymentStatusInsertRequestDto
            {
                UserId = "88fb2f51-2f73-4b93-9894-8a39054cf6d2",
                OrganisationId = "88fb2f51-2f73-4b93-9894-8a39054cf6d2",
                ReferenceNumber = "123",
                Regulator = "Regulator",
                Amount = 20,
                ReasonForPayment = "Reason For Payment",
                Status = Common.Dtos.Enums.Status.Initiated
            };

            var expectedResult = new Guid();

            _paymentsServiceMock.Setup(service => service.InsertPaymentStatusAsync(request))
                .ReturnsAsync(expectedResult);

            //Act
            var result = await _controller.InsertPaymentStatus(request);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task InsertPaymentStatus_ServiceThrowsException_ReturnsInternalServerError()
        {
            // Arrange
            var request = new PaymentStatusInsertRequestDto();

            _paymentsServiceMock.Setup(service => service.InsertPaymentStatusAsync(request))
                               .ThrowsAsync(new Exception("Test Exception"));

            // Act
            var result = await _controller.InsertPaymentStatus(request);

            // Assert
            result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }

        [TestMethod]
        public async Task UpdatePaymentStatus_ReturnOk()
        {
            // Arrange
            var Id = new Guid();
            var request = new PaymentStatusUpdateRequestDto
            {
                GovPayPaymentId = "123",
                UpdatedByUserId = "88fb2f51-2f73-4b93-9894-8a39054cf6d2",
                UpdatedByOrganisationId = "88fb2f51-2f73-4b93-9894-8a39054cf6d2",
                ReferenceNumber = "12345",
                Status = Common.Dtos.Enums.Status.InProgress
            };

            //Act
            var result = await _controller.UpdatePaymentStatus(Id, request);

            //Assert
            result.Should().BeOfType<OkResult>();
        }

        public async Task UpdatePaymentStatus_ServiceThrowsException_ReturnsInternalServerError()
        {
            // Arrange
            var Id = new Guid();
            var request = new PaymentStatusUpdateRequestDto();

            _paymentsServiceMock.Setup(service => service.UpdatePaymentStatusAsync(Id, request))
                               .ThrowsAsync(new Exception("Test Exception"));

            // Act
            var result = await _controller.UpdatePaymentStatus(Id, request);

            // Assert
            result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
    }
}