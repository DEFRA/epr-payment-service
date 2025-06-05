using Asp.Versioning;
using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ReprocessorOrExporter;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.ReprocessorOrExporter;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.ReprocessorOrExporter;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EPR.Payment.Service.Controllers.RegistrationFees.ReprocessorOrExporter
{
    [ApiVersion(1)]
    [ApiController]
    [Route("api/v{version:apiVersion}/reprocessororexporter")]
    [FeatureGate("EnableReprocessorOrExporterRegistrationFeesFeature")]
    public class ReprocessorOrExporterRegistrationFeesController(
        IReprocessorOrExporterFeesCalculatorService reprocessorOrExporterFeesCalculatorService,
        IValidator<ReprocessorOrExporterRegistrationFeesRequestDto> validator) : ControllerBase
    {
        [MapToApiVersion(1)]
        [HttpPost("registration-fee")]
        [SwaggerOperation(
            Summary = "Calculates the registration fees for a reprocessor or an exporter",
            Description = "Calculates the registration fee for a exporter or reprocessor based on provided request details."
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns the calculated registration fees", typeof(ReprocessorOrExporterRegistrationFeesResponseDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request due to validation errors or invalid input")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Registration fees data not found.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error occurred while calculating registration fees")]
        [ProducesResponseType(typeof(ReprocessorOrExporterRegistrationFeesResponseDto), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [FeatureGate("EnableReprocessorOrExporterRegistrationFeesCalculation")]
        public async Task<IActionResult> CalculateFeesAsync(
            [FromBody] ReprocessorOrExporterRegistrationFeesRequestDto request,
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
                ReprocessorOrExporterRegistrationFeesResponseDto? response = await reprocessorOrExporterFeesCalculatorService.CalculateFeesAsync(request, cancellationToken);

                if (response is null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ProblemDetails
                    {
                        Title = "Registration fee record not found",
                        Detail = ReprocessorOrExporterRegistrationFeesCalculationExceptions.RegistrationFeeNotFoundError,
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
                    Detail = $"{ReprocessorOrExporterRegistrationFeesCalculationExceptions.RegistrationFeeCalculationError}: {ex.Message}",
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }
    }
}