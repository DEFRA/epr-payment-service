using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPR.Payment.Service.Common.Data.DataModels
{
    [Table("Payment")]
    [ExcludeFromCodeCoverage]
    [Index(nameof(ExternalPaymentId), IsUnique = true)]
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ExternalPaymentId { get; set; }

        public Guid UserId { get; set; }

        public Guid OrganisationId { get; set; }

        [MaxLength(50)]
        public string? GovpayPaymentId { get; set; }

        [ForeignKey("PaymentStatus")]
        public Enums.Status InternalStatusId { get; set; }

        [MaxLength(20)]
        public string Regulator { get; set; } = null!;

        [MaxLength(20)]
        public string? GovPayStatus { get; set; }

        [MaxLength(255)]
        public string? ErrorCode { get; set; }

        [MaxLength(255)]
        public string? ErrorMessage { get; set; }

        [MaxLength(255)]
        public string Reference { get; set; } = null!;

        [Column(TypeName = "decimal(19,4)")]
        public decimal Amount { get; set; }

        [MaxLength(255)]
        public string ReasonForPayment { get; set; } = null!;

        public DateTime CreatedDate { get; set; }

        public Guid UpdatedByUserId { get; set; }

        public Guid UpdatedByOrganisationId { get; set; }

        public DateTime UpdatedDate { get; set; }

        #region Navigation properties

        public virtual PaymentStatus PaymentStatus { get; set; } = null!;

        #endregion Navigation properties
    }
}