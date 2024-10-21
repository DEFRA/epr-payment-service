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
    public abstract class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 1)]
        public int Id { get; set; }

        [Column(Order = 2)]
        public Guid UserId { get; set; }

        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ExternalPaymentId { get; set; }

        [ForeignKey("PaymentStatus")]
        [Column(Order = 6)]
        public Enums.Status InternalStatusId { get; set; }

        [Column(TypeName = "varchar(20)", Order = 7)]
        public string Regulator { get; set; } = null!;

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