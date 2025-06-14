﻿using System.ComponentModel;

namespace EPR.Payment.Service.Common.Enums
{
    public enum TonnageBands
    {
        [Description("Tonnage upto 500 tonnes")]
        Upto500 = 1,        
        [Description("Tonnage over 500 to 5000 tonnes")]
        Over500To5000 = 2,
        [Description("Tonnage over 5000 to 10000 tonnes")]
        Over5000To10000 = 3,
        [Description("Tonnage over 10000 tonnes")]
        Over10000 = 4        
    }
}