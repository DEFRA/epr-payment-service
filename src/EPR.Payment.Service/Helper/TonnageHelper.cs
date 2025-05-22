using EPR.Payment.Service.Common.Enums;

namespace EPR.Payment.Service.Helper
{
    public static class TonnageHelper
    {
        public static (int TonnesOver, int TonnesUpto) GetTonnageBoundaryByTonnageBand(TonnageBand tonnageBand) => tonnageBand switch
        {
            TonnageBand.Upto500 => new(0, 500),
            TonnageBand.Over500To5000 => new(500, 5000),
            TonnageBand.Over5000To10000 => new(5000, 10000),
            TonnageBand.Over10000 => new(10000, 99999999),
            _ => throw new ArgumentOutOfRangeException(nameof(tonnageBand), $"Not expected tonnage band value: {tonnageBand}")
        };
    }
}
