using System.ComponentModel.DataAnnotations;

namespace EPR.Payment.Service.Common.Dtos.Request
{
    public class PaymentStatusUpdateRequestDto
    {
        [Required(ErrorMessage = "GovPay Payment ID is required")]
        public string? GovPayPaymentId { get; set; }

        [Required(ErrorMessage = "Updated By User ID is required")]
        public string? UpdatedByUserId { get; set; }

        [Required(ErrorMessage = "Updated By Organisation ID is required")]
        public string? UpdatedByOrganisationId { get; set; }

        [Required(ErrorMessage = "Reference Number is required")]
        public string? ReferenceNumber { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public Enums.Status Status { get; set; }

        public string? ErrorCode { get; set; }
    }
}
