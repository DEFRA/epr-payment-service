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
        [HttpPost("{paymentId}/status")]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [FeatureGate("EnablePaymentStatusInsert")]
        public async Task<IActionResult> InsertPaymentStatus(Guid externalPaymentId, string paymentId, PaymentStatusInsertRequestDto paymentStatusInsertRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _paymentsService.InsertPaymentStatusAsync(externalPaymentId, paymentId, paymentStatusInsertRequest);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }
    }
}
