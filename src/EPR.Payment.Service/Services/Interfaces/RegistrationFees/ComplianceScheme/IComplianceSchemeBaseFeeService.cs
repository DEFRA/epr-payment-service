namespace EPR.Payment.Service.Services.Interfaces.RegistrationFees.ComplianceScheme
{
    public interface IComplianceSchemeBaseFeeService
    {
        Task<decimal> GetComplianceSchemeBaseFeeAsync(string regulator, CancellationToken cancellationToken);
    }
}