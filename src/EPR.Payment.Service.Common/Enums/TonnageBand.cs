using System.ComponentModel;

namespace EPR.Payment.Service.Common.Enums
{
    public enum TonnageBand
    {
        [Description("Tonnage upto 500 tonnes")]
        Upto500,        
        [Description("Tonnage over 500 to 5000 tonnes")]
        Over500To5000,
        [Description("Tonnage over 5000 to 10000 tonnes")]
        Over5000To10000,
        [Description("Tonnage over 10000 tonnes")]
        Over10000        
    }
}