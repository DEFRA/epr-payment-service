using Asp.Versioning;
using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.Dtos.FeeSummaries;
using EPR.Payment.Service.Common.Dtos.Request.ResubmissionFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Response.ResubmissionFees.ComplianceScheme;
using EPR.Payment.Service.Common.Enums;
using EPR.Payment.Service.Helper;
using EPR.Payment.Service.Services.Interfaces.FeeSummaries;
using EPR.Payment.Service.Services.Interfaces.ResubmissionFees.ComplianceScheme;
using EPR.Payment.Service.Strategies.Interfaces.FeeSummary;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EPR.Payment.Service.Controllers.ResubmissionFees.ComplianceScheme
{
    [ApiVersion(1)]
    [ApiController]
    [Route("api/v{version:apiVersion}/compliance-scheme/resubmission-fee")]
    [FeatureGate("EnableResubmissionComplianceSchemeFeature")]
    public class ComplianceSchemeResubmissionController : ControllerBase
    {
        private readonly IComplianceSchemeResubmissionService _resubmissionFeeService;
        private readonly IValidator<ComplianceSchemeResubmissionFeeRequestDto> _validator;
        private IFeeSummaryWriter _feeSummaryWriter;
        private readonly IFeeSummarySaveRequestMapper _feeSummarySaveRequestMapper;

        public ComplianceSchemeResubmissionController(
            IComplianceSchemeResubmissionService resubmissionFeeService,
            IValidator<ComplianceSchemeResubmissionFeeRequestDto> validator,
            IFeeSummaryWriter feeSummaryWriter, IFeeSummarySaveRequestMapper feeSummarySaveRequestMapper)
        {
            _resubmissionFeeService = resubmissionFeeService ?? throw new ArgumentNullException(nameof(resubmissionFeeService));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _feeSummaryWriter = feeSummaryWriter ?? throw new ArgumentNullException(nameof(feeSummaryWriter));
            _feeSummarySaveRequestMapper = feeSummarySaveRequestMapper ?? throw new ArgumentNullException(nameof(feeSummarySaveRequestMapper));
        }

        [HttpPost]
        [FeatureGate("EnableResubmissionFeesCalculation")]
        [ProducesResponseType(typeof(ComplianceSchemeResubmissionFeeResult), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Calculates the compliance scheme resubmission fee",
            Description = "Calculates the compliance scheme resubmission fee based on the provided request details, including the member count and regulator.")]
        public async Task<IActionResult> CalculateResubmissionFeeAsync([FromBody] ComplianceSchemeResubmissionFeeRequestDto request, CancellationToken cancellationToken)
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
                var result = await _resubmissionFeeService.CalculateResubmissionFeeAsync(request, cancellationToken);

                if (request.PayerId != null && request.ExternalId != null)
                {
                    var invoicePeriod = new DateTimeOffset(request.ResubmissionDate, TimeSpan.Zero);


                    var saveRequest = _feeSummarySaveRequestMapper.BuildComplianceSchemeResubmissionFeeSummaryRecord(
                        request,
                        result,
                        (int)FeeTypeIds.ComplianceSchemeResubmission,
                        invoicePeriod,
                        (int)PayerTypeIds.ComplianceScheme
                    );

                    await _feeSummaryWriter.Save(saveRequest, cancellationToken);

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
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = ComplianceSchemeFeeCalculationExceptions.CalculationError + ": " + ex.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }
    }
}
