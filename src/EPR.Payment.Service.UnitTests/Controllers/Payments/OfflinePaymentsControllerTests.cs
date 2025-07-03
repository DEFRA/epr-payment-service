using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Dtos.Request.Payments;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Controllers.Payments;
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
    public class OfflinePaymentsControllerTests
    {
        private IFixture _fixture = null!;
        private OfflinePaymentsController _controller = null!;
        private Mock<IOfflinePaymentsService> _offlinePaymentsServiceMock = null!;
        private Mock<IValidator<OfflinePaymentInsertRequestDto>> _offlinePaymentInsertRequestValidatorMock = null!;
        private Mock<IValidator<OfflinePaymentInsertRequestV2Dto>> _offlinePaymentInsertRequestV2ValidatorMock = null!;
        private CancellationToken _cancellationToken;

        [TestInitialize]
        public void TestInitialize()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _offlinePaymentsServiceMock = new Mock<IOfflinePaymentsService>();
            _offlinePaymentInsertRequestValidatorMock = _fixture.Freeze<Mock<IValidator<OfflinePaymentInsertRequestDto>>>();
            _offlinePaymentInsertRequestV2ValidatorMock = _fixture.Freeze<Mock<IValidator<OfflinePaymentInsertRequestV2Dto>>>();
            
            _controller = new OfflinePaymentsController(
                _offlinePaymentsServiceMock.Object,
                _offlinePaymentInsertRequestValidatorMock.Object,
                _offlinePaymentInsertRequestV2ValidatorMock.Object);
            
            _cancellationToken = new CancellationToken();
        }

        [TestMethod]
        public void Constructor_WithValidArguments_ShouldInitializeCorrectly()
        {
            // Act
            var controller = new OfflinePaymentsController(
                _offlinePaymentsServiceMock.Object, 
                _offlinePaymentInsertRequestValidatorMock.Object,
                _offlinePaymentInsertRequestV2ValidatorMock.Object);

            // Assert
            using (new AssertionScope())
            {
                controller.Should().NotBeNull();
                controller.Should().BeAssignableTo<OfflinePaymentsController>();
            }
        }

        [TestMethod]
        public void Constructor_WhenOfflinePaymentsServiceIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            IOfflinePaymentsService? offlinePaymentsServiceMock = null;

            // Act
            Action act = () => new OfflinePaymentsController(offlinePaymentsServiceMock!,
                                _offlinePaymentInsertRequestValidatorMock.Object,
                                _offlinePaymentInsertRequestV2ValidatorMock.Object);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'paymentsService')");
        }

        [TestMethod]
        public void Constructor_WhenOfflinePaymentInsertRequestValidatorIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            IValidator<OfflinePaymentInsertRequestDto>? offlinePaymentInsertRequestValidatorMock = null;

            // Act
            Action act = () => new OfflinePaymentsController(_offlinePaymentsServiceMock.Object!,
                                offlinePaymentInsertRequestValidatorMock!,
                                _offlinePaymentInsertRequestV2ValidatorMock.Object);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'offlinePaymentInsertRequestValidator')");
        }

        [TestMethod]
        public void Constructor_WhenOfflinePaymentInsertRequestV2ValidatorIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            IValidator<OfflinePaymentInsertRequestV2Dto>? _offlinePaymentInsertRequestV2ValidatorMock = null;

            // Act
            Action act = () => new OfflinePaymentsController(_offlinePaymentsServiceMock.Object!,
                                _offlinePaymentInsertRequestValidatorMock.Object!,
                                _offlinePaymentInsertRequestV2ValidatorMock!);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'offlinePaymentInsertRequestV2Validator')");
        }

        [TestMethod, AutoMoqData]
        public async Task InsertOfflinePayment_ValidInput_ShouldReturnOk()
        {
            // Arrange
            var request = _fixture.Build<OfflinePaymentInsertRequestDto>().Create();

            //Act
            var result = await _controller.InsertOfflinePaymentV1(request, _cancellationToken);

            //Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [TestMethod, AutoMoqData]
        public async Task InsertOfflinePaymentForV2_ValidInput_ShouldReturnOk()
        {
            // Arrange
            var request = _fixture.Build<OfflinePaymentInsertRequestV2Dto>().Create();

            //Act
            var result = await _controller.InsertOfflinePaymentV2(request, _cancellationToken);

            //Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [TestMethod, AutoMoqData]
        public async Task InsertOfflinePayment_RequestValidationFails_ShouldReturnsBadRequestWithValidationErrorDetails(
           [Frozen] OfflinePaymentInsertRequestDto request)
        {
            // Arrange
            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("Reference", "Reference is required"),
                new ValidationFailure("Regulator", "Regulator is required")
            };

            _offlinePaymentInsertRequestValidatorMock.Setup(v => v.Validate(It.IsAny<OfflinePaymentInsertRequestDto>()))
                .Returns(new ValidationResult(validationFailures));

            // Act
            var result = await _controller.InsertOfflinePaymentV1(request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Which;
                var problemDetails = badRequestResult.Value.Should().BeOfType<ProblemDetails>().Which;
                problemDetails.Detail.Should().Be("Reference is required; Regulator is required");
            }
        }

        [TestMethod, AutoMoqData]
        public async Task InsertOfflinePaymentForV2_RequestValidationFails_ShouldReturnsBadRequestWithValidationErrorDetails(
           [Frozen] OfflinePaymentInsertRequestV2Dto request)
        {
            // Arrange
            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("Reference", "Reference is required"),
                new ValidationFailure("Regulator", "Regulator is required"),
                new ValidationFailure("PaymentMethod", "PaymentMethod is required"),
                new ValidationFailure("OrganisationId", "Organisation ID is required."),
                new ValidationFailure("UserId", "User id is required."),
                new ValidationFailure("Description", "The Description field is required."),
                new ValidationFailure("Description", "Description is invalid; acceptable values are 'Registration fee' or 'Packaging data resubmission fee'.")
            };

            _offlinePaymentInsertRequestV2ValidatorMock.Setup(v => v.Validate(It.IsAny<OfflinePaymentInsertRequestV2Dto>()))
                .Returns(new ValidationResult(validationFailures));

            // Act
            var result = await _controller.InsertOfflinePaymentV2(request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Which;
                var problemDetails = badRequestResult.Value.Should().BeOfType<ProblemDetails>().Which;
                problemDetails.Detail.Should().Be("Reference is required; Regulator is required; PaymentMethod is required; Organisation ID is required.; User id is required.; The Description field is required. Description is invalid; acceptable values are 'Registration fee' or 'Packaging data resubmission fee'.");
            }
        }

        [TestMethod, AutoMoqData]
        public async Task InsertOfflinePayment_ServiceThrowsException_ShouldReturnInternalServerError([Frozen] OfflinePaymentInsertRequestDto request)
        {
            // Arrange

            _offlinePaymentsServiceMock.Setup(service => service.InsertOfflinePaymentAsync(request, _cancellationToken))
                               .ThrowsAsync(new Exception("Test Exception"));

            // Act
            var result = await _controller.InsertOfflinePaymentV1(request, _cancellationToken);

            // Assert
            result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }

        [TestMethod, AutoMoqData]
        public async Task InsertOfflinePaymentForV2_ServiceThrowsException_ShouldReturnInternalServerError([Frozen] OfflinePaymentInsertRequestV2Dto request)
        {
            // Arrange

            _offlinePaymentsServiceMock.Setup(service => service.InsertOfflinePaymentAsync(request, _cancellationToken))
                               .ThrowsAsync(new Exception("Test Exception"));

            // Act
            var result = await _controller.InsertOfflinePaymentV2(request, _cancellationToken);

            // Assert
            result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }

        [TestMethod, AutoMoqData]
        public async Task InsertOfflinePayment_ArgumentExceptionThrow_ShouldReturnBadRequest([Frozen] OfflinePaymentInsertRequestDto request)
        {
            // Arrange

            _offlinePaymentsServiceMock.Setup(service => service.InsertOfflinePaymentAsync(request, _cancellationToken))
                               .ThrowsAsync(new ArgumentException("Test Exception"));

            // Act
            var result = await _controller.InsertOfflinePaymentV1(request, _cancellationToken);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod, AutoMoqData]
        public async Task InsertOfflinePaymentForV2_ArgumentExceptionThrow_ShouldReturnBadRequest([Frozen] OfflinePaymentInsertRequestV2Dto request)
        {
            // Arrange

            _offlinePaymentsServiceMock.Setup(service => service.InsertOfflinePaymentAsync(request, _cancellationToken))
                               .ThrowsAsync(new ArgumentException("Test Exception"));

            // Act
            var result = await _controller.InsertOfflinePaymentV2(request, _cancellationToken);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [TestMethod]
        public async Task InsertOfflinePayment_ThrowsValidationException_ShouldReturnBadRequest()
        {
            // Arrange

            var request = _fixture.Build<OfflinePaymentInsertRequestDto>().Create();

            var validationException = new ValidationException("Validation error");
            _offlinePaymentsServiceMock.Setup(s => s.InsertOfflinePaymentAsync(It.IsAny<OfflinePaymentInsertRequestDto>(), _cancellationToken)).ThrowsAsync(validationException);

            // Act
            var result = await _controller.InsertOfflinePaymentV1(request, _cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeOfType<BadRequestObjectResult>();

                var badRequestResult = result as BadRequestObjectResult;
                badRequestResult.Should().NotBeNull();
            }
        }

        [TestMethod]
        public async Task InsertOfflinePaymentV2_ThrowsValidationException_ShouldReturnBadRequest()
        {
            // Arrange

            var request = _fixture.Build<OfflinePaymentInsertRequestV2Dto>().Create();

            var validationException = new ValidationException("Validation error");
            _offlinePaymentsServiceMock.Setup(s => s.InsertOfflinePaymentAsync(It.IsAny<OfflinePaymentInsertRequestV2Dto>(), _cancellationToken)).ThrowsAsync(validationException);

            // Act
            var result = await _controller.InsertOfflinePaymentV2(request, _cancellationToken);

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
