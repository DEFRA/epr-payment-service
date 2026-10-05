using System.Collections.Concurrent;
using System.Text;
using Azure;
using EPR.Payment.Service.Services.RegistrationSubmission.Storage;

namespace EPR.Payment.Service.IntegrationTests.Infrastructure.Fakes;

/// <summary>In-memory stand-in for the registration-upload-container; unknown blobs throw a 404 like Azure does.</summary>
public sealed class FakeRegistrationBlobReader : IBlobReader
{
    public const string Headers = "organisation_id,subsidiary_id,home_nation_code,organisation_size,packaging_activity_om,joiner_date,closed_loop_registration";

    private readonly ConcurrentDictionary<string, string> _blobs = new(StringComparer.OrdinalIgnoreCase);

    public void Add(string blobName, params string[] csvLines) =>
        _blobs[blobName] = string.Join(Environment.NewLine, new[] { Headers }.Concat(csvLines)) + Environment.NewLine;

    public Task<Stream> DownloadAsync(string blobName, CancellationToken cancellationToken)
    {
        if (!_blobs.TryGetValue(blobName, out var content))
        {
            throw new RequestFailedException(404, $"The specified blob '{blobName}' does not exist.");
        }

        return Task.FromResult<Stream>(new MemoryStream(Encoding.UTF8.GetBytes(content)));
    }
}
