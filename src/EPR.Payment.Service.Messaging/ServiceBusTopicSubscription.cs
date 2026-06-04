using System.Diagnostics.CodeAnalysis;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EPR.Payment.Service.Messaging;

[ExcludeFromCodeCoverage]
public class ServiceBusTopicSubscription : IServiceBusTopicSubscription
{
    private readonly ILogger<ServiceBusTopicSubscription> _logger;
    private readonly ServiceBusClient? _client;
    private readonly ServiceBusAdministrationClient? _adminClient;
    private readonly string _topicName;
    private readonly string _subscriptionName;
    private ServiceBusProcessor? _processor;

    public ServiceBusTopicSubscription(
        ILogger<ServiceBusTopicSubscription> logger,
        IConfiguration configuration,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _topicName = configuration.GetValue<string>("ServiceBus:TopicName")!;
        _subscriptionName = configuration.GetValue<string>("ServiceBus:SubscriptionName")!;
        _client = serviceProvider.GetService(typeof(ServiceBusClient)) as ServiceBusClient;
        _adminClient = serviceProvider.GetService(typeof(ServiceBusAdministrationClient)) as ServiceBusAdministrationClient;
    }

    public async Task PrepareServiceBusSubscriptionAsync()
    {
        try
        {
            if (_adminClient is null || _client is null)
            {
                throw new InvalidOperationException(
                    "Service bus client is null. Please check your connection string.");
            }

            _logger.LogInformation("Setting up service bus subscription for topic {TopicName}", _topicName);

            try
            {
                var topicExists = await _adminClient.TopicExistsAsync(_topicName);
                if (!topicExists.Value)
                {
                    _logger.LogInformation("Creating topic {TopicName}", _topicName);
                    await _adminClient.CreateTopicAsync(_topicName);
                }

                var subscriptionExists = await _adminClient.SubscriptionExistsAsync(_topicName, _subscriptionName);
                if (!subscriptionExists.Value)
                {
                    _logger.LogInformation("Creating subscription {SubscriptionName} on topic {TopicName}", _subscriptionName, _topicName);
                    await _adminClient.CreateSubscriptionAsync(_topicName, _subscriptionName);
                }

                _logger.LogInformation("Service bus subscription {SubscriptionName} on topic {TopicName} is ready", _subscriptionName, _topicName);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Unable to verify or create topic/subscription via admin client — assuming they already exist and proceeding");
            }

            _processor = _client.CreateProcessor(_topicName, _subscriptionName, new ServiceBusProcessorOptions
            {
                MaxConcurrentCalls = 1,
                AutoCompleteMessages = false
            });

            _processor.ProcessMessageAsync += ProcessMessageAsync;
            _processor.ProcessErrorAsync += ProcessErrorAsync;

            await _processor.StartProcessingAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while setting up the service bus subscription");
        }
    }

    private async Task ProcessMessageAsync(ProcessMessageEventArgs args)
    {
        var message = args.Message.Body.ToObjectFromJson<RegistrationSubmittedMessage>();

        if (message is null)
        {
            _logger.LogWarning("Received a null or undeserializable message, skipping");
            await args.CompleteMessageAsync(args.Message);
            return;
        }

        _logger.LogInformation(
            "Registration submitted message received: SubmissionId={SubmissionId}, RegistrationBlobName={RegistrationBlobName}",
            message.SubmissionId,
            message.RegistrationBlobName);

        await args.CompleteMessageAsync(args.Message);
    }

    private Task ProcessErrorAsync(ProcessErrorEventArgs args)
    {
        _logger.LogError(args.Exception, "Service bus processor error");
        _logger.LogDebug("- ErrorSource: {ErrorSource}", args.ErrorSource);
        _logger.LogDebug("- Entity Path: {EntityPath}", args.EntityPath);
        _logger.LogDebug("- FullyQualifiedNamespace: {FullyQualifiedNamespace}", args.FullyQualifiedNamespace);
        return Task.CompletedTask;
    }

    public async Task CloseSubscriptionAsync()
    {
        if (_processor != null)
        {
            await _processor.CloseAsync();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_processor != null)
        {
            await _processor.DisposeAsync();
        }

        if (_client != null)
        {
            await _client.DisposeAsync();
        }
    }
}
