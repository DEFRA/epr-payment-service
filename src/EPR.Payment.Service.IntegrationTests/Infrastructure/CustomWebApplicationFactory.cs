using EPR.Payment.Service.Common.Data;
using EPR.Payment.Service.Controllers.AccreditationFees;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EPR.Payment.Service.IntegrationTests.Infrastructure;

public class CustomWebApplicationFactory : WebApplicationFactory<ReprocessorExporterAccreditationFeesController>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["FeatureManagement:EnableRegistrationFeesFeature"] = "true",
                ["FeatureManagement:EnableRegistrationFeesCalculation"] = "true",
                ["FeatureManagement:EnableComplianceSchemeFeature"] = "true",
                ["FeatureManagement:EnableComplianceSchemeFees"] = "true",
                ["FeatureManagement:EnableApplyPendingMigrationsFeature"] = "false"
            });
        });

        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            if (descriptor != null)
                services.Remove(descriptor);

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(ContainerFixture.ConnectionString));
        });
    }
}
