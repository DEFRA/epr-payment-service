using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;

namespace EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees
{
    public interface IComplianceSchemeFeesRepository : IFeeRepository
    {
        Task<decimal> GetBaseFeeAsync(RegulatorType regulator, DateTime submissionDate, CancellationToken cancellationToken);
        Task<decimal> GetMemberFeeAsync(string memberType, RegulatorType regulator, DateTime submissionDate, CancellationToken cancellationToken);
    }
}