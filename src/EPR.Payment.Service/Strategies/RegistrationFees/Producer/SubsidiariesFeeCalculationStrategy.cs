using EPR.Payment.Service.Common.Constants.RegistrationFees;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees;

namespace EPR.Payment.Service.Strategies.RegistrationFees.Producer
{
    public class SubsidiariesFeeCalculationStrategy : ISubsidiariesFeeCalculationStrategy<ProducerRegistrationFeesRequestDto>
    {
        private readonly IProducerFeesRepository _feesRepository;

        public SubsidiariesFeeCalculationStrategy(IProducerFeesRepository feesRepository)
        {
            _feesRepository = feesRepository ?? throw new ArgumentNullException(nameof(feesRepository));
        }

        public async Task<decimal> CalculateFeeAsync(ProducerRegistrationFeesRequestDto request, CancellationToken cancellationToken)
        {
            if (request.NumberOfSubsidiaries < 0 || string.IsNullOrEmpty(request.Regulator))
                throw new ArgumentException(ProducerFeesCalculationExceptions.InvalidSubsidiariesNumber);

            if (request.NumberOfSubsidiaries == 0)
                return 0m;

            var regulator = RegulatorType.Create(request.Regulator);
            var feePerSubsidiary = await _feesRepository.GetFirst20SubsidiariesFeeAsync(regulator, cancellationToken);

            if (request.NumberOfSubsidiaries > 20)
            {
                var additionalFee = await _feesRepository.GetAdditionalSubsidiariesFeeAsync(regulator, cancellationToken);
                return feePerSubsidiary * 20 + additionalFee * (request.NumberOfSubsidiaries - 20);
            }
            else
            {
                return feePerSubsidiary * request.NumberOfSubsidiaries;
            }
        }
    }
}
