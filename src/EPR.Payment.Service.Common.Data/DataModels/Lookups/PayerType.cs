using EPR.Payment.Service.Common.Data.DataModels.BaseClasses;
using System.Diagnostics.CodeAnalysis;

namespace EPR.Payment.Service.Common.Data.DataModels.Lookups
{
    [ExcludeFromCodeCoverage]
    public class PayerType : BaseEntity
    {
        public string Name { get; set; } = null!;

        #region Navigation Properties

        public virtual ICollection<FeeItem> FeeItems { get; set; } = new List<FeeItem>();

        #endregion Navigation Properties
    }
}