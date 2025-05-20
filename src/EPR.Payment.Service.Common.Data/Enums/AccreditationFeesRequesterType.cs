using System.ComponentModel;

namespace EPR.Payment.Service.Common.Data.Enums
{
    public enum AccreditationFeesRequesterType
    {
        [Description("Exporter")]
        Exporter = 1,

        [Description("Reprocessor")]
        Reprocessor = 2,
    }
}