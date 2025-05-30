using Asp.Versioning;
using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ReprocessorOrExporter;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.ReprocessorOrExporter;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.ReprocessorOrExporter;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EPR.Payment.Service.Controllers.RegistrationFees.ReprocessorOrExporter
{
    [ApiVersion(1)]
    [ApiController]
    [Route("api/v{version:apiVersion}/reprocessororexporter")]
    [FeatureGate("EnableReprocessorOrExporterRegistrationFeesFeature")]
    public class ReprocessorOrExporterFeesController : ControllerBase
    {
        private readonly IReprocessorOrExporterFeesCalculatorService _reprocessorOrExporterFeesCalculatorService;
        private readonly IValidator<ReprocessorOrExporterRegistrationFeesRequestDto> _validator;

        public ReprocessorOrExporterFeesController(
            IReprocessorOrExporterFeesCalculatorService reprocessorOrExporterFeesCalculatorService,
            IValidator<ReprocessorOrExporterRegistrationFeesRequestDto> validator)
        {
            _reprocessorOrExporterFeesCalculatorService = reprocessorOrExporterFeesCalculatorService ?? throw new ArgumentNullException(nameof(reprocessorOrExporterFeesCalculatorService));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        [MapToApiVersion(1)]
        [HttpPost("registration-fee")]
        [SwaggerOperation(
            Summary = "Calculates the registration fees for a reprocessor or an exporter",
            Description = "Calculates the total fees including base fee, subsidiaries fee, and any additional fees for a reprocessor or an exporter."
        )]
        [SwaggerResponse(200, "Returns the calculated registration fees", typeof(ReprocessorOrExporterRegistrationFeesResponseDto))]
        [SwaggerResponse(400, "Bad request due to validation errors or invalid input")]
        [SwaggerResponse(500, "Internal server error occurred while calculating fees")]
        [ProducesResponseType(typeof(ReprocessorOrExporterRegistrationFeesResponseDto), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [FeatureGate("EnableReprocessorOrExporterRegistrationFeesCalculation")]
        public async Task<ActionResult<ReprocessorOrExporterRegistrationFeesResponseDto>> CalculateFeesAsync(
            [FromBody] ReprocessorOrExporterRegistrationFeesRequestDto request,
            CancellationToken cancellationToken)
        {
            // Manually validate the request
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
                var result = await _reprocessorOrExporterFeesCalculatorService.CalculateFeesAsync(request, cancellationToken);
                return Ok(result); // Return the calculated fees as a resource
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
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ReprocessorOrExporterFeesCalculationExceptions.FeeCalculationError}: {ex.Message}");
            }
        }
    }
}