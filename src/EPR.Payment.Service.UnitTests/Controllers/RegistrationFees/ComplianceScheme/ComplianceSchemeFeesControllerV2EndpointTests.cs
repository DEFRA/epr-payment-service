using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Controllers.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Services.Interfaces.FeeSummaries;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Strategies.Interfaces.FeeSummary;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EPR.Payment.Service.UnitTests.Controllers.RegistrationFees.ComplianceScheme
{
    [TestClass]
    public class ComplianceSchemeFeesControllerV2EndpointTests
    {
        private IFixture _fixture = null!;
        private Mock<IComplianceSchemeCalculatorService> _complianceSchemeCalculatorServiceMock = null!;
        private Mock<IValidator<ComplianceSchemeFeesRequestDto>> _validatorMock = null!;
        private Mock<IFeeSummaryWriter> _feeSummaryWriterMock = null!;
        private Mock<IFeeSummarySaveRequestMapper> _mapperMock = null!;
        private Mock<IValidator<ComplianceSchemeFeesRequestV2Dto>> _validatorV2Mock = null!;
        private ComplianceSchemeFeesController _controller = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _complianceSchemeCalculatorServiceMock = _fixture.Freeze<Mock<IComplianceSchemeCalculatorService>>();
            _validatorMock = _fixture.Freeze<Mock<IValidator<ComplianceSchemeFeesRequestDto>>>();
            _validatorV2Mock = _fixture.Freeze<Mock<IValidator<ComplianceSchemeFeesRequestV2Dto>>>();
            _controller = new ComplianceSchemeFeesController(
                _complianceSchemeCalculatorServiceMock.Object,
                _validatorMock.Object,
                _feeSummaryWriterMock.Object,
                _mapperMock.Object,
                _validatorV2Mock.Object);
        }

        [TestMethod]
        public void Constructor_WithValidArguments_ShouldInitializeCorrectly()
        {
            // Act
            var controller = new ComplianceSchemeFeesController(
                _complianceSchemeCalculatorServiceMock.Object,
                _validatorMock.Object,
                _feeSummaryWriterMock.Object,
                _mapperMock.Object,
                _validatorV2Mock.Object);

            // Assert
            using (new AssertionScope())
            {
                controller.Should().NotBeNull();
                controller.Should().BeAssignableTo<ComplianceSchemeFeesController>();
            }
        }

        [TestMethod]
        public void Constructor_WhenComplianceSchemeCalculatorServiceIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            IComplianceSchemeCalculatorService? baseFeeService = null;

            // Act
            Action act = () => new ComplianceSchemeFeesController(baseFeeService!, _validatorMock.Object, _feeSummaryWriterMock.Object,
                _mapperMock.Object, _validatorV2Mock.Object);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'complianceSchemeCalculatorService')");
        }

        [TestMethod]
        public void Constructor_WhenComplianceSchemeFeesRequestV2DtoValidatorV2IsNull_ThrowsArgumentNullException()
        {
            // Arrange
            IValidator<ComplianceSchemeFeesRequestV2Dto>? validatorV2 = null;

            // Act
            Action act = () => new ComplianceSchemeFeesController(_complianceSchemeCalculatorServiceMock.Object, _validatorMock.Object, _feeSummaryWriterMock.Object,
                _mapperMock.Object, validatorV2!);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'validatorV2')");
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeesAsyncV2_WhenValidRequest_ReturnsOkResultWithCalculatedFees(
            [Frozen] ComplianceSchemeFeesRequestV2Dto request,
            [Frozen] ComplianceSchemeFeesResponseDto response)
        {
            // Arrange
            _validatorV2Mock.Setup(v => v.Validate(It.IsAny<ComplianceSchemeFeesRequestV2Dto>()))
                .Returns(new ValidationResult());

            _complianceSchemeCalculatorServiceMock.Setup(s => s.CalculateFeesAsync(It.IsAny<ComplianceSchemeFeesRequestV2Dto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.CalculateFeesAsyncV2(request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                result.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(response);
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeesAsyncV2_WhenRequestValidationFails_ReturnsBadRequestWithValidationErrorDetails(
            [Frozen] ComplianceSchemeFeesRequestV2Dto request)
        {
            // Arrange
            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("ApplicationReferenceNumber", "ApplicationReferenceNumber is invalid"),
                new ValidationFailure("Regulator", "Regulator is required")
            };

            _validatorV2Mock.Setup(v => v.Validate(It.IsAny<ComplianceSchemeFeesRequestV2Dto>()))
                .Returns(new ValidationResult(validationFailures));

            // Act
            var result = await _controller.CalculateFeesAsyncV2(request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Which;
                var problemDetails = badRequestResult.Value.Should().BeOfType<ProblemDetails>().Which;
                problemDetails.Detail.Should().Be("ApplicationReferenceNumber is invalid; Regulator is required");
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeesAsyncV2_WhenCalculationThrowsValidationException_ReturnsBadRequestWithValidationExceptionDetails(
            [Frozen] ComplianceSchemeFeesRequestV2Dto request)
        {
            // Arrange
            var exceptionMessage = "Validation failed";

            _validatorV2Mock.Setup(v => v.Validate(It.IsAny<ComplianceSchemeFeesRequestV2Dto>()))
                .Returns(new ValidationResult());

            _complianceSchemeCalculatorServiceMock.Setup(s => s.CalculateFeesAsync(It.IsAny<ComplianceSchemeFeesRequestV2Dto>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ValidationException(exceptionMessage));

            // Act
            var result = await _controller.CalculateFeesAsyncV2(request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Which;
                var problemDetails = badRequestResult.Value.Should().BeOfType<ProblemDetails>().Which;
                problemDetails.Detail.Should().Be(exceptionMessage);
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeesAsyncV2_WhenCalculationThrowsArgumentException_ReturnsBadRequestWithArgumentExceptionDetails(
            [Frozen] ComplianceSchemeFeesRequestV2Dto request)
        {
            // Arrange
            var exceptionMessage = "Invalid argument";

            _validatorMock.Setup(v => v.Validate(It.IsAny<ComplianceSchemeFeesRequestV2Dto>()))
                .Returns(new ValidationResult());

            _complianceSchemeCalculatorServiceMock.Setup(s => s.CalculateFeesAsync(It.IsAny<ComplianceSchemeFeesRequestV2Dto>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ArgumentException(exceptionMessage));

            // Act
            var result = await _controller.CalculateFeesAsyncV2(request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Which;
                badRequestResult.Value.Should().Be(exceptionMessage);
            }
        }


        [TestMethod, AutoMoqData]
        public async Task CalculateFeesAsyncV2_WhenCalculationThrowsException_ShouldReturnInternalServerError(
              [Frozen] ComplianceSchemeFeesRequestV2Dto request)
        {
            // Arrange
            var exceptionMessage = "exception";
            _complianceSchemeCalculatorServiceMock.Setup(i => i.CalculateFeesAsync(It.IsAny<ComplianceSchemeFeesRequestV2Dto>(), It.IsAny<CancellationToken>()))
                               .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _controller.CalculateFeesAsyncV2(request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.Result.Should().BeOfType<ObjectResult>().Which.Value.Should().Be($"{ComplianceSchemeFeeCalculationExceptions.CalculationError}: {exceptionMessage}");
            }

        }
    }
}