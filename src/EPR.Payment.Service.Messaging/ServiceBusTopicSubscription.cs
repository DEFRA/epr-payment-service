using System.Diagnostics.CodeAnalysis;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using EPR.Payment.Service.Common.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EPR.Payment.Service.Messaging;

[ExcludeFromCodeCoverage]
public class ServiceBusTopicSubscription : IServiceBusTopicSubscription
{
    private readonly ILogger<ServiceBusTopicSubscription> _logger;
    private readonly IConfiguration _configuration;
    private readonly IServiceProvider _serviceProvider;
    private readonly ServiceBusClient? _client;
    private readonly ServiceBusAdministrationClient? _adminClient;
    private readonly IReadOnlyList<IServiceBusMessageConsumer> _consumers;
    private readonly List<ServiceBusProcessor> _processors = new();

    public ServiceBusTopicSubscription(
        ILogger<ServiceBusTopicSubscription> logger,
        IConfiguration configuration,
        IServiceProvider serviceProvider,
        IEnumerable<IServiceBusMessageConsumer> consumers)
    {
        _logger = logger;
        _configuration = configuration;
        _serviceProvider = serviceProvider;
        _consumers = consumers?.ToList() ?? throw new ArgumentNullException(nameof(consumers));
        _client = serviceProvider.GetService<ServiceBusClient>();
        _adminClient = serviceProvider.GetService<ServiceBusAdministrationClient>();
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

            foreach (var consumer in _consumers)
            {
                await SetupSubscription(consumer);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while setting up the service bus subscription");
        }
    }

    private async Task SetupSubscription(IServiceBusMessageConsumer consumer)
    {
        var topicName = _configuration.GetValue<string>(consumer.TopicConfigKey)
            ?? throw new InvalidOperationException($"Missing configuration value for {consumer.TopicConfigKey}");
        var subscriptionName = _configuration.GetValue<string>(consumer.SubscriptionConfigKey)
            ?? throw new InvalidOperationException($"Missing configuration value for {consumer.SubscriptionConfigKey}");

        using (_logger.AddScopedData(new Dictionary<string, object>
               {
                   ["TopicName"] = topicName,
                   ["SubscriptionName"] = subscriptionName,
                   ["ConsumerType"] = consumer.GetType().Name,
               }))
        {
            _logger.LogInformation("Setting up service bus subscription");

            try
            {
                var topicExists = await _adminClient!.TopicExistsAsync(topicName);
                if (!topicExists.Value)
                {
                    _logger.LogInformation("Creating topic");
                    await _adminClient.CreateTopicAsync(topicName);
                }

                var subscriptionExists = await _adminClient.SubscriptionExistsAsync(topicName, subscriptionName);
                if (!subscriptionExists.Value)
                {
                    _logger.LogInformation("Creating subscription");
                    await _adminClient.CreateSubscriptionAsync(topicName, subscriptionName);
                }

                _logger.LogInformation("Service bus subscription is ready");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex,
                    "Unable to verify or create topic/subscription via admin client — assuming they already exist and proceeding");
            }

            var processor = _client!.CreateProcessor(topicName, subscriptionName, new ServiceBusProcessorOptions
            {
                MaxConcurrentCalls = 1,
                AutoCompleteMessages = false,
            });

            processor.ProcessMessageAsync += args => ProcessMessageAsync(consumer, args);
            processor.ProcessErrorAsync += ProcessErrorAsync;

            await processor.StartProcessingAsync();
            _processors.Add(processor);
        }
    }

    private async Task ProcessMessageAsync(IServiceBusMessageConsumer consumer, ProcessMessageEventArgs args)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            await consumer.ConsumeAsync(args.Message, scope.ServiceProvider, args.CancellationToken);
            await args.CompleteMessageAsync(args.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to process message for consumer {ConsumerType}; abandoning for redelivery",
                consumer.GetType().Name);
            await args.AbandonMessageAsync(args.Message);
        }
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
        foreach (var processor in _processors)
        {
            await processor.CloseAsync();
        }
    }

    public async ValueTask DisposeAsync()
    {
        foreach (var processor in _processors)
        {
            await processor.DisposeAsync();
        }

        if (_client != null)
        {
            await _client.DisposeAsync();
        }
    }
}
