using Asp.Versioning;
using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.ComplianceScheme;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EPR.Payment.Service.Controllers.RegistrationFees
{
    [ApiVersion(1)]
    [ApiController]
    [Route("api/v{version:apiVersion}/compliance-scheme")]
    [FeatureGate("EnableComplianceSchemeFeature")]
    public class ComplianceSchemeRegistrationFeesController : ControllerBase
    {
        private readonly IComplianceSchemeBaseFeeService _complianceSchemeBaseFeeService;
        private readonly IValidator<string> _regulatorValidator;

        public ComplianceSchemeRegistrationFeesController(
            IComplianceSchemeBaseFeeService complianceSchemeBaseFeeService,
            IValidator<string> regulatorValidator)
        {
            _complianceSchemeBaseFeeService = complianceSchemeBaseFeeService ?? throw new ArgumentNullException(nameof(complianceSchemeBaseFeeService));
            _regulatorValidator = regulatorValidator ?? throw new ArgumentNullException(nameof(regulatorValidator));
        }

        [MapToApiVersion(1)]
        [HttpGet("{regulator}")]
        [SwaggerOperation(
            Summary = "Retrieves the base fee for a compliance scheme",
            Description = "Retrieves the base fee based on the specified regulator."
        )]
        [SwaggerResponse(200, "Returns the base fee", typeof(decimal))]
        [SwaggerResponse(400, "Bad request due to validation errors or invalid input")]
        [SwaggerResponse(500, "Internal server error occurred while retrieving the base fee")]
        [ProducesResponseType(typeof(decimal), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [FeatureGate("EnableComplianceSchemeBaseFees")]
        public async Task<IActionResult> GetBaseFeeAsync(
            [FromRoute] string regulator,
            CancellationToken cancellationToken)
        {
            // Manually validate the regulator string
            var validationResult = _regulatorValidator.Validate(regulator);

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
                var fee = await _complianceSchemeBaseFeeService.GetComplianceSchemeBaseFeeAsync(regulator, cancellationToken);
                return Ok(new { BaseFee = fee });
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
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ComplianceSchemeFeeCalculationExceptions.RetrievalError}: {(ex.InnerException != null ? ex.InnerException.Message : ex.Message)}");
            }
        }
    }
}
