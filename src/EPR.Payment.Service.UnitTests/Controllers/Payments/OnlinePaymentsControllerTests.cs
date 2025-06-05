using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Dtos.Request.Payments;
using EPR.Payment.Service.Common.Dtos.Response.Payments;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Controllers;
using EPR.Payment.Service.Services.Interfaces.Payments;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EPR.Payment.Service.UnitTests.Controllers.Payments
{
    [TestClass]
    public class OnlinePaymentsControllerTests
    {
        [TestMethod, AutoMoqData]
        public async Task InsertOnlinePayment_ValidInput_ShouldReturnOkWithGuid(
            [Frozen] Mock<IValidator<OnlinePaymentInsertRequestDto>> validatorMock,
            [Frozen] Mock<IOnlinePaymentsService> onlinePaymentsServiceMock,
            [Greedy] OnlinePaymentsController controllerUnderTest,
            OnlinePaymentInsertRequestDto request,
            CancellationTokenSource cancellationTokenSource,
            Guid expectedResult)
        {
            // Arrange
            validatorMock
                .Setup(mock => mock.Validate(It.IsAny<OnlinePaymentInsertRequestDto>()))
                .Returns(new ValidationResult());

            onlinePaymentsServiceMock
                .Setup(service => service.InsertOnlinePaymentAsync(It.IsAny<OnlinePaymentInsertRequestDto>(), cancellationTokenSource.Token))
                .ReturnsAsync(expectedResult);

            // Act
            ActionResult<Guid> result = await controllerUnderTest.InsertOnlinePayment(request, cancellationTokenSource.Token);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(expectedResult);
        }

        [TestMethod, AutoMoqData]
        public async Task InsertOnlinePayment_RequestValidationFails_ShouldReturnsBadRequestWithValidationErrorDetails(
            [Frozen] Mock<IValidator<OnlinePaymentInsertRequestDto>> validatorMock,
            [Greedy] OnlinePaymentsController controllerUnderTest,
            OnlinePaymentInsertRequestDto request)
        {
            // Arrange
            List<ValidationFailure> validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("Reference", "Reference is required"),
                new ValidationFailure("Regulator", "Regulator is required")
            };

            validatorMock.Setup(v => v.Validate(It.IsAny<OnlinePaymentInsertRequestDto>()))
                .Returns(new ValidationResult(validationFailures));

            // Act
            ActionResult<Guid> result = await controllerUnderTest.InsertOnlinePayment(request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                BadRequestObjectResult badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Which;
                ProblemDetails problemDetails = badRequestResult.Value.Should().BeOfType<ProblemDetails>().Which;
                problemDetails.Detail.Should().Be("Reference is required; Regulator is required");
            }
        }

        [TestMethod, AutoMoqData]
        public async Task InsertOnlinePayment_ServiceThrowsException_ShouldReturnInternalServerError(
            [Frozen] Mock<IValidator<OnlinePaymentInsertRequestDto>> validatorMock,
            [Frozen] Mock<IOnlinePaymentsService> onlinePaymentsServiceMock,
            [Greedy] OnlinePaymentsController controllerUnderTest,
            OnlinePaymentInsertRequestDto request,
            CancellationTokenSource cancellationTokenSource)
        {
            // Arrange
            validatorMock
                .Setup(mock => mock.Validate(It.IsAny<OnlinePaymentInsertRequestDto>()))
                .Returns(new ValidationResult());

            onlinePaymentsServiceMock
                .Setup(service => service.InsertOnlinePaymentAsync(It.IsAny<OnlinePaymentInsertRequestDto>(), cancellationTokenSource.Token))
                .ThrowsAsync(new Exception("Test Exception"));

            // Act
            ActionResult<Guid> result = await controllerUnderTest.InsertOnlinePayment(request, cancellationTokenSource.Token);

            // Assert
            result.Result.Should().BeOfType<ObjectResult>()
                .Which.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }

        [TestMethod, AutoMoqData]
        public async Task InsertOnlinePayment_ArgumentExceptionThrow_ShouldReturnBadRequest(
            [Frozen] Mock<IValidator<OnlinePaymentInsertRequestDto>> validatorMock,
            [Frozen] Mock<IOnlinePaymentsService> onlinePaymentsServiceMock,
            [Greedy] OnlinePaymentsController controllerUnderTest,
            OnlinePaymentInsertRequestDto request,
            CancellationTokenSource cancellationTokenSource)
        {
            // Arrange
            validatorMock
                .Setup(mock => mock.Validate(It.IsAny<OnlinePaymentInsertRequestDto>()))
                .Returns(new ValidationResult());

            onlinePaymentsServiceMock
                .Setup(service => service.InsertOnlinePaymentAsync(It.IsAny<OnlinePaymentInsertRequestDto>(), cancellationTokenSource.Token))
                .ThrowsAsync(new ArgumentException("Test Exception"));

            // Act
            ActionResult<Guid> result = await controllerUnderTest.InsertOnlinePayment(request, cancellationTokenSource.Token);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod, AutoMoqData]
        public async Task InsertOnlinePayment_ThrowsValidationException_ShouldReturnBadRequest(
            [Frozen] Mock<IValidator<OnlinePaymentInsertRequestDto>> validatorMock,
            [Frozen] Mock<IOnlinePaymentsService> onlinePaymentsServiceMock,
            [Greedy] OnlinePaymentsController controllerUnderTest,
            OnlinePaymentInsertRequestDto request,
            CancellationTokenSource cancellationTokenSource,
            ValidationException validationException)
        {
            // Arrange
            onlinePaymentsServiceMock
                .Setup(service => service.InsertOnlinePaymentAsync(It.IsAny<OnlinePaymentInsertRequestDto>(), cancellationTokenSource.Token))
                .ThrowsAsync(validationException);

            // Act
            ActionResult<Guid> result = await controllerUnderTest.InsertOnlinePayment(request, cancellationTokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                result.Result.Should().BeOfType<BadRequestObjectResult>();

                BadRequestObjectResult? badRequestResult = result.Result as BadRequestObjectResult;
                badRequestResult.Should().NotBeNull();
            }
        }

        [TestMethod, AutoMoqData]
        public async Task InsertOnlinePaymentV2_ValidInput_ShouldReturnOkWithGuid(
            [Frozen] Mock<IValidator<OnlinePaymentInsertRequestV2Dto>> validatorMock,
            [Frozen] Mock<IOnlinePaymentsService> onlinePaymentsServiceMock,
            [Greedy] OnlinePaymentsController controllerUnderTest,
            OnlinePaymentInsertRequestV2Dto request,
            CancellationTokenSource cancellationTokenSource,
            Guid expectedResult)
        {
            // Arrange
            validatorMock
                .Setup(mock => mock.Validate(request))
                .Returns(new ValidationResult());

            onlinePaymentsServiceMock
                .Setup(service => service.InsertOnlinePaymentAsync(request, cancellationTokenSource.Token))
                .ReturnsAsync(expectedResult);

            // Act
            ActionResult<Guid> result = await controllerUnderTest.InsertOnlinePaymentV2(request, cancellationTokenSource.Token);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(expectedResult);
        }

        [TestMethod, AutoMoqData]
        public async Task InsertOnlinePaymentV2_RequestValidationFails_ShouldReturnsBadRequestWithValidationErrorDetails(
            [Frozen] Mock<IValidator<OnlinePaymentInsertRequestV2Dto>> validatorMock,
            [Greedy] OnlinePaymentsController controllerUnderTest,
            OnlinePaymentInsertRequestV2Dto request,
            CancellationTokenSource cancellationTokenSource)
        {
            // Arrange
            List<ValidationFailure> validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("Reference", "Reference is required"),
                new ValidationFailure("Regulator", "Regulator is required")
            };

            validatorMock.Setup(v => v.Validate(request))
                .Returns(new ValidationResult(validationFailures));

            // Act
            ActionResult<Guid> result = await controllerUnderTest.InsertOnlinePaymentV2(request, cancellationTokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                BadRequestObjectResult badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Which;
                ProblemDetails problemDetails = badRequestResult.Value.Should().BeOfType<ProblemDetails>().Which;
                problemDetails.Detail.Should().Be("Reference is required; Regulator is required");
            }
        }

        [TestMethod, AutoMoqData]
        public async Task InsertOnlinePaymentV2_ServiceThrowsException_ShouldReturnInternalServerError(
            [Frozen] Mock<IValidator<OnlinePaymentInsertRequestV2Dto>> validatorMock,
            [Frozen] Mock<IOnlinePaymentsService> onlinePaymentsServiceMock,
            [Greedy] OnlinePaymentsController controllerUnderTest,
            OnlinePaymentInsertRequestV2Dto request,
            CancellationTokenSource cancellationTokenSource)
        {
            // Arrange
            validatorMock
                .Setup(mock => mock.Validate(request))
                .Returns(new ValidationResult());

            onlinePaymentsServiceMock
                .Setup(service => service.InsertOnlinePaymentAsync(request, cancellationTokenSource.Token))
                .ThrowsAsync(new Exception("Test Exception"));

            // Act
            ActionResult<Guid> result = await controllerUnderTest.InsertOnlinePaymentV2(request, cancellationTokenSource.Token);

            // Assert
            result.Result.Should().BeOfType<ObjectResult>()
                .Which.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }

        [TestMethod, AutoMoqData]
        public async Task InsertOnlinePaymentV2_ArgumentExceptionThrow_ShouldReturnBadRequest(
            [Frozen] Mock<IValidator<OnlinePaymentInsertRequestV2Dto>> validatorMock,
            [Frozen] Mock<IOnlinePaymentsService> onlinePaymentsServiceMock,
            [Greedy] OnlinePaymentsController controllerUnderTest,
            OnlinePaymentInsertRequestV2Dto request,
            CancellationTokenSource cancellationTokenSource)
        {
            // Arrange
            validatorMock
                .Setup(mock => mock.Validate(request))
                .Returns(new ValidationResult());

            onlinePaymentsServiceMock
                .Setup(service => service.InsertOnlinePaymentAsync(request, cancellationTokenSource.Token))
                .ThrowsAsync(new ArgumentException("Test Exception"));

            // Act
            ActionResult<Guid> result = await controllerUnderTest.InsertOnlinePaymentV2(request, cancellationTokenSource.Token);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod, AutoMoqData]
        public async Task InsertOnlinePaymentV2_ThrowsValidationException_ShouldReturnBadRequest(
            [Frozen] Mock<IValidator<OnlinePaymentInsertRequestV2Dto>> validatorMock,
            [Frozen] Mock<IOnlinePaymentsService> onlinePaymentsServiceMock,
            [Greedy] OnlinePaymentsController controllerUnderTest,
            OnlinePaymentInsertRequestV2Dto request,
            CancellationTokenSource cancellationTokenSource,
            ValidationException validationException)
        {
            // Arrange
            onlinePaymentsServiceMock
                .Setup(service => service.InsertOnlinePaymentAsync(request, cancellationTokenSource.Token))
                .ThrowsAsync(validationException);

            // Act
            ActionResult<Guid> result = await controllerUnderTest.InsertOnlinePaymentV2(request, cancellationTokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                result.Result.Should().BeOfType<BadRequestObjectResult>();

                BadRequestObjectResult? badRequestResult = result.Result as BadRequestObjectResult;
                badRequestResult.Should().NotBeNull();
            }
        }

        [TestMethod, AutoMoqData]
        public async Task UpdateOnlinePayment_ValidInput_ShouldReturnNoContentResult(
            [Frozen] Mock<IValidator<OnlinePaymentUpdateRequestDto>> validatorMock,
            [Frozen] Mock<IOnlinePaymentsService> onlinePaymentsServiceMock,
            [Greedy] OnlinePaymentsController controllerUnderTest,
            OnlinePaymentUpdateRequestDto request,
            CancellationTokenSource cancellationTokenSource, 
            Guid id)
        {
            // Arrange
            validatorMock
                .Setup(mock => mock.Validate(request))
                .Returns(new ValidationResult());

            // Act
            IActionResult result = await controllerUnderTest.UpdateOnlinePayment(id, request, cancellationTokenSource.Token);

            //Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [TestMethod, AutoMoqData]
        public async Task UpdateOnlinePayment_InvalidInput_ShouldReturnBadRequest(
            [Frozen] Mock<IValidator<OnlinePaymentUpdateRequestDto>> validatorMock,
            [Frozen] Mock<IOnlinePaymentsService> onlinePaymentsServiceMock,
            [Greedy] OnlinePaymentsController controllerUnderTest,
            OnlinePaymentUpdateRequestDto request,
            CancellationTokenSource cancellationTokenSource,
            Guid id)
        {
            // Arrange
            List<ValidationFailure> validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("GovPayPaymentId", "Gov Pay Payment ID cannot be null or empty."),
                new ValidationFailure("Regulator", "Regulator is required")
            };

            validatorMock.Setup(v => v.Validate(request))
                .Returns(new ValidationResult(validationFailures));

            // Act
            IActionResult result = await controllerUnderTest.UpdateOnlinePayment(id, request, cancellationTokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                BadRequestObjectResult badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Which;
                ProblemDetails problemDetails = badRequestResult.Value.Should().BeOfType<ProblemDetails>().Which;
                problemDetails.Detail.Should().Be("Gov Pay Payment ID cannot be null or empty.; Regulator is required");
            }
        }

        [TestMethod, AutoMoqData]
        public async Task UpdateOnlinePayment_ArgumentExceptionThrow_ShouldReturnBadRequest(
            [Frozen] Mock<IValidator<OnlinePaymentUpdateRequestDto>> validatorMock,
            [Frozen] Mock<IOnlinePaymentsService> onlinePaymentsServiceMock,
            [Greedy] OnlinePaymentsController controllerUnderTest,
            OnlinePaymentUpdateRequestDto request,
            CancellationTokenSource cancellationTokenSource,
            Guid id)
        {
            // Arrange
            validatorMock
                .Setup(mock => mock.Validate(request))
                .Returns(new ValidationResult());

            onlinePaymentsServiceMock
                .Setup(service => service.UpdateOnlinePaymentAsync(id, request, cancellationTokenSource.Token))
                .ThrowsAsync(new ArgumentException("Test Exception"));

            // Act
            IActionResult result = await controllerUnderTest.UpdateOnlinePayment(id, request, cancellationTokenSource.Token);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod, AutoMoqData]
        public async Task UpdateOnlinePayment_ServiceThrowsException_ShouldReturnInternalServerError(
            [Frozen] Mock<IValidator<OnlinePaymentUpdateRequestDto>> validatorMock,
            [Frozen] Mock<IOnlinePaymentsService> onlinePaymentsServiceMock,
            [Greedy] OnlinePaymentsController controllerUnderTest,
            OnlinePaymentUpdateRequestDto request,
            CancellationTokenSource cancellationTokenSource,
            Guid id)
        {
            // Arrange
            validatorMock
                .Setup(mock => mock.Validate(request))
                .Returns(new ValidationResult());

            onlinePaymentsServiceMock
                .Setup(service => service.UpdateOnlinePaymentAsync(It.IsAny<Guid>(), request, cancellationTokenSource.Token))
                .ThrowsAsync(new Exception("Test Exception"));

            // Act
            IActionResult result = await controllerUnderTest.UpdateOnlinePayment(id, request, cancellationTokenSource.Token);

            // Assert
            result.Should().BeOfType<ObjectResult>()
                .Which.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }

        [TestMethod, AutoMoqData]
        public async Task UpdateOnlinePayment_ThrowsValidationException_ShouldReturnBadRequest(
            [Frozen] Mock<IOnlinePaymentsService> onlinePaymentsServiceMock,
            [Greedy] OnlinePaymentsController controllerUnderTest,
            OnlinePaymentUpdateRequestDto request,
            CancellationTokenSource cancellationTokenSource,
            Guid id,
            ValidationException validationException)
        {
            // Arrange
            onlinePaymentsServiceMock
                .Setup(service => service.UpdateOnlinePaymentAsync(It.IsAny<Guid>(), request, cancellationTokenSource.Token))
                .ThrowsAsync(validationException);

            // Act
            IActionResult result = await controllerUnderTest.UpdateOnlinePayment(id, request, cancellationTokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeOfType<BadRequestObjectResult>();

                BadRequestObjectResult? badRequestResult = result as BadRequestObjectResult;
                badRequestResult.Should().NotBeNull();
            }
        }

        [TestMethod, AutoMoqData]
        public async Task GetOnlinePaymentByExternalPaymentId_ServiceReturnsAResult_ShouldReturnOkResponse(
            [Frozen] Mock<IOnlinePaymentsService> onlinePaymentsServiceMock,
            [Greedy] OnlinePaymentsController controllerUnderTest,
            CancellationTokenSource cancellationTokenSource,
            Guid externalPaymentId,
            OnlinePaymentResponseDto expectedResult)
        {
            // Arrange
            onlinePaymentsServiceMock
                .Setup(service => service.GetOnlinePaymentByExternalPaymentIdAsync(externalPaymentId, cancellationTokenSource.Token))
                .ReturnsAsync(expectedResult);

            // Act
            IActionResult result = await controllerUnderTest.GetOnlinePaymentByExternalPaymentId(externalPaymentId, cancellationTokenSource.Token);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            result.As<OkObjectResult>().Should().NotBeNull();
        }

        [TestMethod, AutoMoqData]
        public async Task GetOnlinePaymentByExternalPaymentId_ServiceThrowsException_ShouldReturnInternalServerError(
            [Frozen] Mock<IOnlinePaymentsService> onlinePaymentsServiceMock,
            [Greedy] OnlinePaymentsController controllerUnderTest,
            CancellationTokenSource cancellationTokenSource,
            Guid externalPaymentId)
        {
            // Arrange
            onlinePaymentsServiceMock
                .Setup(service => service.GetOnlinePaymentByExternalPaymentIdAsync(externalPaymentId, cancellationTokenSource.Token))
                .ThrowsAsync(new Exception("Test Exception"));

            // Act
            IActionResult result = await controllerUnderTest.GetOnlinePaymentByExternalPaymentId(externalPaymentId, cancellationTokenSource.Token);

            // Assert
            result.Should().BeOfType<ObjectResult>()
                .Which.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }

        [TestMethod, AutoMoqData]
        public async Task GetOnlinePaymentByExternalPaymentId_EmptyExternalPaymentId_ShouldReturnBadRequest(
            [Greedy] OnlinePaymentsController controllerUnderTest,
            CancellationTokenSource cancellationTokenSource)
        {
            // Arrange
            Guid externalPaymentId = Guid.Empty;

            IActionResult result = await controllerUnderTest.GetOnlinePaymentByExternalPaymentId(externalPaymentId, cancellationTokenSource.Token);

            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}