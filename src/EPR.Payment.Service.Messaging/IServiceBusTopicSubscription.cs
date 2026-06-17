namespace EPR.Payment.Service.Messaging;

public interface IServiceBusTopicSubscription
{
    Task PrepareServiceBusSubscriptionAsync();
    Task CloseSubscriptionAsync();
    ValueTask DisposeAsync();
}
