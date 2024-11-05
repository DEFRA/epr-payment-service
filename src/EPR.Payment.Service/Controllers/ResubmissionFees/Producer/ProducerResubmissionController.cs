using Asp.Versioning;
using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Common.Dtos.Request.ResubmissionFees.Producer;
using EPR.Payment.Service.Common.Dtos.Response.ResubmissionFees.Producer;
using EPR.Payment.Service.Services.Interfaces.ResubmissionFees.Producer;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EPR.Payment.Service.Controllers.ResubmissionFees.Producer
{
    [ApiVersion(1)]
    [ApiController]
    [Route("api/v{version:apiVersion}/producer")]
    [FeatureGate("EnableProducersResubmissionFeesFeature")]
    public class ProducerResubmissionController : ControllerBase
    {
        private readonly IProducerResubmissionService _producerResubmissionService;
        private readonly IValidator<ProducerResubmissionFeeRequestDto> _validator;

        public ProducerResubmissionController(
            IProducerResubmissionService producerResubmissionService,
            IValidator<ProducerResubmissionFeeRequestDto> validator)
        {
            _producerResubmissionService = producerResubmissionService ?? throw new ArgumentNullException(nameof(producerResubmissionService));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        [HttpPost("resubmission-fee")]
        [ProducesResponseType(typeof(ProducerResubmissionFeeResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Calculates the resubmission fee for a producer",
            Description = "Calculates the resubmission fee for a producer based on provided request details."
        )]
        [FeatureGate("EnableProducerResubmissionFee")]
        public async Task<IActionResult> GetResubmissionAsync([FromBody] ProducerResubmissionFeeRequestDto request, CancellationToken cancellationToken)
        {
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
                var response = await _producerResubmissionService.GetResubmissionFeeAsync(request, cancellationToken);
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
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Unexpected Error",
                    Detail = $"{ProducerResubmissionExceptions.Status500InternalServerError}: {ex.Message}",
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }
    }
}