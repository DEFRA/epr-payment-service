using EPR.Payment.Service.Common.Data.Interfaces.Repositories.FeeItems;
using EPR.Payment.Service.Common.Dtos.FeeItems;
using EPR.Payment.Service.Common.Data.DataModels;
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

        public async Task Save(FeeItemSaveRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            var invoiceDate = request.InvoiceDate ?? DateTimeOffset.UtcNow;

            var mappedRequest = new Common.Data.Dtos.FeeItemMappedRequest
            {
                ExternalId = request.ExternalId,
                AppRefNo = request.ApplicationReferenceNumber,
                InvoiceDate = invoiceDate,
                InvoicePeriod = request.InvoicePeriod,
                PayerTypeId = request.PayerTypeId,
                PayerId = request.PayerId,
                FileId = request.FileId,
                Items = request.Lines.Select(l => new FeeItem
                {
                    FeeTypeId = l.FeeTypeId,
                    UnitPrice = l.UnitPrice,
                    Quantity = l.Quantity ?? 1,
                    Amount = l.Amount
                }).ToList()
            };

            await _repository.UpsertAsync(mappedRequest, cancellationToken);
        }
    }
}