using System.ComponentModel;

namespace EPR.Payment.Service.Common.Enums
{
    public enum Status
    {
        [Description("Initiated")]
        Initiated = 0,

        [Description("In-Progress")]
        InProgress = 1,

        [Description("Success")]
        Success = 2,

        [Description("Failed")]
        Failed = 3,

        [Description("Error")]
        Error = 4,

        [Description("User Cancelled")]
        UserCancelled = 5
    }
}