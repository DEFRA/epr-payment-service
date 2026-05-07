namespace EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer
{
    public class ProducerRegistrationFeesRequestDto
    {
        public required string ProducerType { get; set; } // "large" or "small", case insensitive, cannot be an empty string

        public int NumberOfSubsidiaries { get; set; } // Any integer >= 0

        public required string Regulator { get; set; } // "GB-ENG", "GB-SCT", etc.

        public int NoOfSubsidiariesOnlineMarketplace { get; set; } // Any integer >= 0

        public int NoOfSubsidiariesClosedLoopRecycling { get; set; } // Any integer >= 0; only valid when ProducerType is "LARGE"

        public bool IsProducerOnlineMarketplace { get; set; } // True or False

        public bool IsClosedLoopRecycling { get; set; } // True or False; only valid when ProducerType is "LARGE"

        public bool IsLateFeeApplicable { get; set; } // True or False

        public required string ApplicationReferenceNumber { get; set; }

        public DateTime SubmissionDate { get; set; }
    }
}