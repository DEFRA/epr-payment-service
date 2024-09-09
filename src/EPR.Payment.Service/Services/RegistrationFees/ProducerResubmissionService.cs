using EPR.Payment.Service.Services.Interfaces.RegistrationFees;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees.Producer;

namespace EPR.Payment.Service.Services.RegistrationFees
{
    public class ProducerResubmissionService : IProducerResubmissionService
    {
        private readonly IResubmissionAmountStrategy _resubmissionAmountStrategy;

        public ProducerResubmissionService(IResubmissionAmountStrategy resubmissionAmountStrategy)
        {
            _resubmissionAmountStrategy = resubmissionAmountStrategy ?? throw new ArgumentNullException(nameof(resubmissionAmountStrategy));
        }

        public async Task<decimal?> GetResubmissionAsync(string regulator, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(regulator))
            {
                throw new ArgumentException("Regulator cannot be null or empty", nameof(regulator));
            }

            return await _resubmissionAmountStrategy.CalculateFeeAsync(regulator, cancellationToken);
        }
    }
}