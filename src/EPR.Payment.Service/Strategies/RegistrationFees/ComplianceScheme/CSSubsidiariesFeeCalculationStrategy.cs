using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;

namespace EPR.Payment.Service.Strategies.RegistrationFees.ComplianceScheme
{
    public class CSSubsidiariesFeeCalculationStrategy : BaseSubsidiariesFeeCalculationStrategy<ComplianceSchemeMemberWithRegulatorDto>
    {

        public CSSubsidiariesFeeCalculationStrategy(IComplianceSchemeFeesRepository feesRepository) : base (feesRepository)
        {
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
