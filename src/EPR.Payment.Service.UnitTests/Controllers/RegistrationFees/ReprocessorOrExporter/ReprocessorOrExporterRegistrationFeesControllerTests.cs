using AutoFixture;
using AutoFixture.AutoMoq;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ReprocessorOrExporter;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.ReprocessorOrExporter;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using EPR.Payment.Service.Controllers.RegistrationFees.ReprocessorOrExporter;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.ReprocessorOrExporter;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EPR.Payment.Service.UnitTests.Controllers.RegistrationFees.ReprocessorOrExporter
{
    [TestClass]
    public class ReprocessorOrExporterRegistrationFeesControllerTests
    {
        private IFixture _fixture = null!;
        private readonly Mock<IReprocessorOrExporterFeesCalculatorService> _reprocessorOrExporterFeesCalculatorServiceMock = new();
        private readonly Mock<IValidator<ReprocessorOrExporterRegistrationFeesRequestDto>> _reprocessorOrExporterRegistrationFeesRequestDtoMock = new();
        private ReprocessorOrExporterRegistrationFeesController? _reprocessorOrExporterRegistrationFeesControllerUnderTest;
        private CancellationToken _cancellationToken;

        [TestInitialize]
        public void TestInitialize()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            
            _reprocessorOrExporterRegistrationFeesControllerUnderTest = new ReprocessorOrExporterRegistrationFeesController(
                _reprocessorOrExporterFeesCalculatorServiceMock.Object,
                _reprocessorOrExporterRegistrationFeesRequestDtoMock.Object);
            
            _cancellationToken = new CancellationToken();
        }
        

        [TestMethod, AutoMoqData]
        public async Task CalculateFeesAsync_WhenValidRequest_ReturnsOkResultWithCalculatedFees()
        {
            // Arrange
            var request = _fixture.Build<ReprocessorOrExporterRegistrationFeesRequestDto>().Create();
            var response = _fixture.Build<ReprocessorOrExporterRegistrationFeesResponseDto>().Create();

            // Setup
            _reprocessorOrExporterRegistrationFeesRequestDtoMock.Setup(v => v.Validate(request))
                .Returns(new ValidationResult());
            _reprocessorOrExporterFeesCalculatorServiceMock.Setup(s => s.CalculateFeesAsync(request, _cancellationToken))
                .ReturnsAsync(response);

            // Act
            IActionResult result = await _reprocessorOrExporterRegistrationFeesControllerUnderTest!.CalculateFeesAsync(request, _cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                var okObjectResult = result.Should().BeOfType<OkObjectResult>().Which;
                var registrationFeesResponseDto = okObjectResult.Value.Should().BeOfType<ReprocessorOrExporterRegistrationFeesResponseDto>().Which;
                registrationFeesResponseDto.Should().BeEquivalentTo(response);

                // Verify
                _reprocessorOrExporterRegistrationFeesRequestDtoMock.Verify(v => v.Validate(request), Times.Once());
                _reprocessorOrExporterFeesCalculatorServiceMock.Verify(v => v.CalculateFeesAsync(request, _cancellationToken), Times.Once());
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeesAsync_WhenRequestValidationFails_ReturnsBadRequestWithValidationErrorDetails()
        {
            // Arrange
            var request = _fixture.Build<ReprocessorOrExporterRegistrationFeesRequestDto>().Create();
            var validationFailures = new List<ValidationFailure>
            {
                new("RequestorType", "RequestorType is required"),
                new("Regulator", "Regulator is required")
            };

            // Setup
            _reprocessorOrExporterRegistrationFeesRequestDtoMock.Setup(v => v.Validate(request))
                .Returns(new ValidationResult(validationFailures));

            // Act
            IActionResult result = await _reprocessorOrExporterRegistrationFeesControllerUnderTest!.CalculateFeesAsync(request, _cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Which;
                var problemDetails = badRequestResult.Value.Should().BeOfType<ProblemDetails>().Which;
                problemDetails.Detail.Should().Be("RequestorType is required; Regulator is required");

                // Verify
                _reprocessorOrExporterRegistrationFeesRequestDtoMock.Verify(v => v.Validate(request), Times.Once());
                _reprocessorOrExporterFeesCalculatorServiceMock.Verify(v => v.CalculateFeesAsync(request, _cancellationToken), Times.Never());
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeesAsync_ValidInput_ShouldReturnNotFound_WhenServiceReturnNullResponseObject()
        {
            // Arrange
            var request = _fixture.Build<ReprocessorOrExporterRegistrationFeesRequestDto>().Create();
            ReprocessorOrExporterRegistrationFeesResponseDto? response = null;

            // Setup
            _reprocessorOrExporterRegistrationFeesRequestDtoMock.Setup(v => v.Validate(request))
                .Returns(new ValidationResult());
            _reprocessorOrExporterFeesCalculatorServiceMock.Setup(s => s.CalculateFeesAsync(request, _cancellationToken))
                .ReturnsAsync(response);

            // Act
            IActionResult result = await _reprocessorOrExporterRegistrationFeesControllerUnderTest!.CalculateFeesAsync(request, _cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                var objectResult = result.Should().BeOfType<ObjectResult>().Which;
                var problemDetails = objectResult.Value.Should().BeOfType<ProblemDetails>().Which;
                problemDetails.Detail.Should().Be("Registration fee not found.");

                // Verify
                _reprocessorOrExporterRegistrationFeesRequestDtoMock.Verify(v => v.Validate(request), Times.Once());
                _reprocessorOrExporterFeesCalculatorServiceMock.Verify(v => v.CalculateFeesAsync(request, _cancellationToken), Times.Once());
            }
        }

        [TestMethod, AutoMoqData]
        public async Task CalculateFeesAsync_WhenCalculationThrowsException_ShouldReturnInternalServerError()
        {
            // Arrange
            var request = _fixture.Build<ReprocessorOrExporterRegistrationFeesRequestDto>().Create();
            var exceptionMessage = "Error";

            // Setup
            _reprocessorOrExporterRegistrationFeesRequestDtoMock.Setup(v => v.Validate(request))
                .Returns(new ValidationResult());
            _reprocessorOrExporterFeesCalculatorServiceMock.Setup(i => i.CalculateFeesAsync(request, _cancellationToken))
                .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            IActionResult result = await _reprocessorOrExporterRegistrationFeesControllerUnderTest!.CalculateFeesAsync(request, _cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                var objectResult = result.Should().BeOfType<ObjectResult>().Which;
                var problemDetails = objectResult.Value.Should().BeOfType<ProblemDetails>().Which;
                problemDetails.Detail.Should().Be("An error occurred while calculating registration fees.: Error");

                // Verify
                _reprocessorOrExporterRegistrationFeesRequestDtoMock.Verify(v => v.Validate(request), Times.Once());
                _reprocessorOrExporterFeesCalculatorServiceMock.Verify(v => v.CalculateFeesAsync(request, _cancellationToken), Times.Once());
            }
        }
    }
}