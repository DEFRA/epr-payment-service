namespace EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme
{
    public class ComplianceSchemeFeesRequestV2Dto : ComplianceSchemeFeesRequestDto
    {
        public new Guid? FileId { get; set; } //As its required for v2
        public required Guid ExternalId { get; set; }
        public required DateTimeOffset InvoicePeriod { get; set; }
        public required int PayerTypeId { get; set; }
        public required int PayerId { get; set; }
    }
}
