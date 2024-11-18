using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPR.Payment.Service.Common.Data.DataModels
{
    public class OfflinePayment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 1)]
        public int Id { get; set; }

        [ForeignKey("Payment")]
        [Column(Order = 2)]
        public int PaymentId { get; set; }

        [Column(Order = 3)]
        public DateTime? PaymentDate { get; set; }

        [MaxLength(255)]
        [Column(Order = 4)]
        public string? Comments { get; set; }

        #region Navigation properties

        public virtual Payment Payment { get; set; } = null!;

        #endregion

    }
}