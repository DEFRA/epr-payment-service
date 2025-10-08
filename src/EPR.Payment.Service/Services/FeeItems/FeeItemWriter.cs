using EPR.Payment.Service.Common.Data.Interfaces.Repositories.FeeItems;
using EPR.Payment.Service.Common.Dtos.FeeItems;
using EPR.Payment.Service.Services.Interfaces.FeeItems;

namespace EPR.Payment.Service.Services.FeeItems
{
    public class FeeItemWriter : IFeeItemWriter
    {
        private readonly IFeeItemRepository _repository;

        public FeeItemWriter(IFeeItemRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task Save(FeeSummarySaveRequest request, CancellationToken cancellationToken)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            var invoiceDate = request.InvoiceDate ?? DateTimeOffset.UtcNow;

            var items = request.Lines.Select(l => new Common.Data.DataModels.FeeItem
            {
                FeeTypeId = l.FeeTypeId,
                UnitPrice = l.UnitPrice,
                Quantity = l.Quantity ?? 1, 
                Amount = l.Amount,

            });

            await _repository.UpsertAsync(
                externalId: request.ExternalId,
                appRefNo: request.ApplicationReferenceNumber,
                invoiceDate: invoiceDate,
                invoicePeriod: request.InvoicePeriod,
                payerTypeId: request.PayerTypeId,
                payerId: request.PayerId,
                fileId: request.FileId,
                items: items,
                cancellationToken: cancellationToken);
        }
    }
}
