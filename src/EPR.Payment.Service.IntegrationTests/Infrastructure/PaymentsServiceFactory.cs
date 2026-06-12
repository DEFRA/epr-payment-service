using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Testcontainers.MsSql;

namespace EPR.Payment.Service.IntegrationTests.Infrastructure;

/// <summary>
/// Single shared fixture for the whole IntegrationTests assembly: boots one MsSql container,
/// creates one database, migrates it once, and exposes one <see cref="WebApplicationFactory{TEntryPoint}"/>.
/// Test classes share this via <see cref="PaymentServiceCollection"/> and use unique IDs
/// (Guid.NewGuid + per-class label prefixes) to stay isolated from each other.
/// </summary>
public sealed class PaymentServiceFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private MsSqlContainer _container = null!;
    private string _connectionString = null!;

    public async Task InitializeAsync()
    {
        _container = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2025-latest")
            .WithPassword("Password1!")
            .WithCreateParameterModifier(p => p.HostConfig.Memory = (long)(3.5 * 1024 * 1024 * 1024))
            .Build();
        await _container.StartAsync();

        var master = _container.GetConnectionString();
        var databaseName = "FeesPayment_" + Guid.NewGuid().ToString("N")[..12];
        await using (var conn = new SqlConnection(master))
        {
            await conn.OpenAsync();
            await using var cmd = conn.CreateCommand();
            cmd.CommandText = $"CREATE DATABASE [{databaseName}]";
            await cmd.ExecuteNonQueryAsync();
        }

        _connectionString = new SqlConnectionStringBuilder(master)
        {
            InitialCatalog = databaseName,
        }.ConnectionString;
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await _container.DisposeAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:PaymentConnectionString"] = _connectionString,
                // The first CreateClient() forces Program.Main, which sees RunMigration=true and
                // migrates the empty DB. Subsequent clients reuse the same Host with no further DDL.
                ["RunMigration"] = "true",
            });
        });
    }
}
