using EPR.Payment.Service.IntegrationTests.Infrastructure.Builders;

namespace EPR.Payment.Service.IntegrationTests.Infrastructure;

[Collection(PaymentServiceCollection.Name)]
[Trait("Category", "IntegrationTest")]
public abstract class IntegrationTestBase
{
    protected PaymentServiceFactory Factory { get; }
    protected HttpClient Client { get; }

    /// <summary>
    /// Fluent test-data entrypoint. See <see cref="TestBuilders"/> for the available builders —
    /// <c>Builder.Producer().Build()</c>, <c>Builder.Regulator().InNation(x).Build()</c>,
    /// <c>Builder.SchemeOperator().WithAdmin().Build()</c>, etc.
    /// </summary>
    protected TestBuilders Builder { get; }

    protected IntegrationTestBase(PaymentServiceFactory factory)
    {
        Factory = factory;
        Client = factory.CreateClient();
        Builder = new TestBuilders(factory);
    }
}
