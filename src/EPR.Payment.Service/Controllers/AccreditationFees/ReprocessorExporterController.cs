using Asp.Versioning;
using EPR.Payment.Service.Common.Constants.AccreditationFees.Exceptions;
using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Common.Dtos.Request.AccreditationFees;
using EPR.Payment.Service.Common.Dtos.Response.AccreditationFees;
using EPR.Payment.Service.Common.Dtos.Response.ResubmissionFees.Producer;
using EPR.Payment.Service.Services.Interfaces.AccreditationFees;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EPR.Payment.Service.Controllers.AccreditationFees
{
    [ApiVersion(1)]
    [ApiController]
    [Route("api/v{version:apiVersion}/reprocessorexporter")]
    [FeatureGate("EnableReprocessorExporterAccreditationFeesFeature")]
    public class ReprocessorExporterController(
        IAccreditationFeesCalculatorService accreditationFeesCalculatorService,
        IValidator<AccreditationFeesRequestDto> validator) : ControllerBase
    {
        [HttpPost("accreditation-fee")]
        [ProducesResponseType(typeof(ProducerResubmissionFeeResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Calculates the accreditation fee for a exporter or reprocessor",
            Description = "Calculates the accreditation fee for a exporter or reprocessor based on provided request details."
        )]
        [FeatureGate("EnableReprocessorExporterAccreditationFeesCalculation")]
        public async Task<IActionResult> GetAccreditationFee(
            [FromBody] AccreditationFeesRequestDto request,
            CancellationToken cancellationToken)
        {
            var validationResult = validator.Validate(request);

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
                AccreditationFeesResponseDto? response = await accreditationFeesCalculatorService.CalculateFeesAsync(request, cancellationToken);

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