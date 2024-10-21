using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;

namespace EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme
{
    public class ComplianceSchemeMemberWithRegulatorDto
    {
        public required RegulatorType Regulator { get; set; } // "GB-ENG", "GB-SCT", etc.
        public required string MemberType { get; set; } //"Large" or "Small"
        public bool IsOnlineMarketplace { get; set; }
        public bool IsLateFeeApplicable { get; set; }
        public int NumberOfSubsidiaries { get; set; }
        public int NoOfSubsidiariesOnlineMarketplace { get; set; }
    }
}
