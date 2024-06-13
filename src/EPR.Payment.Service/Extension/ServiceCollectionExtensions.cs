using EPR.Payment.Service.Services;
using EPR.Payment.Service.Services.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace EPR.Payment.Service.Extension
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            services
                .AddScoped<IPaymentsService, PaymentsService>();

            return services;

        }
    }
}
