using EPR.Payment.Service.Common.Dtos.Request;

namespace EPR.Payment.Service.Common.Data.Interfaces.Repositories
{
    public interface IPaymentsRepository
    {
        Task InsertPaymentStatusAsync(Guid externalPaymentId, string paymentId, PaymentStatusInsertRequestDto paymentStatusInsertRequest);
    }
}
