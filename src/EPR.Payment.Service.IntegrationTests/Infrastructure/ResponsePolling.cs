namespace EPR.Payment.Service.IntegrationTests.Infrastructure;

/// <summary>
/// Bounded polling for state produced asynchronously by a Service Bus message consumer -
/// publishing a message and then immediately reading is a race, since consumption happens on the
/// app's own background processor rather than inline with the publish call.
/// </summary>
public static class ResponsePolling
{
    /// <summary>
    /// Repeatedly invokes <paramref name="action"/> until <paramref name="isReady"/> is satisfied
    /// or <paramref name="timeout"/> elapses (default 15s, generous for a locally-running
    /// Testcontainers Service Bus emulator). Throws <see cref="TimeoutException"/> - including the
    /// last observed value - if the condition is never met, so a genuine regression fails loudly
    /// rather than the test silently asserting against a stale/default value.
    /// </summary>
    public static async Task<T> WaitUntilAsync<T>(
        Func<Task<T>> action,
        Func<T, bool> isReady,
        TimeSpan? timeout = null,
        TimeSpan? interval = null)
    {
        var effectiveTimeout = timeout ?? TimeSpan.FromSeconds(15);
        var effectiveInterval = interval ?? TimeSpan.FromMilliseconds(250);

        using var cts = new CancellationTokenSource(effectiveTimeout);
        var last = await action();

        while (!isReady(last))
        {
            if (cts.IsCancellationRequested)
            {
                throw new TimeoutException(
                    $"Condition not met within {effectiveTimeout}. Last observed value: {last}");
            }

            try
            {
                await Task.Delay(effectiveInterval, cts.Token);
            }
            catch (OperationCanceledException)
            {
                throw new TimeoutException(
                    $"Condition not met within {effectiveTimeout}. Last observed value: {last}");
            }

            last = await action();
        }

        return last;
    }
}
