using EPR.Payment.Service.Common.Data;
using EPR.Payment.Service.IntegrationTests.Infrastructure;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Respawn;
using Testcontainers.MsSql;

// SetUpFixture at the root namespace applies to the entire assembly
[SetUpFixture]
public class ContainerFixture
{
    private static MsSqlContainer _container = null!;

    public static string ConnectionString => _container.GetConnectionString();
    public static CustomWebApplicationFactory Factory { get; private set; } = null!;
    public static Respawner Respawner { get; private set; } = null!;

    [OneTimeSetUp]
    public async Task InitializeAsync()
    {
        _container = new MsSqlBuilder().Build();
        await _container.StartAsync();

        // Apply all migrations once — creates schema and seeds all lookup data
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(ConnectionString)
            .Options;

        await using var context = new AppDbContext(options);
        await context.Database.MigrateAsync();

        // Single factory instance shared across all test classes
        Factory = new CustomWebApplicationFactory();

        // Respawner excludes lookup data seeded by migrations
        await using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();
        Respawner = await Respawner.CreateAsync(connection, new RespawnerOptions
        {
            SchemasToExclude = new[] { "Lookup" },
            DbAdapter = DbAdapter.SqlServer
        });
    }

    [OneTimeTearDown]
    public async Task CleanupAsync()
    {
        await Factory.DisposeAsync();
        await _container.DisposeAsync();
    }
}
