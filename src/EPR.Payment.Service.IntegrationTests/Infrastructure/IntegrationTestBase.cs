using Azure.Messaging.ServiceBus.Administration;
using EPR.Payment.Service.IntegrationTests.Infrastructure.Builders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EPR.Payment.Service.IntegrationTests.Infrastructure;

[Collection(PaymentServiceCollection.Name)]
[Trait("Category", "IntegrationTest")]
public abstract class IntegrationTestBase
{
    private readonly ServiceFixture _fixture;
    protected HttpClient Client { get; }
    
    protected readonly ServiceBusAdministrationClient ServiceBusAdministrationClient;
    protected readonly IConfiguration Configuration;

    /// <summary>
    /// Fluent test-data entrypoint. See <see cref="TestBuilders"/> for the available builders —
    /// <c>Builder.Producer().Build()</c>, <c>Builder.Regulator().InNation(x).Build()</c>,
    /// <c>Builder.SchemeOperator().WithAdmin().Build()</c>, etc.
    /// </summary>
    protected TestBuilders Builder { get; }

    /// <summary>Publishes the real production Service Bus messages - see <see cref="ServiceBusEventPublisher"/>.</summary>
    protected ServiceBusEventPublisher Events { get; }

    /// <summary>Everything logged by the hosted app so far, across every test in the collection -
    /// see <see cref="TestLogSink"/>. Filter by something unique to your own test (e.g. a Guid).</summary>
    protected TestLogSink Logs { get; }

    protected IntegrationTestBase(ServiceFixture fixture)
    {
        _fixture = fixture;
        Client = _fixture.CreateHttpClient();
        Builder = new TestBuilders(fixture);
        Events = new ServiceBusEventPublisher(fixture);
        Logs = _fixture.Logs;
        ServiceBusAdministrationClient = _fixture.SharedServices.GetRequiredService<ServiceBusAdministrationClient>();
        Configuration = _fixture.SharedServices.GetRequiredService<IConfiguration>();
    }
}
