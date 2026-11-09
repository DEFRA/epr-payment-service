using System.Diagnostics.CodeAnalysis;

namespace EPR.Payment.Service.Options
{
    [ExcludeFromCodeCoverage]
    public class CommonDataApiOptions
    {
        public const string SectionName = "CommonDataApi";

        public string BaseUrl { get; set; } = string.Empty;

        /// <summary>Must exceed the Synapse query time: the endpoint only sends headers once the query has finished.</summary>
        public TimeSpan Timeout { get; set; } = TimeSpan.FromMinutes(35);
    }
}
