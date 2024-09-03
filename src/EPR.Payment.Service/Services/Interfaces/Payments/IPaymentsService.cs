using EPR.Payment.Service.Common.Dtos.Request.Payments;
using EPR.Payment.Service.Common.Dtos.Response.Payments;

namespace EPR.Payment.Service.Services.Interfaces.Payments
{
    public interface IPaymentsService
    {
        Task<Guid> InsertPaymentStatusAsync(PaymentStatusInsertRequestDto paymentStatusInsertRequest, CancellationToken cancellationToken);
        Task UpdatePaymentStatusAsync(Guid externalPaymentId, PaymentStatusUpdateRequestDto paymentStatusUpdateRequest, CancellationToken cancellationToken);
        Task<int> GetPaymentStatusCountAsync(CancellationToken cancellationToken);
        Task<PaymentResponseDto> GetPaymentByExternalPaymentIdAsync(Guid externalPaymentId, CancellationToken cancellationToken);
    }
}
