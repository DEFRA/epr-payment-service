using AutoMapper;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories;
using EPR.Payment.Service.Common.Dtos.Request;
using EPR.Payment.Service.Services.Interfaces;

namespace EPR.Payment.Service.Services
{
    public class PaymentsService : IPaymentsService
    {
        private readonly IPaymentsRepository _paymentRepository;
        private IMapper _mapper;
        public PaymentsService(IMapper mapper, IPaymentsRepository paymentRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _paymentRepository = paymentRepository;
        }
        public async Task InsertPaymentStatusAsync(Guid externalPaymentId, string paymentId, PaymentStatusInsertRequestDto paymentStatusInsertRequest)
        {
            ValidatePaymentStatusInsertRequest(paymentId, paymentStatusInsertRequest);
            await _paymentRepository.InsertPaymentStatusAsync(externalPaymentId, paymentId, paymentStatusInsertRequest);
        }

        private static void ValidatePaymentStatusInsertRequest(string paymentId, PaymentStatusInsertRequestDto paymentStatusInsertRequest)
        {
            if (string.IsNullOrEmpty(paymentId))
                throw new ArgumentException("PaymentId cannot be null or empty.", nameof(paymentId));
            if (paymentStatusInsertRequest == null || string.IsNullOrEmpty(paymentStatusInsertRequest.Status))
                throw new ArgumentException("Status cannot be null or empty.", nameof(paymentStatusInsertRequest.Status));
        }
    }
}
