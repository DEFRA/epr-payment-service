using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Dtos.Request.ResubmissionFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Response.ResubmissionFees.ComplianceScheme;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Controllers.ResubmissionFees.ComplianceScheme;
using EPR.Payment.Service.Services.Interfaces.ResubmissionFees.ComplianceScheme;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EPR.Payment.Service.UnitTests.Controllers.ResubmissionFees.ComplianceScheme
{
    [TestClass]
    public class ComplianceSchemeResubmissionControllerTests
    {
        private ComplianceSchemeResubmissionController _controller = null!;
        private Mock<IComplianceSchemeResubmissionService> _resubmissionFeeServiceMock = null!;
        private Mock<IValidator<ComplianceSchemeResubmissionFeeRequestDto>> _validatorMock = null!;
        private CancellationToken _cancellationToken;

        [TestInitialize]
        public void TestInitialize()
        {
            _resubmissionFeeServiceMock = new Mock<IComplianceSchemeResubmissionService>();
            _validatorMock = new Mock<IValidator<ComplianceSchemeResubmissionFeeRequestDto>>();
            _controller = new ComplianceSchemeResubmissionController(_resubmissionFeeServiceMock.Object, _validatorMock.Object);
            _cancellationToken = new CancellationToken();
        }

        [TestMethod]
        public void Constructor_WithValidArguments_ShouldInitializeCorrectly()
        {
            // Act
            var controller = new ComplianceSchemeResubmissionController(
                _resubmissionFeeServiceMock.Object,
                _validatorMock.Object
            );

            // Assert
            using (new AssertionScope())
            {
                controller.Should().NotBeNull();
                controller.Should().BeAssignableTo<ComplianceSchemeResubmissionController>();
            }
        }

        [TestMethod]
        public void Constructor_WithNullResubmissionFeeService_ShouldThrowArgumentNullException()
        {
            // Act
            Action act = () => new ComplianceSchemeResubmissionController(
                null!,
                _validatorMock.Object
            );

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("resubmissionFeeService");
        }

        [TestMethod]
        public void Constructor_WithNullValidator_ShouldThrowArgumentNullException()
        {
            // Act
            Action act = () => new ComplianceSchemeResubmissionController(
                _resubmissionFeeServiceMock.Object,
                null!
            );

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("validator");
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateResubmissionFeeAsync_ServiceReturnsAResult_ShouldReturnOkResponse(
            [Frozen] ComplianceSchemeResubmissionFeeRequestDto request,
            [Frozen] ComplianceSchemeResubmissionFeeResult expectedResult)
        {
            // Arrange
            request.ResubmissionDate = DateTime.UtcNow;
            _resubmissionFeeServiceMock
                .Setup(i => i.CalculateResubmissionFeeAsync(request, _cancellationToken))
                .ReturnsAsync(expectedResult);

            _validatorMock
                .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            // Act
            var result = await _controller.CalculateResubmissionFeeAsync(request, _cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeOfType<OkObjectResult>();
                result.As<OkObjectResult>().Value.Should().BeEquivalentTo(expectedResult);
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateResubmissionFeeAsync_ServiceThrowsValidationException_ShouldReturnBadRequest(
            [Frozen] ComplianceSchemeResubmissionFeeRequestDto request)
        {
            // Arrange
            request.ResubmissionDate = DateTime.UtcNow;
            _resubmissionFeeServiceMock
                .Setup(s => s.CalculateResubmissionFeeAsync(request, _cancellationToken))
                .ThrowsAsync(new ValidationException("Validation error"));

            _validatorMock
                .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            // Act
            var result = await _controller.CalculateResubmissionFeeAsync(request, _cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeOfType<BadRequestObjectResult>();
                var badRequestResult = result as BadRequestObjectResult;
                badRequestResult.Should().NotBeNull();
                badRequestResult!.Value.As<ProblemDetails>().Title.Should().Be("Validation Error");
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateResubmissionFeeAsync_ServiceThrowsArgumentException_ShouldReturnBadRequest(
            [Frozen] ComplianceSchemeResubmissionFeeRequestDto request)
        {
            // Arrange
            request.ResubmissionDate = DateTime.UtcNow;
            _resubmissionFeeServiceMock
                .Setup(service => service.CalculateResubmissionFeeAsync(request, _cancellationToken))
                .ThrowsAsync(new ArgumentException("Argument error"));

            _validatorMock
                .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            // Act
            var result = await _controller.CalculateResubmissionFeeAsync(request, _cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeOfType<BadRequestObjectResult>();
                var badRequestResult = result as BadRequestObjectResult;
                badRequestResult.Should().NotBeNull();
                badRequestResult!.Value.As<ProblemDetails>().Title.Should().Be("Argument Error");
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateResubmissionFeeAsync_ServiceThrowsException_ShouldReturnInternalServerError(
            [Frozen] ComplianceSchemeResubmissionFeeRequestDto request)
        {
            // Arrange
            request.ResubmissionDate = DateTime.UtcNow;
            _resubmissionFeeServiceMock
                .Setup(s => s.CalculateResubmissionFeeAsync(request, _cancellationToken))
                .ThrowsAsync(new Exception("Unexpected error"));

            _validatorMock
                .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            // Act
            var result = await _controller.CalculateResubmissionFeeAsync(request, _cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeOfType<ObjectResult>()
                    .Which.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
                result.As<ObjectResult>().Value.As<ProblemDetails>().Title.Should().Be("Internal Server Error");
            }
        }

        [TestMethod]
        public async Task CalculateResubmissionFeeAsync_InvalidRequest_ShouldReturnBadRequest()
        {
            // Arrange
            var request = new ComplianceSchemeResubmissionFeeRequestDto
            {
                Regulator = "GB-ENG",
                ReferenceNumber = "12345",
                ResubmissionDate = DateTime.UtcNow,
                MemberCount = 1
            };

            _controller.ModelState.AddModelError("MemberCount", "Member count is required");

            _validatorMock
                .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult(new[]
                {
                    new FluentValidation.Results.ValidationFailure("MemberCount", "Member count is required")
                }));

            // Act
            var result = await _controller.CalculateResubmissionFeeAsync(request, _cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeOfType<BadRequestObjectResult>();
                var badRequestResult = result as BadRequestObjectResult;
                badRequestResult.Should().NotBeNull();
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateResubmissionFeeAsync_WithNonUtcResubmissionDate_ShouldReturnBadRequest(
            [Frozen] ComplianceSchemeResubmissionFeeRequestDto request)
        {
            // Arrange
            request.ResubmissionDate = DateTime.Now; // Non-UTC date

            _validatorMock
                .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult(new[]
                {
                    new FluentValidation.Results.ValidationFailure("ResubmissionDate", "Resubmission date must be in UTC.")
                }));

            // Act
            var result = await _controller.CalculateResubmissionFeeAsync(request, _cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeOfType<BadRequestObjectResult>();
                var badRequestResult = result as BadRequestObjectResult;
                badRequestResult.Should().NotBeNull();
                badRequestResult!.Value.As<ProblemDetails>().Title.Should().Be("Validation Error");
                badRequestResult!.Value.As<ProblemDetails>().Detail.Should().Contain("Resubmission date must be in UTC.");
            }
        }
    }
}
