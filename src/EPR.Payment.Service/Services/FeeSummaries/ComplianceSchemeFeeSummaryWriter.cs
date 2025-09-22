using EPR.Payment.Service.Common.Data.DataModels;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.FeeSummaries;
using EPR.Payment.Service.Common.Dtos.FeeSummaries;
using EPR.Payment.Service.Services.Interfaces.FeeSummaries;

namespace EPR.Payment.Service.Services.FeeSummaries
{
    public sealed class FeeSummaryWriter : IFeeSummaryWriter
    {
        private readonly IFeeSummaryRepository _repository;
        private readonly ILogger<FeeSummaryWriter> _logger;

        public FeeSummaryWriter(
            IFeeSummaryRepository repository,
            ILogger<FeeSummaryWriter> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Save(FeeSummarySaveRequest request, CancellationToken cancellationToken)
        {

            var invoiceDate = request.InvoiceDate ?? DateTimeOffset.UtcNow;

            var items = request.Lines.Select(l => new FeeSummary
            {
                FeeTypeId = l.FeeTypeId,
                UnitPrice = l.UnitPrice,
                Quantity = l.Quantity ?? 0,
                Amount = l.Amount
            });

            await _repository.UpsertAsync(
                externalId: request.ExternalId,
                appRefNo: request.ApplicationReferenceNumber,
                invoiceDate: invoiceDate,
                invoicePeriod: request.InvoicePeriod,
                payerTypeId: request.PayerTypeId,
                request.PayerId,
                request.FileId,
                items,
                cancellationToken);
        }
    }
}
