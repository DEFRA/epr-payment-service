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
    [ApiController]
    [Route("api/v{version:apiVersion}/producer")]
    [FeatureGate("EnableRegistrationFeesFeature")]
    public class ProducerFeesController : ControllerBase
    {
        private readonly IProducerFeesCalculatorService _producerFeesCalculatorService;
        private readonly IValidator<ProducerRegistrationFeesRequestDto> _validator;
        private readonly IFeeItemWriter _feeSummaryWriter;
        private readonly IFeeItemProducerSaveRequestMapper _feeSummarySaveRequestMapper;
        private readonly IValidator<ProducerRegistrationFeesRequestV2Dto> _validatorV2;

        public ProducerFeesController(
            IProducerFeesCalculatorService producerFeesCalculatorService,
            IValidator<ProducerRegistrationFeesRequestDto> validator,
            IFeeItemWriter feeSummaryWriter,
            IFeeItemProducerSaveRequestMapper feeSummarySaveRequestMapper,
            IValidator<ProducerRegistrationFeesRequestV2Dto> validatorV2
            )
        {
            _producerFeesCalculatorService = producerFeesCalculatorService ?? throw new ArgumentNullException(nameof(producerFeesCalculatorService));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _feeSummaryWriter = feeSummaryWriter ?? throw new ArgumentNullException(nameof(feeSummaryWriter));
            _feeSummarySaveRequestMapper = feeSummarySaveRequestMapper ?? throw new ArgumentNullException(nameof(feeSummarySaveRequestMapper));
            _validatorV2 = validatorV2 ?? throw new ArgumentNullException(nameof(validator));
        }

        [ApiExplorerSettings(GroupName = "v1")]
        [MapToApiVersion(1)]
        [HttpPost("v1/registration-fee")]
        [SwaggerOperation(
            Summary = "Calculates the registration fees for a producer",
            Description = "Calculates the total fees including base fee, subsidiaries fee, and any additional fees for an online marketplace producer."
        )]
        [SwaggerResponse(200, "Returns the calculated registration fees", typeof(RegistrationFeesResponseDto))]
        [SwaggerResponse(400, "Bad request due to validation errors or invalid input")]
        [SwaggerResponse(500, "Internal server error occurred while calculating fees")]
        [ProducesResponseType(typeof(RegistrationFeesResponseDto), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [FeatureGate("EnableRegistrationFeesCalculation")]
        public async Task<ActionResult<RegistrationFeesResponseDto>> CalculateFeesAsync(
            [FromBody] ProducerRegistrationFeesRequestDto request,
            CancellationToken cancellationToken)
        {
            // Manually validate the request.
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
                var Response = await _producerFeesCalculatorService.CalculateFeesAsync(request, cancellationToken);
                return Ok(Response);
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
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ProducerFeesCalculationExceptions.FeeCalculationError}: {ex.Message}");
            }
        }


        [ApiExplorerSettings(GroupName = "v2")]
        [MapToApiVersion(2)]
        [HttpPost("v2/registration-fee")]
        [SwaggerOperation(
         Summary = "Calculates the registration fees for a producer",
         Description = "Calculates the total fees including base fee, subsidiaries fee, and any additional fees for an online marketplace producer."
     )]
        [SwaggerResponse(200, "Returns the calculated registration fees", typeof(RegistrationFeesResponseDto))]
        [SwaggerResponse(400, "Bad request due to validation errors or invalid input")]
        [SwaggerResponse(500, "Internal server error occurred while calculating fees")]
        [ProducesResponseType(typeof(RegistrationFeesResponseDto), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [FeatureGate("EnableRegistrationFeesCalculationV2")]
        public async Task<ActionResult<RegistrationFeesResponseDto>> CalculateFeesAsyncV2(
         [FromBody] ProducerRegistrationFeesRequestV2Dto request,
         CancellationToken cancellationToken)
        {
            // Manually validate the request
            var validationResult = _validatorV2.Validate(request);

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
                var Response = await _producerFeesCalculatorService.CalculateFeesAsync(request, cancellationToken);

                if (request.PayerId != null && request.FileId != null && request.ExternalId != null)
                {
                    var invoicePeriod = new DateTimeOffset(request.SubmissionDate, TimeSpan.Zero);

                    var save = _feeSummarySaveRequestMapper.BuildRegistrationFeeSummaryRecord(
                        request,
                        invoicePeriod,
                        (int)PayerTypeIds.DirectProducer,
                        Response
                    );

                    await _feeSummaryWriter.Save(save, cancellationToken);
                }
                return Ok(Response);
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
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ProducerFeesCalculationExceptions.FeeCalculationError}: {ex.Message}");
            }
        }

    }
}