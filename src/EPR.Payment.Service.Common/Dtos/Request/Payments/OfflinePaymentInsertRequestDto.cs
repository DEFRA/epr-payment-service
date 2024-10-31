using System.ComponentModel.DataAnnotations;

namespace EPR.Payment.Service.Common.Dtos.Request.Payments
{
    public class OfflinePaymentInsertRequestDto
    {
        public required Guid UserId { get; set; }

        public required string Reference { get; set; }

        public required string Regulator { get; set; }

        public required int Amount { get; set; }

        public required string Description { get; set; }

        public DateTime? PaymentDate { get; set; }

        public string? Comments { get; set; }
    }
}
