using EPR.Payment.Service.Common.Dtos.Request;

namespace EPR.Payment.Service.Services.Interfaces
{
    public interface IPaymentsService
    {
        Task<Guid> InsertPaymentStatusAsync(PaymentStatusInsertRequestDto paymentStatusInsertRequest, CancellationToken cancellationToken);
        Task UpdatePaymentStatusAsync(Guid externalPaymentId, PaymentStatusUpdateRequestDto paymentStatusUpdateRequest, CancellationToken cancellationToken);
        Task<int> GetPaymentStatusCountAsync(CancellationToken cancellationToken);
    }
}
