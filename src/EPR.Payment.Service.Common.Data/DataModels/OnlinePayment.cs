using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Data.DataModels.BaseClasses;

namespace EPR.Payment.Service.Common.Data.DataModels
{
    [ExcludeFromCodeCoverage]
    public class OnlinePayment : BaseEntity
    {
        public int PaymentId { get; set; }

        public Guid OrganisationId { get; set; }

        public string? GovPayPaymentId{ get; set; }

        public string? GovPayStatus { get; set; }

        public string? ErrorCode { get; set; }

        public string? ErrorMessage { get; set; }

        public Guid UpdatedByOrgId { get; set; }

        public string? RequestorType { get; set; }

        #region Navigation properties

        public virtual Payment Payment { get; set; } = null!;

        #endregion
    }
}