using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees;

namespace EPR.Payment.Service.Strategies.RegistrationFees.Producer
{
    public class DefaultResubmissionAmountStrategy : IResubmissionAmountStrategy
    {
        private readonly IProducerFeesRepository _feesRepository;

        public DefaultResubmissionAmountStrategy(IProducerFeesRepository feesRepository)
        {
            _feesRepository = feesRepository ?? throw new ArgumentNullException(nameof(feesRepository));
        }

        public async Task<decimal?> GetResubmissionAsync(string regulator, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(regulator))
            {
                throw new ArgumentException("Regulator cannot be null or empty", nameof(regulator));
            }

            var regulatorType = RegulatorType.Create(regulator);
            return await _feesRepository.GetResubmissionAsync(regulatorType, cancellationToken);
        }
    }
}
