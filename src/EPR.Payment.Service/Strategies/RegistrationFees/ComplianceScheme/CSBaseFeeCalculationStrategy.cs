using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees.ComplianceScheme;

namespace EPR.Payment.Service.Strategies.RegistrationFees.ComplianceScheme
{
    public class CSBaseFeeCalculationStrategy : ICSBaseFeeCalculationStrategy<ComplianceSchemeFeesRequestDto, decimal>
    {
        private readonly IComplianceSchemeFeesRepository _feesRepository;

        public CSBaseFeeCalculationStrategy(IComplianceSchemeFeesRepository feesRepository)
        {
            _feesRepository = feesRepository ?? throw new ArgumentNullException(nameof(feesRepository));
        }

        public async Task<decimal> CalculateFeeAsync(ComplianceSchemeFeesRequestDto request, CancellationToken cancellationToken)
        {
            var regulatorType = RegulatorType.Create(request.Regulator);
            return await _feesRepository.GetBaseFeeAsync(regulatorType, request.SubmissionDate, cancellationToken);
        }
    }
}