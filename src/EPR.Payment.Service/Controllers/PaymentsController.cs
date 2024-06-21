using Asp.Versioning;
using EPR.Payment.Service.Common.Dtos.Request;
using EPR.Payment.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;

namespace EPR.Payment.Service.Controllers
{
    [ApiVersion(1)]
    [ApiController]
    [Route("api/v{version:apiVersion}/payments")]
    [FeatureGate("EnablePaymentsFeature")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentsService _paymentsService;

        public PaymentsController(IPaymentsService paymentsService)
        {
            _paymentsService = paymentsService ?? throw new ArgumentNullException(nameof(paymentsService));
        }

        [MapToApiVersion(1)]
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [FeatureGate("EnablePaymentStatusInsert")]
        public async Task<IActionResult> InsertPaymentStatus([FromBody] PaymentStatusInsertRequestDto paymentStatusInsertRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var externalPaymentId = await _paymentsService.InsertPaymentStatusAsync(paymentStatusInsertRequest);
                return Ok(externalPaymentId);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [MapToApiVersion(1)]
        [HttpPost("{externalPaymentId}/UpdatePaymentStatus")]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [FeatureGate("EnablePaymentStatusUpdate")]
        public async Task<IActionResult> UpdatePaymentStatus(Guid externalPaymentId, [FromBody] PaymentStatusUpdateRequestDto paymentStatusUpdateRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _paymentsService.UpdatePaymentStatusAsync(externalPaymentId, paymentStatusUpdateRequest);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }
    }
}
