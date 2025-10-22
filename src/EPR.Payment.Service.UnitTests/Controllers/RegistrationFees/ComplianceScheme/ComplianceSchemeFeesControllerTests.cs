using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Common.Dtos.FeeSummaries;
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
    public class ComplianceSchemeFeesControllerTests
    {
        private IFixture _fixture = null!;
        private Mock<IComplianceSchemeCalculatorService> _complianceSchemeCalculatorServiceMock = null!;
        private Mock<IValidator<ComplianceSchemeFeesRequestDto>> _validatorMock = null!;
        private Mock<IFeeSummaryWriter> _feeSummaryWriterMock = null!;
        private Mock<IFeeSummarySaveRequestMapper> _mapperMock = null!;
        private ComplianceSchemeFeesController _controller = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _complianceSchemeCalculatorServiceMock = _fixture.Freeze<Mock<IComplianceSchemeCalculatorService>>();
            _validatorMock = _fixture.Freeze<Mock<IValidator<ComplianceSchemeFeesRequestDto>>>();
            _feeSummaryWriterMock = _fixture.Freeze<Mock<IFeeSummaryWriter>>();
            _mapperMock = _fixture.Freeze<Mock<IFeeSummarySaveRequestMapper>>();

            _controller = new ComplianceSchemeFeesController(
                _complianceSchemeCalculatorServiceMock.Object,
                _validatorMock.Object,
                _feeSummaryWriterMock.Object,
                _mapperMock.Object);
        }

        [TestMethod]
        public void Constructor_WithValidArguments_ShouldInitializeCorrectly()
        {
            // Act
            var controller = new ComplianceSchemeFeesController(
                _complianceSchemeCalculatorServiceMock.Object,
                _validatorMock.Object,
                _feeSummaryWriterMock.Object,
                _mapperMock.Object);

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
            Action act = () => new ComplianceSchemeFeesController(
                baseFeeService!,
                _validatorMock.Object,
                _feeSummaryWriterMock.Object,
                _mapperMock.Object);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'complianceSchemeCalculatorService')");
        }

        [TestMethod]
        public void Constructor_WhenComplianceSchemeFeesRequestDtoValidatorIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            IValidator<ComplianceSchemeFeesRequestDto>? validator = null;

            // Act
            Action act = () => new ComplianceSchemeFeesController(
                _complianceSchemeCalculatorServiceMock.Object,
                validator!,
                _feeSummaryWriterMock.Object,
                _mapperMock.Object);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'validator')");
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeesAsync_WhenValidRequest_ReturnsOkResultWithCalculatedFees(
            [Frozen] ComplianceSchemeFeesRequestDto request,
            [Frozen] ComplianceSchemeFeesResponseDto response)
        {
            // Arrange
            _validatorMock.Setup(v => v.Validate(It.IsAny<ComplianceSchemeFeesRequestDto>()))
                .Returns(new ValidationResult());

            _complianceSchemeCalculatorServiceMock.Setup(s => s.CalculateFeesAsync(It.IsAny<ComplianceSchemeFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.CalculateFeesAsync(request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                result.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(response);
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeesAsync_WhenRequestValidationFails_ReturnsBadRequestWithValidationErrorDetails(
            [Frozen] ComplianceSchemeFeesRequestDto request)
        {
            // Arrange
            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("ApplicationReferenceNumber", "ApplicationReferenceNumber is invalid"),
                new ValidationFailure("Regulator", "Regulator is required")
            };

            _validatorMock.Setup(v => v.Validate(It.IsAny<ComplianceSchemeFeesRequestDto>()))
                .Returns(new ValidationResult(validationFailures));

            // Act
            var result = await _controller.CalculateFeesAsync(request, CancellationToken.None);

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
            [Frozen] ComplianceSchemeFeesRequestDto request)
        {
            // Arrange
            var exceptionMessage = "Validation failed";

            _validatorMock.Setup(v => v.Validate(It.IsAny<ComplianceSchemeFeesRequestDto>()))
                .Returns(new ValidationResult());

            _complianceSchemeCalculatorServiceMock.Setup(s => s.CalculateFeesAsync(It.IsAny<ComplianceSchemeFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ValidationException(exceptionMessage));

            // Act
            var result = await _controller.CalculateFeesAsync(request, CancellationToken.None);

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
            [Frozen] ComplianceSchemeFeesRequestDto request)
        {
            // Arrange
            var exceptionMessage = "Invalid argument";

            _validatorMock.Setup(v => v.Validate(It.IsAny<ComplianceSchemeFeesRequestDto>()))
                .Returns(new ValidationResult());

            _complianceSchemeCalculatorServiceMock.Setup(s => s.CalculateFeesAsync(It.IsAny<ComplianceSchemeFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ArgumentException(exceptionMessage));

            // Act
            var result = await _controller.CalculateFeesAsync(request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Which;
                badRequestResult.Value.Should().Be(exceptionMessage);
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeesAsync_WhenCalculationThrowsException_ShouldReturnInternalServerError(
              [Frozen] ComplianceSchemeFeesRequestDto request)
        {
            // Arrange
            var exceptionMessage = "exception";
            _complianceSchemeCalculatorServiceMock.Setup(i => i.CalculateFeesAsync(It.IsAny<ComplianceSchemeFeesRequestDto>(), It.IsAny<CancellationToken>()))
                               .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _controller.CalculateFeesAsync(request, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.Result.Should().BeOfType<ObjectResult>().Which.Value.Should().Be($"{ComplianceSchemeFeeCalculationExceptions.CalculationError}: {exceptionMessage}");
            }
        }

        [TestMethod]
        public async Task CalculateFeesAsync_WhenIdentifiersPresent_CallsFeeSummaryWriterSave()
        {
            // Arrange
            var request = new ComplianceSchemeFeesRequestDto
            {
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "CS-2025-0001",
                SubmissionDate = DateTime.UtcNow,
                FileId = Guid.NewGuid(),
                ExternalId = Guid.NewGuid(),
                PayerId = 123,
                ComplianceSchemeMembers = new()
            };

            var response = new ComplianceSchemeFeesResponseDto
            {
                TotalFee = 1000m,
                ComplianceSchemeRegistrationFee = 250m,
                ComplianceSchemeMembersWithFees = new()
            };

            _validatorMock.Setup(v => v.Validate(It.IsAny<ComplianceSchemeFeesRequestDto>()))
                .Returns(new ValidationResult());

            _complianceSchemeCalculatorServiceMock
                .Setup(s => s.CalculateFeesAsync(It.IsAny<ComplianceSchemeFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            _mapperMock
                .Setup(m => m.BuildComplianceSchemeRegistrationFeeSummaryRecord(
                    It.Is<ComplianceSchemeFeesRequestDto>(d => d == request),
                    It.IsAny<DateTimeOffset>(),
                    It.IsAny<int>(),
                    It.Is<ComplianceSchemeFeesResponseDto>(r => r == response),
                    It.IsAny<DateTimeOffset?>()))
                .Returns(() =>
                {
                    return new FeeSummarySaveRequest
                    {
                        ApplicationReferenceNumber = request.ApplicationReferenceNumber,
                        FileId = request.FileId!.Value,
                        ExternalId = request.ExternalId!.Value,
                        InvoicePeriod = new DateTimeOffset(request.SubmissionDate, TimeSpan.Zero),
                        PayerId = request.PayerId!.Value,
                        PayerTypeId = 2,
                        Lines = new List<FeeSummaryLineRequest>
                        {
                            new FeeSummaryLineRequest
                            {
                                FeeTypeId = 2,
                                Amount = response.ComplianceSchemeRegistrationFee,
                                Quantity = 1,
                                UnitPrice = response.ComplianceSchemeRegistrationFee
                            }
                        }
                    };
                });

            // Act
            var result = await _controller.CalculateFeesAsync(request, CancellationToken.None);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();

            _feeSummaryWriterMock.Verify(w => w.Save(
                It.Is<FeeSummarySaveRequest>(save =>
                    save.ApplicationReferenceNumber == request.ApplicationReferenceNumber &&
                    save.FileId == request.FileId &&
                    save.ExternalId == request.ExternalId &&
                    save.InvoicePeriod.Date == request.SubmissionDate.Date &&
                    save.Lines != null && save.Lines.Count > 0
                ), CancellationToken.None), Times.Once);
        }

        [TestMethod]
        public async Task CalculateFeesAsync_WhenIdentifiersMissing_DoesNotCallFeeSummaryWriterSave()
        {
            // Arrange
            var request = new ComplianceSchemeFeesRequestDto
            {
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "CS-2025-0002",
                SubmissionDate = DateTime.UtcNow,
                FileId = null,
                ExternalId = null,
                PayerId = null,
                ComplianceSchemeMembers = new()
            };

            var response = new ComplianceSchemeFeesResponseDto
            {
                TotalFee = 500m,
                ComplianceSchemeRegistrationFee = 200m,
                ComplianceSchemeMembersWithFees = new()
            };

            _validatorMock.Setup(v => v.Validate(It.IsAny<ComplianceSchemeFeesRequestDto>()))
                .Returns(new ValidationResult());

            _complianceSchemeCalculatorServiceMock
                .Setup(s => s.CalculateFeesAsync(It.IsAny<ComplianceSchemeFeesRequestDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.CalculateFeesAsync(request, CancellationToken.None);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            _feeSummaryWriterMock.Verify(w => w.Save(It.IsAny<FeeSummarySaveRequest>(), CancellationToken.None), Times.Never);
            _mapperMock.Verify(m => m.BuildComplianceSchemeRegistrationFeeSummaryRecord(
                It.IsAny<ComplianceSchemeFeesRequestDto>(),
                It.IsAny<DateTimeOffset>(),
                It.IsAny<int>(),
                It.IsAny<ComplianceSchemeFeesResponseDto>(),
                It.IsAny<DateTimeOffset?>()), Times.Never);
        }
    }
}
