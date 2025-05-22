using EPR.Payment.Service.Common.Enums;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using System.Text.Json.Serialization;

namespace EPR.Payment.Service.Common.Dtos.Request.AccreditationFees
{
    public class AccreditationFeesRequestDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public AccreditationFeesRequestorType? RequestorType { get; set; } // "exporter" or "reprocessor", case insensitive, cannot be an empty string        

        public required string Regulator { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TonnageBand? TonnageBand { get; set; }        

        public int NumberOfOverseasSites { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public AccreditationFeesMaterialType? MaterialType { get; set; }

        public required string ApplicationReferenceNumber { get; set; }

        public required DateTime SubmissionDate { get; set; }
    }
}