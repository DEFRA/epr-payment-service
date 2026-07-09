using Asp.Versioning;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EPR.Payment.Service.Controllers.RegistrationFees
{
    [ApiVersion(1)]
    [ApiController]
    [Route("api/")]
    public class SubmissionPeriodsController : ControllerBase
    {
        private readonly ISubmissionPeriodsService _submissionPeriodsService;

        public SubmissionPeriodsController(ISubmissionPeriodsService submissionPeriodsService)
        {
            _submissionPeriodsService = submissionPeriodsService ?? throw new ArgumentNullException(nameof(submissionPeriodsService));
        }

        [ApiExplorerSettings(GroupName = "v1")]
        [HttpGet("v1/submission-periods")]
        [SwaggerOperation(
            Summary = "Lists all submission periods",
            Description = "Returns the full set of registration submission periods (one row per WindowType and RegistrationYear).")]
        [SwaggerResponse(200, "The list of submission periods.", typeof(IReadOnlyList<SubmissionPeriodResponseDto>))]
        [SwaggerResponse(500, "Internal server error occurred while retrieving submission periods.")]
        [ProducesResponseType(typeof(IReadOnlyList<SubmissionPeriodResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSubmissionPeriods(CancellationToken cancellationToken)
        {
            var periods = await _submissionPeriodsService.GetAllAsync(cancellationToken);
            return Ok(periods);
        }
    }
}
