using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Data.DataModels.BaseClasses;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;

namespace EPR.Payment.Service.Common.Data.DataModels
{
    [ExcludeFromCodeCoverage]
    public class FeeSummary : BaseEntity
    {
        public Guid ExternalId { get; set; }

        public string AppRefNo { get; set; } = null!;

        public DateTimeOffset InvoiceDate { get; set; }

        public DateTimeOffset InvoicePeriod { get; set; }

        public int PayerTypeId { get; set; }

        public int PayerId { get; set; }

        public int FeeTypeId { get; set; }

        public decimal? UnitPrice { get; set; }

        public int Quantity { get; set; }

        public decimal Amount { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public DateTimeOffset? UpdatedDate { get; set; }

        #region Navigation Properties

        public virtual FeeType FeeType { get; set; } = null!;

        public virtual PayerType PayerType { get; set; } = null!;

        public virtual ICollection<FileFeeSummaryConnection> FileFeeSummaryConnections { get; set; } = new List<FileFeeSummaryConnection>();

        #endregion Navigation Properties
    }
}
