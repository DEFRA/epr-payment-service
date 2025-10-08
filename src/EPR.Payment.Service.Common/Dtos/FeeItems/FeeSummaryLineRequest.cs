using System.ComponentModel.DataAnnotations;

namespace EPR.Payment.Service.Common.Dtos.FeeItems
{
    public sealed class FeeSummaryLineRequest
    {
        [Required] public int FeeTypeId { get; init; }
        public decimal? UnitPrice { get; init; }
        public int? Quantity { get; init; }
        [Required] public decimal Amount { get; init; }
    }
}
