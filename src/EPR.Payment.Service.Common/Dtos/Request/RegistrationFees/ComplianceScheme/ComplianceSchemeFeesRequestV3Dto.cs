namespace EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme
{
    public class ComplianceSchemeFeesRequestV3Dto
    {
        public required string Regulator { get; set; } // "GB-ENG", "GB-SCT", etc.
        public required string ApplicationReferenceNumber { get; set; }
        public DateTime SubmissionDate { get; set; }
        public List<ComplianceSchemeMemberDto> ComplianceSchemeMembers { get; set; } = new();
        public required Guid FileId { get; set; }
        public required Guid ExternalId { get; set; }
        public required DateTimeOffset InvoicePeriod { get; set; }
        public required int PayerTypeId { get; set; }
        public required int PayerId { get; set; }
    }
}
