using EPR.Payment.Service.Common.Data.DataModels.BaseClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPR.Payment.Service.Common.Data.DataModels.Lookups
{
    public class AdditionalFees : BaseEntity
    {
        [MaxLength(255)]
        public string FeesSubType { get; set; } = null!;

        [MaxLength(255)]
        public string Description { get; set; } = null!;

        [MaxLength(255)]
        public string Country { get; set; } = null!;

        [Column(TypeName = "decimal(19,4)")]
        public decimal Amount { get; set; }

        public DateTime EffectiveFrom { get; set; }
        public DateTime EffectiveTo { get; set; }
    }
}
