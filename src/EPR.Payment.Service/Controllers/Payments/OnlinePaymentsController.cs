using Asp.Versioning;
using EPR.Payment.Service.Common.Constants.Payments;
using EPR.Payment.Service.Common.Dtos.Request.Payments;
using EPR.Payment.Service.Common.Dtos.Response.Payments;
using EPR.Payment.Service.Services.Interfaces.Payments;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;

namespace EPR.Payment.Service.Controllers
{
    [ApiVersion(1)]
    [ApiController]
    [Route("api/v{version:apiVersion}/online-payments")]
    [FeatureGate("EnableOnlinePaymentsFeature")]
    public class OnlinePaymentsController : ControllerBase
    {
        private readonly IOnlinePaymentsService _onlinePaymentsService;
        private readonly IValidator<OnlinePaymentInsertRequestDto> _onlinePaymentInsertRequestValidator;
        private readonly IValidator<OnlinePaymentUpdateRequestDto> _onlinePaymentUpdateRequestValidator;

        public OnlinePaymentsController(IOnlinePaymentsService paymentsService,
            IValidator<OnlinePaymentInsertRequestDto> onlinePaymentInsertRequestValidator,
            IValidator<OnlinePaymentUpdateRequestDto> onlinePaymentUpdateRequestValidator)

        {
            _onlinePaymentsService = paymentsService ?? throw new ArgumentNullException(nameof(paymentsService));
            _onlinePaymentInsertRequestValidator = onlinePaymentInsertRequestValidator ?? throw new ArgumentNullException(nameof(onlinePaymentInsertRequestValidator));
            _onlinePaymentUpdateRequestValidator = onlinePaymentUpdateRequestValidator ?? throw new ArgumentNullException(nameof(onlinePaymentUpdateRequestValidator));
        }

        [MapToApiVersion(1)]
        [HttpPost]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [FeatureGate("EnableOnlinePaymentInsert")]
        public async Task<ActionResult<Guid>> InsertOnlinePayment([FromBody] OnlinePaymentInsertRequestDto onlinePaymentInsertRequest, CancellationToken cancellationToken)
        {
            var validatorResult = _onlinePaymentInsertRequestValidator.Validate(onlinePaymentInsertRequest);

            if (!validatorResult.IsValid)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Detail = string.Join("; ", validatorResult.Errors.Select(e => e.ErrorMessage)),
                    Status = StatusCodes.Status400BadRequest
                });
            }

            try
            {
                var externalPaymentId = await _onlinePaymentsService.InsertOnlinePaymentAsync(onlinePaymentInsertRequest, cancellationToken);
                return Ok(externalPaymentId);
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

                return StatusCode(StatusCodes.Status500InternalServerError, $"{PaymentConstants.InsertingPaymentError}: {ex.Message}");
            }
        }

        [MapToApiVersion(1)]
        [HttpPut("{externalPaymentId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [FeatureGate("EnableOnlinePaymentUpdate")]
        public async Task<IActionResult> UpdateOnlinePayment(Guid externalPaymentId, [FromBody] OnlinePaymentUpdateRequestDto onlinePaymentUpdateRequest, CancellationToken cancellationToken)
        {
            var validatorResult = _onlinePaymentUpdateRequestValidator.Validate(onlinePaymentUpdateRequest);

            if (!validatorResult.IsValid)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Detail = string.Join("; ", validatorResult.Errors.Select(e => e.ErrorMessage)),
                    Status = StatusCodes.Status400BadRequest
                });
            }

            try
            {
                await _onlinePaymentsService.UpdateOnlinePaymentAsync(externalPaymentId, onlinePaymentUpdateRequest, cancellationToken);
                return NoContent();
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
                return StatusCode(StatusCodes.Status500InternalServerError, $"{PaymentConstants.UpdatingPaymentError}: {ex.Message}");
            }
        }

        [MapToApiVersion(1)]
        [HttpGet("{externalPaymentId}")]
        [ProducesResponseType(typeof(OnlinePaymentResponseDto), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [FeatureGate("EnableGetOnlinePaymentByExternalPaymentId")]
        public async Task<IActionResult> GetOnlinePaymentByExternalPaymentId(Guid externalPaymentId, CancellationToken cancellationToken)
        {
            if (externalPaymentId == Guid.Empty)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Detail = "ExternalPaymentId cannot be empty.",
                    Status = StatusCodes.Status400BadRequest
                });
            }
            try
            {
                var onlinePaymentResponse = await _onlinePaymentsService.GetOnlinePaymentByExternalPaymentIdAsync(externalPaymentId, cancellationToken);
                return Ok(onlinePaymentResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{PaymentConstants.ReceivingPaymentError}: {ex.Message}");
            }
        }
    }
}