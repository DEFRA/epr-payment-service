using Asp.Versioning;
using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Common.Dtos.Request.ResubmissionFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Response.ResubmissionFees.ComplianceScheme;
using EPR.Payment.Service.Common.Enums;
using EPR.Payment.Service.Services.Interfaces.FeeItems;
using EPR.Payment.Service.Services.Interfaces.ResubmissionFees.ComplianceScheme;
using EPR.Payment.Service.Strategies.Interfaces.FeeItems;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EPR.Payment.Service.Controllers.ResubmissionFees.ComplianceScheme
{
    [ApiVersion(2)]
    [ApiController]
    [Route("api/v{version:apiVersion}/compliance-scheme/resubmission-fee")]
    [FeatureGate("EnableResubmissionComplianceSchemeFeature")]
    public class ComplianceSchemeResubmissionV2Controller : ControllerBase
    {
        private readonly IComplianceSchemeResubmissionService _resubmissionService;
        private readonly IValidator<ComplianceSchemeResubmissionFeeRequestV2Dto> _validator;
        private readonly IFeeItemWriter _feeItemWriter;
        private readonly IFeeItemSaveRequestMapper _feeItemSaveRequestMapper;

        public ComplianceSchemeResubmissionV2Controller(
            IComplianceSchemeResubmissionService resubmissionService,
            IValidator<ComplianceSchemeResubmissionFeeRequestV2Dto> validator,
            IFeeItemWriter feeItemWriter,
            IFeeItemSaveRequestMapper feeItemSaveRequestMapper)
        {
            _resubmissionService = resubmissionService ?? throw new ArgumentNullException(nameof(resubmissionService));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _feeItemWriter = feeItemWriter ?? throw new ArgumentNullException(nameof(feeItemWriter));
            _feeItemSaveRequestMapper = feeItemSaveRequestMapper ?? throw new ArgumentNullException(nameof(feeItemSaveRequestMapper));
        }

        [MapToApiVersion(2)]
        [HttpPost]
        [FeatureGate("EnableResubmissionFeesCalculation")]
        [SwaggerOperation(
            Summary = "Calculate compliance scheme resubmission fee (v2)",
            Description = "Calculates the compliance scheme resubmission fee and persists fee items (v2 request with required identifiers).")]
        [ProducesResponseType(typeof(ComplianceSchemeResubmissionFeeResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ComplianceSchemeResubmissionFeeResult>> CalculateResubmissionFeeAsync(
            [FromBody] ComplianceSchemeResubmissionFeeRequestV2Dto request,
            CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
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
                var complianceSchemeResubmissionFeeRequestDto = new ComplianceSchemeResubmissionFeeRequestDto
                {
                    Regulator = request.Regulator,
                    ReferenceNumber = request.ApplicationReferenceNumber, 
                    ResubmissionDate = request.SubmissionDate,            
                    MemberCount = request.ComplianceSchemeMembers?.Count ?? 0,                    
                    
                };

                var result = await _resubmissionService.CalculateResubmissionFeeAsync(complianceSchemeResubmissionFeeRequestDto, cancellationToken);

                var saveRequest = _feeItemSaveRequestMapper.BuildComplianceSchemeResubmissionFeeSummaryRecord(
                    request,
                    result,
                    (int)FeeTypeIds.ComplianceSchemeResubmissionFee);

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
                    new ProblemDetails
                    {
                        Title = "Internal Server Error",
                        Detail = $"{ComplianceSchemeFeeCalculationExceptions.CalculationError}: {ex.Message}",
                        Status = StatusCodes.Status500InternalServerError
                    });
            }
        }
    }
}
