using EPR.Payment.Service.Common.Dtos.Request.RegistrationSubmission;
using EPR.Payment.Service.Common.Services.Interfaces.RegistrationSubmission;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EPR.Payment.Service.Messaging;

public class RegulatorRegistrationDecisionConsumer : ServiceBusMessageConsumerBase<RegulatorRegistrationDecisionMessage>
{
    public const string TopicConfigurationKey = "ServiceBus:RegulatorRegistrationDecisionTopicName";
    public const string SubscriptionConfigurationKey = "ServiceBus:RegulatorRegistrationDecisionSubscriptionName";

    private readonly ILogger<RegulatorRegistrationDecisionConsumer> _logger;

    public RegulatorRegistrationDecisionConsumer(ILogger<RegulatorRegistrationDecisionConsumer> logger)
        : base(logger)
    {
        _logger = logger;
    }

    public override string TopicConfigKey => TopicConfigurationKey;

    public override string SubscriptionConfigKey => SubscriptionConfigurationKey;

    protected override async Task HandleAsync(RegulatorRegistrationDecisionMessage message, IServiceProvider scopedProvider, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Regulator registration decision message received: SubmissionId={SubmissionId}, EventName={EventName}, DecisionDate={DecisionDate}",
            message.SubmissionId,
            message.EventName,
            message.DecisionDate);

        var handler = scopedProvider.GetRequiredService<IRegulatorRegistrationDecisionHandler>();

        var request = new RegulatorRegistrationDecisionRequest
        {
            SubmissionId = message.SubmissionId,
            EventName = message.EventName,
            DecisionDate = message.DecisionDate,
        };

        await handler.HandleAsync(request, cancellationToken);
    }
}
