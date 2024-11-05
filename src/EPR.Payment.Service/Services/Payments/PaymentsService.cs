using EPR.Payment.Service.Common.Constants.Payments;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.Payments;
using EPR.Payment.Service.Services.Interfaces.Payments;

namespace EPR.Payment.Service.Services.Payments
{
    public class PaymentsService : IPaymentsService
    {
        private readonly IPaymentsRepository _paymentsRepository;
        public PaymentsService(IPaymentsRepository paymentsRepository)
        {
            _paymentsRepository = paymentsRepository ?? throw new ArgumentNullException(nameof(paymentsRepository));
        }

        public async Task<decimal> GetPreviousPaymentsByReferenceAsync(string reference, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(reference))
            {
                throw new ArgumentException(PaymentConstants.InvalidReference);
            }

            return await _paymentsRepository.GetPreviousPaymentsByReferenceAsync(reference, cancellationToken);
        }
    }
}
