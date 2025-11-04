using AutoFixture;
using AutoFixture.AutoMoq;
using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Common.Dtos.FeeItems;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.Enums;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Controllers.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Services.Interfaces.FeeItems;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Strategies.Interfaces.FeeItems;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EPR.Payment.Service.UnitTests.Controllers.RegistrationFees.ComplianceScheme
{
    [TestClass]
    public class ComplianceSchemeFeesV2ControllerTests
    {
        private IFixture _fixture = null!;
        private Mock<IComplianceSchemeCalculatorService> _calculatorMock = null!;
        private Mock<IValidator<ComplianceSchemeFeesRequestV2Dto>> _validatorMock = null!;
        private Mock<IFeeItemWriter> _feeItemWriterMock = null!;
        private Mock<IFeeItemSaveRequestMapper> _mapperMock = null!;
        private ComplianceSchemeFeesV2Controller _controller = null!;
        private CancellationToken _ct;

        [TestInitialize]
        public void Init()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _calculatorMock = _fixture.Freeze<Mock<IComplianceSchemeCalculatorService>>();
            _validatorMock = _fixture.Freeze<Mock<IValidator<ComplianceSchemeFeesRequestV2Dto>>>();
            _feeItemWriterMock = _fixture.Freeze<Mock<IFeeItemWriter>>();
            _mapperMock = _fixture.Freeze<Mock<IFeeItemSaveRequestMapper>>();

            _controller = new ComplianceSchemeFeesV2Controller(
                _calculatorMock.Object,
                _validatorMock.Object,
                _feeItemWriterMock.Object,
                _mapperMock.Object);

            _ct = CancellationToken.None;
        }

        [TestMethod]
        public void Constructor_WithValidArguments_ShouldInitializeCorrectly()
        {
            // Act
            var controller = new ComplianceSchemeFeesV2Controller(
                _calculatorMock.Object,
                _validatorMock.Object,
                _feeItemWriterMock.Object,
                _mapperMock.Object);

            // Assert
            using (new AssertionScope())
            {
                controller.Should().NotBeNull();
                controller.Should().BeAssignableTo<ComplianceSchemeFeesV2Controller>();
            }
        }

        [TestMethod]
        public void Constructor_WhenComplianceSchemeCalculatorServiceIsNull_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(
                () => new ComplianceSchemeFeesV2Controller(
                    null!, _validatorMock.Object, _feeItemWriterMock.Object, _mapperMock.Object),
                "Value cannot be null. (Parameter 'calculator')");
        }

        [TestMethod]
        public void Constructor_WhenComplianceSchemeFeesRequestV2DtoValidatorIsNull_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(
                () => new ComplianceSchemeFeesV2Controller(
                    _calculatorMock.Object, null!, _feeItemWriterMock.Object, _mapperMock.Object),
                "Value cannot be null. (Parameter 'validator')");
        }

        [TestMethod]
        public void Constructor_WhenFeeItemWriterIsNull_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(
                () => new ComplianceSchemeFeesV2Controller(
                    _calculatorMock.Object, _validatorMock.Object, null!, _mapperMock.Object),
                "Value cannot be null. (Parameter 'feeItemWriter')");
        }

        [TestMethod]
        public void Constructor_WhenFeeItemSaveRequestMapperIsNull_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(
                () => new ComplianceSchemeFeesV2Controller(
                    _calculatorMock.Object, _validatorMock.Object, _feeItemWriterMock.Object, null!),
                "Value cannot be null. (Parameter 'feeItemSaveRequestMapper')");
        }

        [TestMethod]
        public async Task CalculateFeesAsync_WhenValidRequest_ShouldReturnOkResultWithCalculatedFees()
        {
            // Arrange
            var request = new ComplianceSchemeFeesRequestV2Dto
            {
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "CS-2025-0001",
                SubmissionDate = DateTime.UtcNow,
                FileId = Guid.NewGuid(),
                ExternalId = Guid.NewGuid(),
                InvoicePeriod = new DateTimeOffset(DateTime.UtcNow.Date, TimeSpan.Zero),
                PayerTypeId = (int)PayerTypeIds.ComplianceScheme,
                PayerId = 123,
                ComplianceSchemeMembers = new List<ComplianceSchemeMemberDto>
                {
                    new() { MemberId = "M1", MemberType = "Large" },
                    new() { MemberId = "M2", MemberType = "Large" }
                }
            };
            var response = new ComplianceSchemeFeesResponseDto
            {
                TotalFee = 1000m,
                ComplianceSchemeRegistrationFee = 250m,
                ComplianceSchemeMembersWithFees = new List<ComplianceSchemeMembersWithFeesDto>()
            };

            _validatorMock.Setup(v => v.Validate(It.IsAny<ComplianceSchemeFeesRequestV2Dto>()))
                          .Returns(new ValidationResult());

            _calculatorMock.Setup(c => c.CalculateFeesAsync(It.IsAny<ComplianceSchemeFeesRequestV2Dto>(), _ct))
                           .ReturnsAsync(response);

            var mapped = new FeeItemSaveRequest
            {
                ApplicationReferenceNumber = request.ApplicationReferenceNumber,
                FileId = request.FileId,
                ExternalId = request.ExternalId,
                InvoicePeriod = new DateTimeOffset(request.SubmissionDate, TimeSpan.Zero),
                PayerId = request.PayerId,
                PayerTypeId = (int)PayerTypeIds.ComplianceScheme,
                Lines = new List<FeeItemLine>
                {
                    new()
                    {
                        FeeTypeId = (int)FeeTypeIds.ComplianceSchemeRegistrationFee,
                        Amount = response.ComplianceSchemeRegistrationFee,
                        Quantity = 1,
                        UnitPrice = response.ComplianceSchemeRegistrationFee
                    }
                }
            };

            _mapperMock.Setup(m => m.BuildComplianceSchemeRegistrationFeeSummaryRecord(request, response))
                       .Returns(mapped);

            _feeItemWriterMock.Setup(w => w.Save(mapped, _ct))
                              .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CalculateFeesAsync(request, _ct);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeOfType<ActionResult<ComplianceSchemeFeesResponseDto>>();
                var okResult = result.Result.Should().BeOfType<OkObjectResult>().Which;
                okResult.Value.Should().Be(response);
                _mapperMock.Verify(m => m.BuildComplianceSchemeRegistrationFeeSummaryRecord(request, response), Times.Once);
                _feeItemWriterMock.Verify(w => w.Save(mapped, _ct), Times.Once);
            }
        }

        [TestMethod]
        public async Task CalculateFeesAsync_WhenRequestValidationFails_ShouldReturnBadRequestWithValidationErrorDetails()
        {
            // Arrange
            var request = _fixture.Create<ComplianceSchemeFeesRequestV2Dto>();
            var failures = new List<ValidationFailure>
            {
                new("ApplicationReferenceNumber", "ApplicationReferenceNumber is invalid"),
                new("Regulator", "Regulator is required")
            };

            _validatorMock.Setup(v => v.Validate(It.IsAny<ComplianceSchemeFeesRequestV2Dto>()))
                          .Returns(new ValidationResult(failures));

            // Act
            var result = await _controller.CalculateFeesAsync(request, _ct);

            // Assert
            using (new AssertionScope())
            {
                var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Which;
                var problemDetails = badRequestResult.Value.Should().BeOfType<ProblemDetails>().Which;
                problemDetails.Title.Should().Be("Validation Error");
                problemDetails.Detail.Should().Be("ApplicationReferenceNumber is invalid; Regulator is required");
                problemDetails.Status.Should().Be(StatusCodes.Status400BadRequest);

                _calculatorMock.Verify(c => c.CalculateFeesAsync(It.IsAny<ComplianceSchemeFeesRequestV2Dto>(), _ct), Times.Never);
                _mapperMock.Verify(m => m.BuildComplianceSchemeRegistrationFeeSummaryRecord(
                    It.IsAny<ComplianceSchemeFeesRequestV2Dto>(), It.IsAny<ComplianceSchemeFeesResponseDto>()), Times.Never);
                _feeItemWriterMock.Verify(w => w.Save(It.IsAny<FeeItemSaveRequest>(), _ct), Times.Never);
            }
        }

        [TestMethod]
        public async Task CalculateFeesAsync_WhenCalculationThrowsValidationException_ShouldReturnBadRequestWithValidationExceptionDetails()
        {
            // Arrange
            var request = _fixture.Create<ComplianceSchemeFeesRequestV2Dto>();
            var exceptionMessage = "Validation failed";

            _validatorMock.Setup(v => v.Validate(It.IsAny<ComplianceSchemeFeesRequestV2Dto>()))
                          .Returns(new ValidationResult());

            _calculatorMock.Setup(c => c.CalculateFeesAsync(It.IsAny<ComplianceSchemeFeesRequestV2Dto>(), _ct))
                           .ThrowsAsync(new ValidationException(exceptionMessage));

            // Act
            var result = await _controller.CalculateFeesAsync(request, _ct);

            // Assert
            using (new AssertionScope())
            {
                var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Which;
                var problemDetails = badRequestResult.Value.Should().BeOfType<ProblemDetails>().Which;
                problemDetails.Title.Should().Be("Validation Error");
                problemDetails.Detail.Should().Be(exceptionMessage);
                problemDetails.Status.Should().Be(StatusCodes.Status400BadRequest);

                _mapperMock.Verify(m => m.BuildComplianceSchemeRegistrationFeeSummaryRecord(
                    It.IsAny<ComplianceSchemeFeesRequestV2Dto>(), It.IsAny<ComplianceSchemeFeesResponseDto>()), Times.Never);
                _feeItemWriterMock.Verify(w => w.Save(It.IsAny<FeeItemSaveRequest>(), _ct), Times.Never);
            }
        }

        [TestMethod]
        public async Task CalculateFeesAsync_WhenCalculationThrowsArgumentException_ShouldReturnBadRequestWithArgumentExceptionDetails()
        {
            // Arrange
            var request = _fixture.Create<ComplianceSchemeFeesRequestV2Dto>();
            var exceptionMessage = "Invalid argument";

            _validatorMock.Setup(v => v.Validate(It.IsAny<ComplianceSchemeFeesRequestV2Dto>()))
                          .Returns(new ValidationResult());

            _calculatorMock.Setup(c => c.CalculateFeesAsync(It.IsAny<ComplianceSchemeFeesRequestV2Dto>(), _ct))
                           .ThrowsAsync(new ArgumentException(exceptionMessage));

            // Act
            var result = await _controller.CalculateFeesAsync(request, _ct);

            // Assert
            using (new AssertionScope())
            {
                var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Which;
                var problemDetails = badRequestResult.Value.Should().BeOfType<ProblemDetails>().Which;
                problemDetails.Title.Should().Be("Invalid Argument");
                problemDetails.Detail.Should().Be(exceptionMessage);
                problemDetails.Status.Should().Be(StatusCodes.Status400BadRequest);

                _mapperMock.Verify(m => m.BuildComplianceSchemeRegistrationFeeSummaryRecord(
                    It.IsAny<ComplianceSchemeFeesRequestV2Dto>(), It.IsAny<ComplianceSchemeFeesResponseDto>()), Times.Never);
                _feeItemWriterMock.Verify(w => w.Save(It.IsAny<FeeItemSaveRequest>(), _ct), Times.Never);
            }
        }

        [TestMethod]
        public async Task CalculateFeesAsync_WhenCalculationThrowsException_ShouldReturnInternalServerError()
        {
            // Arrange
            var request = _fixture.Create<ComplianceSchemeFeesRequestV2Dto>();
            var exceptionMessage = "boom";

            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ComplianceSchemeFeesRequestV2Dto>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(new ValidationResult());

            _calculatorMock.Setup(c => c.CalculateFeesAsync(It.IsAny<ComplianceSchemeFeesRequestV2Dto>(), _ct))
                           .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _controller.CalculateFeesAsync(request, _ct);

            // Assert
            using (new AssertionScope())
            {
                var objectResult = result.Result.Should().BeOfType<ObjectResult>().Which;
                objectResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

                objectResult.Value.Should().Be($"{ComplianceSchemeFeeCalculationExceptions.CalculationError}: {exceptionMessage}");

                _mapperMock.Verify(m => m.BuildComplianceSchemeRegistrationFeeSummaryRecord(
                    It.IsAny<ComplianceSchemeFeesRequestV2Dto>(), It.IsAny<ComplianceSchemeFeesResponseDto>()), Times.Never);

                _feeItemWriterMock.Verify(w => w.Save(It.IsAny<FeeItemSaveRequest>(), It.IsAny<CancellationToken>()), Times.Never);
            }
        }


        [TestMethod]
        public async Task CalculateFeesAsync_WhenValidRequest_ShouldCallMapperAndWriterOnce()
        {
            // Arrange
            var request = new ComplianceSchemeFeesRequestV2Dto
            {
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "CS-2025-0001",
                SubmissionDate = DateTime.UtcNow,
                FileId = Guid.NewGuid(),
                ExternalId = Guid.NewGuid(),
                InvoicePeriod = new DateTimeOffset(DateTime.UtcNow.Date, TimeSpan.Zero),
                PayerTypeId = (int)PayerTypeIds.ComplianceScheme,
                PayerId = 123,
                ComplianceSchemeMembers = new List<ComplianceSchemeMemberDto>()
            };
            var response = new ComplianceSchemeFeesResponseDto
            {
                TotalFee = 1000m,
                ComplianceSchemeRegistrationFee = 250m,
                ComplianceSchemeMembersWithFees = new List<ComplianceSchemeMembersWithFeesDto>()
            };

            _validatorMock.Setup(v => v.Validate(It.IsAny<ComplianceSchemeFeesRequestV2Dto>()))
                          .Returns(new ValidationResult());

            _calculatorMock.Setup(c => c.CalculateFeesAsync(It.IsAny<ComplianceSchemeFeesRequestV2Dto>(), _ct))
                           .ReturnsAsync(response);

            var mapped = new FeeItemSaveRequest
            {
                ApplicationReferenceNumber = request.ApplicationReferenceNumber,
                FileId = request.FileId,
                ExternalId = request.ExternalId,
                InvoicePeriod = new DateTimeOffset(request.SubmissionDate, TimeSpan.Zero),
                PayerId = request.PayerId,
                PayerTypeId = (int)PayerTypeIds.ComplianceScheme,
                Lines = new List<FeeItemLine>
                {
                    new()
                    {
                        FeeTypeId = (int)FeeTypeIds.ComplianceSchemeRegistrationFee,
                        Amount = response.ComplianceSchemeRegistrationFee,
                        Quantity = 1,
                        UnitPrice = response.ComplianceSchemeRegistrationFee
                    }
                }
            };

            _mapperMock.Setup(m => m.BuildComplianceSchemeRegistrationFeeSummaryRecord(
                                It.Is<ComplianceSchemeFeesRequestV2Dto>(d => d == request),
                                It.Is<ComplianceSchemeFeesResponseDto>(r => r == response)))
                       .Returns(mapped);

            _feeItemWriterMock.Setup(w => w.Save(
                                It.Is<FeeItemSaveRequest>(s => s == mapped),
                                _ct))
                              .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CalculateFeesAsync(request, _ct);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeOfType<ActionResult<ComplianceSchemeFeesResponseDto>>();
                var okResult = result.Result.Should().BeOfType<OkObjectResult>().Which;
                okResult.Value.Should().Be(response);
                _mapperMock.Verify(m => m.BuildComplianceSchemeRegistrationFeeSummaryRecord(request, response), Times.Once);
                _feeItemWriterMock.Verify(w => w.Save(mapped, _ct), Times.Once);
            }
        }
    }
}