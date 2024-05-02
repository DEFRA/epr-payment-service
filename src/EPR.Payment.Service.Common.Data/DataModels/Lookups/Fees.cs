using EPR.Payment.Service.Common.Data.DataModels.BaseClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPR.Payment.Service.Common.Data.DataModels.Lookups
{
    [Table("Fees", Schema = "Lookup")]
    public class Fees : IdBaseEntity
    {
        [MaxLength(200)]
        public bool Large { get; set; }

        [MaxLength(20)]
        public string Regulator { get; set; } = null!;

        [Column(TypeName = "decimal(19,4)")]
        public decimal Amount { get; set; }

        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
    }
}