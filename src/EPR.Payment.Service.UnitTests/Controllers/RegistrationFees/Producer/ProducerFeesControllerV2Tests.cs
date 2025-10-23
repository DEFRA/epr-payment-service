using AutoFixture;
using AutoFixture.AutoMoq;
using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Common.Dtos.FeeItems;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.Producer;
using EPR.Payment.Service.Common.Enums;
using EPR.Payment.Service.Controllers.RegistrationFees.Producer;
using EPR.Payment.Service.Services.Interfaces.FeeItems;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.Producer;
using EPR.Payment.Service.Strategies.Interfaces.FeeItems;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EPR.Payment.Service.UnitTests.Controllers.RegistrationFees.Producer
{
    [TestClass]
    public class ProducerFeesV2ControllerTests
    {
        private IFixture _fixture = null!;
        private Mock<IProducerFeesCalculatorService> _calculatorMock = null!;
        private Mock<IValidator<ProducerRegistrationFeesRequestV2Dto>> _validatorMock = null!;
        private Mock<IFeeItemWriter> _feeItemWriterMock = null!;
        private Mock<IFeeItemProducerSaveRequestMapper> _mapperMock = null!;
        private ProducerFeesV2Controller _controller = null!;
        private CancellationToken _cancellationToken;

        [TestInitialize]
        public void Init()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _calculatorMock = _fixture.Freeze<Mock<IProducerFeesCalculatorService>>();
            _validatorMock = _fixture.Freeze<Mock<IValidator<ProducerRegistrationFeesRequestV2Dto>>>();
            _feeItemWriterMock = _fixture.Freeze<Mock<IFeeItemWriter>>();
            _mapperMock = _fixture.Freeze<Mock<IFeeItemProducerSaveRequestMapper>>();

            _controller = new ProducerFeesV2Controller(
                _calculatorMock.Object,
                _validatorMock.Object,
                _feeItemWriterMock.Object,
                _mapperMock.Object);

            _cancellationToken = CancellationToken.None;
        }

        [TestMethod]
        public void Constructor_WithValidArguments_ShouldInitializeCorrectly()
        {
            // Act
            var controller = new ProducerFeesV2Controller(
                _calculatorMock.Object,
                _validatorMock.Object,
                _feeItemWriterMock.Object,
                _mapperMock.Object);

            // Assert
            using (new AssertionScope())
            {
                controller.Should().NotBeNull();
                controller.Should().BeAssignableTo<ProducerFeesV2Controller>();
            }
        }

        [TestMethod]
        public void Constructor_WhenProducerFeesCalculatorServiceIsNull_ShouldThrowArgumentNullException()
        {
            // Act
            Action act = () => new ProducerFeesV2Controller(
                null!,
                _validatorMock.Object,
                _feeItemWriterMock.Object,
                _mapperMock.Object);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'producerFeesCalculatorService')");
        }

        [TestMethod]
        public void Constructor_WhenValidatorIsNull_ShouldThrowArgumentNullException()
        {
            // Act
            Action act = () => new ProducerFeesV2Controller(
                _calculatorMock.Object,
                null!,
                _feeItemWriterMock.Object,
                _mapperMock.Object);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'validator')");
        }

        [TestMethod]
        public void Constructor_WhenFeeItemWriterIsNull_ShouldThrowArgumentNullException()
        {
            // Act
            Action act = () => new ProducerFeesV2Controller(
                _calculatorMock.Object,
                _validatorMock.Object,
                null!,
                _mapperMock.Object);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'feeSummaryWriter')");
        }

        [TestMethod]
        public void Constructor_WhenFeeItemProducerSaveRequestMapperIsNull_ShouldThrowArgumentNullException()
        {
            // Act
            Action act = () => new ProducerFeesV2Controller(
                _calculatorMock.Object,
                _validatorMock.Object,
                _feeItemWriterMock.Object,
                null!);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'feeSummarySaveRequestMapper')");
        }

        [TestMethod]
        public async Task CalculateFeesAsync_WhenValidRequestWithIds_ShouldReturnOkResultAndPersistFeeItems()
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestV2Dto
            {
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "PR-2025-0001",
                SubmissionDate = DateTime.UtcNow,
                ProducerType = "large",
                InvoicePeriod = new DateTimeOffset(DateTime.UtcNow),
                PayerTypeId = (int)PayerTypeIds.DirectProducer,
                PayerId = 123,
                FileId = Guid.NewGuid(),
                ExternalId = Guid.NewGuid(),
                NumberOfSubsidiaries = 5
            };
            var response = new RegistrationFeesResponseDto
            {
                TotalFee = 1000m,
                ProducerRegistrationFee = 500m,
                SubsidiariesFee = 500m,
                SubsidiariesFeeBreakdown = new SubsidiariesFeeBreakdown()
            };
            var invoicePeriod = new DateTimeOffset(request.SubmissionDate, TimeSpan.Zero);
            var saveRequest = new FeeItemSaveRequest
            {
                ApplicationReferenceNumber = request.ApplicationReferenceNumber,
                FileId = request.FileId!.Value,
                ExternalId = request.ExternalId!.Value,
                PayerId = request.PayerId!.Value,
                PayerTypeId = (int)PayerTypeIds.DirectProducer,
                InvoicePeriod = invoicePeriod,
                Lines = new List<FeeItemLine>
                {
                    new()
                    {
                        FeeTypeId = (int)FeeTypeIds.ProducerRegistrationFee,
                        Amount = response.TotalFee,
                        Quantity = 1,
                        UnitPrice = response.TotalFee
                    }
                }
            };

            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ProducerRegistrationFeesRequestV2Dto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _calculatorMock.Setup(s => s.CalculateFeesAsync(It.IsAny<ProducerRegistrationFeesRequestV2Dto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            _mapperMock.Setup(m => m.BuildRegistrationFeeSummaryRecord(
                It.Is<ProducerRegistrationFeesRequestV2Dto>(r => r == request),
                It.Is<DateTimeOffset>(ip => ip == invoicePeriod),
                It.Is<int>(pt => pt == (int)PayerTypeIds.DirectProducer),
                It.Is<RegistrationFeesResponseDto>(r => r == response), It.IsAny<DateTimeOffset?>()))
                .Returns(saveRequest);

            _feeItemWriterMock.Setup(w => w.Save(It.Is<FeeItemSaveRequest>(s => s == saveRequest), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CalculateFeesAsync(request, _cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeOfType<ActionResult<RegistrationFeesResponseDto>>();
                var okResult = result.Result.Should().BeOfType<OkObjectResult>().Which;
                okResult.Value.Should().Be(response);

                _mapperMock.Verify(m => m.BuildRegistrationFeeSummaryRecord(request,
                    invoicePeriod,
                    (int)PayerTypeIds.DirectProducer, response, 
                    It.IsAny<DateTimeOffset?>()), Times.Once());

                _feeItemWriterMock.Verify(w => w.Save(saveRequest, It.IsAny<CancellationToken>()), Times.Once());
            }
        }   

        [TestMethod]
        public async Task CalculateFeesAsync_WhenValidationFails_ShouldReturnBadRequestWithValidationErrors()
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestV2Dto
            {
                Regulator = "",
                ApplicationReferenceNumber = "",
                SubmissionDate = DateTime.UtcNow,
                ProducerType = "large",
                InvoicePeriod = new DateTimeOffset(DateTime.UtcNow),
                PayerTypeId = (int)PayerTypeIds.DirectProducer
            };
            var validationFailures = new List<ValidationFailure>
            {
                new("Regulator", "Regulator is required"),
                new("ApplicationReferenceNumber", "ApplicationReferenceNumber is invalid")
            };

            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ProducerRegistrationFeesRequestV2Dto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(validationFailures));

            // Act
            var result = await _controller.CalculateFeesAsync(request, _cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Which;
                var problemDetails = badRequestResult.Value.Should().BeOfType<ProblemDetails>().Which;
                problemDetails.Title.Should().Be("Validation Error");
                problemDetails.Detail.Should().Be("Regulator is required; ApplicationReferenceNumber is invalid");
                problemDetails.Status.Should().Be(StatusCodes.Status400BadRequest);
                _calculatorMock.Verify(s => s.CalculateFeesAsync(It.IsAny<ProducerRegistrationFeesRequestV2Dto>(), It.IsAny<CancellationToken>()), Times.Never());
                _mapperMock.Verify(m => m.BuildRegistrationFeeSummaryRecord(It.IsAny<ProducerRegistrationFeesRequestV2Dto>(), It.IsAny<DateTimeOffset>(), It.IsAny<int>(), It.IsAny<RegistrationFeesResponseDto>(), It.IsAny<DateTimeOffset?>()), Times.Never());
                _feeItemWriterMock.Verify(w => w.Save(It.IsAny<FeeItemSaveRequest>(), It.IsAny<CancellationToken>()), Times.Never());
            }
        }

        [TestMethod]
        public async Task CalculateFeesAsync_WhenCalculationThrowsValidationException_ShouldReturnBadRequest()
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestV2Dto
            {
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "PR-2025-0001",
                SubmissionDate = DateTime.UtcNow,
                ProducerType = "large",
                InvoicePeriod = new DateTimeOffset(DateTime.UtcNow),
                PayerTypeId = (int)PayerTypeIds.DirectProducer
            };
            var exceptionMessage = "Invalid fee calculation data";

            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ProducerRegistrationFeesRequestV2Dto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _calculatorMock.Setup(s => s.CalculateFeesAsync(It.IsAny<ProducerRegistrationFeesRequestV2Dto>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ValidationException(exceptionMessage));

            // Act
            var result = await _controller.CalculateFeesAsync(request, _cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Which;
                var problemDetails = badRequestResult.Value.Should().BeOfType<ProblemDetails>().Which;
                problemDetails.Title.Should().Be("Validation Error");
                problemDetails.Detail.Should().Be(exceptionMessage);
                problemDetails.Status.Should().Be(StatusCodes.Status400BadRequest);
                _mapperMock.Verify(m => m.BuildRegistrationFeeSummaryRecord(It.IsAny<ProducerRegistrationFeesRequestV2Dto>(), It.IsAny<DateTimeOffset>(), It.IsAny<int>(), It.IsAny<RegistrationFeesResponseDto>(), It.IsAny<DateTimeOffset?>()), Times.Never());
                _feeItemWriterMock.Verify(w => w.Save(It.IsAny<FeeItemSaveRequest>(), It.IsAny<CancellationToken>()), Times.Never());
            }
        }

        [TestMethod]
        public async Task CalculateFeesAsync_WhenCalculationThrowsArgumentException_ShouldReturnBadRequest()
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestV2Dto
            {
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "PR-2025-0001",
                SubmissionDate = DateTime.UtcNow,
                ProducerType = "large",
                InvoicePeriod = new DateTimeOffset(DateTime.UtcNow),
                PayerTypeId = (int)PayerTypeIds.DirectProducer
            };
            var exceptionMessage = "Invalid argument";

            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ProducerRegistrationFeesRequestV2Dto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _calculatorMock.Setup(s => s.CalculateFeesAsync(It.IsAny<ProducerRegistrationFeesRequestV2Dto>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ArgumentException(exceptionMessage));

            // Act
            var result = await _controller.CalculateFeesAsync(request, _cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Which;
                var problemDetails = badRequestResult.Value.Should().BeOfType<ProblemDetails>().Which;
                problemDetails.Title.Should().Be("Invalid Argument");
                problemDetails.Detail.Should().Be(exceptionMessage);
                problemDetails.Status.Should().Be(StatusCodes.Status400BadRequest);
                _mapperMock.Verify(m => m.BuildRegistrationFeeSummaryRecord(It.IsAny<ProducerRegistrationFeesRequestV2Dto>(), It.IsAny<DateTimeOffset>(), It.IsAny<int>(), It.IsAny<RegistrationFeesResponseDto>(), It.IsAny<DateTimeOffset?>()), Times.Never());
                _feeItemWriterMock.Verify(w => w.Save(It.IsAny<FeeItemSaveRequest>(), It.IsAny<CancellationToken>()), Times.Never());
            }
        }

        [TestMethod]
        public async Task CalculateFeesAsync_WhenCalculationThrowsException_ShouldReturnInternalServerError()
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestV2Dto
            {
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "PR-2025-0001",
                SubmissionDate = DateTime.UtcNow,
                ProducerType = "large",
                InvoicePeriod = new DateTimeOffset(DateTime.UtcNow),
                PayerTypeId = (int)PayerTypeIds.DirectProducer
            };
            var exceptionMessage = "Unexpected error";

            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ProducerRegistrationFeesRequestV2Dto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _calculatorMock.Setup(s => s.CalculateFeesAsync(It.IsAny<ProducerRegistrationFeesRequestV2Dto>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _controller.CalculateFeesAsync(request, _cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                var errorResult = result.Result.Should().BeOfType<ObjectResult>().Which;
                errorResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
                var problemDetails = errorResult.Value.Should().BeOfType<ProblemDetails>().Which;
                problemDetails.Title.Should().Be("Internal Server Error");
                problemDetails.Detail.Should().Be($"{ProducerFeesCalculationExceptions.FeeCalculationError}: {exceptionMessage}");
                problemDetails.Status.Should().Be(StatusCodes.Status500InternalServerError);
                _mapperMock.Verify(m => m.BuildRegistrationFeeSummaryRecord(It.IsAny<ProducerRegistrationFeesRequestV2Dto>(), It.IsAny<DateTimeOffset>(), It.IsAny<int>(), It.IsAny<RegistrationFeesResponseDto>(), It.IsAny<DateTimeOffset?>()), Times.Never());
                _feeItemWriterMock.Verify(w => w.Save(It.IsAny<FeeItemSaveRequest>(), It.IsAny<CancellationToken>()), Times.Never());
            }
        }


        [TestMethod]
        public async Task CalculateFeesAsync_WhenPayerTypeIdOrPayerIdInvalid_ShouldReturnBadRequestWithErrors()
        {
            // Arrange
            var request = new ProducerRegistrationFeesRequestV2Dto
            {
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "PR-2025-0001",
                SubmissionDate = DateTime.UtcNow.AddMinutes(-5),
                ProducerType = "large",
                InvoicePeriod = new DateTimeOffset(DateTime.UtcNow, TimeSpan.Zero),
                PayerTypeId = 0,
                PayerId = null, 
                FileId = Guid.NewGuid(),
                ExternalId = Guid.NewGuid(),
                NumberOfSubsidiaries = 0
            };

            var failures = new List<ValidationFailure>
            {
                new("PayerTypeId", "PayerTypeId is required"),
                new("PayerId", "PayerId is required")
            };

            _validatorMock
                .Setup(v => v.ValidateAsync(It.IsAny<ProducerRegistrationFeesRequestV2Dto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(failures));

            // Act
            var result = await _controller.CalculateFeesAsync(request, _cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                var badRequest = result.Result.Should().BeOfType<BadRequestObjectResult>().Which;
                var problem = badRequest.Value.Should().BeOfType<ProblemDetails>().Which;

                problem.Title.Should().Be("Validation Error");
                problem.Status.Should().Be(StatusCodes.Status400BadRequest);
                problem.Detail.Should().Contain("PayerTypeId is required")
                                  .And.Contain("PayerId is required");

                _calculatorMock.Verify(c => c.CalculateFeesAsync(It.IsAny<ProducerRegistrationFeesRequestV2Dto>(), It.IsAny<CancellationToken>()), Times.Never);
                _mapperMock.Verify(m => m.BuildRegistrationFeeSummaryRecord(It.IsAny<ProducerRegistrationFeesRequestV2Dto>(), It.IsAny<DateTimeOffset>(), It.IsAny<int>(), It.IsAny<RegistrationFeesResponseDto>(), It.IsAny<DateTimeOffset?>()), Times.Never);
                _feeItemWriterMock.Verify(w => w.Save(It.IsAny<FeeItemSaveRequest>(), It.IsAny<CancellationToken>()), Times.Never);
            }
        }
    }
}