using EPR.Payment.Service.Common.Dtos.Response.RegistrationSubmission;
using EPR.Payment.Service.Services.Interfaces.RegistrationSubmission;
using Microsoft.AspNetCore.Mvc;

namespace EPR.Payment.Service.Controllers.RegistrationSubmission
{
    [ApiController]
    [Route("api/")]
    public class RegistrationSubmissionDataController(
        IRegistrationFeeCalculationDetailsService feeCalculationDetailsService) : ControllerBase
    {
        [ApiExplorerSettings(GroupName = "v1")]
        [HttpGet("v1/registration-submission-data/{submissionId:guid}/fee-calculation-details")]
        [ProducesResponseType(typeof(IReadOnlyList<RegistrationFeeCalculationDetailsDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFeeCalculationDetails(Guid submissionId, CancellationToken cancellationToken)
        {
            if (submissionId == Guid.Empty)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Validation Error",
                    Detail = "SubmissionId is required.",
                    Status = StatusCodes.Status400BadRequest,
                });
            }

            try
            {
                var details = await feeCalculationDetailsService.GetAsync(submissionId, cancellationToken);
                if (details.Count == 0)
                {
                    return NotFound();
                }

                return Ok(details);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving registration fee calculation details: {ex.Message}");
            }
        }
    }
}
