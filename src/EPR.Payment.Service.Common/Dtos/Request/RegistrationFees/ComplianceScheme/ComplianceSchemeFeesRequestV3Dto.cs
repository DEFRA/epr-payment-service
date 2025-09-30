namespace EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme
{
    public class ComplianceSchemeFeesRequestV3Dto : ComplianceSchemeFeesRequestDto
    {
        public required Guid FileId { get; set; }
        public required Guid ExternalId { get; set; }
        public required DateTimeOffset InvoicePeriod { get; set; }
        public required int PayerTypeId { get; set; }
        public required int PayerId { get; set; }
    }
}
