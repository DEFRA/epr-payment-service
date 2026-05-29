namespace EPR.Payment.Service.Messaging;

public interface IServiceBusTopicPublisher
{
    Task SendMessageAsync(RegistrationSubmittedMessage message);
}
