using DotNet.Testcontainers.Builders;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;
using Testcontainers.ServiceBus;

namespace EPR.Payment.Service.IntegrationTests.Infrastructure;

public class ServiceFixture : IAsyncLifetime, IDisposable
{
    private bool _disposed;

    private MsSqlContainer _sqlContainer = null!;
    private ServiceBusContainer _serviceBusContainer = null!;

    private PaymentServiceFactory? _factory;
    private IConfigurationRoot? _testConfig;
    
    private HttpClient? _httpClient;
    
    public HttpClient CreateHttpClient() => this._factory?.CreateClient() ?? throw new InvalidOperationException("WebApplicationFactory is null");

    public TestLogSink Logs => _factory?.Logs ?? throw new InvalidOperationException("WebApplicationFactory is null");
    
    public async Task InitializeAsync()
    {
        const string sqlPassword = "Password1!";
        const string sqlContainerAlias = "int-tests-sql"; 
        var containerNetwork = new NetworkBuilder().WithName("integration-tests").Build();
        
        _sqlContainer = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2025-latest")
            .WithPassword(sqlPassword)
            .WithNetwork(containerNetwork)
            .WithNetworkAliases(sqlContainerAlias)
            .WithCreateParameterModifier(p => p.HostConfig.Memory = (long)(3.5 * 1024 * 1024 * 1024))
            .Build();
        await _sqlContainer.StartAsync();

        var master = _sqlContainer.GetConnectionString();
        var databaseName = "FeesPayment_" + Guid.NewGuid().ToString("N")[..12];
        await using (var conn = new SqlConnection(master))
        {
            await conn.OpenAsync();
            await using var cmd = conn.CreateCommand();
            cmd.CommandText = $"CREATE DATABASE [{databaseName}]";
            await cmd.ExecuteNonQueryAsync();
        }

        var connectionString = new SqlConnectionStringBuilder(master)
        {
            InitialCatalog = databaseName,
        }.ConnectionString;

        _serviceBusContainer = new ServiceBusBuilder("mcr.microsoft.com/azure-messaging/servicebus-emulator:latest")
            .WithAcceptLicenseAgreement(true)
            .WithMsSqlContainer(containerNetwork, _sqlContainer, sqlContainerAlias, sqlPassword)
            .Build();
        await _serviceBusContainer.StartAsync();

        var serviceBusConnectionString = _serviceBusContainer.GetConnectionString();
        var serviceBusAdminConnectionString = _serviceBusContainer.GetHttpConnectionString();
        
        // builds config from only the test appsettings
        var testConfig = new ConfigurationBuilder()
            // could replace the abstract ConfigurationFilePath with something more complicated if necessary
            .AddJsonFile("appsettings.test.json")
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:PaymentConnectionString"] = connectionString,
                ["ServiceBus:ConnectionString"] = serviceBusConnectionString,
                ["ServiceBus:AdminConnectionString"] = serviceBusAdminConnectionString
            })
            .Build();

        // Initialize the test server
        StartTestServer(testConfig);
    }

    /// <summary>
    /// Scope is disposable. Ensure that the scope gets disposed
    /// </summary>
    public IServiceScope CreateScope()
    {
        if (this._factory is null)
        {
            throw new InvalidOperationException("You must call InitializeAsync before attempting to resolve services.");
        }

        return this._factory.Services.CreateScope();
    }

    /// <summary>
    /// A second host on the same database with services overridden (e.g. fakes for external storage).
    /// The caller must dispose it. Its Service Bus worker is removed so it does not compete with the shared host.
    /// </summary>
    public WebApplicationFactory<Program> CreateHostWithServices(Action<IServiceCollection> configureServices)
    {
        if (this._testConfig is null)
        {
            throw new InvalidOperationException("You must call InitializeAsync before creating a derived host.");
        }

        // Built from the same test configuration (Testcontainers connection strings) as the shared host.
        return new PaymentServiceFactory(this._testConfig, services =>
        {
            foreach (var worker in services.Where(d => d.ServiceType == typeof(IHostedService)
                                                       && d.ImplementationType?.Name is "WorkerServiceBus" or "RegistrationHistoryMigrationHostedService").ToList())
            {
                services.Remove(worker);
            }

            configureServices(services);
        });
    }

    public string TestConnectionString => _testConfig?["ConnectionStrings:PaymentConnectionString"]
        ?? throw new InvalidOperationException("You must call InitializeAsync first.");

    public IServiceProvider SharedServices
    {
        get => _factory?.Services ?? throw new InvalidOperationException("SharedServices not initialized");
    }

    public async Task DisposeAsync()
    {
        await _serviceBusContainer.DisposeAsync();
        await _sqlContainer.DisposeAsync();

        if (_factory != null)
        {
            await _factory.DisposeAsync();
        }
    }

    public virtual void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed && disposing && _factory != null)
        {
            _factory.Dispose();
            this._httpClient?.Dispose();
            this._disposed = true;
        }
    }

    private void StartTestServer(IConfigurationRoot testConfig)
    {
        this._testConfig = testConfig;
        this._factory = new PaymentServiceFactory(testConfig);
        
        // start the host
        this._httpClient = this._factory.CreateClient();
    }
}