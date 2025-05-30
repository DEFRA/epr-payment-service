namespace EPR.Payment.Service.Common.Dtos.Response.Common
{
    public abstract class BasePreviousPaymentDetailDto
    {
        public string PaymentMode { get; set; } = default!; // "offline" or "online"

        public string? PaymentMethod { get; set; }

        public decimal PaymentAmount { get; set; }

        public DateTime PaymentDate { get; set; }
    }
}
