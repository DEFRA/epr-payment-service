using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;

namespace EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees
{
    public interface IProducerFeesRepository
    {
        Task<decimal> GetBaseFeeAsync(string producer, RegulatorType regulator, CancellationToken cancellationToken);

        Task<decimal> GetFirstBandFeeAsync(RegulatorType regulator, CancellationToken cancellationToken);

        Task<decimal> GetSecondBandFeeAsync(RegulatorType regulator, CancellationToken cancellationToken);

        Task<decimal> GetThirdBandFeeAsync(RegulatorType regulator, CancellationToken cancellationToken);

        Task<decimal> GetOnlineMarketFeeAsync(RegulatorType regulator, CancellationToken cancellationToken);

        Task<decimal> GetResubmissionAsync(RegulatorType regulator, CancellationToken cancellationToken);
    }
}