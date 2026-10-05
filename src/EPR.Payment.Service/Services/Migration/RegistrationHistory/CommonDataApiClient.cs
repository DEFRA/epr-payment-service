using System.Runtime.CompilerServices;
using System.Text.Json;

namespace EPR.Payment.Service.Services.Migration.RegistrationHistory
{
    public interface ICommonDataApiClient
    {
        IAsyncEnumerable<RegistrationHistoryRecord> GetRegistrationHistoryAsync(CancellationToken cancellationToken);
    }

    public class CommonDataApiClient : ICommonDataApiClient
    {
        public const string RegistrationHistoryPath = "api/migration/registration-history";

        private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

        private readonly HttpClient _httpClient;

        public CommonDataApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async IAsyncEnumerable<RegistrationHistoryRecord> GetRegistrationHistoryAsync([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            using var response = await _httpClient.GetAsync(RegistrationHistoryPath, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            response.EnsureSuccessStatusCode();

            await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            using var reader = new StreamReader(stream);

            var lineNumber = 0;
            while (await reader.ReadLineAsync(cancellationToken) is { } line)
            {
                lineNumber++;
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                yield return JsonSerializer.Deserialize<RegistrationHistoryRecord>(line, JsonOptions)
                    ?? throw new InvalidDataException($"Registration history line {lineNumber} deserialised to null.");
            }
        }
    }
}
