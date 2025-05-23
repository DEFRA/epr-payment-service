using EPR.Payment.Service.Common.Dtos.Request.Payments;

namespace EPR.Payment.Service.Services.Interfaces.Payments
{
    public interface IOfflinePaymentsService
    {
        Task InsertOfflinePaymentAsync(
            OfflinePaymentInsertRequestDto paymentInsertRequest,
            CancellationToken cancellationToken);

        Task InsertOfflinePaymentAsync(
            OfflinePaymentInsertRequestV2Dto paymentInsertRequest,
            CancellationToken cancellationToken);
    }
}
