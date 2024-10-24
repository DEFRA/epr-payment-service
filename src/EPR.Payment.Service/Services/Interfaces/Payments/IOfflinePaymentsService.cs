using EPR.Payment.Service.Common.Dtos.Request.Payments;
using EPR.Payment.Service.Common.Dtos.Response.Payments;

namespace EPR.Payment.Service.Services.Interfaces.Payments
{
    public interface IOfflinePaymentsService
    {
        Task<Guid> InsertOfflinePaymentAsync(OfflinePaymentStatusInsertRequestDto paymentInsertRequest, CancellationToken cancellationToken);
    }
}
