using EPR.Payment.Service.Common.Data.DataModels.BaseClasses;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPR.Payment.Service.Common.Data.DataModels.Lookups
{
    [Table("SubsidiariesRegistrationFees", Schema = "Lookup")]
    [ExcludeFromCodeCoverage]
    public class SubsidiariesRegistrationFees : BaseEntity
    {
        [MaxLength(255)]
        public int MinNumberOfSubsidiaries { get; set; }

        [MaxLength(255)]
        public int MaxNumberOfSubsidiaries { get; set; }

        [MaxLength(255)]
        public string Description { get; set; } = null!;

        [MaxLength(255)]
        public string Regulator { get; set; } = null!;

        [Column(TypeName = "decimal(19,4)")]
        public decimal Amount { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime EffectiveTo { get; set; }
    }
}
