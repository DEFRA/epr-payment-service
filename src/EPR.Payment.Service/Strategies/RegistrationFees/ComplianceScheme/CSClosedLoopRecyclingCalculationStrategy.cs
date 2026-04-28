using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees.ComplianceScheme;

namespace EPR.Payment.Service.Strategies.RegistrationFees.ComplianceScheme
{
    public class CSClosedLoopRecyclingCalculationStrategy : ICSClosedLoopRecyclingCalculationStrategy<ComplianceSchemeMemberWithRegulatorDto, decimal>
    {
        private readonly IComplianceSchemeFeesRepository _feesRepository;

        public CSClosedLoopRecyclingCalculationStrategy(IComplianceSchemeFeesRepository feesRepository)
        {
            _feesRepository = feesRepository ?? throw new ArgumentNullException(nameof(feesRepository));
        }

        public async Task<decimal> CalculateFeeAsync(ComplianceSchemeMemberWithRegulatorDto request, CancellationToken cancellationToken)
        {
            if (!request.IsClosedLoopRecycling)
                return 0m;

            return await _feesRepository.GetClosedLoopRecyclingFeeAsync(request.Regulator, request.SubmissionDate, cancellationToken);
        }
    }
}
