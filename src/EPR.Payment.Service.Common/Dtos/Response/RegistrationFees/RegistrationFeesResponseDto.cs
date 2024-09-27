namespace EPR.Payment.Service.Common.Dtos.Response.RegistrationFees
{
    public class RegistrationFeesResponseDto
    {
        public decimal ProducerRegistrationFee { get; set; } = 0; // Default to 0 if not applicable
        public decimal ProducerOnlineMarketPlaceFee { get; set; } = 0; // Default to 0 if not applicable
        public decimal SubsidiariesFee { get; set; } = 0; // Default to 0 if not applicable
        public decimal TotalFee { get; set; } // Total fee will be computed
        public decimal PreviousPayment { get; set; } 
        public decimal OutstandingPayment { get; set; } 
        public required SubsidiariesFeeBreakdown SubsidiariesFeeBreakdown { get; set; }
    }

    public class SubsidiariesFeeBreakdown
    {
        public decimal TotalSubsidiariesOMPFees { get; set; }
        public int CountOfOMPSubsidiaries { get; set; }
        public decimal UnitOMPFees { get; set; }
        public List<FeeBreakdown> FeeBreakdowns { get; set; } = new();
    }

    public class FeeBreakdown
    {
        public int BandNumber { get; set; }
        public int UnitCount { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
