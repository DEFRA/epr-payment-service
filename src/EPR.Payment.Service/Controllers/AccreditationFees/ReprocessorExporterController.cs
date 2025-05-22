using Asp.Versioning;
using EPR.Payment.Service.Common.Constants.AccreditationFees.Exceptions;
using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Common.Dtos.Request.AccreditationFees;
using EPR.Payment.Service.Common.Dtos.Request.ResubmissionFees.Producer;
using EPR.Payment.Service.Common.Dtos.Response.ResubmissionFees.Producer;
using EPR.Payment.Service.Services.AccreditationFees;
using EPR.Payment.Service.Services.Interfaces.AccreditationFees;
using EPR.Payment.Service.Services.Interfaces.ResubmissionFees.Producer;
using EPR.Payment.Service.Validations.AccreditationFees;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EPR.Payment.Service.Controllers.ResubmissionFees.Producer
{
    [ApiVersion(1)]
    [ApiController]
    [Route("api/v{version:apiVersion}/reprocessorexporter")]
    [FeatureGate("EnableReprocessorExporterAccreditationFeesFeature")]
    public class ReprocessorExporterController : ControllerBase
    {
        private readonly IAccreditationFeesCalculatorService _accreditationFeesCalculatorService;
        private readonly IValidator<AccreditationFeesRequestDto> _validator;

        public ReprocessorExporterController(
            IAccreditationFeesCalculatorService accreditationFeesCalculatorService,
            IValidator<AccreditationFeesRequestDto> validator)
        {
            _accreditationFeesCalculatorService = accreditationFeesCalculatorService ?? throw new ArgumentNullException(nameof(_accreditationFeesCalculatorService));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        [HttpPost("accriditation-fee")]
        [ProducesResponseType(typeof(ProducerResubmissionFeeResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Calculates the accreditation fee for a exporter or reprocessor",
            Description = "Calculates the accreditation fee for a exporter or reprocessor based on provided request details."
        )]
        [FeatureGate("EnableReprocessorExporterAccreditationFeesCalculation")]
        public async Task<IActionResult> GetAccreditationFee([FromBody] AccreditationFeesRequestDto request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);

            if (!validationResult.IsValid)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Detail = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)),
                    Status = StatusCodes.Status400BadRequest
                });
            }

            try
            {
                var response = await _accreditationFeesCalculatorService.CalculateFeesAsync(request, cancellationToken);

                if(response == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ProblemDetails
                    {
                        Title = "Accreditation fee record not found",
                        Detail = AccreditationFeeCalculationExceptions.AccreditationFeeNotFoundError,
                        Status = StatusCodes.Status404NotFound
                    });
                }

                return Ok(response);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Detail = ex.Message,
                    Status = StatusCodes.Status400BadRequest
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Unexpected Error",
                    Detail = $"{ProducerResubmissionExceptions.Status500InternalServerError}: {ex.Message}",
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }
    }
}