using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;

namespace EPR.Payment.Service.Services.Interfaces.RegistrationFees.ComplianceScheme
{
    public interface IComplianceSchemeBaseFeeService
    {
        Task<decimal> GetComplianceSchemeBaseFeeAsync(RegulatorType regulator, CancellationToken cancellationToken);
    }
}