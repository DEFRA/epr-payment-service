using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;

namespace EPR.Payment.Service.Strategies.Interfaces.RegistrationFees
{
    public interface IResubmissionAmountStrategy
    {
        Task<decimal?> GetResubmissionAsync(RegulatorDto producerResubmissionFeeRequestDto, CancellationToken cancellationToken);
    }
}
