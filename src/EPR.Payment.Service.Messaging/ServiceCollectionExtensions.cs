using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EPR.Payment.Service.Messaging;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["ServiceBus:ConnectionString"];

        if (!string.IsNullOrEmpty(connectionString))
        {
            services.AddAzureClients(clientBuilder =>
            {
                clientBuilder.AddServiceBusClient(connectionString);
                clientBuilder.AddServiceBusAdministrationClient(connectionString);
            });
        }

        services.AddSingleton<IServiceBusTopicSubscription, ServiceBusTopicSubscription>();
        services.AddHostedService<WorkerServiceBus>();

        return services;
    }
}
