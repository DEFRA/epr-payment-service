using Asp.Versioning;
using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.ComplianceScheme;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EPR.Payment.Service.Controllers.RegistrationFees.ComplianceScheme
{
    [ApiVersion(1)]
    [ApiController]
    [Route("api/v{version:apiVersion}/compliance-scheme")]
    [FeatureGate("EnableComplianceSchemeFeature")]
    public class ComplianceSchemeFeesController : ControllerBase
    {
        private readonly IComplianceSchemeCalculatorService _complianceSchemeCalculatorService;
        private readonly IValidator<ComplianceSchemeFeesRequestDto> _validator;

        public ComplianceSchemeFeesController(
            IComplianceSchemeCalculatorService complianceSchemeCalculatorService,
            IValidator<ComplianceSchemeFeesRequestDto> validator)
        {
            _complianceSchemeCalculatorService = complianceSchemeCalculatorService ?? throw new ArgumentNullException(nameof(complianceSchemeCalculatorService));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        [MapToApiVersion(1)]
        [HttpPost("registration-fee")]
        [SwaggerOperation(
            Summary = "Calculate compliance scheme fees",
            Description = "Calculates the total fees including registration fee, subsidiaries fee, and any additional fees for an online marketplace for compliance scheme."
        )]
        [SwaggerResponse(200, "Returns the calculated fees", typeof(ComplianceSchemeFeesResponseDto))]
        [SwaggerResponse(400, "Bad request due to validation errors or invalid input")]
        [SwaggerResponse(500, "Internal server error occurred while retrieving the base fee")]
        [ProducesResponseType(typeof(ComplianceSchemeFeesResponseDto), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [FeatureGate("EnableComplianceSchemeMembersFees")]
        public async Task<ActionResult<ComplianceSchemeFeesResponseDto>> CalculateFeesAsync([FromBody] ComplianceSchemeFeesRequestDto complianceSchemeFeesRequestDto, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(complianceSchemeFeesRequestDto);

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
                var result = await _complianceSchemeCalculatorService.CalculateFeesAsync(complianceSchemeFeesRequestDto, cancellationToken);
                return Ok(result);
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
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ComplianceSchemeFeeCalculationExceptions.CalculationError}: {(ex.InnerException != null ? ex.InnerException.Message : ex.Message)}");
            }
        }
    }
}