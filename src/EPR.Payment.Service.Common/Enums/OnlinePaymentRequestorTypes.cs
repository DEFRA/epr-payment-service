using System.ComponentModel;

namespace EPR.Payment.Service.Common.Enums
{
    public enum OnlinePaymentRequestorTypes
    {
        [Description("Producers")]
        Producers = 2,
        
        [Description("Compliance Schemes")]
        ComplianceSchemes = 3,

        [Description("Exporters")]
        Exporters = 4,

        [Description("Reprocessors")]
        Reprocessors = 5,
    }
}
