using EPR.Payment.Service.Common.Data.DataModels;

namespace EPR.Payment.Service.Common.Data.Interfaces.Repositories.FeeItems
{
    public interface IFeeItemRepository
    {
        Task UpsertAsync(
            Guid externalId,
            string appRefNo,
            DateTimeOffset invoiceDate,
            DateTimeOffset invoicePeriod,
            int payerTypeId,
            int payerId,
            Guid fileId,
            IEnumerable<FeeItem> items,
            CancellationToken cancellationToken);
    }
}
