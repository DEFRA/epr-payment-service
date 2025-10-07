namespace EPR.Payment.Service.Common.Dtos.Request.ResubmissionFees.ComplianceScheme
{
    public class ComplianceSchemeResubmissionFeeRequestDto
    {
        public required string Regulator { get; set; }
        public DateTime ResubmissionDate { get; set; }
        public required string ReferenceNumber { get; set; }
        public int MemberCount { get; set; }
        public Guid? FileId { get; set; }
        public Guid? ExternalId { get; set; }
        public int? PayerId { get; set; }
    }
}