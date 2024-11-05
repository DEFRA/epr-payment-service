using EPR.Payment.Service.Common.Dtos.Request.ResubmissionFees.Producer;
using EPR.Payment.Service.Common.Dtos.Response.ResubmissionFees.Producer;
using EPR.Payment.Service.Services.Interfaces.Payments;
using EPR.Payment.Service.Services.Interfaces.ResubmissionFees.Producer;
using EPR.Payment.Service.Strategies.Interfaces.ResubmissionFees.Producer;

namespace EPR.Payment.Service.Services.ResubmissionFees.Producer
{
    public class ProducerResubmissionService : IProducerResubmissionService
    {
        private readonly IResubmissionAmountStrategy<ProducerResubmissionFeeRequestDto, decimal> _resubmissionAmountStrategy;
        private readonly IPaymentsService _paymentsService;

        public ProducerResubmissionService(
            IResubmissionAmountStrategy<ProducerResubmissionFeeRequestDto, decimal> resubmissionAmountStrategy,
            IPaymentsService paymentsService)
        {
            _resubmissionAmountStrategy = resubmissionAmountStrategy ?? throw new ArgumentNullException(nameof(resubmissionAmountStrategy));
            _paymentsService = paymentsService ?? throw new ArgumentNullException(nameof(paymentsService));
        }

        public async Task<ProducerResubmissionFeeResponseDto> GetResubmissionFeeAsync(
            ProducerResubmissionFeeRequestDto request, CancellationToken cancellationToken)
        {
            var baseFee = await _resubmissionAmountStrategy.CalculateFeeAsync(request, cancellationToken);
            var previousPayments = await _paymentsService.GetPreviousPaymentsByReferenceAsync(request.ReferenceNumber, cancellationToken);

            var totalFee = baseFee;
            var outstandingPayment = totalFee - previousPayments;

            return new ProducerResubmissionFeeResponseDto
            {
                TotalResubmissionFee = totalFee,
                PreviousPayments = previousPayments,
                OutstandingPayment = outstandingPayment
            };
        }
    }
}