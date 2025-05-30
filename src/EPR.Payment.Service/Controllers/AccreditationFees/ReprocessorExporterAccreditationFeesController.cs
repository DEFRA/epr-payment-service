using Asp.Versioning;
using EPR.Payment.Service.Common.Constants.AccreditationFees.Exceptions;
using EPR.Payment.Service.Common.Dtos.Request.AccreditationFees;
using EPR.Payment.Service.Common.Dtos.Response.AccreditationFees;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.ReprocessorOrExporter;
using EPR.Payment.Service.Services.Interfaces.AccreditationFees;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EPR.Payment.Service.Controllers.AccreditationFees
{
    [ApiVersion(1)]
    [ApiController]
    [Route("api/v{version:apiVersion}/reprocessorexporter")]
    [FeatureGate("EnableReprocessorExporterAccreditationFeesFeature")]
    public class ReprocessorExporterAccreditationFeesController(
        IAccreditationFeesCalculatorService accreditationFeesCalculatorService,
        IValidator<AccreditationFeesRequestDto> validator) : ControllerBase
    {
        [MapToApiVersion(1)]
        [HttpPost("accreditation-fee")]
        [SwaggerOperation(
            Summary = "Calculates the accreditation fee for a exporter or reprocessor",
            Description = "Calculates the accreditation fee for a exporter or reprocessor based on provided request details."
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns the calculated accreditation fees", typeof(ReprocessorOrExporterRegistrationFeesResponseDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request due to validation errors or invalid input")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Accreditation fees data not found.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error occurred while calculating accreditation fees")]
        [ProducesResponseType(typeof(AccreditationFeesResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [FeatureGate("EnableReprocessorExporterAccreditationFeesCalculation")]
        public async Task<IActionResult> GetAccreditationFee(
            [FromBody] AccreditationFeesRequestDto request,
            CancellationToken cancellationToken)
        {
            ValidationResult validationResult = validator.Validate(request);

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

                if(response is null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ProblemDetails
                    {
                        Title = "Accreditation fee record not found",
                        Detail = ReprocessorOrExporterAccreditationFeeCalculationExceptions.AccreditationFeeNotFoundError,
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
                    Detail = $"{ReprocessorOrExporterAccreditationFeeCalculationExceptions.AccreditationFeeCalculationError}: {ex.Message}",
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }
    }
}