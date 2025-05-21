namespace EPR.Payment.Service.Common.Dtos.Response.AccreditationFees
{
    public class AccreditationFeesPreviousPayment
    {
        public string? PaymentMode { get; set; } // "offline" or "online"

        public string? PaymentMethod { get; set; } 
        
        public decimal PaymentAmount { get; set; }
        
        public DateTime PaymentDate { get; set; }
    }
}