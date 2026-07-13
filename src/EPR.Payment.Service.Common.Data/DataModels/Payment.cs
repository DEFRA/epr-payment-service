using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Data.DataModels.BaseClasses;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;

namespace EPR.Payment.Service.Common.Data.DataModels
{
    [ExcludeFromCodeCoverage]
    public class Payment : BaseEntity
    {
        public Guid UserId { get; set; }

        public Guid ExternalPaymentId { get; set; }

        public Enums.Status InternalStatusId { get; set; }

        public string Regulator { get; set; } = null!;

        public string Reference { get; set; } = null!;

        public decimal Amount { get; set; }

        public string ReasonForPayment { get; set; } = null!;

        public DateTime CreatedDate { get; set; }

        public Guid UpdatedByUserId { get; set; }

        public DateTime UpdatedDate { get; set; }

        #region Navigation properties

        public virtual PaymentStatus PaymentStatus { get; set; } = null!;
        
        public virtual OnlinePayment OnlinePayment { get; set; } = null!;
        
        public virtual OfflinePayment OfflinePayment { get; set; } = null!;

        #endregion Navigation properties
    }
}