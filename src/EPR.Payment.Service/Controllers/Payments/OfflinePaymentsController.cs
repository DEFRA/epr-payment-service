using Asp.Versioning;
using EPR.Payment.Service.Common.Constants.Payments;
using EPR.Payment.Service.Common.Dtos.Request.Payments;
using EPR.Payment.Service.Services.Interfaces.Payments;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;

namespace EPR.Payment.Service.Controllers.Payments
{
    [ApiVersion(1)]
    [ApiController]
    [Route("api/v{version:apiVersion}/offline-payments")]
    [FeatureGate("EnableOfflinePaymentsFeature")]
    public class OfflinePaymentsController : ControllerBase
    {
        private readonly IOfflinePaymentsService _offlinePaymentsService;
        private readonly IValidator<OfflinePaymentInsertRequestDto> _offlinePaymentInsertRequestValidator;

        public OfflinePaymentsController(IOfflinePaymentsService paymentsService,
            IValidator<OfflinePaymentInsertRequestDto> offlinePaymentInsertRequestValidator)
        {
            _offlinePaymentsService = paymentsService ?? throw new ArgumentNullException(nameof(paymentsService));
            _offlinePaymentInsertRequestValidator = offlinePaymentInsertRequestValidator ?? throw new ArgumentNullException(nameof(offlinePaymentInsertRequestValidator));
        }

        [MapToApiVersion(1)]
        [HttpPost]
        [ProducesResponseType(typeof(NoContentResult), 204)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [FeatureGate("EnableOfflinePayment")]
        public async Task<ActionResult> InsertOfflinePayment([FromBody] OfflinePaymentInsertRequestDto offlinePaymentInsertRequest, CancellationToken cancellationToken)
        {
            var validatorResult = _offlinePaymentInsertRequestValidator.Validate(offlinePaymentInsertRequest);

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
                await _offlinePaymentsService.InsertOfflinePaymentAsync(offlinePaymentInsertRequest, cancellationToken);

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
                return StatusCode(StatusCodes.Status500InternalServerError, $"{PaymentConstants.InsertingPaymentError}: {ex.Message}");
            }
        }
    }
}
