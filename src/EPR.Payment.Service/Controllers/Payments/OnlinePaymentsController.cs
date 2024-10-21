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
    [Route("api/v{version:apiVersion}/onlinepayments")]
    [FeatureGate("EnablePaymentsFeature")]
    public class OnlinePaymentsController : ControllerBase
    {
        private readonly IOnlinePaymentsService _onlinePaymentsService;

        public OnlinePaymentsController(IOnlinePaymentsService paymentsService)
        {
            _onlinePaymentsService = paymentsService ?? throw new ArgumentNullException(nameof(paymentsService));
        }

        [MapToApiVersion(1)]
        [HttpPost]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [FeatureGate("EnablePaymentStatusInsert")]
        public async Task<ActionResult<Guid>> InsertOnlinePaymentStatus([FromBody] OnlinePaymentStatusInsertRequestDto onlinePaymentStatusInsertRequest, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var externalPaymentId = await _onlinePaymentsService.InsertOnlinePaymentStatusAsync(onlinePaymentStatusInsertRequest, cancellationToken);
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
                return StatusCode(StatusCodes.Status500InternalServerError, $"{PaymentConstants.Status500InternalServerError}: {(ex.InnerException != null ? ex.InnerException.Message : ex.Message)}");
            }
        }

        [MapToApiVersion(1)]
        [HttpPut("{externalPaymentId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [FeatureGate("EnablePaymentStatusUpdate")]
        public async Task<IActionResult> UpdateOnlinePaymentStatus(Guid externalPaymentId, [FromBody] OnlinePaymentStatusUpdateRequestDto onlinePaymentStatusUpdateRequest, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _onlinePaymentsService.UpdateOnlinePaymentStatusAsync(externalPaymentId, onlinePaymentStatusUpdateRequest, cancellationToken);
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
                return StatusCode(StatusCodes.Status500InternalServerError, $"{PaymentConstants.Status500InternalServerError}: {(ex.InnerException != null ? ex.InnerException.Message : ex.Message)}");
            }
        }

        [MapToApiVersion(1)]
        [HttpGet("{externalPaymentId}")]
        [ProducesResponseType(typeof(OnlinePaymentResponseDto), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [FeatureGate("EnableGetPaymentByExternalPaymentId")]
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
                return StatusCode(StatusCodes.Status500InternalServerError, $"{PaymentConstants.Status500InternalServerError}: {(ex.InnerException != null ? ex.InnerException.Message : ex.Message)}");
            }
        }
    }
}