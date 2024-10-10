using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees.ComplianceScheme;

namespace EPR.Payment.Service.Strategies.RegistrationFees.ComplianceScheme
{
    public class CSBaseFeeCalculationStrategy : ICSBaseFeeCalculationStrategy<RegulatorType, decimal>
    {
        private readonly IComplianceSchemeFeesRepository _feesRepository;

        public CSBaseFeeCalculationStrategy(IComplianceSchemeFeesRepository feesRepository)
        {
            _feesRepository = feesRepository ?? throw new ArgumentNullException(nameof(feesRepository));
        }

        public async Task<decimal> CalculateFeeAsync(RegulatorType regulator, CancellationToken cancellationToken)
        {
            return await _feesRepository.GetBaseFeeAsync(regulator, cancellationToken);
        }
    }
}