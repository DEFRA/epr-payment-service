using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;

namespace EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees
{
    public interface IFeeRepository
    {
        Task<decimal> GetFirstBandFeeAsync(RegulatorType regulator, DateTime submissionDate, CancellationToken cancellationToken);

        Task<decimal> GetSecondBandFeeAsync(RegulatorType regulator, DateTime submissionDate, CancellationToken cancellationToken);

        Task<decimal> GetLateFeeAsync(RegulatorType regulator, DateTime submissionDate, CancellationToken cancellationToken);

        Task<decimal> GetThirdBandFeeAsync(RegulatorType regulator, DateTime submissionDate, CancellationToken cancellationToken);

        Task<decimal> GetOnlineMarketFeeAsync(RegulatorType regulator, DateTime submissionDate, CancellationToken cancellationToken);
    }
}
