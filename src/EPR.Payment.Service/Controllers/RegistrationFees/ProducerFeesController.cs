using Asp.Versioning;
using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.Producer;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EPR.Payment.Service.Controllers
{
    [ApiVersion(1)]
    [ApiController]
    [Route("api/v{version:apiVersion}/producer")]
    [FeatureGate("EnableRegistrationFeesFeature")]
    public class ProducerFeesController : ControllerBase
    {
        private readonly IProducerFeesCalculatorService _producerFeesCalculatorService;
        private readonly IValidator<ProducerRegistrationFeesRequestDto> _validator;

        public ProducerFeesController(
            IProducerFeesCalculatorService producerFeesCalculatorService,
            IValidator<ProducerRegistrationFeesRequestDto> validator)
        {
            _producerFeesCalculatorService = producerFeesCalculatorService ?? throw new ArgumentNullException(nameof(producerFeesCalculatorService));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        [MapToApiVersion(1)]
        [HttpPost("registration-fee")]
        [SwaggerOperation(
            Summary = "Calculates the registration fees for a producer",
            Description = "Calculates the total fees including base fee, subsidiaries fee, and any additional fees for an online marketplace producer."
        )]
        [SwaggerResponse(200, "Returns the calculated registration fees", typeof(RegistrationFeesResponseDto))]
        [SwaggerResponse(400, "Bad request due to validation errors or invalid input")]
        [SwaggerResponse(500, "Internal server error occurred while calculating fees")]
        [ProducesResponseType(typeof(RegistrationFeesResponseDto), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [FeatureGate("EnableRegistrationFeesCalculation")]
        public async Task<ActionResult<RegistrationFeesResponseDto>> CalculateFeesAsync(
            [FromBody] ProducerRegistrationFeesRequestDto request,
            CancellationToken cancellationToken)
        {
            // Manually validate the request
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
                var result = await _producerFeesCalculatorService.CalculateFeesAsync(request, cancellationToken);
                return Ok(result); // Return the calculated fees as a resource
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
                return StatusCode(StatusCodes.Status500InternalServerError, $"{ProducerFeesCalculationExceptions.FeeCalculationError}: {(ex.InnerException != null ? ex.InnerException.Message : ex.Message)}");
            }
        }
    }
}