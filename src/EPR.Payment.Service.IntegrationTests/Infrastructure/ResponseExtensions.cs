using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EPR.Payment.Service.IntegrationTests.Infrastructure;

public static class ResponseExtensions
{
    /// <summary>
    /// Deserialisation options matching the API's wire format: camelCase property names and
    /// string enums (the API serialises enums by name, not by integer).
    /// </summary>
    public static readonly JsonSerializerOptions JsonOpts = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() },
    };

    /// <summary>
    /// Deserialises the response body as <typeparamref name="T"/> using the API's wire format.
    /// Throws if the body is null/empty — that's a precondition for the caller's assertions,
    /// not an assertion itself, so failures don't masquerade as test failures inside this helper.
    /// </summary>
    public static async Task<T> ReadJson<T>(this HttpResponseMessage response)
    {
        var payload = await response.Content.ReadFromJsonAsync<T>(JsonOpts);
        return payload ?? throw new InvalidOperationException(
            $"Response body deserialised to null as {typeof(T).Name}. Status: {(int)response.StatusCode} {response.StatusCode}.");
    }
}
