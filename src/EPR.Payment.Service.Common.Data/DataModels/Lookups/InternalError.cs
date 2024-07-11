using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPR.Payment.Service.Common.Data.DataModels.Lookups
{
    [Table("InternalError", Schema = "Lookup")]
    [ExcludeFromCodeCoverage]
    public class InternalError
    {
        [Key]
        [MaxLength(10)]
        public string InternalErrorCode { get; set; } = null!;

        [MaxLength(255)]
        public string? ErrorMessage { get; set; }

        [Required]
        [MaxLength(10)]
        public string GovPayErrorCode { get; set; } = null!;

        [MaxLength(255)]
        public string? GovPayErrorMessage { get; set; }

        #region Navigation properties
        public virtual ICollection<Payment> Payments { get; set; } = null!;
        #endregion
    }
}
