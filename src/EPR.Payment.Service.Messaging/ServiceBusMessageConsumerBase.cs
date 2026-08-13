using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;

namespace EPR.Payment.Service.Messaging;

public abstract class ServiceBusMessageConsumerBase<TMessage> : IServiceBusMessageConsumer
    where TMessage : class
{
    private readonly ILogger _logger;

    protected ServiceBusMessageConsumerBase(ILogger logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public abstract string TopicConfigKey { get; }

    public abstract string SubscriptionConfigKey { get; }

    public async Task ConsumeAsync(ServiceBusReceivedMessage message, IServiceProvider scopedProvider, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(message);
        ArgumentNullException.ThrowIfNull(scopedProvider);

        var payload = message.Body.ToObjectFromJson<TMessage>();
        if (payload is null)
        {
            _logger.LogWarning("Received a null or undeserializable {MessageType} message; skipping", typeof(TMessage).Name);
            return;
        }

        await HandleAsync(payload, scopedProvider, cancellationToken);
    }

    protected abstract Task HandleAsync(TMessage message, IServiceProvider scopedProvider, CancellationToken cancellationToken);
}
