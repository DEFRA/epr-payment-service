using EPR.Payment.Service.Common.Dtos.Request.ResubmissionFees.Producer;
using EPR.Payment.Service.Common.Dtos.Response.ResubmissionFees.Producer;

namespace EPR.Payment.Service.Services.Interfaces.ResubmissionFees.Producer
{
    public interface IProducerResubmissionService
    {
        Task<ProducerResubmissionFeeResponseDto> GetResubmissionFeeAsync(ProducerResubmissionFeeRequestDto request, CancellationToken cancellationToken);
    }
}