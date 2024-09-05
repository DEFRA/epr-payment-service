using EPR.Payment.Service.Common.Data.DataModels.BaseClasses;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace EPR.Payment.Service.Common.Data.DataModels.Lookups
{
    [Table("RegistrationFees", Schema = "Lookup")]
    [ExcludeFromCodeCoverage]
    public class RegistrationFees : BaseEntity
    {
        [ForeignKey("Group")]
        public int GroupId { get; set; }

        [ForeignKey("SubGroup")]
        public int SubGroupId { get; set; }

        [ForeignKey("Regulator")]
        public int RegulatorId { get; set; }

        [Column(TypeName = "decimal(19,4)")]
        public decimal Amount { get; set; }

        private DateTime _effectiveFrom;
        private DateTime _effectiveTo;

        public DateTime EffectiveFrom
        {
            get => _effectiveFrom;
            set => _effectiveFrom = value.Kind == DateTimeKind.Utc ? value : value.ToUniversalTime();
        }

        public DateTime EffectiveTo
        {
            get => _effectiveTo;
            set => _effectiveTo = value.Kind == DateTimeKind.Utc ? value : value.ToUniversalTime();
        }

        #region Navigation properties

        public virtual Group Group { get; set; } = null!;
        public virtual SubGroup SubGroup { get; set; } = null!;
        public virtual Regulator Regulator { get; set; } = null!;

        #endregion Navigation properties
    }
}
