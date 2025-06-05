using System.Text.Json.Serialization;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ReprocessorOrExporter;
using EPR.Payment.Service.Common.Enums;

namespace EPR.Payment.Service.Common.Dtos.Request.AccreditationFees
{
    public class ReprocessorOrExporterAccreditationFeesRequestDto : ReprocessorOrExporterRegistrationFeesRequestDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TonnageBands? TonnageBand { get; set; }        

        public int NumberOfOverseasSites { get; set; }
    }
}