using EPR.Payment.Service.Common.Dtos.Request;

namespace EPR.Payment.Service.Services.Interfaces
{
    public interface IPaymentsService
    {
        Task<Guid> InsertPaymentStatusAsync(PaymentStatusInsertRequestDto paymentStatusInsertRequest);
        Task UpdatePaymentStatusAsync(Guid id, PaymentStatusUpdateRequestDto paymentStatusUpdateRequest);
        Task<int> GetPaymentStatusCount();
    }
}
