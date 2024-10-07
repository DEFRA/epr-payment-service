using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;

namespace EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees
{
    public interface IComplianceSchemeFeesRepository
    {
        Task<decimal> GetBaseFeeAsync(RegulatorType regulator, CancellationToken cancellationToken);
        Task<decimal> GetMemberFeeAsync(string memberType, RegulatorType regulator, CancellationToken cancellationToken);
        Task<decimal> GetFirstBandFeeAsync(RegulatorType regulator, CancellationToken cancellationToken);
        Task<decimal> GetSecondBandFeeAsync(RegulatorType regulator, CancellationToken cancellationToken);
        Task<decimal> GetThirdBandFeeAsync(RegulatorType regulator, CancellationToken cancellationToken);
        Task<decimal> GetOnlineMarketFeeAsync(RegulatorType regulator, CancellationToken cancellationToken);
    }
}