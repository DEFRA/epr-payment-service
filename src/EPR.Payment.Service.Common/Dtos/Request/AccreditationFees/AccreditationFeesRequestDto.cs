namespace EPR.Payment.Service.Common.Dtos.Request.AccreditationFees
{
    public class AccreditationFeesRequestDto
    {
        public required string RequestType { get; set; } // "exporter" or "reprocessor", case insensitive, cannot be an empty string        

        public required string Regulator { get; set; } // "GB-ENG", "GB-SCT", etc.

        public required string TonnageBand { get; set; }        

        public int NumberOfOverseasSites { get; set; }

        public required string MaterialType { get; set; }

        public required string ApplicationReferenceNumber { get; set; }

        public required DateTime SubmissionDate { get; set; }
    }
}