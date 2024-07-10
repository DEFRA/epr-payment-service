using EPR.Payment.Service.Common.Data.Interfaces;
using EPR.Payment.Service.Common.Data;
using EPR.Payment.Service.Services;
using EPR.Payment.Service.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories;
using EPR.Payment.Service.Common.Data.Repositories;

namespace EPR.Payment.Service.Extension
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddTransient<IPaymentsRepository, PaymentsRepository>();
            services.AddScoped<IPaymentsService, PaymentsService>();

            return services;

        }
        public static IServiceCollection AddDataContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString, o => o.CommandTimeout((int)TimeSpan.FromMinutes(5).TotalSeconds))

            );

            services.AddTransient<IAppDbContext, AppDbContext>(provider => provider.GetService<AppDbContext>()!);
            AddDependencies(services);
            return services;
        }
    }
}
