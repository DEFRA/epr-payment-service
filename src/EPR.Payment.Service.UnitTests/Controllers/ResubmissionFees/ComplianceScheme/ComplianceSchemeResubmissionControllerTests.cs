using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Dtos.FeeItems;
using EPR.Payment.Service.Common.Dtos.Request.ResubmissionFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Response.ResubmissionFees.ComplianceScheme;
using EPR.Payment.Service.Common.Enums;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
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
    public class ComplianceSchemeResubmissionControllerTests
    {
        private IFixture _fixture = null!;
        private ComplianceSchemeResubmissionController _controller = null!;
        private Mock<IComplianceSchemeResubmissionService> _resubmissionFeeServiceMock = null!;
        private Mock<IValidator<ComplianceSchemeResubmissionFeeRequestDto>> _validatorMock = null!;
        private Mock<IFeeItemWriter> _feeItemWriterMock = null!;
        private Mock<IFeeItemSaveRequestMapper> _feeItemSaveRequestMapper = null!;
        private CancellationToken _cancellationToken;

        [TestInitialize]
        public void TestInitialize()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _resubmissionFeeServiceMock = _fixture.Freeze<Mock<IComplianceSchemeResubmissionService>>();
            _validatorMock = _fixture.Freeze<Mock<IValidator<ComplianceSchemeResubmissionFeeRequestDto>>>();
            _feeItemWriterMock = _fixture.Freeze<Mock<IFeeItemWriter>>();
            _feeItemSaveRequestMapper = _fixture.Freeze<Mock<IFeeItemSaveRequestMapper>>();

            _controller = new ComplianceSchemeResubmissionController(
                _resubmissionFeeServiceMock.Object,
                _validatorMock.Object,
                _feeItemWriterMock.Object,
                _feeItemSaveRequestMapper.Object);
            _cancellationToken = new CancellationToken();
        }

        [TestMethod]
        public void Constructor_WithValidArguments_ShouldInitializeCorrectly()
        {
            // Act
            var controller = new ComplianceSchemeResubmissionController(
                _resubmissionFeeServiceMock.Object,
                _validatorMock.Object,
                _feeItemWriterMock.Object,
                _feeItemSaveRequestMapper.Object);

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
            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => new ComplianceSchemeResubmissionController(
                null!,
                _validatorMock.Object,
                _feeItemWriterMock.Object,
                _feeItemSaveRequestMapper.Object
            ));
        }

        [TestMethod]
        public void Constructor_WithNullValidator_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => new ComplianceSchemeResubmissionController(
                _resubmissionFeeServiceMock.Object,
                null!,
                 _feeItemWriterMock.Object,
                _feeItemSaveRequestMapper.Object
            ));
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
            request.ResubmissionDate = DateTime.UtcNow; // Non-UTC date

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

        [TestMethod, AutoMoqData]
        public async Task GetResubmissionAsync_ServiceThrowsArgumentException_ShouldReturnBadRequest(
        [Frozen] ComplianceSchemeResubmissionFeeRequestDto request)
        {

            // Arrange
            _validatorMock.Setup(v => v.Validate(request)).Returns(new ValidationResult());
            _resubmissionFeeServiceMock
                .Setup(s => s.CalculateResubmissionFeeAsync(request, _cancellationToken))
                .ThrowsAsync(new ArgumentException("Invalid input parameter."));

            // Act
            var result = await _controller.CalculateResubmissionFeeAsync(request, _cancellationToken);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result.As<BadRequestObjectResult>();
            badRequestResult.Value.Should().Be("Invalid input parameter.");
        }

        [TestMethod]
        public async Task CalculateResubmissionFeeAsync_WhenIdentifiersPresent_CallsFeeSummaryWriterSave_WithExpectedPayload()
        {
            // Arrange
            var request = new ComplianceSchemeResubmissionFeeRequestDto
            {
                Regulator = "GB-ENG",
                ReferenceNumber = "CS-RESUB-001",
                ResubmissionDate = DateTime.UtcNow,
                MemberCount = 5,
                FileId = Guid.NewGuid(),
                ExternalId = Guid.NewGuid(),
                PayerId = 123
            };

            var resultDto = new ComplianceSchemeResubmissionFeeResult
            {
                TotalResubmissionFee = 987.65m
            };

            _validatorMock
                .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _resubmissionFeeServiceMock
                .Setup(s => s.CalculateResubmissionFeeAsync(request, _cancellationToken))
                .ReturnsAsync(resultDto);

            _feeItemSaveRequestMapper
                .Setup(m => m.BuildComplianceSchemeResubmissionFeeSummaryRecord(
                    It.Is<ComplianceSchemeResubmissionFeeRequestDto>(r => r == request),
                    It.Is<ComplianceSchemeResubmissionFeeResult>(r => r == resultDto),
                    It.Is<int>(t => t == (int)FeeTypeIds.ComplianceSchemeResubmissionFee),
                    It.IsAny<DateTimeOffset>(),
                    It.Is<int>(p => p == (int)PayerTypeIds.ComplianceScheme),
                    It.IsAny<DateTimeOffset?>()))
                .Returns(() =>
                {
                    return new FeeSummarySaveRequest
                    {
                        ApplicationReferenceNumber = request.ReferenceNumber,
                        FileId = request.FileId.Value,
                        ExternalId = request.ExternalId.Value,
                        PayerTypeId = (int)PayerTypeIds.ComplianceScheme,
                        PayerId = request.PayerId.Value,
                        InvoicePeriod = new DateTimeOffset(request.ResubmissionDate, TimeSpan.Zero),
                        Lines = new[]
                        {
                            new FeeSummaryLineRequest
                            {
                                FeeTypeId = (int)FeeTypeIds.ComplianceSchemeResubmissionFee,
                                UnitPrice = resultDto.TotalResubmissionFee,
                                Quantity = 1,
                                Amount = resultDto.TotalResubmissionFee
                            }
                        }
                    };
                });

            // Act
            var actionResult = await _controller.CalculateResubmissionFeeAsync(request, _cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                actionResult.Should().BeOfType<OkObjectResult>();
                actionResult.As<OkObjectResult>().Value.Should().Be(resultDto);
            }

            // Assert
            _feeItemWriterMock.Verify(w => w.Save(
                It.Is<FeeSummarySaveRequest>(save =>
                    save.ApplicationReferenceNumber == request.ReferenceNumber &&
                    save.FileId == request.FileId &&
                    save.ExternalId == request.ExternalId &&
                    save.PayerTypeId == (int)PayerTypeIds.ComplianceScheme &&
                    save.PayerId == request.PayerId &&
                    save.InvoicePeriod.Date == request.ResubmissionDate.Date &&
                    save.Lines != null &&
                    save.Lines.Count == 1 &&
                    save.Lines.Single().FeeTypeId == (int)FeeTypeIds.ComplianceSchemeResubmissionFee &&
                    save.Lines.Single().Quantity == 1 &&
                    save.Lines.Single().Amount == resultDto.TotalResubmissionFee &&
                    save.Lines.Single().UnitPrice == resultDto.TotalResubmissionFee
                ), CancellationToken.None), Times.Once);
        }

        [TestMethod]
        public async Task CalculateResubmissionFeeAsync_WhenIdentifiersMissing_ValidationFails_DoesNotCallFeeSummaryWriterSave()
        {
            // Arrange
            var request = new ComplianceSchemeResubmissionFeeRequestDto
            {
                Regulator = "GB-ENG",
                ReferenceNumber = "CS-RESUB-002",
                ResubmissionDate = DateTime.UtcNow,
                MemberCount = 3,
                FileId = Guid.NewGuid(),
                ExternalId = Guid.NewGuid(),
                PayerId = 1
            };

            var failures = new List<ValidationFailure>
            {
                new("FileId", "FileId is required"),
                new("ExternalId", "ExternalId is required"),
                new("PayerId", "PayerId must be greater than 0")
            };

            _validatorMock
                .Setup(v => v.ValidateAsync(request, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(failures));

            // Act
            var actionResult = await _controller.CalculateResubmissionFeeAsync(request, _cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                actionResult.Should().BeOfType<BadRequestObjectResult>();
                _resubmissionFeeServiceMock.Verify(
                    s => s.CalculateResubmissionFeeAsync(It.IsAny<ComplianceSchemeResubmissionFeeRequestDto>(), It.IsAny<CancellationToken>()),
                    Times.Never);
                _feeItemSaveRequestMapper.Verify(m => m.BuildComplianceSchemeResubmissionFeeSummaryRecord(
                        It.IsAny<ComplianceSchemeResubmissionFeeRequestDto>(),
                        It.IsAny<ComplianceSchemeResubmissionFeeResult>(),
                        It.IsAny<int>(),
                        It.IsAny<DateTimeOffset>(),
                        It.IsAny<int>(),
                        It.IsAny<DateTimeOffset?>()),
                    Times.Never);
                _feeItemWriterMock.Verify(w => w.Save(It.IsAny<FeeSummarySaveRequest>(), CancellationToken.None), Times.Never);
            }
        }
    }
}
