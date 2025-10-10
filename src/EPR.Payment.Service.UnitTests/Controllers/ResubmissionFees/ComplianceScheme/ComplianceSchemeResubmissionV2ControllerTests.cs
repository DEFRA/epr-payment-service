using AutoFixture;
using AutoFixture.AutoMoq;
using EPR.Payment.Service.Common.Dtos.FeeItems;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Request.ResubmissionFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Response.ResubmissionFees.ComplianceScheme;
using EPR.Payment.Service.Common.Enums;
using EPR.Payment.Service.Controllers.ResubmissionFees.ComplianceScheme;
using EPR.Payment.Service.Services.Interfaces.FeeItems;
using EPR.Payment.Service.Services.Interfaces.ResubmissionFees.ComplianceScheme;
using EPR.Payment.Service.Strategies.Interfaces.FeeItems;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EPR.Payment.Service.UnitTests.Controllers.ResubmissionFees.ComplianceScheme
{
    [TestClass]
    public class ComplianceSchemeResubmissionV2ControllerTests
    {
        private IFixture _fixture = null!;
        private ComplianceSchemeResubmissionV2Controller _controller = null!;
        private Mock<IComplianceSchemeResubmissionService> _serviceMock = null!;
        private Mock<IValidator<ComplianceSchemeResubmissionFeeRequestV2Dto>> _validatorMock = null!;
        private Mock<IFeeItemWriter> _feeItemWriterMock = null!;
        private Mock<IFeeItemSaveRequestMapper> _mapperMock = null!;
        private CancellationToken _ct;

        [TestInitialize]
        public void SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _serviceMock = _fixture.Freeze<Mock<IComplianceSchemeResubmissionService>>();
            _validatorMock = _fixture.Freeze<Mock<IValidator<ComplianceSchemeResubmissionFeeRequestV2Dto>>>();
            _feeItemWriterMock = _fixture.Freeze<Mock<IFeeItemWriter>>();
            _mapperMock = _fixture.Freeze<Mock<IFeeItemSaveRequestMapper>>();

            _controller = new ComplianceSchemeResubmissionV2Controller(
                _serviceMock.Object,
                _validatorMock.Object,
                _feeItemWriterMock.Object,
                _mapperMock.Object);

            _ct = new CancellationToken();
        }

        [TestMethod]
        public void Constructor_WithValidArguments_ShouldInitializeCorrectly()
        {
            // Act
            var controller = new ComplianceSchemeResubmissionV2Controller(
                _serviceMock.Object,
                _validatorMock.Object,
                _feeItemWriterMock.Object,
                _mapperMock.Object);

            // Assert
            using (new AssertionScope())
            {
                controller.Should().NotBeNull();
                controller.Should().BeAssignableTo<ComplianceSchemeResubmissionV2Controller>();
            }
        }

        [TestMethod]
        public void Constructor_WithNullResubmissionService_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => new ComplianceSchemeResubmissionV2Controller(
                null!, _validatorMock.Object, _feeItemWriterMock.Object, _mapperMock.Object),
                "Value cannot be null. (Parameter 'resubmissionService')");
        }

        [TestMethod]
        public void Constructor_WithNullValidator_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => new ComplianceSchemeResubmissionV2Controller(
                _serviceMock.Object, null!, _feeItemWriterMock.Object, _mapperMock.Object),
                "Value cannot be null. (Parameter 'validator')");
        }

        [TestMethod]
        public void Constructor_WithNullFeeItemWriter_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => new ComplianceSchemeResubmissionV2Controller(
                _serviceMock.Object, _validatorMock.Object, null!, _mapperMock.Object),
                "Value cannot be null. (Parameter 'feeItemWriter')");
        }

        [TestMethod]
        public void Constructor_WithNullFeeItemSaveRequestMapper_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => new ComplianceSchemeResubmissionV2Controller(
                _serviceMock.Object, _validatorMock.Object, _feeItemWriterMock.Object, null!),
                "Value cannot be null. (Parameter 'feeItemSaveRequestMapper')");
        }

        [TestMethod]
        public async Task CalculateResubmissionFeeAsync_ServiceReturnsAResult_ShouldReturnOkResponse()
        {
            // Arrange
            var subDate = DateTime.UtcNow;
            var req = new ComplianceSchemeResubmissionFeeRequestV2Dto
            {
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "CS-RES-123",
                SubmissionDate = subDate,
                FileId = Guid.NewGuid(),
                ExternalId = Guid.NewGuid(),
                InvoicePeriod = new DateTimeOffset(subDate, TimeSpan.Zero),
                PayerTypeId = (int)PayerTypeIds.ComplianceScheme,
                PayerId = 321,
                ComplianceSchemeMembers = new List<ComplianceSchemeMemberDto>
                {
                    new() { MemberId = "M1", MemberType = "Large" },
                    new() { MemberId = "M2", MemberType = "Large" }
                }
            };

            var calcResult = new ComplianceSchemeResubmissionFeeResult
            {
                TotalResubmissionFee = 555.55m
            };

            _validatorMock
                .Setup(v => v.ValidateAsync(req, _ct))
                .ReturnsAsync(new ValidationResult());

            _serviceMock
                .Setup(s => s.CalculateResubmissionFeeAsync(
                    It.Is<ComplianceSchemeResubmissionFeeRequestDto>(v1 =>
                        v1.Regulator == req.Regulator &&
                        v1.ReferenceNumber == req.ApplicationReferenceNumber &&
                        v1.ResubmissionDate == req.SubmissionDate &&
                        v1.MemberCount == req.ComplianceSchemeMembers!.Count),
                    _ct))
                .ReturnsAsync(calcResult)
                .Verifiable();

            var mappedSave = new FeeItemSaveRequest
            {
                ApplicationReferenceNumber = req.ApplicationReferenceNumber,
                FileId = req.FileId,
                ExternalId = req.ExternalId,
                PayerTypeId = req.PayerTypeId,
                PayerId = req.PayerId,
                InvoicePeriod = new DateTimeOffset(req.SubmissionDate, TimeSpan.Zero),
                InvoiceDate = DateTimeOffset.UtcNow,
                Lines = new List<FeeItemLine>
                {
                    new()
                    {
                        FeeTypeId = (int)FeeTypeIds.ComplianceSchemeResubmissionFee,
                        UnitPrice = calcResult.TotalResubmissionFee,
                        Quantity = 1,
                        Amount = calcResult.TotalResubmissionFee
                    }
                }
            };

            _mapperMock
                .Setup(m => m.BuildComplianceSchemeResubmissionFeeSummaryRecord(
                    req, calcResult, (int)FeeTypeIds.ComplianceSchemeResubmissionFee))
                .Returns(mappedSave)
                .Verifiable();

            _feeItemWriterMock
                .Setup(w => w.Save(mappedSave, _ct))
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            var result = await _controller.CalculateResubmissionFeeAsync(req, _ct);

            // Assert
            using (new AssertionScope())
            {
                result.Result.Should().BeOfType<OkObjectResult>()
                    .Which.Value.Should().Be(calcResult);
                _serviceMock.Verify();
                _mapperMock.Verify();
                _feeItemWriterMock.Verify();
            }
        }

        [TestMethod]
        public async Task CalculateResubmissionFeeAsync_InvalidRequest_ShouldReturnBadRequest()
        {
            // Arrange
            var req = new ComplianceSchemeResubmissionFeeRequestV2Dto
            {
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "CS-RES-ERR",
                SubmissionDate = DateTime.UtcNow,
                FileId = Guid.NewGuid(),
                ExternalId = Guid.NewGuid(),
                InvoicePeriod = new DateTimeOffset(DateTime.UtcNow, TimeSpan.Zero),
                PayerTypeId = (int)PayerTypeIds.ComplianceScheme,
                PayerId = 0,
                ComplianceSchemeMembers = new List<ComplianceSchemeMemberDto>()
            };

            var failures = new List<ValidationFailure>
            {
                new("PayerId", "PayerId must be greater than zero")
            };

            _validatorMock
                .Setup(v => v.ValidateAsync(req, _ct))
                .ReturnsAsync(new ValidationResult(failures));

            // Act
            var result = await _controller.CalculateResubmissionFeeAsync(req, _ct);

            // Assert
            using (new AssertionScope())
            {
                result.Result.Should().BeOfType<BadRequestObjectResult>();
                var badRequestResult = result.Result.As<BadRequestObjectResult>();
                badRequestResult.Should().NotBeNull();
                badRequestResult!.Value.As<ProblemDetails>().Title.Should().Be("Validation Error");
                badRequestResult!.Value.As<ProblemDetails>().Detail.Should().Contain("PayerId must be greater than zero");
                badRequestResult!.Value.As<ProblemDetails>().Status.Should().Be(StatusCodes.Status400BadRequest);

                _serviceMock.Verify(s => s.CalculateResubmissionFeeAsync(
                    It.IsAny<ComplianceSchemeResubmissionFeeRequestDto>(), It.IsAny<CancellationToken>()),
                    Times.Never);

                _mapperMock.Verify(m => m.BuildComplianceSchemeResubmissionFeeSummaryRecord(
                    It.IsAny<ComplianceSchemeResubmissionFeeRequestV2Dto>(),
                    It.IsAny<ComplianceSchemeResubmissionFeeResult>(),
                    It.IsAny<int>()),
                    Times.Never);

                _feeItemWriterMock.Verify(w => w.Save(
                    It.IsAny<FeeItemSaveRequest>(), It.IsAny<CancellationToken>()),
                    Times.Never);
            }
        }

        [TestMethod]
        public async Task CalculateResubmissionFeeAsync_ServiceThrowsValidationException_ShouldReturnBadRequest()
        {
            // Arrange
            var req = new ComplianceSchemeResubmissionFeeRequestV2Dto
            {
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "CS-RES-VAL",
                SubmissionDate = DateTime.UtcNow,
                FileId = Guid.NewGuid(),
                ExternalId = Guid.NewGuid(),
                InvoicePeriod = new DateTimeOffset(DateTime.UtcNow, TimeSpan.Zero),
                PayerTypeId = (int)PayerTypeIds.ComplianceScheme,
                PayerId = 100,
                ComplianceSchemeMembers = new List<ComplianceSchemeMemberDto>()
            };

            _validatorMock
                .Setup(v => v.ValidateAsync(req, _ct))
                .ReturnsAsync(new ValidationResult());

            _serviceMock
                .Setup(s => s.CalculateResubmissionFeeAsync(It.Is<ComplianceSchemeResubmissionFeeRequestDto>(v1 =>
                    v1.Regulator == req.Regulator &&
                    v1.ReferenceNumber == req.ApplicationReferenceNumber &&
                    v1.ResubmissionDate == req.SubmissionDate &&
                    v1.MemberCount == 0), _ct))
                .ThrowsAsync(new ValidationException("Validation failed"));

            // Act
            var result = await _controller.CalculateResubmissionFeeAsync(req, _ct);

            // Assert
            using (new AssertionScope())
            {
                result.Result.Should().BeOfType<BadRequestObjectResult>();
                var badRequestResult = result.Result.As<BadRequestObjectResult>();
                badRequestResult.Should().NotBeNull();
                badRequestResult!.Value.As<ProblemDetails>().Title.Should().Be("Validation Error");
                badRequestResult!.Value.As<ProblemDetails>().Detail.Should().Be("Validation failed");
                badRequestResult!.Value.As<ProblemDetails>().Status.Should().Be(StatusCodes.Status400BadRequest);

                _mapperMock.Verify(m => m.BuildComplianceSchemeResubmissionFeeSummaryRecord(
                    It.IsAny<ComplianceSchemeResubmissionFeeRequestV2Dto>(),
                    It.IsAny<ComplianceSchemeResubmissionFeeResult>(),
                    It.IsAny<int>()),
                    Times.Never);

                _feeItemWriterMock.Verify(w => w.Save(It.IsAny<FeeItemSaveRequest>(), _ct), Times.Never);
            }
        }

        [TestMethod]
        public async Task CalculateResubmissionFeeAsync_ServiceThrowsArgumentException_ShouldReturnBadRequest()
        {
            // Arrange
            var req = new ComplianceSchemeResubmissionFeeRequestV2Dto
            {
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "CS-RES-ARG",
                SubmissionDate = DateTime.UtcNow,
                FileId = Guid.NewGuid(),
                ExternalId = Guid.NewGuid(),
                InvoicePeriod = new DateTimeOffset(DateTime.UtcNow, TimeSpan.Zero),
                PayerTypeId = (int)PayerTypeIds.ComplianceScheme,
                PayerId = 999,
                ComplianceSchemeMembers = new List<ComplianceSchemeMemberDto>()
            };

            _validatorMock
                .Setup(v => v.ValidateAsync(req, _ct))
                .ReturnsAsync(new ValidationResult());

            _serviceMock
                .Setup(s => s.CalculateResubmissionFeeAsync(It.Is<ComplianceSchemeResubmissionFeeRequestDto>(v1 =>
                    v1.Regulator == req.Regulator &&
                    v1.ReferenceNumber == req.ApplicationReferenceNumber &&
                    v1.ResubmissionDate == req.SubmissionDate &&
                    v1.MemberCount == 0), _ct))
                .ThrowsAsync(new ArgumentException("Invalid argument"));

            // Act
            var result = await _controller.CalculateResubmissionFeeAsync(req, _ct);

            // Assert
            using (new AssertionScope())
            {
                result.Result.Should().BeOfType<BadRequestObjectResult>();
                var badRequestResult = result.Result.As<BadRequestObjectResult>();
                badRequestResult.Should().NotBeNull();
                badRequestResult!.Value.As<ProblemDetails>().Title.Should().Be("Invalid Argument");
                badRequestResult!.Value.As<ProblemDetails>().Detail.Should().Be("Invalid argument");
                badRequestResult!.Value.As<ProblemDetails>().Status.Should().Be(StatusCodes.Status400BadRequest);

                _mapperMock.Verify(m => m.BuildComplianceSchemeResubmissionFeeSummaryRecord(
                    It.IsAny<ComplianceSchemeResubmissionFeeRequestV2Dto>(),
                    It.IsAny<ComplianceSchemeResubmissionFeeResult>(),
                    It.IsAny<int>()),
                    Times.Never);

                _feeItemWriterMock.Verify(w => w.Save(It.IsAny<FeeItemSaveRequest>(), _ct), Times.Never);
            }
        }

        [TestMethod]
        public async Task CalculateResubmissionFeeAsync_ServiceThrowsException_ShouldReturnInternalServerError()
        {
            // Arrange
            var req = new ComplianceSchemeResubmissionFeeRequestV2Dto
            {
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "CS-RES-EX",
                SubmissionDate = DateTime.UtcNow,
                FileId = Guid.NewGuid(),
                ExternalId = Guid.NewGuid(),
                InvoicePeriod = new DateTimeOffset(DateTime.UtcNow, TimeSpan.Zero),
                PayerTypeId = (int)PayerTypeIds.ComplianceScheme,
                PayerId = 111,
                ComplianceSchemeMembers = new List<ComplianceSchemeMemberDto>()
            };

            _validatorMock
                .Setup(v => v.ValidateAsync(req, _ct))
                .ReturnsAsync(new ValidationResult());

            _serviceMock
                .Setup(s => s.CalculateResubmissionFeeAsync(It.Is<ComplianceSchemeResubmissionFeeRequestDto>(v1 =>
                    v1.Regulator == req.Regulator &&
                    v1.ReferenceNumber == req.ApplicationReferenceNumber &&
                    v1.ResubmissionDate == req.SubmissionDate &&
                    v1.MemberCount == 0), _ct))
                .ThrowsAsync(new Exception("Boom"));

            // Act
            var result = await _controller.CalculateResubmissionFeeAsync(req, _ct);

            // Assert
            using (new AssertionScope())
            {
                result.Result.Should().BeOfType<ObjectResult>()
                    .Which.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

                var problemDetails = result.Result.As<ObjectResult>().Value.As<ProblemDetails>();
                problemDetails.Title.Should().Be("Internal Server Error");
                problemDetails.Detail.Should().Be("An error occurred while calculating the compliance scheme fees.: Boom");
                problemDetails.Status.Should().Be(StatusCodes.Status500InternalServerError);

                _mapperMock.Verify(m => m.BuildComplianceSchemeResubmissionFeeSummaryRecord(
                    It.IsAny<ComplianceSchemeResubmissionFeeRequestV2Dto>(),
                    It.IsAny<ComplianceSchemeResubmissionFeeResult>(),
                    It.IsAny<int>()),
                    Times.Never);

                _feeItemWriterMock.Verify(w => w.Save(It.IsAny<FeeItemSaveRequest>(), _ct), Times.Never);
            }
        }

        [TestMethod]
        public async Task CalculateResubmissionFeeAsync_WithNonUtcSubmissionDate_ShouldReturnBadRequest()
        {
            // Arrange
            var nonUtcDate = new DateTime(2025, 10, 3, 12, 0, 0, DateTimeKind.Unspecified);
            var req = new ComplianceSchemeResubmissionFeeRequestV2Dto
            {
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "CS-RES-NONUTC",
                SubmissionDate = nonUtcDate,
                FileId = Guid.NewGuid(),
                ExternalId = Guid.NewGuid(),
                InvoicePeriod = new DateTimeOffset(nonUtcDate, TimeSpan.FromHours(1)), 
                PayerTypeId = (int)PayerTypeIds.ComplianceScheme,
                PayerId = 123,
                ComplianceSchemeMembers = new List<ComplianceSchemeMemberDto>()
            };

            _validatorMock
                .Setup(v => v.ValidateAsync(req, _ct))
                .ReturnsAsync(new ValidationResult(new[]
                {
                    new ValidationFailure("SubmissionDate", "Submission date must be in UTC.")
                }));

            // Act
            var result = await _controller.CalculateResubmissionFeeAsync(req, _ct);

            // Assert
            using (new AssertionScope())
            {
                result.Result.Should().BeOfType<BadRequestObjectResult>();
                var badRequestResult = result.Result.As<BadRequestObjectResult>();
                badRequestResult.Should().NotBeNull();
                badRequestResult!.Value.As<ProblemDetails>().Title.Should().Be("Validation Error");
                badRequestResult!.Value.As<ProblemDetails>().Detail.Should().Contain("Submission date must be in UTC.");
                badRequestResult!.Value.As<ProblemDetails>().Status.Should().Be(StatusCodes.Status400BadRequest);

                _serviceMock.Verify(s => s.CalculateResubmissionFeeAsync(
                    It.IsAny<ComplianceSchemeResubmissionFeeRequestDto>(), It.IsAny<CancellationToken>()),
                    Times.Never);

                _mapperMock.Verify(m => m.BuildComplianceSchemeResubmissionFeeSummaryRecord(
                    It.IsAny<ComplianceSchemeResubmissionFeeRequestV2Dto>(),
                    It.IsAny<ComplianceSchemeResubmissionFeeResult>(),
                    It.IsAny<int>()),
                    Times.Never);

                _feeItemWriterMock.Verify(w => w.Save(
                    It.IsAny<FeeItemSaveRequest>(), It.IsAny<CancellationToken>()),
                    Times.Never);
            }
        }
    }
}
