using AutoMapper;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories;
using EPR.Payment.Service.Common.Dtos.Request;
using EPR.Payment.Service.Services.Interfaces;
using FluentValidation;

namespace EPR.Payment.Service.Services
{
    public class PaymentsService : IPaymentsService
    {
        private readonly IPaymentsRepository _paymentRepository;
        private IMapper _mapper;
        private readonly IValidator<PaymentStatusInsertRequestDto> _paymentStatusInsertRequestValidator;
        private readonly IValidator<PaymentStatusUpdateRequestDto> _paymentStatusUpdateRequestValidator;
        public PaymentsService(IMapper mapper, 
            IPaymentsRepository paymentRepository, 
            IValidator<PaymentStatusInsertRequestDto> paymentStatusInsertRequestValidator, 
            IValidator<PaymentStatusUpdateRequestDto> paymentStatusUpdateRequestValidator)
        {
            _mapper = mapper;
            _paymentRepository = paymentRepository;
            _paymentStatusInsertRequestValidator = paymentStatusInsertRequestValidator;
            _paymentStatusUpdateRequestValidator = paymentStatusUpdateRequestValidator;
        }
        public async Task<Guid> InsertPaymentStatusAsync(PaymentStatusInsertRequestDto paymentStatusInsertRequest, CancellationToken cancellationToken)
        {
            var validatorResult = await _paymentStatusInsertRequestValidator.ValidateAsync(paymentStatusInsertRequest);

            if (!validatorResult.IsValid)
            {
                throw new ValidationException(validatorResult.Errors);
            }

            var entity = _mapper.Map<Common.Data.DataModels.Payment>(paymentStatusInsertRequest);
            return await _paymentRepository.InsertPaymentStatusAsync(entity, cancellationToken);
        }

        public async Task UpdatePaymentStatusAsync(Guid id, PaymentStatusUpdateRequestDto paymentStatusUpdateRequest, CancellationToken cancellationToken)
        {
            var validatorResult = await _paymentStatusUpdateRequestValidator.ValidateAsync(paymentStatusUpdateRequest);

            if (!validatorResult.IsValid)
            {
                throw new ValidationException(validatorResult.Errors);
            }

            var entity = await _paymentRepository.GetPaymentByIdAsync(id, cancellationToken);
            entity = _mapper.Map(paymentStatusUpdateRequest, entity);
            await _paymentRepository.UpdatePaymentStatusAsync(entity, cancellationToken);
        }
        public async Task<int> GetPaymentStatusCountAsync(CancellationToken cancellationToken)
        {
            return await _paymentRepository.GetPaymentStatusCount(cancellationToken);
        }

    }
}
