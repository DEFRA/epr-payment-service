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
            ValidateRequest(request);

            if (request.NumberOfSubsidiaries == 0)
            {
                return 0m;
            }

            var regulator = RegulatorType.Create(request.Regulator);
            var baseFee = await _feesRepository.GetFirst20SubsidiariesFeeAsync(regulator, cancellationToken);

            if (request.NumberOfSubsidiaries <= 20)
            {
                return CalculateFeeForUpTo20Subsidiaries(baseFee, request.NumberOfSubsidiaries);
            }

            var upTo100Fee = await _feesRepository.GetAdditionalUpTo100SubsidiariesFeeAsync(regulator, cancellationToken);

            if (request.NumberOfSubsidiaries <= 100)
            {
                return CalculateFeeForUpTo100Subsidiaries(baseFee, upTo100Fee, request.NumberOfSubsidiaries);
            }

            var moreThan100Fee = await _feesRepository.GetAdditionalMoreThan100SubsidiariesFeeAsync(regulator, cancellationToken);

            return CalculateFeeForMoreThan100Subsidiaries(baseFee, upTo100Fee, moreThan100Fee, request.NumberOfSubsidiaries);
        }

        private void ValidateRequest(ProducerRegistrationFeesRequestDto request)
        {
            if (request.NumberOfSubsidiaries < 0 || string.IsNullOrEmpty(request.Regulator))
            {
                throw new ArgumentException(ProducerFeesCalculationExceptions.InvalidSubsidiariesNumber);
            }
        }

        private decimal CalculateFeeForUpTo20Subsidiaries(decimal baseFee, int numberOfSubsidiaries)
        {
            return baseFee * numberOfSubsidiaries;
        }

        private decimal CalculateFeeForUpTo100Subsidiaries(decimal baseFee, decimal upTo100Fee, int numberOfSubsidiaries)
        {
            return baseFee * 20 + upTo100Fee * (numberOfSubsidiaries - 20);
        }

        private decimal CalculateFeeForMoreThan100Subsidiaries(decimal baseFee, decimal upTo100Fee, decimal moreThan100Fee, int numberOfSubsidiaries)
        {
            return baseFee * 20 + upTo100Fee * 80 + moreThan100Fee * (numberOfSubsidiaries - 100);
        }
    }

}
