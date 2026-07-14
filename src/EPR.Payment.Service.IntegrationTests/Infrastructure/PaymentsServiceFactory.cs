using Azure.Storage.Blobs;
using EPR.Payment.Service.Common.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EPR.Payment.Service.IntegrationTests.Infrastructure;

/// <summary>
/// Single shared fixture for the whole IntegrationTests assembly: boots one MsSql container,
/// creates one database, migrates it once, and exposes one <see cref="WebApplicationFactory{TEntryPoint}"/>.
/// Test classes share this via <see cref="PaymentServiceCollection"/> and use unique IDs
/// (Guid.NewGuid + per-class label prefixes) to stay isolated from each other.
/// </summary>
public sealed class PaymentServiceFactory(IConfiguration? configuration = null)
    : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        // ConfigureAppConfiguration runs after the app's default sources, so values added here
        // take precedence over appsettings.json. Feature flags are set explicitly in-memory;
        // dynamic values (connection strings, RunMigration) come from ServiceFixture via `configuration`.
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

            if (configuration != null)
            {
                config.AddConfiguration(configuration);
            }
        });

        // ConfigureServices runs after ConfigureAppConfiguration, so ctx.Configuration has the
        // real test values. Program.cs registers both AppDbContext and the Azure Service Bus clients
        // before ConfigureAppConfiguration applies the test overrides, so they end up with the empty
        // connection strings from appsettings.json. We replace them here with the real ones.
        builder.ConfigureServices((ctx, services) =>
        {
            var sqlCs = ctx.Configuration["ConnectionStrings:PaymentConnectionString"];
            var sbCs = ctx.Configuration["ServiceBus:ConnectionString"];
            var sbAdminCs = ctx.Configuration["ServiceBus:AdminConnectionString"];

            // Replace AppDbContext registered with empty connection string
            var dbDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            if (dbDescriptor != null)
                services.Remove(dbDescriptor);

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(sqlCs, o => o.CommandTimeout((int)TimeSpan.FromMinutes(5).TotalSeconds)));

            // Replace Service Bus clients registered with empty connection strings
            services.AddAzureClients(clients =>
            {
                if (!string.IsNullOrEmpty(sbCs))
                    clients.AddServiceBusClient(sbCs);
                if (!string.IsNullOrEmpty(sbAdminCs))
                    clients.AddServiceBusAdministrationClient(sbAdminCs);
            });

            // Replace BlobServiceClient — Program.cs constructs it from StorageAccount:ConnectionString
            // which is empty in test environments. Use the Azurite development endpoint so construction
            // succeeds without needing a real storage account.
            var blobDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(BlobServiceClient));
            if (blobDescriptor != null)
                services.Remove(blobDescriptor);

            services.AddSingleton(_ => new BlobServiceClient("UseDevelopmentStorage=true"));
        });
    }
}
