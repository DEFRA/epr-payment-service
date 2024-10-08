﻿using EPR.Payment.Service.Common.Data;
using EPR.Payment.Service.Common.Data.Interfaces;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.Payments;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.Data.Repositories.Payments;
using EPR.Payment.Service.Common.Data.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using EPR.Payment.Service.Services.Interfaces.Payments;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.Producer;
using EPR.Payment.Service.Services.Payments;
using EPR.Payment.Service.Services.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Services.RegistrationFees.Producer;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees.Producer;
using EPR.Payment.Service.Strategies.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Strategies.RegistrationFees.Producer;
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

            // Register the specific implementations of IFeeCalculationStrategy for Producer
            services.AddScoped<IBaseFeeCalculationStrategy<ProducerRegistrationFeesRequestDto, decimal>, BaseFeeCalculationStrategy>();
            services.AddScoped<ISubsidiariesFeeCalculationStrategy<ProducerRegistrationFeesRequestDto, SubsidiariesFeeBreakdown>, SubsidiariesFeeCalculationStrategy >();
            services.AddScoped<IOnlineMarketCalculationStrategy<ProducerRegistrationFeesRequestDto, decimal>, OnlineMarketCalculationStrategy>();
            services.AddScoped<IResubmissionAmountStrategy<RegulatorDto, decimal>, DefaultResubmissionAmountStrategy>();

            // Register the specific implementations of IFeeCalculationStrategy for Compliance Scheme
            services.AddScoped<IComplianceSchemeBaseFeeCalculationStrategy<RegulatorType, decimal>, ComplianceSchemeBaseFeeCalculationStrategy>();

            // Register repositories
            services.AddScoped<IProducerFeesRepository, ProducerFeesRepository>();
            services.AddScoped<IComplianceSchemeFeesRepository, ComplianceSchemeFeesRepository>();
            services.AddTransient<IPaymentsRepository, PaymentsRepository>();

            // Register the main services
            services.AddScoped<IProducerFeesCalculatorService, ProducerFeesCalculatorService>();
            services.AddScoped<IComplianceSchemeBaseFeeService, ComplianceSchemeBaseFeeService>();
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
