using EPR.Payment.Service.Common.Dtos.Request.Payments;
using EPR.Payment.Service.Common.Dtos.Response.Payments;

namespace EPR.Payment.Service.Services.Interfaces.Payments
{
    public interface IOnlinePaymentsService
    {
        Task<Guid> InsertOnlinePaymentStatusAsync(OnlinePaymentStatusInsertRequestDto paymentStatusInsertRequest, CancellationToken cancellationToken);
        Task UpdateOnlinePaymentStatusAsync(Guid externalPaymentId, OnlinePaymentStatusUpdateRequestDto onlinePaymentStatusUpdateRequest, CancellationToken cancellationToken);
        Task<int> GetOnlinePaymentStatusCountAsync(CancellationToken cancellationToken);
        Task<OnlinePaymentResponseDto> GetOnlinePaymentByExternalPaymentIdAsync(Guid externalPaymentId, CancellationToken cancellationToken);
    }
}
