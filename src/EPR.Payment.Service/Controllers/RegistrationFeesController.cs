using Asp.Versioning;
using EPR.Payment.Service.Common.Constants;
using EPR.Payment.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;

namespace EPR.Payment.Service.Controllers
{
    [ApiVersion(1)]
    [ApiController]
    [Route("api/v{version:apiVersion}/registrationFees")]
    [FeatureGate("EnableRegistrationFeesFeature")]
    public class RegistrationFeesController : ControllerBase
    {
        private readonly IRegistrationFeesService _registrationFeesService;

        public RegistrationFeesController(IRegistrationFeesService registrationFeesService)
        {
            _registrationFeesService = registrationFeesService ?? throw new ArgumentNullException(nameof(registrationFeesService));
        }

        [MapToApiVersion(1)]
        [HttpGet("{regulator}/ProducerResubmission")]
        [ProducesResponseType(typeof(decimal?), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [FeatureGate("EnableProducerResubmissionAmount")]
        public async Task<IActionResult> GetProducerResubmissionAmountByRegulator(string regulator, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(regulator))
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Detail = "Regulator cannot be null or empty.",
                    Status = StatusCodes.Status400BadRequest
                });
            }
            try
            {
                var registrationFeesResponse = await _registrationFeesService.GetProducerResubmissionAmountByRegulatorAsync(regulator, cancellationToken);
                return Ok(registrationFeesResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{FeesConstants.Status500InternalServerError}: {(ex.InnerException != null ? ex.InnerException.Message : ex.Message)}");
            }
        }
    }
}
