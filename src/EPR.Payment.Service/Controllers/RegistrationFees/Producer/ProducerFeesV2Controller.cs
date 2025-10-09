using Asp.Versioning;
using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.Producer;
using EPR.Payment.Service.Common.Enums;
using EPR.Payment.Service.Services.Interfaces.FeeItems;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.Producer;
using EPR.Payment.Service.Strategies.Interfaces.FeeItems;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EPR.Payment.Service.Controllers.RegistrationFees.Producer
{
    [ApiVersion(2)]
    [ApiController]
    [Route("api/v{version:apiVersion}/producer")]
    [FeatureGate("EnableRegistrationFeesFeature")]
    public class ProducerFeesV2Controller : ControllerBase
    {
        private readonly IProducerFeesCalculatorService _producerFeesCalculatorService;
        private readonly IValidator<ProducerRegistrationFeesRequestV2Dto> _validator;
        private readonly IFeeItemWriter _feeSummaryWriter;
        private readonly IFeeItemProducerSaveRequestMapper _feeSummarySaveRequestMapper;

        public ProducerFeesV2Controller(
            IProducerFeesCalculatorService producerFeesCalculatorService,
            IValidator<ProducerRegistrationFeesRequestV2Dto> validator,
            IFeeItemWriter feeSummaryWriter,
            IFeeItemProducerSaveRequestMapper feeSummarySaveRequestMapper)
        {
            _producerFeesCalculatorService = producerFeesCalculatorService ?? throw new ArgumentNullException(nameof(producerFeesCalculatorService));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _feeSummaryWriter = feeSummaryWriter ?? throw new ArgumentNullException(nameof(feeSummaryWriter));
            _feeSummarySaveRequestMapper = feeSummarySaveRequestMapper ?? throw new ArgumentNullException(nameof(feeSummarySaveRequestMapper));
        }

        [MapToApiVersion(2)]
        [HttpPost("v2/registration-fee")]
        [SwaggerOperation(
            Summary = "Calculates the registration fees for a producer (v2)",
            Description = "Calculates the total fees including base fee, subsidiaries fee, and any additional fees for an online marketplace producer. Persists fee items if required IDs are provided."
        )]
        [SwaggerResponse(200, "Returns the calculated registration fees", typeof(RegistrationFeesResponseDto))]
        [SwaggerResponse(400, "Bad request due to validation errors or invalid input")]
        [SwaggerResponse(500, "Internal server error occurred while calculating fees")]
        [ProducesResponseType(typeof(RegistrationFeesResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RegistrationFeesResponseDto>> CalculateFeesAsync(
            [FromBody] ProducerRegistrationFeesRequestV2Dto request,
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
                var response = await _producerFeesCalculatorService.CalculateFeesAsync(request, cancellationToken);

                var invoicePeriod = new DateTimeOffset(request.SubmissionDate, TimeSpan.Zero);
                var save = _feeSummarySaveRequestMapper.BuildRegistrationFeeSummaryRecord(
                    request,
                    invoicePeriod,
                    (int)PayerTypeIds.DirectProducer,
                    response
                );
                await _feeSummaryWriter.Save(save, cancellationToken);

                return Ok(response);
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
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = $"{ProducerFeesCalculationExceptions.FeeCalculationError}: {ex.Message}",
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }
    }
}