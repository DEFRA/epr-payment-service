using EPR.Payment.Service.Common.Dtos.Request.Common;

namespace EPR.Payment.Service.Services.Interfaces.ResubmissionFees.Producer
{
    public interface IProducerResubmissionService
    {
        Task<decimal?> GetResubmissionAsync(RegulatorDto request, CancellationToken cancellationToken);
    }
}
