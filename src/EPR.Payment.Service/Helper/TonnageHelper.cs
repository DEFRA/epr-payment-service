using EPR.Payment.Service.Common.Enums;

namespace EPR.Payment.Service.Helper
{
    public static class TonnageHelper
    {
        public static (int TonnesOver, int TonnesUpto) GetTonnageBoundaryByTonnageBand(TonnageBands? tonnageBand) => tonnageBand switch
        {
            TonnageBands.Upto500 => new(0, 500),
            TonnageBands.Over500To5000 => new(500, 5000),
            TonnageBands.Over5000To10000 => new(5000, 10000),
            TonnageBands.Over10000 => new(10000, 99999999),
            _ => throw new ArgumentOutOfRangeException(nameof(tonnageBand), $"Not expected tonnage band value: {tonnageBand}")
        };
    }
}
