using EPR.Payment.Service.Common.Dtos.Request;

namespace EPR.Payment.Service.Services.Interfaces
{
    public interface IPaymentsService
    {
        Task InsertPaymentStatusAsync(string paymentId, PaymentStatusInsertRequestDto paymentStatusInsertRequest);
    }
}
