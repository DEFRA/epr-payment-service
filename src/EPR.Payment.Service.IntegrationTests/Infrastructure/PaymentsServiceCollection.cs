namespace EPR.Payment.Service.IntegrationTests.Infrastructure;

/// <summary>
/// xUnit collection that owns the single <see cref="ServiceFixture"/> shared across every
/// test class in this assembly. Defined here (no test methods, no class body) so test classes
/// pick it up via <see cref="IntegrationTestBase"/>'s <c>[Collection]</c> attribute.
///
/// Side effect: classes in this collection run serially. That's deliberate — running them in
/// parallel against one shared DB would invite ordering surprises that aren't worth chasing
/// down. Tests are sub-second once the fixture is up, so serial is fine.
/// </summary>
[CollectionDefinition(Name)]
public sealed class PaymentServiceCollection : ICollectionFixture<ServiceFixture>
{
    public const string Name = "ServiceFixture";
}
