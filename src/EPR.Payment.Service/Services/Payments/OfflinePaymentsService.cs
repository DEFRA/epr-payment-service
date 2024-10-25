using AutoMapper;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.Payments;
using EPR.Payment.Service.Common.Dtos.Request.Payments;
using EPR.Payment.Service.Services.Interfaces.Payments;
using FluentValidation;

namespace EPR.Payment.Service.Services.Payments
{
    public class OfflinePaymentsService : IOfflinePaymentsService
    {
        private readonly IOfflinePaymentsRepository _OfflinePaymentRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<OfflinePaymentStatusInsertRequestDto> _offlinePaymentStatusInsertRequestValidator;
        public OfflinePaymentsService(IMapper mapper,
            IOfflinePaymentsRepository offlinePaymentRepository,
            IValidator<OfflinePaymentStatusInsertRequestDto> offlinePaymentStatusInsertRequestValidator)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _OfflinePaymentRepository = offlinePaymentRepository ?? throw new ArgumentNullException(nameof(offlinePaymentRepository));
            _offlinePaymentStatusInsertRequestValidator = offlinePaymentStatusInsertRequestValidator ?? throw new ArgumentNullException(nameof(offlinePaymentStatusInsertRequestValidator));
        }
        public async Task InsertOfflinePaymentAsync(OfflinePaymentStatusInsertRequestDto offlinePaymentInsertRequest, CancellationToken cancellationToken)
        {
            var validatorResult = await _offlinePaymentStatusInsertRequestValidator.ValidateAsync(offlinePaymentInsertRequest, cancellationToken);

            if (!validatorResult.IsValid)
            {
                throw new ValidationException(validatorResult.Errors);
            }

            var entity = _mapper.Map<Common.Data.DataModels.OfflinePayment>(offlinePaymentInsertRequest);
            
            await _OfflinePaymentRepository.InsertOfflinePaymentAsync(entity, cancellationToken);
        }
    }
}
