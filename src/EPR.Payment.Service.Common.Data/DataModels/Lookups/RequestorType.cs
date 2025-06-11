using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Data.DataModels.BaseClasses;

namespace EPR.Payment.Service.Common.Data.DataModels.Lookups
{
    [ExcludeFromCodeCoverage]
    public class RequestorType : CommonBaseEntity
    {
        #region Navigation properties

        public virtual OnlinePayment OnlinePayment { get; set; } = null!;

        #endregion Navigation properties
    }
}
