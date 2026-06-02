using System.Diagnostics.CodeAnalysis;
using Asp.Versioning;
using EPR.Payment.Service.Messaging;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EPR.Payment.Service.Controllers.Messaging;

[ExcludeFromCodeCoverage]
[ApiVersion(1)]
[ApiController]
[Route("api/")]
public class TestMessagingController : ControllerBase
{
    private readonly IServiceBusTopicPublisher _publisher;

    public TestMessagingController(IServiceBusTopicPublisher publisher)
    {
        _publisher = publisher;
    }

    [ApiExplorerSettings(GroupName = "v1")]
    [HttpPost("v1/test/publish-registration-submitted")]
    [SwaggerOperation(
        Summary = "Test endpoint - publishes a registration submitted message to the service bus topic",
        Description = "For local testing only. Publishes a message to the registration submitted topic."
    )]
    [SwaggerResponse(204, "Message published successfully")]
    [SwaggerResponse(500, "Failed to publish message")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PublishTestMessage([FromBody] TestPublishRequest request)
    {
        var message = new RegistrationSubmittedMessage(
            request.SubmissionId,
            request.RegistrationBlobName,
            request.ComplianceSchemeId,
            request.SubmissionPeriod,
            request.SubmissionDate
        );

        await _publisher.SendMessageAsync(message);
        return NoContent();
    }
}

[ExcludeFromCodeCoverage]
public record TestPublishRequest(
    Guid SubmissionId,
    string RegistrationBlobName,
    Guid? ComplianceSchemeId,
    string SubmissionPeriod,
    DateTime SubmissionDate
);
