using System.Text.Json.Serialization;
using EPR.Payment.Service.Common.Enums;

namespace EPR.Payment.Service.Common.Dtos.Request.AccreditationFees
{
    public class AccreditationFeesRequestDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public RequestorTypes? RequestorType { get; set; } 

        public string? Regulator { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TonnageBands? TonnageBand { get; set; }        

        public int NumberOfOverseasSites { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public MaterialTypes? MaterialType { get; set; }

        public string? ApplicationReferenceNumber { get; set; }

        public required DateTime SubmissionDate { get; set; }
    }
}