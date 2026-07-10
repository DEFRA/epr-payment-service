using Asp.Versioning;
using EPR.Payment.Service.Common.Dtos.Response.SubmissionPeriods;
using EPR.Payment.Service.Services.Interfaces.SubmissionPeriods;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EPR.Payment.Service.Controllers.SubmissionPeriods
{
    [ApiVersion(1)]
    [ApiController]
    [Route("api/")]
    public class SubmissionPeriodsController : ControllerBase
    {
        private readonly ISubmissionPeriodsService _submissionPeriodsService;
        private readonly ILogger<SubmissionPeriodsController> _logger;

        public SubmissionPeriodsController(
            ISubmissionPeriodsService submissionPeriodsService,
            ILogger<SubmissionPeriodsController> logger)
        {
            _submissionPeriodsService = submissionPeriodsService ?? throw new ArgumentNullException(nameof(submissionPeriodsService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
            using var logScope = _logger.BeginScope(new Dictionary<string, object>
            {
                ["Operation"] = nameof(GetSubmissionPeriods),
            });

            _logger.LogInformation("Retrieving submission periods lookup rows.");

            var periods = await _submissionPeriodsService.GetAllAsync(cancellationToken);

            _logger.LogInformation("Retrieved {PeriodCount} submission periods.", periods.Count);

            return Ok(periods);
        }
    }
}
