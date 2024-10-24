using AutoFixture;
using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Dtos.Request.Payments;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Controllers;
using EPR.Payment.Service.Services.Interfaces.Payments;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EPR.Payment.Service.UnitTests.Controllers.Payments
{
    [TestClass]
    public class OfflinePaymentsControllerTests
    {
        private Fixture _fixture = null!;
        private OfflinePaymentsController _controller = null!;
        private Mock<IOfflinePaymentsService> _offlinePaymentsServiceMock = null!;
        private CancellationToken _cancellationToken;

        [TestInitialize]
        public void TestInitialize()
        {
            _fixture = new Fixture();
            _offlinePaymentsServiceMock = new Mock<IOfflinePaymentsService>();
            _controller = new OfflinePaymentsController(_offlinePaymentsServiceMock.Object);
            _cancellationToken = new CancellationToken();
        }

        [TestMethod, AutoMoqData]
        public async Task InsertOfflinePaymentStatus_ValidInput_ShouldReturnOk([Frozen] Guid expectedResult)
        {
            // Arrange
            var request = _fixture.Build<OfflinePaymentStatusInsertRequestDto>().Create();

            _offlinePaymentsServiceMock.Setup(service => service.InsertOfflinePaymentAsync(request, _cancellationToken))
                .ReturnsAsync(expectedResult);

            //Act
            var result = await _controller.InsertOfflinePaymentStatus(request, _cancellationToken);

            //Assert
            result.Result.Should().BeOfType<NoContentResult>();
        }

        [TestMethod]
        public async Task InsertOfflinePaymentStatus_InValidInput_ShouldReturnBadRequest()
        {
            // Arrange
            var request = new OfflinePaymentStatusInsertRequestDto();
            _controller!.ModelState.AddModelError("Reference", string.Empty);

            // Act
            var result = await _controller.InsertOfflinePaymentStatus(request, _cancellationToken);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task InsertOfflinePaymentStatus_ServiceThrowsException_ShouldReturnInternalServerError()
        {
            // Arrange
            var request = new OfflinePaymentStatusInsertRequestDto();

            _offlinePaymentsServiceMock.Setup(service => service.InsertOfflinePaymentAsync(request, _cancellationToken))
                               .ThrowsAsync(new Exception("Test Exception"));

            // Act
            var result = await _controller.InsertOfflinePaymentStatus(request, _cancellationToken);

            // Assert
            result.Result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }

        [TestMethod]
        public async Task InsertOfflinePaymentStatus_ArgumentExceptionThrow_ShouldReturnBadRequest()
        {
            // Arrange
            var request = new OfflinePaymentStatusInsertRequestDto();

            _offlinePaymentsServiceMock.Setup(service => service.InsertOfflinePaymentAsync(request, _cancellationToken))
                               .ThrowsAsync(new ArgumentException("Test Exception"));

            // Act
            var result = await _controller.InsertOfflinePaymentStatus(request, _cancellationToken);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task InsertOfflinePaymentStatus_ThrowsValidationException_ShouldReturnBadRequest()
        {
            // Arrange

            var request = _fixture.Build<OfflinePaymentStatusInsertRequestDto>().Create();

            var validationException = new ValidationException("Validation error");
            _offlinePaymentsServiceMock.Setup(s => s.InsertOfflinePaymentAsync(It.IsAny<OfflinePaymentStatusInsertRequestDto>(), _cancellationToken)).ThrowsAsync(validationException);

            // Act
            var result = await _controller.InsertOfflinePaymentStatus(request, _cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                result.Result.Should().BeOfType<BadRequestObjectResult>();

                var badRequestResult = result.Result as BadRequestObjectResult;
                badRequestResult.Should().NotBeNull();
            }
        }
    }
}