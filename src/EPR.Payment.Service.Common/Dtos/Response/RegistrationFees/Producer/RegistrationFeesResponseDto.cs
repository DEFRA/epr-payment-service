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
        public decimal ProducerOnlyLateFee { get; set; } = 0;
        public decimal SubsidiariesOnlyLateFee { get; set; } = 0;
        public required SubsidiariesFeeBreakdown SubsidiariesFeeBreakdown { get; set; }
    }
}
