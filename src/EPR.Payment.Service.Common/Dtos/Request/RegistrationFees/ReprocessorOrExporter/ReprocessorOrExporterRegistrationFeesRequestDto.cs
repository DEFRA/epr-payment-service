using EPR.Payment.Service.Common.Enums;

namespace EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ReprocessorOrExporter
{
    public class ReprocessorOrExporterRegistrationFeesRequestDto
    {
        public required RequestorTypes RequestorType { get; set; } 

        public required string Regulator { get; set; } // "GB-ENG", "GB-SCT", etc.

        public required DateTime SubmissionDate { get; set; }

        public required MaterialTypes MaterialType { get; set; }

        public string? ApplicationReferenceNumber { get; set; } 
    }
}