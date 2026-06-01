using System.Diagnostics.CodeAnalysis;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace EPR.Payment.Service.Messaging;

[ExcludeFromCodeCoverage]
public class ServiceBusTopicPublisher : IServiceBusTopicPublisher
{
    private readonly ILogger<ServiceBusTopicPublisher> _logger;
    private readonly ServiceBusClient? _client;
    private readonly string _topicName;

    public ServiceBusTopicPublisher(ILogger<ServiceBusTopicPublisher> logger, IConfiguration configuration)
    {
        _logger = logger;
        _topicName = configuration.GetValue<string>("ServiceBus:TopicName")!;

        var connectionString = configuration.GetValue<string>("ServiceBus:ConnectionString");
        if (!string.IsNullOrEmpty(connectionString))
        {
            _client = new ServiceBusClient(connectionString);
        }
    }

    public async Task SendMessageAsync(RegistrationSubmittedMessage message)
    {
        if (_client is null)
        {
            throw new InvalidOperationException("Service bus client is null. Please check your connection string.");
        }

        var sender = _client.CreateSender(_topicName);
        var body = JsonSerializer.Serialize(message);

        try
        {
            _logger.LogInformation("Publishing message to topic {TopicName}: {Body}", _topicName, body);
            await sender.SendMessageAsync(new ServiceBusMessage(body));
            _logger.LogInformation("Message published successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish message to topic {TopicName}", _topicName);
            throw;
        }
        finally
        {
            await sender.DisposeAsync();
        }
    }
}
