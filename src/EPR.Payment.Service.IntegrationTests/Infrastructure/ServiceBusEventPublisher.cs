using System.Text.Json;
using Azure.Messaging.ServiceBus;
using EPR.Payment.Service.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EPR.Payment.Service.IntegrationTests.Infrastructure;

/// <summary>
/// Publishes the real Service Bus messages the production consumers deserialise, using the same
/// message record types as <c>EPR.Payment.Service.Messaging</c> so the wire shape can never drift
/// from what <see cref="RegistrationSubmittedForRegulatorApprovalConsumer"/> actually expects.
/// </summary>
public sealed class ServiceBusEventPublisher
{
    private readonly string _connectionString;
    private readonly string _topicName;

    public ServiceBusEventPublisher(ServiceFixture fixture)
    {
        var configuration = fixture.SharedServices.GetRequiredService<IConfiguration>();
        _connectionString = configuration.GetValue<string>("ServiceBus:ConnectionString")
            ?? throw new InvalidOperationException("ServiceBus:ConnectionString is not configured.");
        _topicName = configuration.GetValue<string>(RegistrationSubmittedForRegulatorApprovalConsumer.TopicConfigurationKey)
            ?? throw new InvalidOperationException($"{RegistrationSubmittedForRegulatorApprovalConsumer.TopicConfigurationKey} is not configured.");
    }

    /// <summary>
    /// Publishes a SubmittedForRegulatorApproval message, the trigger for
    /// <see cref="EPR.Payment.Service.Services.RegistrationSubmission.RegistrationFeeSnapshotHandler"/>.
    /// Fire-and-forget from the test's perspective - consumption happens on the app's own
    /// background processor, so callers should follow up with <see cref="ResponsePolling.WaitUntilAsync"/>
    /// rather than assuming the snapshot exists immediately after this returns.
    /// </summary>
    public async Task PublishSubmittedForRegulatorApprovalAsync(
        Guid submissionId,
        string applicationReferenceNumber,
        DateTime submissionDate,
        CancellationToken cancellationToken = default)
    {
        var message = new RegistrationSubmittedForRegulatorApprovalMessage(
            submissionId,
            applicationReferenceNumber,
            submissionDate);

        await using var client = new ServiceBusClient(_connectionString);
        await using var sender = client.CreateSender(_topicName);
        await sender.SendMessageAsync(
            new ServiceBusMessage(JsonSerializer.SerializeToUtf8Bytes(message)),
            cancellationToken);
    }
}
