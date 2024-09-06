using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
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
        private Mock<IValidator<string>> _regulatorValidatorMock = null!;
        private ComplianceSchemeRegistrationFeesController _controller = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _complianceSchemeBaseFeeServiceMock = _fixture.Freeze<Mock<IComplianceSchemeBaseFeeService>>();
            _regulatorValidatorMock = _fixture.Freeze<Mock<IValidator<string>>>();
            _controller = new ComplianceSchemeRegistrationFeesController(
                _complianceSchemeBaseFeeServiceMock.Object,
                _regulatorValidatorMock.Object);
        }

        [TestMethod]
        [AutoMoqData]
        public void Constructor_WhenBaseFeeServiceIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            IComplianceSchemeBaseFeeService? baseFeeService = null;

            // Act
            Action act = () => new ComplianceSchemeRegistrationFeesController(baseFeeService!, _regulatorValidatorMock.Object);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'complianceSchemeBaseFeeService')");
        }

        [TestMethod]
        [AutoMoqData]
        public void Constructor_WhenValidatorIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            IValidator<string>? validator = null;

            // Act
            Action act = () => new ComplianceSchemeRegistrationFeesController(_complianceSchemeBaseFeeServiceMock.Object, validator!);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'regulatorValidator')");
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetBaseFeeAsync_WhenValidRegulator_ReturnsOkResultWithBaseFee(
            [Frozen] string regulator,
            decimal baseFee)
        {
            // Arrange
            _regulatorValidatorMock.Setup(v => v.Validate(It.IsAny<string>()))
                .Returns(new ValidationResult());

            _complianceSchemeBaseFeeServiceMock.Setup(s => s.GetComplianceSchemeBaseFeeAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(baseFee);

            // Act
            var result = await _controller.GetBaseFeeAsync(regulator, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                var okResult = result.Should().BeOfType<OkObjectResult>().Which;
                var value = okResult.Value.Should().BeEquivalentTo(new { BaseFee = baseFee });
            }
        }


        [TestMethod]
        [AutoMoqData]
        public async Task GetBaseFeeAsync_WhenRequestValidationFails_ReturnsBadRequestWithValidationErrorDetails(
            [Frozen] string regulator)
        {
            // Arrange
            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("regulator", "Regulator is required")
            };

            _regulatorValidatorMock.Setup(v => v.Validate(It.IsAny<string>()))
                .Returns(new ValidationResult(validationFailures));

            // Act
            var result = await _controller.GetBaseFeeAsync(regulator, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Which;
                var problemDetails = badRequestResult.Value.Should().BeOfType<ProblemDetails>().Which;
                problemDetails.Detail.Should().Be("Regulator is required");
            }
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetBaseFeeAsync_WhenServiceThrowsValidationException_ReturnsBadRequestWithValidationExceptionDetails(
            [Frozen] string regulator)
        {
            // Arrange
            var exceptionMessage = "Validation failed";

            _regulatorValidatorMock.Setup(v => v.Validate(It.IsAny<string>()))
                .Returns(new ValidationResult());

            _complianceSchemeBaseFeeServiceMock.Setup(s => s.GetComplianceSchemeBaseFeeAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ValidationException(exceptionMessage));

            // Act
            var result = await _controller.GetBaseFeeAsync(regulator, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Which;
                var problemDetails = badRequestResult.Value.Should().BeOfType<ProblemDetails>().Which;
                problemDetails.Detail.Should().Be(exceptionMessage);
            }
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetBaseFeeAsync_WhenServiceThrowsArgumentException_ReturnsBadRequestWithArgumentExceptionDetails(
            [Frozen] string regulator)
        {
            // Arrange
            var exceptionMessage = "Invalid argument";

            _regulatorValidatorMock.Setup(v => v.Validate(It.IsAny<string>()))
                .Returns(new ValidationResult());

            _complianceSchemeBaseFeeServiceMock.Setup(s => s.GetComplianceSchemeBaseFeeAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ArgumentException(exceptionMessage));

            // Act
            var result = await _controller.GetBaseFeeAsync(regulator, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Which;
                badRequestResult.Value.Should().Be(exceptionMessage);
            }
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetBaseFeeAsync_WhenServiceThrowsException_ReturnsInternalServerErrorWithExceptionDetails(
            [Frozen] string regulator)
        {
            // Arrange
            var exceptionMessage = "Unexpected error";

            _regulatorValidatorMock.Setup(v => v.Validate(It.IsAny<string>()))
                .Returns(new ValidationResult());

            _complianceSchemeBaseFeeServiceMock.Setup(s => s.GetComplianceSchemeBaseFeeAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _controller.GetBaseFeeAsync(regulator, CancellationToken.None);

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
