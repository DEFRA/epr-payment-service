using EPR.Payment.Service.Common.Dtos.Request.Payments;
using EPR.Payment.Service.Common.Dtos.Response.Payments;

namespace EPR.Payment.Service.Services.Interfaces.Payments
{
    public interface IOnlinePaymentsService
    {
        Task<Guid> InsertOnlinePaymentAsync(OnlinePaymentInsertRequestDto onlinePaymentInsertRequest, CancellationToken cancellationToken);
        Task UpdateOnlinePaymentAsync(Guid externalPaymentId, OnlinePaymentUpdateRequestDto onlinePaymentUpdateRequest, CancellationToken cancellationToken);
        Task<int> GetOnlinePaymentStatusCountAsync(CancellationToken cancellationToken);
        Task<OnlinePaymentResponseDto> GetOnlinePaymentByExternalPaymentIdAsync(Guid externalPaymentId, CancellationToken cancellationToken);
    }
}
