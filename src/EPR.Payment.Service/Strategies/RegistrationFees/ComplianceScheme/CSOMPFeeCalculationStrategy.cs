using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees.ComplianceScheme;

namespace EPR.Payment.Service.Strategies.RegistrationFees.ComplianceScheme
{
    public class CSOMPFeeCalculationStrategy : ICSOMPFeeCalculationStrategy<ComplianceSchemeMemberWithRegulatorDto, decimal>
    {
        private readonly IComplianceSchemeFeesRepository _feesRepository;

        public CSOMPFeeCalculationStrategy(IComplianceSchemeFeesRepository feesRepository)
        {
            _feesRepository = feesRepository ?? throw new ArgumentNullException(nameof(feesRepository));
        }
        public async Task<decimal> CalculateFeeAsync(ComplianceSchemeMemberWithRegulatorDto request, CancellationToken cancellationToken)
        {            
            // If Online Market is false, return zero
            if (!request.IsOnlineMarketplace)
                return 0m;

            return await _feesRepository.GetOnlineMarketFeeAsync(request.Regulator, cancellationToken);
        }
    }
}
