namespace EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.ReprocessorOrExporter
{
    public class PreviousPaymentDetailDto
    {
        public string PaymentMode { get; set; } = default!;

        public string? PaymentMethod { get; set; }

        public decimal PaymentAmount { get; set; }

        public DateTime PaymentDate { get; set; }
    }
}
