using EPR.Payment.Service.Common.Data.DataModels.BaseClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPR.Payment.Service.Common.Data.DataModels.Lookups
{
    [Table("Regulator", Schema = "Lookup")]
    [ExcludeFromCodeCoverage]
    public class Regulator : BaseEntity
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
