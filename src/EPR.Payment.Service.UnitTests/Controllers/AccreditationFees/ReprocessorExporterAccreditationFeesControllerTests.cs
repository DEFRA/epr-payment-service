using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Dtos.Request.AccreditationFees;
using EPR.Payment.Service.Common.Dtos.Response.AccreditationFees;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Controllers.AccreditationFees;
using EPR.Payment.Service.Services.Interfaces.AccreditationFees;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;


namespace EPR.Payment.Service.UnitTests.Controllers.AccreditationFees
{
    [TestClass]
    public class ReprocessorExporterAccreditationFeesControllerTests
    {
        private IFixture _fixture = null!;
        private readonly Mock<IAccreditationFeesCalculatorService> _accreditationFeesCalculatorServiceMock = new();
        private readonly Mock<IValidator<ReprocessorOrExporterAccreditationFeesRequestDto>> _accreditationFeesRequestDtoMock = new();
        private ReprocessorExporterAccreditationFeesController? _reprocessorExporterAccreditationFeesControllerUnderTest;
        private CancellationToken _cancellationToken;

        [TestInitialize]
        public void Setup()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _reprocessorExporterAccreditationFeesControllerUnderTest = new ReprocessorExporterAccreditationFeesController(
                _accreditationFeesCalculatorServiceMock.Object,
                _accreditationFeesRequestDtoMock.Object);
            _cancellationToken = new CancellationToken();
        }

        [TestMethod, AutoMoqData]
        public async Task GetAccreditationFee_RequestValidationFails_ShouldReturnsBadRequestWithValidationErrorDetails(
            [Frozen] ReprocessorOrExporterAccreditationFeesRequestDto request)
        {
            // Arrange
            var validationFailures = new List<ValidationFailure>
            {
                new("RequestorType", "RequestorType is required"),
                new("Regulator", "Regulator is required")
            };


            // Setup
            _accreditationFeesRequestDtoMock.Setup(v => v.Validate(request))
                .Returns(new ValidationResult(validationFailures));

            // Act
            IActionResult result = await _reprocessorExporterAccreditationFeesControllerUnderTest!.GetAccreditationFee(request, _cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Which;
                var problemDetails = badRequestResult.Value.Should().BeOfType<ProblemDetails>().Which;
                problemDetails.Detail.Should().Be("RequestorType is required; Regulator is required");

                // Verify
                _accreditationFeesRequestDtoMock.Verify(v => v.Validate(request), Times.Once());
                _accreditationFeesCalculatorServiceMock.Verify(v => v.CalculateFeesAsync(request, _cancellationToken), Times.Never());
            }
        }

        [TestMethod, AutoMoqData]
        public async Task GetAccreditationFee_ValidInput_ShouldReturnOk_WhenServiceReturnResponseObject()
        {
            // Arrange
            var request = _fixture.Build<ReprocessorOrExporterAccreditationFeesRequestDto>().Create();
            var response = _fixture.Build<ReprocessorOrExporterAccreditationFeesResponseDto>().Create();

            // Setup
            _accreditationFeesRequestDtoMock.Setup(v => v.Validate(request))
                .Returns(new ValidationResult());
            _accreditationFeesCalculatorServiceMock.Setup(v => v.CalculateFeesAsync(request, _cancellationToken))
                .ReturnsAsync(response);

            //Act
            IActionResult result = await _reprocessorExporterAccreditationFeesControllerUnderTest!.GetAccreditationFee(request, _cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                var okObjectResult = result.Should().BeOfType<OkObjectResult>().Which;
                var accreditationFeesResponseDto = okObjectResult.Value.Should().BeOfType<ReprocessorOrExporterAccreditationFeesResponseDto>().Which;
                accreditationFeesResponseDto.Should().BeEquivalentTo(response);
                
                // Verify
                _accreditationFeesRequestDtoMock.Verify(v => v.Validate(request), Times.Once());
                _accreditationFeesCalculatorServiceMock.Verify(v => v.CalculateFeesAsync(request, _cancellationToken), Times.Once());
            }
        }

        [TestMethod, AutoMoqData]
        public async Task GetAccreditationFee_ValidInput_ShouldReturnNotFound_WhenServiceReturnNullResponseObject()
        {
            // Arrange
            var request = _fixture.Build<ReprocessorOrExporterAccreditationFeesRequestDto>().Create();
            ReprocessorOrExporterAccreditationFeesResponseDto? response = null;

            // Setup
            _accreditationFeesRequestDtoMock.Setup(v => v.Validate(request))
                .Returns(new ValidationResult());
            _accreditationFeesCalculatorServiceMock.Setup(v => v.CalculateFeesAsync(request, _cancellationToken))
                .ReturnsAsync(response);

            //Act
            IActionResult result = await _reprocessorExporterAccreditationFeesControllerUnderTest!.GetAccreditationFee(request, _cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                var objectResult = result.Should().BeOfType<ObjectResult>().Which;
                var problemDetails = objectResult.Value.Should().BeOfType<ProblemDetails>().Which;
                problemDetails.Detail.Should().Be("Accreditation fee not found.");

                // Verify
                _accreditationFeesRequestDtoMock.Verify(v => v.Validate(request), Times.Once());
                _accreditationFeesCalculatorServiceMock.Verify(v => v.CalculateFeesAsync(request, _cancellationToken), Times.Once());
            }
        }

        [TestMethod, AutoMoqData]
        public async Task GetAccreditationFee_ValidInput_ShouldReturnServerError_WhenServiceCallsThrowsException()
        {
            // Arrange
            var request = _fixture.Build<ReprocessorOrExporterAccreditationFeesRequestDto>().Create();

            // Setup
            _accreditationFeesRequestDtoMock.Setup(v => v.Validate(request))
                .Returns(new ValidationResult());
            _accreditationFeesCalculatorServiceMock.Setup(v => v.CalculateFeesAsync(request, _cancellationToken))
                .ThrowsAsync(new Exception("Error"));

            //Act
            IActionResult result = await _reprocessorExporterAccreditationFeesControllerUnderTest!.GetAccreditationFee(request, _cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                var objectResult = result.Should().BeOfType<ObjectResult>().Which;
                var problemDetails = objectResult.Value.Should().BeOfType<ProblemDetails>().Which;
                problemDetails.Detail.Should().Be("An error occurred while calculating accreditation fees.: Error");

                // Verify
                _accreditationFeesRequestDtoMock.Verify(v => v.Validate(request), Times.Once());
                _accreditationFeesCalculatorServiceMock.Verify(v => v.CalculateFeesAsync(request, _cancellationToken), Times.Once());
            }
        }
    }
}
