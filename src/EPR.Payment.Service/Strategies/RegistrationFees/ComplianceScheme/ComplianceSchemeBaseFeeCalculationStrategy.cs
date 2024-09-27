using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees.ComplianceScheme;

namespace EPR.Payment.Service.Strategies.RegistrationFees.ComplianceScheme
{
    public class ComplianceSchemeBaseFeeCalculationStrategy : IComplianceSchemeBaseFeeCalculationStrategy<RegulatorType, decimal>
    {
        private readonly IComplianceSchemeFeesRepository _feesRepository;

        public ComplianceSchemeBaseFeeCalculationStrategy(IComplianceSchemeFeesRepository feesRepository)
        {
            _feesRepository = feesRepository ?? throw new ArgumentNullException(nameof(feesRepository));
        }

        public async Task<decimal> CalculateFeeAsync(RegulatorType regulator, CancellationToken cancellationToken)
        {
            var baseFee = await _feesRepository.GetBaseFeeAsync(regulator, cancellationToken);

            if (baseFee == 0)
            {
                throw new KeyNotFoundException(string.Format(ComplianceSchemeFeeCalculationExceptions.InvalidRegulatorError, regulator.Value));
            }

            return baseFee;
        }
    }
}