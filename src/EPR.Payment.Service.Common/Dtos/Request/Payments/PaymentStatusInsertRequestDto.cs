namespace EPR.Payment.Service.Common.Dtos.Request.Payments
{
    public class PaymentStatusInsertRequestDto
    {
        public Guid? UserId { get; set; }

        public Guid? OrganisationId { get; set; }

        public string? Reference { get; set; }

        public string? Regulator { get; set; }

        public int? Amount { get; set; }

        public string? ReasonForPayment { get; set; }

        public Enums.Status Status { get; set; }
    }
}
