using System.ComponentModel;

namespace EPR.Payment.Service.Common.Enums
{
    public enum PaymentType
    {
        [Description("Offline")]
        Offline = 1,

        [Description("Online")]
        Online = 2,
    }
}