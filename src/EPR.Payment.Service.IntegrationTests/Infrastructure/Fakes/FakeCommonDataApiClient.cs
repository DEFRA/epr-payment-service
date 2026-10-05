using System.Runtime.CompilerServices;
using EPR.Payment.Service.Services.Migration.RegistrationHistory;

namespace EPR.Payment.Service.IntegrationTests.Infrastructure.Fakes;

/// <summary>Serves a fixed registration history dataset in place of the epr-common-data-api migration endpoint.</summary>
public sealed class FakeCommonDataApiClient(IReadOnlyList<RegistrationHistoryRecord> records) : ICommonDataApiClient
{
    public int CallCount { get; private set; }

    public async IAsyncEnumerable<RegistrationHistoryRecord> GetRegistrationHistoryAsync([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        CallCount++;
        foreach (var record in records)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await Task.Yield();
            yield return record;
        }
    }
}
