using Microsoft.Extensions.DependencyInjection;

namespace EPR.Payment.Service.Messaging;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMessaging(this IServiceCollection services)
    {
        services.AddSingleton<IServiceBusTopicSubscription, ServiceBusTopicSubscription>();
        services.AddHostedService<WorkerServiceBus>();

        return services;
    }
}
