using EPR.Payment.Service.Common.Dtos.Request.RegistrationSubmission;
using EPR.Payment.Service.Services.Interfaces.RegistrationSubmission;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;

namespace EPR.Payment.Service.Controllers.RegistrationSubmission
{
    [ApiController]
    [Route("api/")]
    [FeatureGate("EnableRegistrationSubmissionDataHandler")]
    public class RegistrationSubmissionDataController(
        IRegistrationSubmissionDataHandler handler,
        IValidator<CreateRegistrationSubmissionDataRequest> validator) : ControllerBase
    {
        [ApiExplorerSettings(GroupName = "v1")]
        [HttpPost("v1/registration-submission-data")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Guid>> Create(
            [FromBody] CreateRegistrationSubmissionDataRequest request,
            CancellationToken cancellationToken)
        {
            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Detail = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)),
                    Status = StatusCodes.Status400BadRequest,
                });
            }

            try
            {
                var id = await handler.HandleAsync(request, cancellationToken);
                return Ok(id);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Detail = ex.Message,
                    Status = StatusCodes.Status400BadRequest,
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Invalid Registration Submission",
                    Detail = ex.Message,
                    Status = StatusCodes.Status400BadRequest,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error processing registration submission: {ex.Message}");
            }
        }
    }
}
