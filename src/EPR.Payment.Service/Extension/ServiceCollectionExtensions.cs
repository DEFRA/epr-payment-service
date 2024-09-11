using EPR.Payment.Service.Common.Data;
using EPR.Payment.Service.Common.Data.Interfaces;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.Payments;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.Data.Repositories.Payments;
using EPR.Payment.Service.Common.Data.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees;
using EPR.Payment.Service.Services.Interfaces.Payments;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.Producer;
using EPR.Payment.Service.Services.Payments;
using EPR.Payment.Service.Services.RegistrationFees.Producer;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees.Producer;
using EPR.Payment.Service.Strategies.RegistrationFees.Producer;
using EPR.Payment.Service.Utilities.RegistrationFees.Interfaces;
using EPR.Payment.Service.Utilities.RegistrationFees.Producer;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace EPR.Payment.Service.Extension
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Register the specific implementations of IFeeCalculationStrategy
            services.AddScoped<IBaseFeeCalculationStrategy<ProducerRegistrationFeesRequestDto>, BaseFeeCalculationStrategy>();
            services.AddScoped<ISubsidiariesFeeCalculationStrategy<ProducerRegistrationFeesRequestDto>, SubsidiariesFeeCalculationStrategy>();
            services.AddScoped<IOnlineMarketCalculationStrategy<ProducerRegistrationFeesRequestDto>, OnlineMarketCalculationStrategy>();
            services.AddScoped<IResubmissionAmountStrategy, DefaultResubmissionAmountStrategy>();


            // Register the fee breakdown generator
            services.AddScoped<IFeeBreakdownGenerator<ProducerRegistrationFeesRequestDto, RegistrationFeesResponseDto>, FeeBreakdownGenerator>();

            // Register repositories
            services.AddScoped<IProducerFeesRepository, ProducerFeesRepository>();
            services.AddTransient<IPaymentsRepository, PaymentsRepository>();

            // Register the main services
            services.AddScoped<IProducerFeesCalculatorService, ProducerFeesCalculatorService>();
            services.AddScoped<IProducerResubmissionService, ProducerResubmissionService>();
            services.AddScoped<IPaymentsService, PaymentsService>();

            return services;
        }
        public static IServiceCollection AddDataContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString, o => o.CommandTimeout((int)TimeSpan.FromMinutes(5).TotalSeconds))

            );

            services.AddTransient<IAppDbContext, AppDbContext>(provider => provider.GetService<AppDbContext>()!);
            return services;
        }
    }
}
