using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;

namespace EPR.Payment.Service.Strategies.RegistrationFees.Producer
{
    public class SubsidiariesFeeCalculationStrategy : BaseSubsidiariesFeeCalculationStrategy<ProducerRegistrationFeesRequestDto>
    {
        public SubsidiariesFeeCalculationStrategy(IProducerFeesRepository feesRepository) : base (feesRepository)
        {
        }

        protected override int GetNoOfOMPSubsidiaries(ProducerRegistrationFeesRequestDto request)
        {
            return request.NoOfSubsidiariesOnlineMarketplace;
        }

        protected override int GetNoOfSubsidiaries(ProducerRegistrationFeesRequestDto request)
        {
            return request.NumberOfSubsidiaries;
        }

        protected override RegulatorType GetRegulator(ProducerRegistrationFeesRequestDto request)
        {
            return RegulatorType.Create(request.Regulator);
        }
        protected override DateTime GetSubmissionDate(ProducerRegistrationFeesRequestDto request)
        {
            return request.SubmissionDate;
        }
    }
}
