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
        public int Id { get; set; }

        [Column(Order = 2)]
        public Guid UserId { get; set; }

        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ExternalPaymentId { get; set; }

        [ForeignKey("PaymentStatus")]
        [Column(Order = 4)]
        public Enums.Status InternalStatusId { get; set; }

        [Column(TypeName = "varchar(20)", Order = 5)]
        public string Regulator { get; set; } = null!;

        [MaxLength(255)]
        [Column(Order = 6)]
        public string Reference { get; set; } = null!;

        [Column(TypeName = "decimal(19,4)", Order = 7)]
        public decimal Amount { get; set; }

        [MaxLength(255)]
        [Column(Order = 8)]
        public string ReasonForPayment { get; set; } = null!;

        [Column(Order = 9)]
        public DateTime CreatedDate { get; set; }

        [Column(Order = 10)]
        public Guid UpdatedByUserId { get; set; }

        [Column(Order = 11)]
        public DateTime UpdatedDate { get; set; }

        #region Navigation properties

        public virtual PaymentStatus PaymentStatus { get; set; } = null!;
        public virtual OnlinePayment OnlinePayment { get; set; } = null!;
        public virtual OfflinePayment OfflinePayment { get; set; } = null!;

        #endregion Navigation properties
    }
}