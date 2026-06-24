using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using EPR.Payment.Service.IntegrationTests.Infrastructure.Builders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EPR.Payment.Service.IntegrationTests.Infrastructure;

[Collection(PaymentServiceCollection.Name)]
[Trait("Category", "IntegrationTest")]
public abstract class IntegrationTestBase : IAsyncLifetime
{
    private readonly ServiceFixture _fixture;
    private readonly string _topicName;
    private readonly string _subscriptionName;

    protected HttpClient Client { get; }

    protected readonly ServiceBusAdministrationClient ServiceBusAdministrationClient;
    protected readonly ServiceBusClient ServiceBusClient;
    protected readonly IConfiguration Configuration;

    /// <summary>
    /// Fluent test-data entrypoint. See <see cref="TestBuilders"/> for the available builders —
    /// <c>Builder.Producer().Build()</c>, <c>Builder.Regulator().InNation(x).Build()</c>,
    /// <c>Builder.SchemeOperator().WithAdmin().Build()</c>, etc.
    /// </summary>
    protected TestBuilders Builder { get; }

    protected IntegrationTestBase(ServiceFixture fixture)
    {
        _fixture = fixture;
        Client = _fixture.CreateHttpClient();
        Builder = new TestBuilders(fixture);
        ServiceBusAdministrationClient = _fixture.SharedServices.GetRequiredService<ServiceBusAdministrationClient>();
        ServiceBusClient = _fixture.SharedServices.GetRequiredService<ServiceBusClient>();
        Configuration = _fixture.SharedServices.GetRequiredService<IConfiguration>();
        _topicName = Configuration.GetValue<string>("ServiceBus:TopicName")!;
        _subscriptionName = Configuration.GetValue<string>("ServiceBus:SubscriptionName")!;
    }

    public Task InitializeAsync() => Task.CompletedTask;

    // clean up the subscription after each test
    public async Task DisposeAsync()
    {
        await using var cleaner = ServiceBusClient.CreateReceiver(
            _topicName,
            _subscriptionName,
            new ServiceBusReceiverOptions { ReceiveMode = ServiceBusReceiveMode.ReceiveAndDelete });

        IReadOnlyList<ServiceBusReceivedMessage> batch;
        do
        {
            batch = await cleaner.ReceiveMessagesAsync(maxMessages: 100, maxWaitTime: TimeSpan.FromSeconds(1));
        } while (batch.Count > 0);
    }

    protected async Task SendMessageAndWaitUntilConsumed<T>(T message)
    {
        await using var sender = ServiceBusClient.CreateSender(_topicName);

        await sender.SendMessageAsync(new ServiceBusMessage(BinaryData.FromObjectAsJson(message)));

        var deadline = DateTimeOffset.UtcNow.AddSeconds(30);

        // Phase 1: wait until the message appears in the subscription. this assumes that the first poll will be faster
        // than the message can be consumed
        await PollSubscriptionUntilAsync(count => count > 0, deadline, TimeSpan.FromMilliseconds(50));

        // Phase 2: wait until TotalMessageCount returns to 0 — message completed by the worker
        await PollSubscriptionUntilAsync(count => count == 0, deadline, TimeSpan.FromMilliseconds(500));
    }

    private async Task PollSubscriptionUntilAsync(
        Func<long, bool> predicate,
        DateTimeOffset deadline,
        TimeSpan pollInterval)
    {
        while (DateTimeOffset.UtcNow < deadline)
        {
            var props = await ServiceBusAdministrationClient.GetSubscriptionRuntimePropertiesAsync(_topicName, _subscriptionName);
            if (predicate(props.Value.TotalMessageCount)) break;
            await Task.Delay(pollInterval);
        }
    }
}
