using Asp.Versioning;
using EPR.Payment.Service.Common.Constants.RegistrationFees.LookUps;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.Producer;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EPR.Payment.Service.Controllers.RegistrationFees
{
    [ApiVersion(1)]
    [ApiController]
    [Route("api/v{version:apiVersion}/resubmissions")]
    [FeatureGate("EnableProducerResubmissionAmount")]
    public class ProducerResubmissionController : ControllerBase
    {
        private readonly IProducerResubmissionService _producerResubmissionService;

        public ProducerResubmissionController(IProducerResubmissionService producerResubmissionService)
        {
            _producerResubmissionService = producerResubmissionService ?? throw new ArgumentNullException(nameof(producerResubmissionService));
        }

        [HttpGet]
        [ProducesResponseType(typeof(decimal?), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Gets the resubmission amount for a producer",
            Description = "Retrieves the resubmission amount for a producer based on the specified regulator."
        )]
        public async Task<IActionResult> GetResubmissionAsync([FromQuery] RegulatorDto request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var resubmissionAmount = await _producerResubmissionService.GetResubmissionAsync(request, cancellationToken);
                return Ok(resubmissionAmount);
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
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ProducerResubmissionConstants.Status500InternalServerError}: {(ex.InnerException != null ? ex.InnerException.Message : ex.Message)}");
            }
        }
    }
}
