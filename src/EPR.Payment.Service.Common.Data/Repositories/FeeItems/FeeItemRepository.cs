using EPR.Payment.Service.Common.Data.DataModels;
using EPR.Payment.Service.Common.Data.Interfaces;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.FeeItems;
using Microsoft.EntityFrameworkCore;

namespace EPR.Payment.Service.Common.Data.Repositories.FeeItems
{
    public class FeeItemRepository : IFeeItemRepository
    {
        private readonly IAppDbContext _dbContext;

        public FeeItemRepository(IAppDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task UpsertAsync(
            Guid externalId,
            string appRefNo,
            DateTimeOffset invoiceDate,
            DateTimeOffset invoicePeriod,
            int payerTypeId,
            int payerId,
            Guid fileId,
            IEnumerable<FeeItem> items,
            CancellationToken cancellationToken)
        {
            if (items is null) throw new ArgumentNullException(nameof(items));

            foreach (var item in items)
            {
                var existing = await _dbContext.FeeItems
                    .FirstOrDefaultAsync(s =>
                        s.AppRefNo == appRefNo &&
                        s.InvoicePeriod == invoicePeriod &&
                        s.FeeTypeId == item.FeeTypeId &&
                        s.PayerTypeId == payerTypeId &&
                        s.PayerId == payerId &&
                        s.FileId == fileId,
                        cancellationToken);

                if (existing is null)
                {
                    item.ExternalId = externalId;
                    item.AppRefNo = appRefNo;
                    item.InvoiceDate = invoiceDate;
                    item.InvoicePeriod = invoicePeriod;
                    item.PayerTypeId = payerTypeId;
                    item.PayerId = payerId;
                    item.FileId = fileId;
                    item.CreatedDate = DateTimeOffset.UtcNow;
                    item.UpdatedDate = null;

                    await _dbContext.FeeItems.AddAsync(item, cancellationToken);
                }
                else
                {
                    existing.UnitPrice = item.UnitPrice;
                    existing.Quantity = item.Quantity;
                    existing.Amount = item.Amount;
                    existing.UpdatedDate = DateTimeOffset.UtcNow;
                }
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
