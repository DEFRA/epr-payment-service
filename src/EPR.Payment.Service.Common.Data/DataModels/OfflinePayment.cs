using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Data.DataModels.BaseClasses;

namespace EPR.Payment.Service.Common.Data.DataModels
{
    [ExcludeFromCodeCoverage]
    public class OfflinePayment : BaseEntity
    {
        public int PaymentId { get; set; }

        public DateTime? PaymentDate { get; set; }
        
        public string? Comments { get; set; }

        #region Navigation properties

        public virtual Payment Payment { get; set; } = null!;

        #endregion

    }
}