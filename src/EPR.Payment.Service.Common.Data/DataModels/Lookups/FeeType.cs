using EPR.Payment.Service.Common.Data.DataModels.BaseClasses;
using System.Diagnostics.CodeAnalysis;

namespace EPR.Payment.Service.Common.Data.DataModels.Lookups
{
    [ExcludeFromCodeCoverage]
    public class FeeType : BaseEntity
    {
        public string Name { get; set; } = null!;

        #region Navigation Properties

        public virtual ICollection<FeeSummary> FeeSummaries { get; set; } = new List<FeeSummary>();

        #endregion Navigation Properties
    }
}