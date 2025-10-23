using EPR.Payment.Service.Common.Data.DataModels;

namespace EPR.Payment.Service.Common.Data.Dtos
{
    public class FeeItemMappedRequest
    {
        public Guid ExternalId { get; set; }
        public string AppRefNo { get; set; } = string.Empty;
        public DateTimeOffset InvoiceDate { get; set; }
        public DateTimeOffset InvoicePeriod { get; set; }
        public int PayerTypeId { get; set; }
        public int PayerId { get; set; }
        public Guid FileId { get; set; }
        public IEnumerable<FeeItem> Items { get; set; } = Array.Empty<FeeItem>();
    }
}
