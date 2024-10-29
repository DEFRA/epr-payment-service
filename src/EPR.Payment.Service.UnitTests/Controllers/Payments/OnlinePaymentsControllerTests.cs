using AutoFixture;
using AutoFixture.AutoMoq;
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
        private IFixture _fixture = null!;
        private OnlinePaymentsController _controller = null!;
        private Mock<IValidator<OnlinePaymentInsertRequestDto>> _onlinePaymentInsertRequestValidatorMock = null!;
        private Mock<IValidator<OnlinePaymentUpdateRequestDto>> _onlinePaymentUpdateRequestValidatorMock = null!;
        private Mock<IOnlinePaymentsService> _onlinePaymentsServiceMock = null!;
        private CancellationToken _cancellationToken;

        [TestInitialize]
        public void TestInitialize()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _onlinePaymentsServiceMock = new Mock<IOnlinePaymentsService>();
            _onlinePaymentInsertRequestValidatorMock = _fixture.Freeze<Mock<IValidator<OnlinePaymentInsertRequestDto>>>(); 
            _onlinePaymentUpdateRequestValidatorMock = _fixture.Freeze<Mock<IValidator<OnlinePaymentUpdateRequestDto>>>(); 
            _controller = new OnlinePaymentsController(_onlinePaymentsServiceMock.Object, _onlinePaymentInsertRequestValidatorMock.Object, _onlinePaymentUpdateRequestValidatorMock.Object);
            _cancellationToken = new CancellationToken();
        }

        [TestMethod]
        public void Constructor_WithValidArguments_ShouldInitializeCorrectly()
        {
            // Act
            var controller = new OnlinePaymentsController(_onlinePaymentsServiceMock.Object,
                            _onlinePaymentInsertRequestValidatorMock.Object,
                            _onlinePaymentUpdateRequestValidatorMock.Object);

            // Assert
            using (new AssertionScope())
            {
                controller.Should().NotBeNull();
                controller.Should().BeAssignableTo<OnlinePaymentsController>();
            }
        }

        [TestMethod]
        public void Constructor_WhenOnlinePaymentsServiceIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            IOnlinePaymentsService?  onlinePaymentsService = null;

            // Act
            Action act = () => new OnlinePaymentsController(onlinePaymentsService!,
                            _onlinePaymentInsertRequestValidatorMock.Object,
                            _onlinePaymentUpdateRequestValidatorMock.Object);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'paymentsService')");
        }

        [TestMethod]
        public void Constructor_WhenOnlinePaymentInsertRequestValidatorIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            IValidator<OnlinePaymentInsertRequestDto>? onlinePaymentInsertRequestValidator = null!;

            // Act
            Action act = () => new OnlinePaymentsController(_onlinePaymentsServiceMock.Object,
                            onlinePaymentInsertRequestValidator!,
                            _onlinePaymentUpdateRequestValidatorMock.Object);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'onlinePaymentInsertRequestValidator')");
        }


        [TestMethod]
        public void Constructor_WhenOnlinePaymentUpdateRequestDtoIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            IValidator<OnlinePaymentUpdateRequestDto>? onlinePaymentUpdateRequestValidator = null!;

            // Act
            Action act = () => new OnlinePaymentsController(_onlinePaymentsServiceMock.Object,
                             _onlinePaymentInsertRequestValidatorMock.Object,
                            onlinePaymentUpdateRequestValidator);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'onlinePaymentUpdateRequestValidator')");
        }

        [TestMethod, AutoMoqData]
        public async Task InsertOnlinePayment_ValidInput_ShouldReturnOkWithGuid([Frozen] Guid expectedResult)
        {
            // Arrange
            var request = new OnlinePaymentInsertRequestDto
            {
                UserId = Guid.NewGuid(),
                OrganisationId = Guid.NewGuid(),
                Regulator = "GB-ENG",
                Reference = "Test Reference",
                ReasonForPayment = "Test Reason",
                Amount = 10 
            };

            _onlinePaymentsServiceMock.Setup(service => service.InsertOnlinePaymentAsync(request, _cancellationToken))
                .ReturnsAsync(expectedResult);

            //Act
            var result = await _controller.InsertOnlinePayment(request, _cancellationToken);

            //Assert
            result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(expectedResult);
        }

        [TestMethod, AutoMoqData]
        public async Task InsertOnlinePayment_RequestValidationFails_ShouldReturnsBadRequestWithValidationErrorDetails(
            [Frozen] OnlinePaymentInsertRequestDto request)
        {
            // Arrange
            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("Reference", "Reference is required"),
                new ValidationFailure("Regulator", "Regulator is required")
            };

            _onlinePaymentInsertRequestValidatorMock.Setup(v => v.Validate(It.IsAny<OnlinePaymentInsertRequestDto>()))
                .Returns(new ValidationResult(validationFailures));

            // Act
            var result = await _controller.InsertOnlinePayment(request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Which;
                var problemDetails = badRequestResult.Value.Should().BeOfType<ProblemDetails>().Which;
                problemDetails.Detail.Should().Be("Reference is required; Regulator is required");
            }
        }

        [TestMethod]
        public async Task InsertOnlinePayment_ServiceThrowsException_ShouldReturnInternalServerError()
        {
            // Arrange
            var request = new OnlinePaymentInsertRequestDto();

            _onlinePaymentsServiceMock.Setup(service => service.InsertOnlinePaymentAsync(request, _cancellationToken))
                               .ThrowsAsync(new Exception("Test Exception"));

            // Act
            var result = await _controller.InsertOnlinePayment(request, _cancellationToken);

            // Assert
            result.Result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }

        [TestMethod]
        public async Task InsertOnlinePayment_ArgumentExceptionThrow_ShouldReturnBadRequest()
        {
            // Arrange
            var request = new OnlinePaymentInsertRequestDto();

            _onlinePaymentsServiceMock.Setup(service => service.InsertOnlinePaymentAsync(request, _cancellationToken))
                               .ThrowsAsync(new ArgumentException("Test Exception"));

            // Act
            var result = await _controller.InsertOnlinePayment(request, _cancellationToken);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task InsertOnlinePayment_ThrowsValidationException_ShouldReturnBadRequest()
        {
            // Arrange

            var request = _fixture.Build<OnlinePaymentInsertRequestDto>().Create();

            var validationException = new ValidationException("Validation error");
            _onlinePaymentsServiceMock.Setup(s => s.InsertOnlinePaymentAsync(It.IsAny<OnlinePaymentInsertRequestDto>(), _cancellationToken)).ThrowsAsync(validationException);

            // Act
            var result = await _controller.InsertOnlinePayment(request, _cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                result.Result.Should().BeOfType<BadRequestObjectResult>();

                var badRequestResult = result.Result as BadRequestObjectResult;
                badRequestResult.Should().NotBeNull();
            }
        }

        [TestMethod, AutoMoqData]
        public async Task UpdateOnlinePayment_ValidInput_ShouldReturnNoContentResult([Frozen] Guid id)
        {
            // Arrange
            var request = _fixture.Build<OnlinePaymentUpdateRequestDto>().Create();

            //Act
            var result = await _controller.UpdateOnlinePayment(id, request, _cancellationToken);

            //Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [TestMethod, AutoMoqData]
        public async Task UpdateOnlinePayment_InvalidInput_ShouldReturnBadRequest([Frozen] Guid id, [Frozen] OnlinePaymentUpdateRequestDto request)
        {
            // Arrange
            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("GovPayPaymentId", "Gov Pay Payment ID cannot be null or empty."),
                new ValidationFailure("Regulator", "Regulator is required")
            };

            _onlinePaymentUpdateRequestValidatorMock.Setup(v => v.Validate(It.IsAny<OnlinePaymentUpdateRequestDto>()))
                .Returns(new ValidationResult(validationFailures));

            // Act
            var result = await _controller.UpdateOnlinePayment(id, request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Which;
                var problemDetails = badRequestResult.Value.Should().BeOfType<ProblemDetails>().Which;
                problemDetails.Detail.Should().Be("Gov Pay Payment ID cannot be null or empty.; Regulator is required");
            }
        }

        [TestMethod, AutoMoqData]
        public async Task UpdateOnlinePayment_ArgumentExceptionThrow_ShouldReturnBadRequest([Frozen] Guid id)
        {
            // Arrange
            var request = new OnlinePaymentUpdateRequestDto();

            _onlinePaymentsServiceMock.Setup(service => service.UpdateOnlinePaymentAsync(id, request, _cancellationToken))
                               .ThrowsAsync(new ArgumentException("Test Exception"));

            // Act
            var result = await _controller.UpdateOnlinePayment(id, request, _cancellationToken);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod, AutoMoqData]
        public async Task UpdateOnlinePayment_ServiceThrowsException_ShouldReturnInternalServerError([Frozen] Guid id)
        {
            // Arrange
            var request = new OnlinePaymentUpdateRequestDto();

            _onlinePaymentsServiceMock.Setup(service => service.UpdateOnlinePaymentAsync(id, request, _cancellationToken))
                               .ThrowsAsync(new Exception("Test Exception"));

            // Act
            var result = await _controller.UpdateOnlinePayment(id, request, _cancellationToken);

            // Assert
            result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }

        [TestMethod, AutoMoqData]
        public async Task UpdateOnlinePayment_ThrowsValidationException_ShouldReturnBadRequest([Frozen] Guid id)
        {
            // Arrange
            var request = new OnlinePaymentUpdateRequestDto();

            var validationException = new ValidationException("Validation error");
            _onlinePaymentsServiceMock.Setup(s => s.UpdateOnlinePaymentAsync(id, request, _cancellationToken)).ThrowsAsync(validationException);


            // Act
            var result = await _controller.UpdateOnlinePayment(id, request, _cancellationToken);

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