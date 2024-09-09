using EPR.Payment.Service.Common.Constants.RegistrationFees;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees;

namespace EPR.Payment.Service.Strategies.RegistrationFees.Producer
{
    public class BaseFeeCalculationStrategy : IBaseFeeCalculationStrategy<ProducerRegistrationFeesRequestDto>
    {
        private readonly IProducerFeesRepository _feesRepository;

        public BaseFeeCalculationStrategy(IProducerFeesRepository feesRepository)
        {
            _feesRepository = feesRepository ?? throw new ArgumentNullException(nameof(feesRepository));
        }

        public async Task<decimal> CalculateFeeAsync(ProducerRegistrationFeesRequestDto request, CancellationToken cancellationToken)
        {
            // If ProducerType is empty, return a base fee of zero
            if (string.IsNullOrEmpty(request.ProducerType))
                return 0m;

            // Ensure Regulator is not null or empty
            if (string.IsNullOrEmpty(request.Regulator))
                throw new ArgumentException(ProducerFeesCalculationExceptions.RegulatorMissing);

            var regulator = RegulatorType.Create(request.Regulator);
            return await _feesRepository.GetBaseFeeAsync(request.ProducerType, regulator, cancellationToken);
        }
    }
}