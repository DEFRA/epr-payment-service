using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EPR.Payment.Service.Common.Data.DataModels.Lookups
{
    [Table("PaymentStatus", Schema = "Lookup")]
    public class PaymentStatus
    {
        [Key]
        public Enums.Status Id { get; set; }

        [Required]
        [MaxLength(10)]
        public string Status { get; set; } = null!;

        #region Navigation properties
        public virtual ICollection<Payment> Payments { get; set; } = null!;
        #endregion
    }
}
