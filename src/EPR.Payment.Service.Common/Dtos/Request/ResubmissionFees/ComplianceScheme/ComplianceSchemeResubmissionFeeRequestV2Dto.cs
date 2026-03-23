using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;

namespace EPR.Payment.Service.Common.Dtos.Request.ResubmissionFees.ComplianceScheme
{
    public class ComplianceSchemeResubmissionFeeRequestV2Dto : ComplianceSchemeFeesRequestDto
    {
        public new required Guid FileId { get; set; } //Required for V2
        public required Guid ExternalId { get; set; }
        public required DateTimeOffset InvoicePeriod { get; set; }
        public required int PayerTypeId { get; set; }
        public required int PayerId { get; set; }
    }
}