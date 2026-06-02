using System.Diagnostics.CodeAnalysis;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using EPR.Payment.Service.Common.Data.DataModels;
using EPR.Payment.Service.Common.Data.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EPR.Payment.Service.Messaging;

[ExcludeFromCodeCoverage]
public class ServiceBusTopicSubscription : IServiceBusTopicSubscription
{
    private readonly ILogger<ServiceBusTopicSubscription> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ServiceBusClient? _client;
    private readonly ServiceBusAdministrationClient? _adminClient;
    private readonly string _topicName;
    private readonly string _subscriptionName;
    private ServiceBusProcessor? _processor;

    public ServiceBusTopicSubscription(
        ILogger<ServiceBusTopicSubscription> logger,
        IConfiguration configuration,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        _topicName = configuration.GetValue<string>("ServiceBus:TopicName")!;
        _subscriptionName = configuration.GetValue<string>("ServiceBus:SubscriptionName")!;

        var connectionString = configuration.GetValue<string>("ServiceBus:ConnectionString");

        if (!string.IsNullOrEmpty(connectionString))
        {
            _client = new ServiceBusClient(connectionString);
            _adminClient = new ServiceBusAdministrationClient(connectionString);
        }
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

        _logger.LogInformation("Received registration submitted message for SubmissionId {SubmissionId}", message.SubmissionId);

        try
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<IAppDbContext>();

            var entity = new RegistrationSubmissionData
            {
                Id = Guid.NewGuid(),
                SubmissionId = message.SubmissionId,
                RegistrationBlobName = message.RegistrationBlobName,
                ComplianceSchemeId = message.ComplianceSchemeId,
                SubmissionPeriod = message.SubmissionPeriod,
                SubmissionDate = message.SubmissionDate,
                CreatedDate = DateTimeOffset.UtcNow
            };

            dbContext.RegistrationSubmissionData.Add(entity);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            await args.CompleteMessageAsync(args.Message);

            _logger.LogInformation("Saved RegistrationSubmissionData for SubmissionId {SubmissionId}", message.SubmissionId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to process registration submitted message for SubmissionId {SubmissionId}", message.SubmissionId);
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
