using EPR.Payment.Service.Common.Dtos.Request;

namespace EPR.Payment.Service.Services.Interfaces
{
    public interface IPaymentsService
    {
        Task InsertPaymentStatusAsync(Guid externalPaymentId, string paymentId, PaymentStatusInsertRequestDto paymentStatusInsertRequest);
    }
}
