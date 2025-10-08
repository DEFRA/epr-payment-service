using EPR.Payment.Service.Common.Data;
using EPR.Payment.Service.Common.Data.Helper;
using EPR.Payment.Service.Common.Data.Interfaces;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.Fees;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.Payments;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.Data.Repositories.Fees;
using EPR.Payment.Service.Common.Data.Repositories.Payments;
using EPR.Payment.Service.Common.Data.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Common.Dtos.Request.ResubmissionFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Request.ResubmissionFees.Producer;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees;
using EPR.Payment.Service.Services.AccreditationFees;
using EPR.Payment.Service.Services.FeeSummaries;
using EPR.Payment.Service.Services.Interfaces.AccreditationFees;
using EPR.Payment.Service.Services.Interfaces.FeeSummaries;
using EPR.Payment.Service.Services.Interfaces.Payments;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.Producer;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.ReprocessorOrExporter;
using EPR.Payment.Service.Services.Interfaces.ResubmissionFees.ComplianceScheme;
using EPR.Payment.Service.Services.Interfaces.ResubmissionFees.Producer;
using EPR.Payment.Service.Services.Payments;
using EPR.Payment.Service.Services.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Services.RegistrationFees.Producer;
using EPR.Payment.Service.Services.RegistrationFees.ReprocessorOrExporter;
using EPR.Payment.Service.Services.ResubmissionFees.ComplianceScheme;
using EPR.Payment.Service.Services.ResubmissionFees.Producer;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees.Producer;
using EPR.Payment.Service.Strategies.Interfaces.ResubmissionFees.ComplianceScheme;
using EPR.Payment.Service.Strategies.Interfaces.ResubmissionFees.Producer;
using EPR.Payment.Service.Strategies.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Strategies.RegistrationFees.Producer;
using EPR.Payment.Service.Strategies.ResubmissionFees.ComplianceScheme;
using EPR.Payment.Service.Strategies.ResubmissionFees.Producer;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Strategies.FeeSummary;
using EPR.Payment.Service.Strategies.Interfaces.FeeSummary;

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
            services.AddScoped<IBaseSubsidiariesFeeCalculationStrategy<ProducerRegistrationFeesRequestDto, SubsidiariesFeeBreakdown>, SubsidiariesFeeCalculationStrategy>();
            services.AddScoped<IOnlineMarketCalculationStrategy<ProducerRegistrationFeesRequestDto, decimal>, OnlineMarketCalculationStrategy>();
            services.AddScoped<ILateFeeCalculationStrategy<ProducerRegistrationFeesRequestDto, decimal>, LateFeeCalculationStrategy>();
            services.AddScoped<IResubmissionAmountStrategy<ProducerResubmissionFeeRequestDto, decimal>, ProducerResubmissionAmountStrategy>();

            // Register the specific implementations of IFeeCalculationStrategy for Compliance Scheme
            services.AddScoped<ICSBaseFeeCalculationStrategy<ComplianceSchemeFeesRequestDto, decimal>, CSBaseFeeCalculationStrategy>();
            services.AddScoped<ICSMemberFeeCalculationStrategy<ComplianceSchemeMemberWithRegulatorDto, decimal>, CSMemberFeeCalculationStrategy>();
            services.AddScoped<ICSOnlineMarketCalculationStrategy<ComplianceSchemeMemberWithRegulatorDto, decimal>, CSOnlineMarketCalculationStrategy>();
            services.AddScoped<ICSLateFeeCalculationStrategy<ComplianceSchemeLateFeeRequestDto, decimal>, CSLateFeeCalculationStrategy>();
            services.AddScoped<IBaseSubsidiariesFeeCalculationStrategy<ComplianceSchemeMemberWithRegulatorDto, SubsidiariesFeeBreakdown>, CSSubsidiariesFeeCalculationStrategy>();
            services.AddScoped<IComplianceSchemeResubmissionStrategy<ComplianceSchemeResubmissionFeeRequestDto, decimal>, ComplianceSchemeResubmissionStrategy>();

            // Register repositories
            services.AddScoped<IProducerFeesRepository, ProducerFeesRepository>();
            services.AddScoped<IComplianceSchemeFeesRepository, ComplianceSchemeFeesRepository>();
            services.AddTransient<IOnlinePaymentsRepository, OnlinePaymentsRepository>();
            services.AddTransient<IOfflinePaymentsRepository, OfflinePaymentsRepository>();
            services.AddTransient<IPaymentsRepository, PaymentsRepository>();
            services.AddTransient<IAccreditationFeesRepository, AccreditationFeesRepository>();
            services.AddScoped<IReprocessorOrExporterFeeRepository, ReprocessorOrExporterFeeRepository>();

            // Register the main services
            services.AddScoped<IProducerFeesCalculatorService, ProducerFeesCalculatorService>();
            services.AddScoped<IComplianceSchemeCalculatorService, ComplianceSchemeCalculatorService>();
            services.AddScoped<IProducerResubmissionService, ProducerResubmissionService>();
            services.AddScoped<IComplianceSchemeResubmissionService, ComplianceSchemeResubmissionService>();
            services.AddScoped<IOnlinePaymentsService, OnlinePaymentsService>();
            services.AddScoped<IOfflinePaymentsService, OfflinePaymentsService>();
            services.AddScoped<IPaymentsService, PaymentsService>();
            services.AddScoped<IPreviousPaymentsHelper, PreviousPaymentsHelper>();
            services.AddScoped<IAccreditationFeesCalculatorService, AccreditationFeesCalculatorService>();
            services.AddScoped<IReprocessorOrExporterFeesCalculatorService, ReprocessorOrExporterFeesCalculatorService>();

            services.AddScoped<IFeeItemRepository, FeeItemRepository>();
            services.AddScoped<IFeeItemSaveRequestMapper, FeeItemSaveRequestMapper>();
            services.AddScoped<IFeeItemWriter, FeeItemWriter>();

            services.AddScoped<IFeeSummaryWriter, FeeSummaryWriter>();

            services.AddScoped<FeesKeyValueStore>();

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
