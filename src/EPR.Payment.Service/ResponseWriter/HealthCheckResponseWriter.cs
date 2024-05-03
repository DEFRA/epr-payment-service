using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EPR.Payment.Service.ResponseWriter
{
    public static class HealthCheckResponseWriter
    {
        public static Task WriteJsonResponse(HttpContext httpContext, HealthReport result)
        {
            httpContext.Response.ContentType = "application/json";
            JObject jObject = new JObject(
                new JProperty("status", result.Status.ToString()),
                new JProperty("results",
                new JObject(
                    result.Entries.Select<KeyValuePair<string, HealthReportEntry>,
                    JProperty>((KeyValuePair<string, HealthReportEntry> pair) => new JProperty(
                        pair.Key,
                        new JObject(new JProperty("status", pair.Value.Status.ToString()),
                        new JProperty("description", pair.Value.Description),
                        new JProperty(
                            "data",
                            new JObject(
                                pair.Value.Data.Select<KeyValuePair<string, object>,
                                JProperty>((KeyValuePair<string, object> p) => new JProperty(p.Key, p.Value))))))))));

            return httpContext.Response.WriteAsync(jObject.ToString(Formatting.Indented));
        }
    }
}