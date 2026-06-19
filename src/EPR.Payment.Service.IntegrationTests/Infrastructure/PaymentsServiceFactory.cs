using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Testcontainers.MsSql;

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
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        builder.ConfigureHostConfiguration(config =>
        {
            // config here is in the context of the host - the web application - so this
            // builds it from the web application's appsettings file
            config.AddJsonFile("appsettings.json");
            
            // And then add any custom config passed in from the test project
            if (configuration != null)
            {
                config.AddConfiguration(configuration);
            }
        });
        return base.CreateHost(builder);
    }
}
