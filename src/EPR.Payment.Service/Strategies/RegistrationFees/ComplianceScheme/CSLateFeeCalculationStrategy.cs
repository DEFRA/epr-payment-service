using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees.ComplianceScheme;

namespace EPR.Payment.Service.Strategies.RegistrationFees.ComplianceScheme
{
    public class CSLateFeeCalculationStrategy : ICSLateFeeCalculationStrategy<ComplianceSchemeMemberWithRegulatorDto, decimal>
    {
        private readonly IComplianceSchemeFeesRepository _feesRepository;

        public CSLateFeeCalculationStrategy(IComplianceSchemeFeesRepository feesRepository)
        {
            _feesRepository = feesRepository ?? throw new ArgumentNullException(nameof(feesRepository));
        }
        public async Task<decimal> CalculateFeeAsync(ComplianceSchemeMemberWithRegulatorDto request, CancellationToken cancellationToken)
        {            
            // If Online Market is false, return zero
            if (!request.IsLateFeeApplicable)
                return 0m;

            return await _feesRepository.GetLateFeeAsync(request.Regulator, cancellationToken);
        }
    }
}
