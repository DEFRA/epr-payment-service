using EPR.Payment.Service.Common.Data.DataModels.BaseClasses;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using System.Diagnostics.CodeAnalysis;

namespace EPR.Payment.Service.Common.Data.DataModels
{
    [ExcludeFromCodeCoverage]
    public class OfflinePayment : BaseEntity
    {
        public int PaymentId { get; set; }

        public DateTime? PaymentDate { get; set; }
        
        public string? Comments { get; set; }

        public int PaymentMethodId { get; set; }

        #region Navigation properties

        public virtual Payment Payment { get; set; } = null!;

        public virtual PaymentMethod PaymentMethod { get; set; } = null!;

        #endregion

    }
}