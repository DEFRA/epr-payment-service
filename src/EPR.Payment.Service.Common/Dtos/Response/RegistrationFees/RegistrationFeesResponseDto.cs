namespace EPR.Payment.Service.Common.Dtos.Response.RegistrationFees
{
    public class RegistrationFeesResponseDto
    {
        public decimal BaseFee { get; set; } = 0; // Default to 0 if not applicable
        public decimal SubsidiariesFee { get; set; } = 0; // Default to 0 if not applicable
        public decimal TotalFee { get; set; } // Total fee will be computed
        public List<FeeBreakdown> FeeBreakdowns { get; set; } = new();
    }

    public class FeeBreakdown
    {
        public required string Description { get; set; } = string.Empty; // Description of the fee component
        public decimal Amount { get; set; } // Fee amount, should be valid and non-negative
    }
}
