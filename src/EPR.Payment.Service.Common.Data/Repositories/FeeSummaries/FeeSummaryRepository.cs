using EPR.Payment.Service.Common.Data.DataModels;
using EPR.Payment.Service.Common.Data.Interfaces;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.FeeSummaries;
using Microsoft.EntityFrameworkCore;

namespace EPR.Payment.Service.Common.Data.Repositories.FeeSummaries
{
    public class FeeSummaryRepository : IFeeSummaryRepository
    {
        private readonly IAppDbContext _db;

        public FeeSummaryRepository(IAppDbContext db) => _db = db;

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
                var existing = await _db.FeeSummaries
                    .Include(s => s.FileFeeSummaryConnections)
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

                    await _db.FeeSummaries.AddAsync(item);
                    await _db.FileFeeSummaryConnections.AddAsync(new FileFeeSummaryConnection
                    {
                        FileId = fileId,
                        FeeSummary = item
                    });
                }
                else
                {
                    existing.UnitPrice = item.UnitPrice;
                    existing.Quantity = item.Quantity;
                    existing.Amount = item.Amount;
                    existing.UpdatedDate = DateTimeOffset.UtcNow;

                    var hasLink = existing.FileFeeSummaryConnections.Any(x => x.FileId == fileId);
                    if (!hasLink)
                    {
                        await _db.FileFeeSummaryConnections.AddAsync(new FileFeeSummaryConnection
                        {
                            FileId = fileId,
                            FeeSummaryId = existing.Id
                        });
                    }
                }
            }

            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}
