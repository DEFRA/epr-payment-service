using System.ComponentModel.DataAnnotations;

namespace EPR.Payment.Service.Common.Dtos.FeeItems
{

    public sealed class FeeItemSaveRequest
    {
        [Required] public Guid FileId { get; init; }
        [Required] public Guid ExternalId { get; init; }

        [Required, MinLength(1)]
        public string ApplicationReferenceNumber { get; init; } = default!;
        public DateTimeOffset? InvoiceDate { get; init; }

        [Required] public DateTimeOffset InvoicePeriod { get; init; }

        [Required] public int PayerTypeId { get; init; }

        [Required] public int PayerId { get; init; }

        [Required] public IReadOnlyCollection<FeeItemLine> Lines { get; init; } = Array.Empty<FeeItemLine>();

    }
}
