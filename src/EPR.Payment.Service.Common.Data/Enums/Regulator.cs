using System.ComponentModel;

namespace EPR.Payment.Service.Common.Data.Enums
{
    public enum Regulator
    {
        [Description("England")]
        England = 1,

        [Description("Scotland")]
        Scotland = 2,

        [Description("Wales")]
        Wales = 3,

        [Description("Northern Ireland")]
        NorthernIreland = 4,
    }
}