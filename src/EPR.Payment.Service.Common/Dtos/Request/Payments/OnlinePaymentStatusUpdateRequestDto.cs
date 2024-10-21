namespace EPR.Payment.Service.Common.Dtos.Request.Payments
{
    public class OnlinePaymentStatusUpdateRequestDto
    {
        public string? GovPayPaymentId { get; set; }

        public Guid? UpdatedByUserId { get; set; }

        public Guid? UpdatedByOrganisationId { get; set; }

        public string? Reference { get; set; }

        public Enums.Status Status { get; set; }

        public string? ErrorCode { get; set; }

        public string? ErrorMessage { get; set; }
    }
}
