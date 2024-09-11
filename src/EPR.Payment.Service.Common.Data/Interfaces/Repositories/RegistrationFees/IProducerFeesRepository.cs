using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;

namespace EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees
{
    public interface IProducerFeesRepository
    {
        Task<decimal> GetBaseFeeAsync(string producer, RegulatorType regulator, CancellationToken cancellationToken);

        Task<decimal> GetFirst20SubsidiariesFeeAsync(RegulatorType regulator, CancellationToken cancellationToken);

        Task<decimal> GetAdditionalUpTo100SubsidiariesFeeAsync(RegulatorType regulator, CancellationToken cancellationToken);
        Task<decimal> GetAdditionalMoreThan100SubsidiariesFeeAsync(RegulatorType regulator, CancellationToken cancellationToken);

        Task<decimal> GetOnlineMarketFeeAsync(RegulatorType regulator, CancellationToken cancellationToken);

        Task<decimal> GetResubmissionAsync(RegulatorType regulator, CancellationToken cancellationToken);
    }
}