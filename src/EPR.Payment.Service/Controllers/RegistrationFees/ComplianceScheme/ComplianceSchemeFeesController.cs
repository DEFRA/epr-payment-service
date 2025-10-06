using Asp.Versioning;
using Azure.Core;
using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Common.Dtos.FeeSummaries;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.Enums;
using EPR.Payment.Service.Helper;
using EPR.Payment.Service.Services.Interfaces.FeeSummaries;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Strategies.Interfaces.FeeSummary;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EPR.Payment.Service.Controllers.RegistrationFees.ComplianceScheme
{
    [ApiController]
    [Route("api/")]
    [FeatureGate("EnableComplianceSchemeFeature")]
    public class ComplianceSchemeFeesController : ControllerBase
    {
        private readonly IComplianceSchemeCalculatorService _complianceSchemeCalculatorService;
        private readonly IValidator<ComplianceSchemeFeesRequestDto> _validator;
        private readonly IFeeSummaryWriter _feeSummaryWriter;
        private readonly IFeeSummarySaveRequestMapper _feeSummarySaveRequestMapper;
        private readonly IValidator<ComplianceSchemeFeesRequestV2Dto> _validatorV2;

        public ComplianceSchemeFeesController(
            IComplianceSchemeCalculatorService complianceSchemeCalculatorService,
            IValidator<ComplianceSchemeFeesRequestDto> validator, IFeeSummaryWriter feeSummaryWriter, IFeeSummarySaveRequestMapper feeSummarySaveRequestMapper)
            IValidator<ComplianceSchemeFeesRequestDto> validator,
            IValidator<ComplianceSchemeFeesRequestV2Dto> validatorV2)
        {
            _complianceSchemeCalculatorService = complianceSchemeCalculatorService ?? throw new ArgumentNullException(nameof(complianceSchemeCalculatorService));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _feeSummaryWriter = feeSummaryWriter ?? throw new ArgumentNullException(nameof(feeSummaryWriter));
            _feeSummarySaveRequestMapper = feeSummarySaveRequestMapper ?? throw new ArgumentNullException(nameof(feeSummarySaveRequestMapper));
            _validatorV2 = validatorV2 ?? throw new ArgumentNullException(nameof(validatorV2));
        }

        [ApiExplorerSettings(GroupName = "v1")]
        [HttpPost("v1/registration-fee")]
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
        [FeatureGate("EnableComplianceSchemeFees")]
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
                
                if (complianceSchemeFeesRequestDto.PayerId != null && complianceSchemeFeesRequestDto.FileId != null && complianceSchemeFeesRequestDto.ExternalId != null)
                {
                    var invoicePeriod = new DateTimeOffset(complianceSchemeFeesRequestDto.SubmissionDate, TimeSpan.Zero);

                    var save = _feeSummarySaveRequestMapper.BuildComplianceSchemeRegistrationFeeSummaryRecord(
                        complianceSchemeFeesRequestDto,
                        invoicePeriod,
                        (int)PayerTypeIds.ComplianceScheme,
                        result
                    );

                    await _feeSummaryWriter.Save(save, cancellationToken);
                }
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
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ComplianceSchemeFeeCalculationExceptions.CalculationError}: {ex.Message}");
            }
        }

        [ApiExplorerSettings(GroupName = "v2")]
        [HttpPost("v2/registration-fee")]
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
        [FeatureGate("EnableComplianceSchemeFees")]
        public async Task<ActionResult<ComplianceSchemeFeesResponseDto>> CalculateFeesAsyncV2([FromBody] ComplianceSchemeFeesRequestV2Dto complianceSchemeFeesRequestDto, CancellationToken cancellationToken)
        {
            var validationResult = _validatorV2.Validate(complianceSchemeFeesRequestDto);

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
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ComplianceSchemeFeeCalculationExceptions.CalculationError}: {ex.Message}");
            }
        }
    }
}