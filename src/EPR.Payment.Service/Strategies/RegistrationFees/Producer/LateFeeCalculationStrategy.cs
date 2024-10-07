using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees.Producer;

namespace EPR.Payment.Service.Strategies.RegistrationFees.Producer
{
    public class LateFeeCalculationStrategy : ILateFeeCalculationStrategy<ProducerRegistrationFeesRequestDto, decimal>
    {
        private readonly IProducerFeesRepository _feesRepository;

        public LateFeeCalculationStrategy(IProducerFeesRepository feesRepository)
        {
            _feesRepository = feesRepository ?? throw new ArgumentNullException(nameof(feesRepository));
        }

        public async Task<decimal> CalculateFeeAsync(ProducerRegistrationFeesRequestDto request, CancellationToken cancellationToken)
        {
            // If Late fee is false, return zero
            if (!request.IsLateFeeApplicable)
                return 0m;

            // Ensure Regulator is not null or empty
            if (string.IsNullOrEmpty(request.Regulator))
                throw new ArgumentException(ProducerFeesCalculationExceptions.RegulatorMissing);

            var regulator = RegulatorType.Create(request.Regulator);
            return await _feesRepository.GetLateFeeAsync(regulator, cancellationToken);
        }
    }
}
