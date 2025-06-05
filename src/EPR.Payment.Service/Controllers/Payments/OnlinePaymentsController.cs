using EPR.Payment.Service.Common.Constants.Payments;
using EPR.Payment.Service.Common.Dtos.Request.Payments;
using EPR.Payment.Service.Common.Dtos.Response.Payments;
using EPR.Payment.Service.Services.Interfaces.Payments;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;

namespace EPR.Payment.Service.Controllers
{
    [ApiController]
    [Route("api/")]
    [FeatureGate("EnableOnlinePaymentsFeature")]
    public class OnlinePaymentsController(IOnlinePaymentsService onlinePaymentsService,
        IValidator<OnlinePaymentInsertRequestDto> onlinePaymentInsertRequestValidator,
        IValidator<OnlinePaymentInsertRequestV2Dto> onlinePaymentInsertRequestValidatorV2,
        IValidator<OnlinePaymentUpdateRequestDto> onlinePaymentUpdateRequestValidator) : ControllerBase
    {
        [ApiExplorerSettings(GroupName = "v1")]
        [HttpPost("v1/online-payments")]
        [ProducesResponseType(typeof(Guid), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [FeatureGate("EnableOnlinePaymentInsert")]
        public async Task<ActionResult<Guid>> InsertOnlinePayment([FromBody] OnlinePaymentInsertRequestDto onlinePaymentInsertRequest, CancellationToken cancellationToken)
        {
            var validatorResult = onlinePaymentInsertRequestValidator.Validate(onlinePaymentInsertRequest);

            if (!validatorResult.IsValid)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Detail = string.Join("; ", validatorResult.Errors.Select(e => e.ErrorMessage)),
                    Status = StatusCodes.Status400BadRequest
                });
            }

            return await InsertWithErrorHanding(() => onlinePaymentsService.InsertOnlinePaymentAsync(onlinePaymentInsertRequest, cancellationToken), PaymentConstants.InsertingPaymentError);
        }


        [ApiExplorerSettings(GroupName = "v2")]
        [HttpPost("v2/online-payments")]
        [ProducesResponseType(typeof(NoContentResult), 204)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [FeatureGate("EnableOnlinePaymentInsertV2")]
        public async Task<ActionResult<Guid>> InsertOnlinePaymentV2(
            [FromBody] OnlinePaymentInsertRequestV2Dto onlinePaymentInsertRequest,
            CancellationToken cancellationToken)
        {
            var validatorResult = onlinePaymentInsertRequestValidatorV2.Validate(onlinePaymentInsertRequest);

            if (!validatorResult.IsValid)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Detail = string.Join("; ", validatorResult.Errors.Select(e => e.ErrorMessage)),
                    Status = StatusCodes.Status400BadRequest
                });
            }

            return await InsertWithErrorHanding(() => onlinePaymentsService.InsertOnlinePaymentAsync(onlinePaymentInsertRequest, cancellationToken), PaymentConstants.InsertingPaymentError);
        }

        [ApiExplorerSettings(GroupName = "v1")]
        [HttpPut("v1/online-payments/{externalPaymentId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [FeatureGate("EnableOnlinePaymentUpdate")]
        public async Task<IActionResult> UpdateOnlinePayment(Guid externalPaymentId, [FromBody] OnlinePaymentUpdateRequestDto onlinePaymentUpdateRequest, CancellationToken cancellationToken)
        {
            var validatorResult = onlinePaymentUpdateRequestValidator.Validate(onlinePaymentUpdateRequest);

            if (!validatorResult.IsValid)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Detail = string.Join("; ", validatorResult.Errors.Select(e => e.ErrorMessage)),
                    Status = StatusCodes.Status400BadRequest
                });
            }

            return await UpdateWithErrorHanding(() => onlinePaymentsService.UpdateOnlinePaymentAsync(externalPaymentId, onlinePaymentUpdateRequest, cancellationToken), PaymentConstants.UpdatingPaymentError);
        }

        [ApiExplorerSettings(GroupName = "v1")]
        [HttpGet("v1/online-payments/{externalPaymentId}")]
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
                var onlinePaymentResponse = await onlinePaymentsService.GetOnlinePaymentByExternalPaymentIdAsync(externalPaymentId, cancellationToken);
                return Ok(onlinePaymentResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{PaymentConstants.ReceivingPaymentError}: {ex.Message}");
            }
        }

        private async Task<ActionResult> InsertWithErrorHanding<T>(Func<Task<T>> asyncAction, string errorMessage)
        {
            try
            {
                var result = await asyncAction();
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
                return StatusCode(StatusCodes.Status500InternalServerError, $"{errorMessage}: {ex.Message}");
            }
        }

        private async Task<ActionResult> UpdateWithErrorHanding(Func<Task> asyncAction, string errorMessage)
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
                return StatusCode(StatusCodes.Status500InternalServerError, $"{errorMessage}: {ex.Message}");
            }
        }
    }
}
