﻿namespace EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme
{
    public class ComplianceSchemeFeesRequestDto
    {
        public required string Regulator { get; set; } // "GB-ENG", "GB-SCT", etc.
        public required string ApplicationReferenceNumber { get; set; }
        public DateTime SubmissionDate { get; set; }
        public List<ComplianceSchemeMemberDto> ComplianceSchemeMembers { get; set; } = new();
    }

    public class ComplianceSchemeMemberDto
    {
        public required string MemberId { get; set; }
        public required string MemberType { get; set; } //"Large" or "Small"
        public bool IsOnlineMarketplace { get; set; }
        public bool IsLateFeeApplicable { get; set; }
        public int NumberOfSubsidiaries { get; set; }
        public int NoOfSubsidiariesOnlineMarketplace { get; set; }
    }
}
