using Asp.Versioning;
using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.Enums;
using EPR.Payment.Service.Services.Interfaces.FeeItems;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Strategies.Interfaces.FeeItems;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EPR.Payment.Service.Controllers.RegistrationFees.ComplianceScheme
{
    [ApiVersion(2)]
    [ApiController]
    [Route("api/v{version:apiVersion}/compliance-scheme")]
    [FeatureGate("EnableComplianceSchemeFeature")]
    public class ComplianceSchemeFeesV2Controller : ControllerBase
    {
        private readonly IComplianceSchemeCalculatorService _calculator;
        private readonly IValidator<ComplianceSchemeFeesRequestV2Dto> _validator;
        private readonly IFeeItemWriter _feeItemWriter;
        private readonly IFeeItemSaveRequestMapper _feeItemSaveRequestMapper;

        public ComplianceSchemeFeesV2Controller(
            IComplianceSchemeCalculatorService calculator,
            IValidator<ComplianceSchemeFeesRequestV2Dto> validator,
            IFeeItemWriter feeItemWriter,
            IFeeItemSaveRequestMapper feeItemSaveRequestMapper)
        {
            _calculator = calculator ?? throw new ArgumentNullException(nameof(calculator));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _feeItemWriter = feeItemWriter ?? throw new ArgumentNullException(nameof(feeItemWriter));
            _feeItemSaveRequestMapper = feeItemSaveRequestMapper ?? throw new ArgumentNullException(nameof(feeItemSaveRequestMapper));
        }

        [MapToApiVersion(2)]
        [HttpPost("registration-fee")]
        [SwaggerOperation(
            Summary = "Calculate compliance scheme fees (v2)",
            Description = "Calculates total fees for a compliance scheme (registration, subsidiaries/bands, member OMP, late fees). Writes fee items when required IDs are supplied."
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns the calculated fees", typeof(ComplianceSchemeFeesResponseDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Validation errors")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Unexpected error")]
        [ProducesResponseType(typeof(ComplianceSchemeFeesResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [FeatureGate("EnableComplianceSchemeFees")]
        public async Task<ActionResult<ComplianceSchemeFeesResponseDto>> CalculateFeesAsync(
            [FromBody] ComplianceSchemeFeesRequestV2Dto request,
            CancellationToken cancellationToken)
        {

            //Validate request
            
            var validation = _validator.Validate(request);
            if (!validation.IsValid)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Detail = string.Join("; ", validation.Errors.Select(e => e.ErrorMessage)),
                    Status = StatusCodes.Status400BadRequest
                });
            }

            try
            {
                var result = await _calculator.CalculateFeesAsync(request, cancellationToken);

                var saveRequest = _feeItemSaveRequestMapper.BuildComplianceSchemeRegistrationFeeSummaryRecord(
                    request,                    
                    result);

                await _feeItemWriter.Save(saveRequest, cancellationToken);

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
                return BadRequest(new ProblemDetails
                {
                    Title = "Invalid Argument",
                    Detail = ex.Message,
                    Status = StatusCodes.Status400BadRequest
                });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    $"{ComplianceSchemeFeeCalculationExceptions.CalculationError}: {ex.Message}");
            }
        }
    }
}
