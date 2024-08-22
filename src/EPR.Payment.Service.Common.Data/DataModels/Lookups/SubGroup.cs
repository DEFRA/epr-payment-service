using EPR.Payment.Service.Common.Data.DataModels.BaseClasses;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPR.Payment.Service.Common.Data.DataModels.Lookups
{
    [Table("SubGroup", Schema = "Lookup")]
    [ExcludeFromCodeCoverage]
    public class SubGroup : BaseEntity
    {
        [Column(TypeName = "varchar(50)")]
        public string Type { get; set; } = null!;

        [Column(TypeName = "varchar(255)")]
        public string Description { get; set; } = null!;

        #region Navigation properties
        public virtual ICollection<RegistrationFees> RegistrationFees { get; set; } = null!;
        #endregion
    }
}
