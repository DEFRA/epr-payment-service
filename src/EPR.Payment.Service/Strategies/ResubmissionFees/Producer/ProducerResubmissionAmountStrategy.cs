using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.ResubmissionFees.Producer;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using EPR.Payment.Service.Strategies.Interfaces.ResubmissionFees.Producer;

namespace EPR.Payment.Service.Strategies.ResubmissionFees.Producer
{
    public class ProducerResubmissionAmountStrategy : IResubmissionAmountStrategy<ProducerResubmissionFeeRequestDto, decimal>
    {
        private readonly IProducerFeesRepository _feesRepository;

        public ProducerResubmissionAmountStrategy(IProducerFeesRepository feesRepository)
        {
            _feesRepository = feesRepository ?? throw new ArgumentNullException(nameof(feesRepository));
        }

        public async Task<decimal> CalculateFeeAsync(ProducerResubmissionFeeRequestDto request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Regulator))
            {
                throw new ArgumentException("Regulator cannot be null or empty");
            }

            var regulatorType = RegulatorType.Create(request.Regulator);

            var fee = await _feesRepository.GetResubmissionAsync(regulatorType, request.ResubmissionDate, cancellationToken);

            if (fee == 0)
            {
                throw new KeyNotFoundException(string.Format(ProducerFeesCalculationExceptions.InvalidRegulatorError, request.Regulator));
            }

            return fee;
        }
    }
}