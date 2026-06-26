using EPR.Payment.Service.IntegrationTests.Infrastructure;
using AwesomeAssertions;
using Microsoft.Extensions.Configuration;

namespace EPR.Payment.Service.IntegrationTests.Features;

public class ServiceBusStartupTests(ServiceFixture fixture) : IntegrationTestBase(fixture)
{
    [Fact]
    public async Task GIVEN_service_bus_has_no_topic_WHEN_service_starts_THEN_topic_and_subscription_created()
    {
        // by default, when running in test containers, on system startup the container will contain no topics or subscriptions, 
        // so no setup is required
        var expectedTopicName = Configuration.GetValue<string>("ServiceBus:TopicName");
        var topicExistsResponse = await ServiceBusAdministrationClient.TopicExistsAsync(expectedTopicName);

        var expectedSubscriptionName = Configuration.GetValue<string>("ServiceBus:SubscriptionName");
        var subscriptionExistsResponse = await ServiceBusAdministrationClient.SubscriptionExistsAsync(expectedTopicName, expectedSubscriptionName);

        topicExistsResponse.Should().NotBeNull();
        topicExistsResponse.Value.Should().BeTrue();
        subscriptionExistsResponse.Should().NotBeNull();
        subscriptionExistsResponse.Value.Should().BeTrue();
    }
}
