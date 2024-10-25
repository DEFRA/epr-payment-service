using EPR.Payment.Service.Common.Dtos.Request.Payments;

namespace EPR.Payment.Service.Services.Interfaces.Payments
{
    public interface IOfflinePaymentsService
    {
        Task InsertOfflinePaymentAsync(OfflinePaymentStatusInsertRequestDto paymentInsertRequest, CancellationToken cancellationToken);
    }
}
