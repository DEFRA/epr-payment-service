using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees.Producer;

namespace EPR.Payment.Service.Strategies.RegistrationFees.Producer
{
    public class BaseFeeCalculationStrategyV2 : IBaseFeeCalculationStrategy<ProducerRegistrationFeesRequestV2Dto, decimal>
    {
        private readonly IProducerFeesRepository _feesRepository;

        public BaseFeeCalculationStrategyV2(IProducerFeesRepository feesRepository)
        {
            _feesRepository = feesRepository ?? throw new ArgumentNullException(nameof(feesRepository));
        }

        public async Task<decimal> CalculateFeeAsync(ProducerRegistrationFeesRequestV2Dto request, CancellationToken cancellationToken)
        {
            // If ProducerType is empty, return a base fee of zero
            if (string.IsNullOrEmpty(request.ProducerType))
                return 0m;

            // Ensure Regulator is not null or empty
            if (string.IsNullOrEmpty(request.Regulator))
                throw new ArgumentException(ProducerFeesCalculationExceptions.RegulatorMissing);

            var regulator = RegulatorType.Create(request.Regulator);
            return await _feesRepository.GetBaseFeeAsync(request.ProducerType, regulator, request.SubmissionDate, cancellationToken);
        }
    }
}