using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;

namespace EPR.Payment.Service.Common.Data.DataModels.BaseClasses
{
    [ExcludeFromCodeCoverage]
    public abstract class CommonBaseEntity : BaseEntity
    {
        public string Type { get; set; } = null!;

        public string Description { get; set; } = null!;

        #region Navigation properties
        public virtual ICollection<RegistrationFees> RegistrationFees { get; set; } = null!;
        #endregion
    }
}
