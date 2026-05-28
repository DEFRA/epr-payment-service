using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EPR.Payment.Service.Messaging;

public class WorkerServiceBus : IHostedService, IDisposable
{
    private readonly ILogger<WorkerServiceBus> _logger;
    private readonly IServiceBusTopicSubscription _serviceBusTopicSubscription;

    public WorkerServiceBus(
        IServiceBusTopicSubscription serviceBusTopicSubscription,
        ILogger<WorkerServiceBus> logger)
    {
        _serviceBusTopicSubscription = serviceBusTopicSubscription;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting service bus subscription");
        await _serviceBusTopicSubscription.PrepareServiceBusSubscriptionAsync();
        _logger.LogInformation("Service bus subscription started");
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping service bus subscription");
        await _serviceBusTopicSubscription.CloseSubscriptionAsync();
        _logger.LogInformation("Service bus subscription stopped");
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual async void Dispose(bool disposing)
    {
        if (disposing)
        {
            await _serviceBusTopicSubscription.DisposeAsync();
        }
    }
}
