using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Logging;

namespace EPR.Payment.Service.Services.Migration.RegistrationHistory
{
    /// <summary>Runs the registration history migration once in the background at startup, without delaying the API.</summary>
    [ExcludeFromCodeCoverage(Justification = "Thin hosting shim; the runner it invokes is unit tested.")]
    public class RegistrationHistoryMigrationHostedService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<RegistrationHistoryMigrationHostedService> _logger;

        public RegistrationHistoryMigrationHostedService(
            IServiceScopeFactory scopeFactory,
            ILogger<RegistrationHistoryMigrationHostedService> logger)
        {
            _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Yield();

            using var logScope = _logger.AddScopedData(new Dictionary<string, object>
            {
                ["MigrationName"] = "RegistrationHistory",
                ["MigrationInstanceId"] = Environment.MachineName,
            });

            try
            {
                await using var scope = _scopeFactory.CreateAsyncScope();
                await scope.ServiceProvider.GetRequiredService<IRegistrationHistoryMigrationRunner>().RunAsync(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                // Shutdown; an unfinished run stays Running and resumes on next startup.
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Registration history migration terminated unexpectedly.");
            }
        }
    }
}
