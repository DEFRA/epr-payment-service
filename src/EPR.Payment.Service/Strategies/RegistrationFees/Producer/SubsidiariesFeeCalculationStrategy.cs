using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees.Producer;

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
            var baseFee = await _feesRepository.GetFirst20SubsidiariesFeeAsync(regulator, cancellationToken);

            if (request.NumberOfSubsidiaries <= 20)
                return baseFee * request.NumberOfSubsidiaries;

            var upTo100Fee = await _feesRepository.GetAdditionalUpTo100SubsidiariesFeeAsync(regulator, cancellationToken);

            if (request.NumberOfSubsidiaries <= 100)
                return baseFee * 20 + upTo100Fee * (request.NumberOfSubsidiaries - 20);

            var moreThan100Fee = await _feesRepository.GetAdditionalMoreThan100SubsidiariesFeeAsync(regulator, cancellationToken);

            return baseFee * 20 + upTo100Fee * 80 + moreThan100Fee * (request.NumberOfSubsidiaries - 100);
        }
    }
}
