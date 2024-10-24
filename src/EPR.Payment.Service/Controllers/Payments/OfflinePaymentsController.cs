using Asp.Versioning;
using EPR.Payment.Service.Common.Constants.Payments;
using EPR.Payment.Service.Common.Dtos.Request.Payments;
using EPR.Payment.Service.Services.Interfaces.Payments;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;

namespace EPR.Payment.Service.Controllers
{
    [ApiVersion(1)]
    [ApiController]
    [Route("api/v{version:apiVersion}/offline-payments")]
    [FeatureGate("EnableOfflinePaymentsFeature")]
    public class OfflinePaymentsController : ControllerBase
    {
        private readonly IOfflinePaymentsService _offlinePaymentsService;

        public OfflinePaymentsController(IOfflinePaymentsService paymentsService)
        {
            _offlinePaymentsService = paymentsService ?? throw new ArgumentNullException(nameof(paymentsService));
        }

        [MapToApiVersion(1)]
        [HttpPost]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [FeatureGate("EnableOfflinePaymentStatusInsert")]
        public async Task<ActionResult<Guid>> InsertOfflinePaymentStatus([FromBody] OfflinePaymentStatusInsertRequestDto offlinePaymentStatusInsertRequest, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _offlinePaymentsService.InsertOfflinePaymentAsync(offlinePaymentStatusInsertRequest, cancellationToken);

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
    }
}