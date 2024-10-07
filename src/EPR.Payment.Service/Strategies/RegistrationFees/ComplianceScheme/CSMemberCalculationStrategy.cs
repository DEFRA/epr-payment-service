using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees.ComplianceScheme;

namespace EPR.Payment.Service.Strategies.RegistrationFees.ComplianceScheme
{
    public class CSMemberCalculationStrategy : ICSMemberCalculationStrategy<ComplianceSchemeMemberWithRegulatorDto, decimal>
    {
        private readonly IComplianceSchemeFeesRepository _feesRepository;

        public CSMemberCalculationStrategy(IComplianceSchemeFeesRepository feesRepository)
        {
            _feesRepository = feesRepository ?? throw new ArgumentNullException(nameof(feesRepository));
        }

        public async Task<decimal> CalculateFeeAsync(ComplianceSchemeMemberWithRegulatorDto request, CancellationToken cancellationToken)
        {
            return await _feesRepository.GetMemberFeeAsync(request.MemberType, request.Regulator, cancellationToken);
        }
    }
}
