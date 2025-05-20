using Asp.Versioning;
using EPR.Payment.Service.Common.Constants.Payments;
using EPR.Payment.Service.Common.Dtos.Request.Payments;
using EPR.Payment.Service.Services.Interfaces.Payments;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;

namespace EPR.Payment.Service.Controllers.Payments
{
    [ApiController]
    [Route("api/")]
    [FeatureGate("EnableOfflinePaymentsFeature")]
    public class OfflinePaymentsController : ControllerBase
    {
        private readonly IOfflinePaymentsService _offlinePaymentsService;
        private readonly IValidator<OfflinePaymentInsertRequestDto> _offlinePaymentInsertRequestValidator;
        private readonly IValidator<OfflinePaymentInsertRequestV2Dto> _offlinePaymentInsertRequestV2Validator;

        public OfflinePaymentsController(
            IOfflinePaymentsService paymentsService,
            IValidator<OfflinePaymentInsertRequestDto> offlinePaymentInsertRequestValidator,
            IValidator<OfflinePaymentInsertRequestV2Dto> offlinePaymentInsertRequestV2Validator)
        {
            _offlinePaymentsService = paymentsService ?? throw new ArgumentNullException(nameof(paymentsService));
            _offlinePaymentInsertRequestValidator = offlinePaymentInsertRequestValidator ?? throw new ArgumentNullException(nameof(offlinePaymentInsertRequestValidator));
            _offlinePaymentInsertRequestV2Validator = offlinePaymentInsertRequestV2Validator ?? throw new ArgumentNullException(nameof(offlinePaymentInsertRequestV2Validator)); ;
        }

        [ApiExplorerSettings(GroupName = "v1")]
        [HttpPost("v1/offline-payments")]
        [ProducesResponseType(typeof(NoContentResult), 204)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [FeatureGate("EnableOfflinePayment")]
        public async Task<ActionResult> InsertOfflinePaymentV1(
            [FromBody] OfflinePaymentInsertRequestDto offlinePaymentInsertRequest,
            CancellationToken cancellationToken)
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

            return await ExecuteWithErrorHanding(() => _offlinePaymentsService.InsertOfflinePaymentAsync(offlinePaymentInsertRequest, cancellationToken));
        }

        [ApiExplorerSettings(GroupName = "v2")]
        [HttpPost("v2/offline-payments")]
        [ProducesResponseType(typeof(NoContentResult), 204)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [FeatureGate("EnableOfflinePayment")]
        public async Task<ActionResult> InsertOfflinePaymentV2(
            [FromBody] OfflinePaymentInsertRequestV2Dto offlinePaymentInsertRequest,
            CancellationToken cancellationToken)
        {
            var validatorResult = _offlinePaymentInsertRequestV2Validator.Validate(offlinePaymentInsertRequest);

            if (!validatorResult.IsValid)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Detail = string.Join("; ", validatorResult.Errors.Select(e => e.ErrorMessage)),
                    Status = StatusCodes.Status400BadRequest
                });
            }

            return await ExecuteWithErrorHanding(() => _offlinePaymentsService.InsertOfflinePaymentAsync(offlinePaymentInsertRequest, cancellationToken));
        }

        private async Task<ActionResult> ExecuteWithErrorHanding(Func<Task> asyncAction)
        {
            try
            {
                await asyncAction();

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
