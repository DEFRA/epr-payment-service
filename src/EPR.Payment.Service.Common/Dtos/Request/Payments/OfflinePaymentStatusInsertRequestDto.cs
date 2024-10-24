namespace EPR.Payment.Service.Common.Dtos.Request.Payments
{
    public class OfflinePaymentStatusInsertRequestDto
    {
        public Guid? UserId { get; set; }

        public string? Reference { get; set; }

        public int? Amount { get; set; }

        public string? Description { get; set; }

        public DateTime? PaymentDate { get; set; }

        public string? Comments { get; set; }
    }
}
