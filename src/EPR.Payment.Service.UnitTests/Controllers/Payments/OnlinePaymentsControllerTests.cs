using AutoFixture;
using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Dtos.Request.Payments;
using EPR.Payment.Service.Common.Dtos.Response.Payments;
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
    public class OnlinePaymentsControllerTests
    {
        private Fixture _fixture = null!;
        private OnlinePaymentsController _controller = null!;
        private Mock<IOnlinePaymentsService> _onlinePaymentsServiceMock = null!;
        private CancellationToken _cancellationToken;

        [TestInitialize]
        public void TestInitialize()
        {
            _fixture = new Fixture();
            _onlinePaymentsServiceMock = new Mock<IOnlinePaymentsService>();
            _controller = new OnlinePaymentsController(_onlinePaymentsServiceMock.Object);
            _cancellationToken = new CancellationToken();
        }

        [TestMethod, AutoMoqData]
        public async Task InsertOnlinePaymentStatus_ValidInput_ShouldReturnOkWithGuid([Frozen] Guid expectedResult)
        {
            // Arrange
            var request = _fixture.Build<OnlinePaymentStatusInsertRequestDto>().Create();

            _onlinePaymentsServiceMock.Setup(service => service.InsertOnlinePaymentStatusAsync(request, _cancellationToken))
                .ReturnsAsync(expectedResult);

            //Act
            var result = await _controller.InsertOnlinePaymentStatus(request, _cancellationToken);

            //Assert
            result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(expectedResult);
        }

        [TestMethod]
        public async Task InsertOnlinePaymentStatus_InValidInput_ShouldReturnBadRequest()
        {
            // Arrange
            var request = new OnlinePaymentStatusInsertRequestDto();
            _controller!.ModelState.AddModelError("Regulator", "Regulator is required");

            // Act
            var result = await _controller.InsertOnlinePaymentStatus(request, _cancellationToken);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task InsertOnlinePaymentStatus_ServiceThrowsException_ShouldReturnInternalServerError()
        {
            // Arrange
            var request = new OnlinePaymentStatusInsertRequestDto();

            _onlinePaymentsServiceMock.Setup(service => service.InsertOnlinePaymentStatusAsync(request, _cancellationToken))
                               .ThrowsAsync(new Exception("Test Exception"));

            // Act
            var result = await _controller.InsertOnlinePaymentStatus(request, _cancellationToken);

            // Assert
            result.Result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }

        [TestMethod]
        public async Task InsertOnlinePaymentStatus_ArgumentExceptionThrow_ShouldReturnBadRequest()
        {
            // Arrange
            var request = new OnlinePaymentStatusInsertRequestDto();

            _onlinePaymentsServiceMock.Setup(service => service.InsertOnlinePaymentStatusAsync(request, _cancellationToken))
                               .ThrowsAsync(new ArgumentException("Test Exception"));

            // Act
            var result = await _controller.InsertOnlinePaymentStatus(request, _cancellationToken);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task InsertOnlinePaymentStatus_ThrowsValidationException_ShouldReturnBadRequest()
        {
            // Arrange

            var request = _fixture.Build<OnlinePaymentStatusInsertRequestDto>().Create();

            var validationException = new ValidationException("Validation error");
            _onlinePaymentsServiceMock.Setup(s => s.InsertOnlinePaymentStatusAsync(It.IsAny<OnlinePaymentStatusInsertRequestDto>(), _cancellationToken)).ThrowsAsync(validationException);

            // Act
            var result = await _controller.InsertOnlinePaymentStatus(request, _cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                result.Result.Should().BeOfType<BadRequestObjectResult>();

                var badRequestResult = result.Result as BadRequestObjectResult;
                badRequestResult.Should().NotBeNull();
            }
        }

        [TestMethod, AutoMoqData]
        public async Task UpdateOnlinePaymentStatus_ValidInput_ShouldReturnNoContentResult([Frozen] Guid id)
        {
            // Arrange
            var request = _fixture.Build<OnlinePaymentStatusUpdateRequestDto>().Create();

            //Act
            var result = await _controller.UpdateOnlinePaymentStatus(id, request, _cancellationToken);

            //Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [TestMethod, AutoMoqData]
        public async Task UpdateOnlinePaymentStatus_InvalidInput_ShouldReturnBadRequest([Frozen] Guid id)
        {
            // Arrange
            var request = new OnlinePaymentStatusUpdateRequestDto();
            _controller!.ModelState.AddModelError("Regulator", "Regulator is required");

            // Act
            var result = await _controller.UpdateOnlinePaymentStatus(id, request, _cancellationToken);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod, AutoMoqData]
        public async Task UpdateOnlinePaymentStatus_ArgumentExceptionThrow_ShouldReturnBadRequest([Frozen] Guid id)
        {
            // Arrange
            var request = new OnlinePaymentStatusUpdateRequestDto();

            _onlinePaymentsServiceMock.Setup(service => service.UpdateOnlinePaymentStatusAsync(id, request, _cancellationToken))
                               .ThrowsAsync(new ArgumentException("Test Exception"));

            // Act
            var result = await _controller.UpdateOnlinePaymentStatus(id, request, _cancellationToken);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod, AutoMoqData]
        public async Task UpdateOnlinePaymentStatus_ServiceThrowsException_ShouldReturnInternalServerError([Frozen] Guid id)
        {
            // Arrange
            var request = new OnlinePaymentStatusUpdateRequestDto();

            _onlinePaymentsServiceMock.Setup(service => service.UpdateOnlinePaymentStatusAsync(id, request, _cancellationToken))
                               .ThrowsAsync(new Exception("Test Exception"));

            // Act
            var result = await _controller.UpdateOnlinePaymentStatus(id, request, _cancellationToken);

            // Assert
            result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }

        [TestMethod, AutoMoqData]
        public async Task UpdateOnlinePaymentStatus_ThrowsValidationException_ShouldReturnBadRequest([Frozen] Guid id)
        {
            // Arrange
            var request = new OnlinePaymentStatusUpdateRequestDto();

            var validationException = new ValidationException("Validation error");
            _onlinePaymentsServiceMock.Setup(s => s.UpdateOnlinePaymentStatusAsync(id, request, _cancellationToken)).ThrowsAsync(validationException);


            // Act
            var result = await _controller.UpdateOnlinePaymentStatus(id, request, _cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeOfType<BadRequestObjectResult>();

                var badRequestResult = result as BadRequestObjectResult;
                badRequestResult.Should().NotBeNull();
            }
        }

        [TestMethod, AutoMoqData]
        public async Task GetOnlinePaymentByExternalPaymentId_ServiceReturnsAResult_ShouldReturnOkResponse(
            Guid externalPaymentId,
            OnlinePaymentResponseDto expectedResult)
        {
            //Arrange
            _onlinePaymentsServiceMock.Setup(i => i.GetOnlinePaymentByExternalPaymentIdAsync(externalPaymentId, _cancellationToken)).ReturnsAsync(expectedResult);

            //Act
            var result = await _controller.GetOnlinePaymentByExternalPaymentId(externalPaymentId, _cancellationToken);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            result.As<OkObjectResult>().Should().NotBeNull();
        }

        [TestMethod, AutoMoqData]
        public async Task GetOnlinePaymentByExternalPaymentId_ServiceThrowsException_ShouldReturnInternalServerError([Frozen] Guid externalPaymentId)
        {
            // Arrange
            _onlinePaymentsServiceMock.Setup(i => i.GetOnlinePaymentByExternalPaymentIdAsync(externalPaymentId, _cancellationToken))
                               .ThrowsAsync(new Exception("Test Exception"));

            // Act
            var result = await _controller.GetOnlinePaymentByExternalPaymentId(externalPaymentId, _cancellationToken);

            // Assert
            result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }

        [TestMethod, AutoMoqData]
        public async Task GetOnlinePaymentByExternalPaymentId_EmptyExternalPaymentId_ShouldReturnBadRequest()
        {
            // Arrange
            var externalPaymentId = Guid.Empty;

            var result = await _controller.GetOnlinePaymentByExternalPaymentId(externalPaymentId, _cancellationToken);

            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}