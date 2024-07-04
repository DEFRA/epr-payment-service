using System.ComponentModel.DataAnnotations;

namespace EPR.Payment.Service.Common.Dtos.Request
{
    public class PaymentStatusInsertRequestDto
    {
        [Required(ErrorMessage = "User ID is required")]
        public Guid? UserId { get; set; }

        [Required(ErrorMessage = "Organisation ID is required")]
        public Guid? OrganisationId { get; set; }

        [Required(ErrorMessage = "Reference is required")]
        public string? Reference { get; set; }

        [Required(ErrorMessage = "Regulator is required")]
        public string? Regulator { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        public int? Amount { get; set; }

        [Required(ErrorMessage = "Reason For Payment is required")]
        public string? ReasonForPayment { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public Enums.Status Status { get; set; } 
    }
}
