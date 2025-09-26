using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Controllers.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.ComplianceScheme;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EPR.Payment.Service.UnitTests.Controllers.RegistrationFees.ComplianceScheme
{
    [TestClass]
    public class ComplianceSchemeFeesV3ControllerTests
    {
        private IFixture _fixture = null!;
        private Mock<IComplianceSchemeCalculatorService> _complianceSchemeCalculatorServiceMock = null!;
        private Mock<IValidator<ComplianceSchemeFeesRequestV3Dto>> _validatorMock = null!;
        private ComplianceSchemeFeesV3Controller _controller = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _complianceSchemeCalculatorServiceMock = _fixture.Freeze<Mock<IComplianceSchemeCalculatorService>>();
            _validatorMock = _fixture.Freeze<Mock<IValidator<ComplianceSchemeFeesRequestV3Dto>>>();
            _controller = new ComplianceSchemeFeesV3Controller(
                _complianceSchemeCalculatorServiceMock.Object,
                _validatorMock.Object);
        }

        [TestMethod]
        public void Constructor_WithValidArguments_ShouldInitializeCorrectly()
        {
            // Act
            var controller = new ComplianceSchemeFeesV3Controller(
                _complianceSchemeCalculatorServiceMock.Object,
                _validatorMock.Object);

            // Assert
            using (new AssertionScope())
            {
                controller.Should().NotBeNull();
                controller.Should().BeAssignableTo<ComplianceSchemeFeesV3Controller>();
            }
        }

        [TestMethod]
        public void Constructor_WhenComplianceSchemeCalculatorServiceIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            IComplianceSchemeCalculatorService? baseFeeService = null;

            // Act
            Action act = () => new ComplianceSchemeFeesV3Controller(baseFeeService!, _validatorMock.Object);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'complianceSchemeCalculatorService')");
        }

        [TestMethod]
        public void Constructor_WhenComplianceSchemeFeesRequestV3DtoValidatorIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            IValidator<ComplianceSchemeFeesRequestV3Dto>? validator = null;

            // Act
            Action act = () => new ComplianceSchemeFeesV3Controller(_complianceSchemeCalculatorServiceMock.Object, validator!);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'validator')");
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeesAsync_WhenValidRequest_ReturnsOkResultWithCalculatedFees(
            [Frozen] ComplianceSchemeFeesRequestV3Dto request,
            [Frozen] ComplianceSchemeFeesResponseDto response)
        {
            // Arrange
            _validatorMock.Setup(v => v.Validate(It.IsAny<ComplianceSchemeFeesRequestV3Dto>()))
                .Returns(new ValidationResult());

            _complianceSchemeCalculatorServiceMock.Setup(s => s.CalculateFeesAsync(It.IsAny<ComplianceSchemeFeesRequestV3Dto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.CalculateFeesAsyncV3(request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                result.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(response);
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeesAsync_WhenRequestValidationFails_ReturnsBadRequestWithValidationErrorDetails(
            [Frozen] ComplianceSchemeFeesRequestV3Dto request)
        {
            // Arrange
            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("ApplicationReferenceNumber", "ApplicationReferenceNumber is invalid"),
                new ValidationFailure("Regulator", "Regulator is required")
            };

            _validatorMock.Setup(v => v.Validate(It.IsAny<ComplianceSchemeFeesRequestV3Dto>()))
                .Returns(new ValidationResult(validationFailures));

            // Act
            var result = await _controller.CalculateFeesAsyncV3(request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Which;
                var problemDetails = badRequestResult.Value.Should().BeOfType<ProblemDetails>().Which;
                problemDetails.Detail.Should().Be("ApplicationReferenceNumber is invalid; Regulator is required");
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeesAsync_WhenCalculationThrowsValidationException_ReturnsBadRequestWithValidationExceptionDetails(
            [Frozen] ComplianceSchemeFeesRequestV3Dto request)
        {
            // Arrange
            var exceptionMessage = "Validation failed";

            _validatorMock.Setup(v => v.Validate(It.IsAny<ComplianceSchemeFeesRequestV3Dto>()))
                .Returns(new ValidationResult());

            _complianceSchemeCalculatorServiceMock.Setup(s => s.CalculateFeesAsync(It.IsAny<ComplianceSchemeFeesRequestV3Dto>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ValidationException(exceptionMessage));

            // Act
            var result = await _controller.CalculateFeesAsyncV3(request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Which;
                var problemDetails = badRequestResult.Value.Should().BeOfType<ProblemDetails>().Which;
                problemDetails.Detail.Should().Be(exceptionMessage);
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeesAsync_WhenCalculationThrowsArgumentException_ReturnsBadRequestWithArgumentExceptionDetails(
            [Frozen] ComplianceSchemeFeesRequestV3Dto request)
        {
            // Arrange
            var exceptionMessage = "Invalid argument";

            _validatorMock.Setup(v => v.Validate(It.IsAny<ComplianceSchemeFeesRequestV3Dto>()))
                .Returns(new ValidationResult());

            _complianceSchemeCalculatorServiceMock.Setup(s => s.CalculateFeesAsync(It.IsAny<ComplianceSchemeFeesRequestV3Dto>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ArgumentException(exceptionMessage));

            // Act
            var result = await _controller.CalculateFeesAsyncV3(request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Which;
                badRequestResult.Value.Should().Be(exceptionMessage);
            }
        }


        [TestMethod, AutoMoqData]
        public async Task CalculateFeesAsync_WhenCalculationThrowsException_ShouldReturnInternalServerError(
              [Frozen] ComplianceSchemeFeesRequestV3Dto request)
        {
            // Arrange
            var exceptionMessage = "exception";
            _complianceSchemeCalculatorServiceMock.Setup(i => i.CalculateFeesAsync(It.IsAny<ComplianceSchemeFeesRequestV3Dto>(), It.IsAny<CancellationToken>()))
                               .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _controller.CalculateFeesAsyncV3(request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.Result.Should().BeOfType<ObjectResult>().Which.Value.Should().Be($"{ComplianceSchemeFeeCalculationExceptions.CalculationError}: {exceptionMessage}");
            }

        }
    }
}