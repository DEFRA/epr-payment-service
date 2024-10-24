using AutoMapper;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.Payments;
using EPR.Payment.Service.Common.Dtos.Request.Payments;
using EPR.Payment.Service.Common.Dtos.Response.Payments;
using EPR.Payment.Service.Services.Interfaces.Payments;
using FluentValidation;

namespace EPR.Payment.Service.Services.Payments
{
    public class OnlinePaymentsService : IOnlinePaymentsService
    {
        private readonly IOnlinePaymentsRepository _OnlinePaymentRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<OnlinePaymentStatusInsertRequestDto> _onlinePaymentStatusInsertRequestValidator;
        private readonly IValidator<OnlinePaymentStatusUpdateRequestDto> _OnlinePaymentStatusUpdateRequestValidator;
        public OnlinePaymentsService(IMapper mapper,
            IOnlinePaymentsRepository onlinePaymentRepository,
            IValidator<OnlinePaymentStatusInsertRequestDto> onlinePaymentStatusInsertRequestValidator,
            IValidator<OnlinePaymentStatusUpdateRequestDto> onlinePaymentStatusUpdateRequestValidator)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _OnlinePaymentRepository = onlinePaymentRepository ?? throw new ArgumentNullException(nameof(onlinePaymentRepository));
            _onlinePaymentStatusInsertRequestValidator = onlinePaymentStatusInsertRequestValidator ?? throw new ArgumentNullException(nameof(onlinePaymentStatusInsertRequestValidator));
            _OnlinePaymentStatusUpdateRequestValidator = onlinePaymentStatusUpdateRequestValidator ?? throw new ArgumentNullException(nameof(onlinePaymentStatusUpdateRequestValidator));
        }
        public async Task<Guid> InsertOnlinePaymentStatusAsync(OnlinePaymentStatusInsertRequestDto onlinePaymentStatusInsertRequest, CancellationToken cancellationToken)
        {
            var validatorResult = await _onlinePaymentStatusInsertRequestValidator.ValidateAsync(onlinePaymentStatusInsertRequest, cancellationToken);

            if (!validatorResult.IsValid)
            {
                throw new ValidationException(validatorResult.Errors);
            }

            var entity = _mapper.Map<Common.Data.DataModels.OnlinePayment>(onlinePaymentStatusInsertRequest);
            return await _OnlinePaymentRepository.InsertOnlinePaymentAsync(entity, cancellationToken);
        }

        public async Task UpdateOnlinePaymentStatusAsync(Guid externalPaymentId, OnlinePaymentStatusUpdateRequestDto onlinePaymentStatusUpdateRequest, CancellationToken cancellationToken)
        {
            var validatorResult = await _OnlinePaymentStatusUpdateRequestValidator.ValidateAsync(onlinePaymentStatusUpdateRequest, cancellationToken);

            if (!validatorResult.IsValid)
            {
                throw new ValidationException(validatorResult.Errors);
            }

            var entity = await _OnlinePaymentRepository.GetOnlinePaymentByExternalPaymentIdAsync(externalPaymentId, cancellationToken);
            entity = _mapper.Map(onlinePaymentStatusUpdateRequest, entity);
            await _OnlinePaymentRepository.UpdateOnlinePaymentAsync(entity, cancellationToken);
        }
        public async Task<int> GetOnlinePaymentStatusCountAsync(CancellationToken cancellationToken)
        {
            return await _OnlinePaymentRepository.GetPaymentStatusCount(cancellationToken);
        }

        public async Task<OnlinePaymentResponseDto> GetOnlinePaymentByExternalPaymentIdAsync(Guid externalPaymentId, CancellationToken cancellationToken)
        {
            var entity = await _OnlinePaymentRepository.GetOnlinePaymentByExternalPaymentIdAsync(externalPaymentId, cancellationToken);
            return _mapper.Map<OnlinePaymentResponseDto>(entity);
        }

    }
}
