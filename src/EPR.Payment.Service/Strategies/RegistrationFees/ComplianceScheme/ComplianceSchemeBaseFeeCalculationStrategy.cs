using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees.ComplianceScheme;

namespace EPR.Payment.Service.Strategies.RegistrationFees.ComplianceScheme
{
    public class ComplianceSchemeBaseFeeCalculationStrategy : IComplianceSchemeBaseFeeCalculationStrategy
    {
        private readonly IComplianceSchemeFeesRepository _feesRepository;

        public ComplianceSchemeBaseFeeCalculationStrategy(IComplianceSchemeFeesRepository feesRepository)
        {
            _feesRepository = feesRepository ?? throw new ArgumentNullException(nameof(feesRepository));
        }

        public async Task<decimal> CalculateFeeAsync(string regulator, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(regulator))
            {
                throw new ArgumentException(ComplianceSchemeFeeCalculationExceptions.RegulatorMissing);
            }

            var regulatorType = RegulatorType.Create(regulator);

            var baseFee = await _feesRepository.GetBaseFeeAsync(regulatorType, cancellationToken);

            if (baseFee == 0)
            {
                throw new KeyNotFoundException(string.Format(ComplianceSchemeFeeCalculationExceptions.InvalidRegulatorError, regulator));
            }

            return baseFee;
        }
    }
}