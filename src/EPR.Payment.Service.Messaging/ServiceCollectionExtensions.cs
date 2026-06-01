using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace EPR.Payment.Service.Messaging;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMessaging(this IServiceCollection services)
    {
        services.AddSingleton<IServiceBusTopicSubscription, ServiceBusTopicSubscription>();
        services.AddSingleton<IServiceBusTopicPublisher, ServiceBusTopicPublisher>();
        services.AddHostedService<WorkerServiceBus>();

        return services;
    }
}
