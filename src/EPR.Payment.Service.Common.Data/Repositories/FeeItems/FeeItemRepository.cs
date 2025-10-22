using EPR.Payment.Service.Common.Data.Dtos;
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

        public async Task UpsertAsync(FeeItemMappedRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            ArgumentNullException.ThrowIfNull(request.Items);

            var items = request.Items.ToList();
            if (items.Count == 0)
            {
                return;
            }

            var externalId = request.ExternalId;
            var appRefNo = request.AppRefNo;
            var invoiceDate = request.InvoiceDate;
            var invoicePeriod = request.InvoicePeriod;
            var payerTypeId = request.PayerTypeId;
            var payerId = request.PayerId;
            var fileId = request.FileId;

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
