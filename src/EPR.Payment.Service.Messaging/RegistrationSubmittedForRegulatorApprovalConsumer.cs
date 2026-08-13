using EPR.Payment.Service.Common.Dtos.Request.RegistrationSubmission;
using EPR.Payment.Service.Common.Services.Interfaces.RegistrationSubmission;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EPR.Payment.Service.Messaging;

public class RegistrationSubmittedForRegulatorApprovalConsumer : ServiceBusMessageConsumerBase<RegistrationSubmittedForRegulatorApprovalMessage>
{
    public const string TopicConfigurationKey = "ServiceBus:RegistrationSubmittedForRegulatorApprovalTopicName";
    public const string SubscriptionConfigurationKey = "ServiceBus:RegistrationSubmittedForRegulatorApprovalSubscriptionName";

    private readonly ILogger<RegistrationSubmittedForRegulatorApprovalConsumer> _logger;

    public RegistrationSubmittedForRegulatorApprovalConsumer(ILogger<RegistrationSubmittedForRegulatorApprovalConsumer> logger)
        : base(logger)
    {
        _logger = logger;
    }

    public override string TopicConfigKey => TopicConfigurationKey;

    public override string SubscriptionConfigKey => SubscriptionConfigurationKey;

    protected override async Task HandleAsync(RegistrationSubmittedForRegulatorApprovalMessage message, IServiceProvider scopedProvider, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Registration submitted (regulator approval) message received: SubmissionId={SubmissionId}, ApplicationReferenceNumber={ApplicationReferenceNumber}",
            message.SubmissionId,
            message.ApplicationReferenceNumber);

        var handler = scopedProvider.GetRequiredService<IRegistrationSubmittedForRegulatorApprovalHandler>();

        var request = new RegistrationSubmittedForRegulatorApprovalRequest
        {
            SubmissionId = message.SubmissionId,
            ApplicationReferenceNumber = message.ApplicationReferenceNumber,
            SubmissionDate = message.SubmissionDate,
        };

        await handler.HandleAsync(request, cancellationToken);
    }
}
