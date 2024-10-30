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
        public OfflinePaymentsService(IMapper mapper,
            IOfflinePaymentsRepository offlinePaymentRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _OfflinePaymentRepository = offlinePaymentRepository ?? throw new ArgumentNullException(nameof(offlinePaymentRepository));
        }
        public async Task InsertOfflinePaymentAsync(OfflinePaymentInsertRequestDto paymentInsertRequest, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Common.Data.DataModels.Payment>(paymentInsertRequest);
            entity.OfflinePayment = _mapper.Map<Common.Data.DataModels.OfflinePayment>(paymentInsertRequest);

            await _OfflinePaymentRepository.InsertOfflinePaymentAsync(entity, cancellationToken);
        }
    }
}
