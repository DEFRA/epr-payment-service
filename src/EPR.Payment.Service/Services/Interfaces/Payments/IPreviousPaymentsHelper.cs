using EPR.Payment.Service.Common.Dtos.Response.Payments;

namespace EPR.Payment.Service.Services.Interfaces.Payments
{
    public interface IPreviousPaymentsHelper
    {
        Task<PreviousPaymentDetailResponseDto?> GetPreviousPaymentAsync(
            string applicationReferenceNumber,
            CancellationToken cancellationToken);
    }
}
