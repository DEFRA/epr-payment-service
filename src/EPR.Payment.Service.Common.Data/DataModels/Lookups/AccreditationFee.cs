using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Data.DataModels.BaseClasses;

namespace EPR.Payment.Service.Common.Data.DataModels.Lookups
{
    [ExcludeFromCodeCoverage]
    public class AccreditationFee: BaseEntity
    {
        public int GroupId { get; set; }

        public int SubGroupId { get; set; }

        public int RegulatorId { get; set; }

        public int TonnageBandId { get; set; }

        public decimal Amount { get; set; }

        public decimal FeesPerSite { get; set; }

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
        
        public virtual TonnageBand TonnageBand { get; set; } = null!;

        #endregion Navigation properties
    }
}
