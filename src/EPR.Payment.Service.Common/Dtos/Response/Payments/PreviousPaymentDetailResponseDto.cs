namespace EPR.Payment.Service.Common.Dtos.Response.Payments
{
    public class PreviousPaymentDetailResponseDto
    {
        public string PaymentMode { get; set; } = string.Empty;

        public string PaymentMethod { get; set; } = string.Empty;

        public DateTime PaymentDate { get; set; }

        public decimal PaymentAmount { get; set; }
    }
}
