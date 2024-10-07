using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;

namespace EPR.Payment.Service.Strategies.RegistrationFees.ComplianceScheme
{
    public class CSSubsidiariesFeeCalculationStrategy : BaseSubsidiariesFeeCalculationStrategy<ComplianceSchemeMemberWithRegulatorDto>
    {
        private readonly IComplianceSchemeFeesRepository _feesRepository;

        public CSSubsidiariesFeeCalculationStrategy(IComplianceSchemeFeesRepository feesRepository)
        {
            _feesRepository = feesRepository ?? throw new ArgumentNullException(nameof(feesRepository));
        }

        protected override async Task<decimal> GetFirstBandFeeAsync(RegulatorType regulator, CancellationToken cancellationToken)
        {
            return await _feesRepository.GetFirstBandFeeAsync(regulator, cancellationToken);
        }
        protected override async Task<decimal> GetSecondBandFeeAsync(RegulatorType regulator, CancellationToken cancellationToken)
        {
            return await _feesRepository.GetSecondBandFeeAsync(regulator, cancellationToken);
        }

        protected override async Task<decimal> GetThirdBandFeeAsync(RegulatorType regulator, CancellationToken cancellationToken)
        {
            return await _feesRepository.GetThirdBandFeeAsync(regulator, cancellationToken);
        }

        protected override async Task<decimal> GetOnlineMarketFeeAsync(RegulatorType regulator, CancellationToken cancellationToken)
        {
            return await _feesRepository.GetOnlineMarketFeeAsync(regulator, cancellationToken);
        }

        protected override int GetNoOfOMPSubsidiaries(ComplianceSchemeMemberWithRegulatorDto request)
        {
            return request.NoOfSubsidiariesOnlineMarketplace;
        }

        protected override int GetNoOfSubsidiaries(ComplianceSchemeMemberWithRegulatorDto request)
        {
            return request.NumberOfSubsidiaries;
        }

        protected override RegulatorType GetRegulator(ComplianceSchemeMemberWithRegulatorDto request)
        {
            return request.Regulator;
        }
    }
}
