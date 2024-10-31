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
        private CancellationToken _cancellationToken;

        [TestInitialize]
        public void TestInitialize()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _offlinePaymentsServiceMock = new Mock<IOfflinePaymentsService>();
            _offlinePaymentInsertRequestValidatorMock = _fixture.Freeze<Mock<IValidator<OfflinePaymentInsertRequestDto>>>(); 
            _controller = new OfflinePaymentsController(_offlinePaymentsServiceMock.Object, _offlinePaymentInsertRequestValidatorMock.Object);
            _cancellationToken = new CancellationToken();
        }

        [TestMethod]
        public void Constructor_WithValidArguments_ShouldInitializeCorrectly()
        {
            // Act
            var controller = new OfflinePaymentsController(_offlinePaymentsServiceMock.Object, 
                _offlinePaymentInsertRequestValidatorMock.Object);

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
                                _offlinePaymentInsertRequestValidatorMock.Object);

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
                                offlinePaymentInsertRequestValidatorMock!);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'offlinePaymentInsertRequestValidator')");
        }


        [TestMethod, AutoMoqData]
        public async Task InsertOfflinePayment_ValidInput_ShouldReturnOk()
        {
            // Arrange
            var request = _fixture.Build<OfflinePaymentInsertRequestDto>().Create();

            //Act
            var result = await _controller.InsertOfflinePayment(request, _cancellationToken);

            //Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [TestMethod, AutoMoqData]
        public async Task InsertOnlinePayment_RequestValidationFails_ShouldReturnsBadRequestWithValidationErrorDetails(
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
            var result = await _controller.InsertOfflinePayment(request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Which;
                var problemDetails = badRequestResult.Value.Should().BeOfType<ProblemDetails>().Which;
                problemDetails.Detail.Should().Be("Reference is required; Regulator is required");
            }
        }

        [TestMethod, AutoMoqData]
        public async Task InsertOfflinePayment_ServiceThrowsException_ShouldReturnInternalServerError([Frozen] OfflinePaymentInsertRequestDto request)
        {
            // Arrange

            _offlinePaymentsServiceMock.Setup(service => service.InsertOfflinePaymentAsync(request, _cancellationToken))
                               .ThrowsAsync(new Exception("Test Exception"));

            // Act
            var result = await _controller.InsertOfflinePayment(request, _cancellationToken);

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
            var result = await _controller.InsertOfflinePayment(request, _cancellationToken);

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
            var result = await _controller.InsertOfflinePayment(request, _cancellationToken);

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
