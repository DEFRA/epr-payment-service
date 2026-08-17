using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Enums;

namespace EPR.Payment.Service.Common.Data.DataModels
{
    [ExcludeFromCodeCoverage]
    public class RegistrationFeeLineItem
    {
        public Guid Id { get; set; }

        public Guid RegistrationFeeSnapshotId { get; set; }

        public FeeTypeIds FeeTypeId { get; set; }

        public string FeeTypeName { get; set; } = null!;

        public decimal? UnitPrice { get; set; }

        public int? Quantity { get; set; }

        public decimal Amount { get; set; }

        public string? MemberId { get; set; }

        public int? BandNumber { get; set; }

        public virtual RegistrationFeeSnapshot RegistrationFeeSnapshot { get; set; } = null!;
    }
}
