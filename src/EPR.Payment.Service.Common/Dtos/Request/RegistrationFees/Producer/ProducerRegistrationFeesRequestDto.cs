﻿namespace EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer
{
    public class ProducerRegistrationFeesRequestDto
    {
        public string ProducerType { get; set; } = string.Empty; // "large" or "small", case insensitive, empty indicates no base fee

        public int NumberOfSubsidiaries { get; set; } // Any integer >= 0

        public required string Regulator { get; set; } // "GB-ENG", "GB-SCT", etc.

        public bool IsOnlineMarketplace { get; set; } // True or False
    }
}