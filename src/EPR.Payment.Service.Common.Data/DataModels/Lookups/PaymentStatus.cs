using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;


namespace EPR.Payment.Service.Common.Data.DataModels.Lookups
{
    [Table("PaymentStatus", Schema = "Lookup")]
    [ExcludeFromCodeCoverage]
    public class PaymentStatus
    {
        [Key]
        public Enums.Status Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = null!;

        #region Navigation properties
        public virtual ICollection<Payment> Payments { get; set; } = null!;
        #endregion
    }
}
