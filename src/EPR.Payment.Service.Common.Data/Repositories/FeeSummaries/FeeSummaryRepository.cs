using EPR.Payment.Service.Common.Data.DataModels;
using EPR.Payment.Service.Common.Data.Interfaces;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.FeeSummaries;
using Microsoft.EntityFrameworkCore;

namespace EPR.Payment.Service.Common.Data.Repositories.FeeSummaries
{
    public class FeeSummaryRepository : IFeeSummaryRepository
    {
        private readonly IAppDbContext _dbContext;

        public FeeSummaryRepository(IAppDbContext dbContext)
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
            IEnumerable<FeeSummary> items,
            CancellationToken cancellationToken)
        {
            foreach (var item in items)
            {
                var existing = await _dbContext.FeeSummaries
                    .FirstOrDefaultAsync(s =>
                        s.AppRefNo == appRefNo &&
                        s.InvoicePeriod == invoicePeriod &&
                        s.FeeTypeId == item.FeeTypeId &&
                        s.PayerTypeId == payerTypeId &&
                        s.PayerId == payerId);

                if (existing is null)
                {
                    item.ExternalId = externalId;
                    item.AppRefNo = appRefNo;
                    item.InvoiceDate = invoiceDate;
                    item.InvoicePeriod = invoicePeriod;
                    item.PayerTypeId = payerTypeId; 
                    item.PayerId = payerId;
                    item.CreatedDate = DateTimeOffset.UtcNow;
                    item.FileId = fileId;
                    await _dbContext.FeeSummaries.AddAsync(item);
 
                }
                else if ((existing is not null) && (existing.FileId == fileId) && (existing.InvoicePeriod == invoicePeriod))
                {
                    existing.UnitPrice = item.UnitPrice;
                    existing.Quantity = item.Quantity;
                    existing.Amount = item.Amount;
                    existing.UpdatedDate = DateTimeOffset.UtcNow;                 
                }
                else if ((existing is not null) && (existing.FileId != fileId) && (existing.InvoicePeriod != invoicePeriod))
                {
                    item.ExternalId = externalId;
                    item.AppRefNo = appRefNo;
                    item.InvoiceDate = invoiceDate;
                    item.InvoicePeriod = invoicePeriod;
                    item.PayerTypeId = payerTypeId;
                    item.PayerId = payerId;
                    item.CreatedDate = DateTimeOffset.UtcNow;
                    item.FileId = fileId;
                    await _dbContext.FeeSummaries.AddAsync(item);
                }
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
