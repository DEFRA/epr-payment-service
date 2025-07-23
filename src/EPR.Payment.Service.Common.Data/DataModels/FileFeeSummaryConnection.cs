using EPR.Payment.Service.Common.Data.DataModels.BaseClasses;
using System.Diagnostics.CodeAnalysis;

namespace EPR.Payment.Service.Common.Data.DataModels
{
    [ExcludeFromCodeCoverage]
    public class FileFeeSummaryConnection : BaseEntity
    {
        public Guid FileId { get; set; }

        public int FeeSummaryId { get; set; }

        #region Navigation Properties

        public virtual FeeSummary FeeSummary { get; set; } = null!;

        #endregion Navigation Properties
    }
}