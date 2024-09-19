using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;

namespace EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees
{
    public interface IComplianceSchemeFeesRepository
    {
        Task<decimal> GetBaseFeeAsync(RegulatorType regulator, CancellationToken cancellationToken);
    }
}