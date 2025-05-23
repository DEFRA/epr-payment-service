using System.Threading;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Dtos.Request.AccreditationFees;
using EPR.Payment.Service.Common.Dtos.Request.Payments;
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
    public class ReprocessorExporterControllerTests
    {
        private IFixture _fixture = null!;
        private readonly Mock<IAccreditationFeesCalculatorService> _accreditationFeesCalculatorServiceMock = new();
        private readonly Mock<IValidator<AccreditationFeesRequestDto>> _accreditationFeesRequestDtoMock = new();
        private ReprocessorExporterController? _reprocessorExporterControllerUnderTest;
        private CancellationToken _cancellationToken;

        [TestInitialize]
        public void Setup()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _reprocessorExporterControllerUnderTest = new ReprocessorExporterController(
                _accreditationFeesCalculatorServiceMock.Object,
                _accreditationFeesRequestDtoMock.Object);
            _cancellationToken = new CancellationToken();
        }

        [TestMethod, AutoMoqData]
        public async Task GetAccreditationFee_RequestValidationFails_ShouldReturnsBadRequestWithValidationErrorDetails(
            [Frozen] AccreditationFeesRequestDto request)
        {
            // Arrange
            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("Reference", "Reference is required"),
                new ValidationFailure("Regulator", "Regulator is required")
            };


            // Setup
            _accreditationFeesRequestDtoMock.Setup(v => v.Validate(request))
                .Returns(new ValidationResult(validationFailures));

            // Act
            IActionResult result = await _reprocessorExporterControllerUnderTest!.GetAccreditationFee(request, _cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Which;
                var problemDetails = badRequestResult.Value.Should().BeOfType<ProblemDetails>().Which;
                problemDetails.Detail.Should().Be("Reference is required; Regulator is required");

                // Verify
                _accreditationFeesRequestDtoMock.Verify(v => v.Validate(request), Times.Once());
                _accreditationFeesCalculatorServiceMock.Verify(v => v.CalculateFeesAsync(request, _cancellationToken), Times.Never());
            }
        }

        [TestMethod, AutoMoqData]
        public async Task GetAccreditationFee_ValidInput_ShouldReturnOk_WhenServiceReturnResponseObject()
        {
            // Arrange
            var request = _fixture.Build<AccreditationFeesRequestDto>().Create();
            var response = _fixture.Build<AccreditationFeesResponseDto>().Create();

            // Setup
            _accreditationFeesRequestDtoMock.Setup(v => v.Validate(request))
                .Returns(new ValidationResult());
            _accreditationFeesCalculatorServiceMock.Setup(v => v.CalculateFeesAsync(request, _cancellationToken))
                .ReturnsAsync(response);

            //Act
            IActionResult result = await _reprocessorExporterControllerUnderTest!.GetAccreditationFee(request, _cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                var okObjectResult = result.Should().BeOfType<OkObjectResult>().Which;
                var accreditationFeesResponseDto = okObjectResult.Value.Should().BeOfType<AccreditationFeesResponseDto>().Which;
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
            var request = _fixture.Build<AccreditationFeesRequestDto>().Create();
            AccreditationFeesResponseDto? response = null;

            // Setup
            _accreditationFeesRequestDtoMock.Setup(v => v.Validate(request))
                .Returns(new ValidationResult());
            _accreditationFeesCalculatorServiceMock.Setup(v => v.CalculateFeesAsync(request, _cancellationToken))
                .ReturnsAsync(response);

            //Act
            IActionResult result = await _reprocessorExporterControllerUnderTest!.GetAccreditationFee(request, _cancellationToken);

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
            var request = _fixture.Build<AccreditationFeesRequestDto>().Create();

            // Setup
            _accreditationFeesRequestDtoMock.Setup(v => v.Validate(request))
                .Returns(new ValidationResult());
            _accreditationFeesCalculatorServiceMock.Setup(v => v.CalculateFeesAsync(request, _cancellationToken))
                .ThrowsAsync(new Exception("Error"));

            //Act
            IActionResult result = await _reprocessorExporterControllerUnderTest!.GetAccreditationFee(request, _cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                var objectResult = result.Should().BeOfType<ObjectResult>().Which;
                var problemDetails = objectResult.Value.Should().BeOfType<ProblemDetails>().Which;
                problemDetails.Detail.Should().Be("Internal server error: Error");

                // Verify
                _accreditationFeesRequestDtoMock.Verify(v => v.Validate(request), Times.Once());
                _accreditationFeesCalculatorServiceMock.Verify(v => v.CalculateFeesAsync(request, _cancellationToken), Times.Once());
            }
        }
    }
}
