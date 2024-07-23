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
        [Column(Order = 1)]
        public Guid Id { get; set; }

        [Column(Order = 2)]
        public Guid UserId { get; set; }

        [Column(Order = 3)]
        public Guid OrganisationId { get; set; }

        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ExternalPaymentId { get; set; }

        [MaxLength(50)]
        [Column(Order = 5)]
        public string? GovpayPaymentId { get; set; }

        [ForeignKey("PaymentStatus")]
        [Column(Order = 6)]
        public Enums.Status InternalStatusId { get; set; }

        [MaxLength(20)]
        [Column(Order = 7)]
        public string Regulator { get; set; } = null!;

        [MaxLength(20)]
        [Column(Order = 8)]
        public string? GovPayStatus { get; set; }

        [MaxLength(255)]
        [Column(Order = 9)]
        public string? ErrorCode { get; set; }

        [MaxLength(255)]
        [Column(Order = 10)]
        public string? ErrorMessage { get; set; }

        [MaxLength(255)]
        [Column(Order = 11)]
        public string Reference { get; set; } = null!;

        [Column(TypeName = "decimal(19,4)", Order = 12)]
        public decimal Amount { get; set; }

        [MaxLength(255)]
        [Column(Order = 13)]
        public string ReasonForPayment { get; set; } = null!;

        [Column(Order = 14)]
        public DateTime CreatedDate { get; set; }

        [Column(Order = 15)]
        public Guid UpdatedByUserId { get; set; }

        [Column(Order = 16)]
        public Guid UpdatedByOrganisationId { get; set; }

        [Column(Order = 17)]
        public DateTime UpdatedDate { get; set; }

        #region Navigation properties

        public virtual PaymentStatus PaymentStatus { get; set; } = null!;

        #endregion Navigation properties
    }
}