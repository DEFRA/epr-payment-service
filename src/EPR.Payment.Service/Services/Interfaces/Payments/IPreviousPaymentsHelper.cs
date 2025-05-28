using EPR.Payment.Service.Common.Dtos.Response.Common;

namespace EPR.Payment.Service.Services.Interfaces.Payments
{
    public interface IPreviousPaymentsHelper
    {
        Task<T?> GetPreviousPaymentAsync<T>(string applicationReferenceNumber, CancellationToken cancellationToken) where T : BasePreviousPaymentDetailDto, new();
    }
}
