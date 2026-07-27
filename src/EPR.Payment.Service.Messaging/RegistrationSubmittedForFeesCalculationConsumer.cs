using EPR.Payment.Service.Common.Dtos.Request.RegistrationSubmission;
using EPR.Payment.Service.Common.Services.Interfaces.RegistrationSubmission;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EPR.Payment.Service.Messaging;

public class RegistrationSubmittedForFeesCalculationConsumer : ServiceBusMessageConsumerBase<RegistrationSubmittedMessage>
{
    public const string TopicConfigurationKey = "ServiceBus:RegistrationSubmittedForFeesCalculationTopicName";
    public const string SubscriptionConfigurationKey = "ServiceBus:RegistrationSubmittedForFeesCalculationSubscriptionName";

    private readonly ILogger<RegistrationSubmittedForFeesCalculationConsumer> _logger;

    public RegistrationSubmittedForFeesCalculationConsumer(ILogger<RegistrationSubmittedForFeesCalculationConsumer> logger)
        : base(logger)
    {
        _logger = logger;
    }

    public override string TopicConfigKey => TopicConfigurationKey;

    public override string SubscriptionConfigKey => SubscriptionConfigurationKey;

    protected override async Task HandleAsync(RegistrationSubmittedMessage message, IServiceProvider scopedProvider, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Registration submitted (fees calculation) message received: SubmissionId={SubmissionId}, RegistrationBlobName={RegistrationBlobName}",
            message.SubmissionId,
            message.RegistrationBlobName);

        var handler = scopedProvider.GetRequiredService<IRegistrationSubmissionDataHandler>();

        var request = new CreateRegistrationSubmissionDataRequest
        {
            SubmissionId = message.SubmissionId,
            RegistrationBlobName = message.RegistrationBlobName,
            ComplianceSchemeId = message.ComplianceSchemeId,
            SubmissionPeriodId = message.SubmissionPeriodId,
            SubmissionDate = message.SubmissionDate,
            RegulatorNation = message.RegulatorNation,
            ApplicationReferenceNumber = message.ApplicationReferenceNumber,
        };

        await handler.HandleAsync(request, cancellationToken);

        _logger.LogInformation(
            "Processed registration submitted (fees calculation) message for SubmissionId {SubmissionId}",
            message.SubmissionId);
    }
}
