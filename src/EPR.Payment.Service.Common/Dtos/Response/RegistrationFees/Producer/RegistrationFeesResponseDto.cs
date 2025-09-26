using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.ComplianceScheme;

namespace EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.Producer
{
    public class RegistrationFeesResponseDto
    {
        public decimal ProducerRegistrationFee { get; set; } = 0; // Default to 0 if not applicable
        public decimal ProducerOnlineMarketPlaceFee { get; set; } = 0; // Default to 0 if not applicable
        public decimal SubsidiariesFee { get; set; } = 0; // Default to 0 if not applicable
        public decimal ProducerLateRegistrationFee { get; set; } = 0; // Default to 0 if not applicable
        public decimal TotalFee { get; set; } // Total fee will be computed
        public decimal PreviousPayment { get; set; }
        public decimal OutstandingPayment { get; set; }
        public required SubsidiariesFeeBreakdown SubsidiariesFeeBreakdown { get; set; }
        public string MemberId { get; set; }
        public decimal MemberRegistrationFee { get; set; }
        public decimal MemberOnlineMarketPlaceFee { get; set; }
        public decimal MemberLateRegistrationFee { get; set; }
        public decimal TotalMemberFee { get; set; }
    }
}
