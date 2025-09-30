using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPR.Payment.Service.Common.Dtos.FeeSummaries
{
    public sealed class FeeSummarySaveRequest
    {
        [Required] public Guid FileId { get; init; }
        [Required] public Guid ExternalId { get; init; }

        [Required, MinLength(1)]
        public string ApplicationReferenceNumber { get; init; } = default!;
        public DateTimeOffset? InvoiceDate { get; init; }

        [Required] public DateTimeOffset InvoicePeriod { get; init; }

        [Required] public int PayerTypeId { get; init; }

        [Required] public int PayerId { get; init; }

        [Required] public IReadOnlyCollection<FeeSummaryLineRequest> Lines { get; init; } = Array.Empty<FeeSummaryLineRequest>();

    }
}
