using AutoFixture;
using AutoFixture.AutoMoq;
using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using EPR.Payment.Service.Controllers.RegistrationFees;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.ComplianceScheme;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EPR.Payment.Service.UnitTests.Controllers.RegistrationFees
{
    [TestClass]
    public class ComplianceSchemeRegistrationFeeControllerTests
    {
        private IFixture _fixture = null!;
        private Mock<IComplianceSchemeBaseFeeService> _complianceSchemeBaseFeeServiceMock = null!;
        private Mock<IValidator<RegulatorDto>> _regulatorDtoValidatorMock = null!;
        private ComplianceSchemeRegistrationFeesController _controller = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _complianceSchemeBaseFeeServiceMock = _fixture.Freeze<Mock<IComplianceSchemeBaseFeeService>>();
            _regulatorDtoValidatorMock = _fixture.Freeze<Mock<IValidator<RegulatorDto>>>();
            _controller = new ComplianceSchemeRegistrationFeesController(
                _complianceSchemeBaseFeeServiceMock.Object,
                _regulatorDtoValidatorMock.Object);
        }

        [TestMethod]
        public void Constructor_WhenBaseFeeServiceIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            IComplianceSchemeBaseFeeService? baseFeeService = null;

            // Act
            Action act = () => new ComplianceSchemeRegistrationFeesController(baseFeeService!, _regulatorDtoValidatorMock.Object);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'complianceSchemeBaseFeeService')");
        }

        [TestMethod]
        public void Constructor_WhenValidatorIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            IValidator<RegulatorDto>? validator = null;

            // Act
            Action act = () => new ComplianceSchemeRegistrationFeesController(_complianceSchemeBaseFeeServiceMock.Object, validator!);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'regulatorDtoValidator')");
        }

        [TestMethod]
        public async Task GetBaseFeeAsync_WhenValidRegulator_ReturnsOkResultWithBaseFee()
        {
            // Arrange
            var expectedBaseFee = 203M;
            var regulatorDto = new RegulatorDto { Regulator = "GB-ENG" }; // Ensure this matches a valid regulator

            // Set up the validator to succeed
            _regulatorDtoValidatorMock.Setup(v => v.Validate(It.IsAny<RegulatorDto>()))
                .Returns(new ValidationResult());

            // Set up the service to return the expected base fee
            _complianceSchemeBaseFeeServiceMock
                .Setup(s => s.GetComplianceSchemeBaseFeeAsync(
                    It.IsAny<RegulatorType>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedBaseFee);

            // Act
            var result = await _controller.GetBaseFeeAsync(regulatorDto, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                var okResult = result.Should().BeOfType<OkObjectResult>().Which;
                var value = okResult.Value.Should().BeEquivalentTo(new { BaseFee = result });
            }
        }

        [TestMethod]
        public async Task GetBaseFeeAsync_WhenRequestValidationFails_ReturnsBadRequestWithValidationErrorDetails()
        {
            // Arrange
            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("Regulator", "Regulator is required")
            };

            var regulatorDto = new RegulatorDto { Regulator = "" }; // Invalid input to trigger validation failure

            _regulatorDtoValidatorMock.Setup(v => v.Validate(It.IsAny<RegulatorDto>()))
                .Returns(new ValidationResult(validationFailures));

            // Act
            var result = await _controller.GetBaseFeeAsync(regulatorDto, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Which;
                var problemDetails = badRequestResult.Value.Should().BeOfType<ProblemDetails>().Which;
                problemDetails.Detail.Should().Contain("Regulator is required");
            }
        }

        [TestMethod]
        public async Task GetBaseFeeAsync_WhenServiceThrowsValidationException_ReturnsBadRequestWithValidationExceptionDetails()
        {
            // Arrange
            var exceptionMessage = "Validation failed";
            var regulatorDto = new RegulatorDto { Regulator = "GB-ENG" };

            _regulatorDtoValidatorMock.Setup(v => v.Validate(It.IsAny<RegulatorDto>()))
                .Returns(new ValidationResult());

            _complianceSchemeBaseFeeServiceMock.Setup(s => s.GetComplianceSchemeBaseFeeAsync(It.IsAny<RegulatorType>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ValidationException(exceptionMessage));

            // Act
            var result = await _controller.GetBaseFeeAsync(regulatorDto, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Which;
                var problemDetails = badRequestResult.Value.Should().BeOfType<ProblemDetails>().Which;
                problemDetails.Detail.Should().Be(exceptionMessage);
            }
        }

        [TestMethod]
        public async Task GetBaseFeeAsync_WhenServiceThrowsArgumentException_ReturnsBadRequestWithArgumentExceptionDetails()
        {
            // Arrange
            var exceptionMessage = "Invalid argument";
            var regulatorDto = new RegulatorDto { Regulator = "GB-ENG" };

            _regulatorDtoValidatorMock.Setup(v => v.Validate(It.IsAny<RegulatorDto>()))
                .Returns(new ValidationResult());

            _complianceSchemeBaseFeeServiceMock.Setup(s => s.GetComplianceSchemeBaseFeeAsync(It.IsAny<RegulatorType>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ArgumentException(exceptionMessage));

            // Act
            var result = await _controller.GetBaseFeeAsync(regulatorDto, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Which;
                badRequestResult.Value.Should().Be(exceptionMessage);
            }
        }

        [TestMethod]
        public async Task GetBaseFeeAsync_WhenServiceThrowsException_ReturnsInternalServerErrorWithExceptionDetails()
        {
            // Arrange
            var exceptionMessage = "Unexpected error";
            var regulatorDto = new RegulatorDto { Regulator = "GB-ENG" };

            _regulatorDtoValidatorMock.Setup(v => v.Validate(It.IsAny<RegulatorDto>()))
                .Returns(new ValidationResult());

            _complianceSchemeBaseFeeServiceMock.Setup(s => s.GetComplianceSchemeBaseFeeAsync(It.IsAny<RegulatorType>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _controller.GetBaseFeeAsync(regulatorDto, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                var internalServerErrorResult = result.Should().BeOfType<ObjectResult>().Which;
                internalServerErrorResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
                internalServerErrorResult.Value.Should().Be($"{ComplianceSchemeFeeCalculationExceptions.RetrievalError}: {exceptionMessage}");
            }
        }
    }
}