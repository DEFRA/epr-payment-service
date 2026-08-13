using Azure.Messaging.ServiceBus;

namespace EPR.Payment.Service.Messaging;

public interface IServiceBusMessageConsumer
{
    string TopicConfigKey { get; }

    string SubscriptionConfigKey { get; }

    Task ConsumeAsync(ServiceBusReceivedMessage message, IServiceProvider scopedProvider, CancellationToken cancellationToken);
}
