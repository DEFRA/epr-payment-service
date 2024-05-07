using EPR.Payment.Service.Common.Data.DataModels.BaseClasses;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPR.Payment.Service.Common.Data.DataModels
{
    [Table("Payment")]
    [ExcludeFromCodeCoverage]
    public class Payment : IdBaseEntity
    {
        public Guid UserId { get; set; }

        public Guid OrganisationId { get; set; }

        public Guid ExternalPaymentId { get; set; }

        [MaxLength(50)]
        public string GovpayPaymentId { get; set; } = null!;

        [ForeignKey("Status")]
        public Enums.Status InternalStatusId { get; set; }

        [MaxLength(200)]
        public string Regulator { get; set; } = null!;

        [MaxLength(20)]
        public string GovPayStatus { get; set; } = null!;

        [MaxLength(255)]
        public string ErrorCode { get; set; } = null!;

        [MaxLength(255)]
        public string ErrorMessage { get; set; } = null!;

        [MaxLength(255)]
        public string ReferenceNumber { get; set; } = null!;

        [Column(TypeName = "decimal(19,4)")]
        public decimal Amount { get; set; }

        [MaxLength(255)]
        public string ReasonForPayment { get; set; } = null!;

        public DateTime CreatedDate { get; set; }

        public Guid UpdatedByUserId { get; set; }

        public Guid UpdatedByOrganisationId { get; set; }

        public DateTime UpdatedDate { get; set; }

        #region Navigation properties

        public virtual PaymentStatus Status { get; set; } = null!;

        #endregion Navigation properties
    }
}