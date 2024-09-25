using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees.ComplianceScheme;

namespace EPR.Payment.Service.Services.RegistrationFees.ComplianceScheme
{
    public class ComplianceSchemeBaseFeeService : IComplianceSchemeBaseFeeService
    {
        private readonly IComplianceSchemeBaseFeeCalculationStrategy<RegulatorType, decimal> _baseFeeCalculationStrategy;

        public ComplianceSchemeBaseFeeService(
            IComplianceSchemeBaseFeeCalculationStrategy<RegulatorType, decimal> baseFeeCalculationStrategy)
        {
            _baseFeeCalculationStrategy = baseFeeCalculationStrategy ?? throw new ArgumentNullException(nameof(baseFeeCalculationStrategy));
        }

        public async Task<decimal> GetComplianceSchemeBaseFeeAsync(RegulatorType regulator, CancellationToken cancellationToken)
        {
            try
            {
                return await _baseFeeCalculationStrategy.CalculateFeeAsync(regulator, cancellationToken);
            }
            catch (KeyNotFoundException ex)
            {
                throw new InvalidOperationException(ComplianceSchemeFeeCalculationExceptions.BaseFeeCalculationInvalidOperation, ex);
            }
        }
    }
}