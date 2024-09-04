using EPR.Payment.Service.Services.Interfaces.RegistrationFees;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees;

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
            return await _resubmissionAmountStrategy.GetResubmissionAsync(regulator, cancellationToken);
        }
    }
}