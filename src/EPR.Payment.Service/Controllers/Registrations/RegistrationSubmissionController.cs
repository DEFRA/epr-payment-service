using Asp.Versioning;
using EPR.Payment.Service.Services.Interfaces.Registrations;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EPR.Payment.Service.Controllers.Registrations
{
    [ApiVersion(1)]
    [ApiController]
    [Route("api/")]
    public class RegistrationSubmissionController : ControllerBase
    {
        private readonly IRegistrationSubmissionService _registrationSubmissionService;

        public RegistrationSubmissionController(IRegistrationSubmissionService registrationSubmissionService)
        {
            _registrationSubmissionService = registrationSubmissionService ?? throw new ArgumentNullException(nameof(registrationSubmissionService));
        }

        [ApiExplorerSettings(GroupName = "v1")]
        [HttpHead("v1/registrations/{submissionId}")]
        [SwaggerOperation(
            Summary = "Checks whether a registration submission exists",
            Description = "Returns 204 if the submission has been received, 404 if it has not."
        )]
        [SwaggerResponse(204, "Submission exists")]
        [SwaggerResponse(404, "Submission not found")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SubmissionExistsAsync(Guid submissionId, CancellationToken cancellationToken)
        {
            var exists = await _registrationSubmissionService.SubmissionExistsAsync(submissionId, cancellationToken);
            return exists ? NoContent() : NotFound();
        }
    }
}
