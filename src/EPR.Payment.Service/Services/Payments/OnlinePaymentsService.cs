using AutoMapper;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.Payments;
using EPR.Payment.Service.Common.Dtos.Request.Payments;
using EPR.Payment.Service.Common.Dtos.Response.Payments;
using EPR.Payment.Service.Services.Interfaces.Payments;

namespace EPR.Payment.Service.Services.Payments
{
    public class OnlinePaymentsService : IOnlinePaymentsService
    {
        private readonly IOnlinePaymentsRepository _onlinePaymentRepository;
        private readonly IMapper _mapper;
        public OnlinePaymentsService(IMapper mapper,
            IOnlinePaymentsRepository onlinePaymentRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _onlinePaymentRepository = onlinePaymentRepository ?? throw new ArgumentNullException(nameof(onlinePaymentRepository));
        }

        public async Task<Guid> InsertOnlinePaymentAsync(OnlinePaymentInsertRequestDto onlinePaymentInsertRequest, CancellationToken cancellationToken)
        {
            var paymentEntity = _mapper.Map<Common.Data.DataModels.Payment>(onlinePaymentInsertRequest);
            paymentEntity.OnlinePayment = _mapper.Map<Common.Data.DataModels.OnlinePayment>(onlinePaymentInsertRequest);

            return await _onlinePaymentRepository.InsertOnlinePaymentAsync(paymentEntity, cancellationToken);
        }

        public async Task<Guid> InsertOnlinePaymentAsync(OnlinePaymentInsertRequestV2Dto onlinePaymentInsertRequest, CancellationToken cancellationToken)
        {
            var paymentEntity = _mapper.Map<Common.Data.DataModels.Payment>(onlinePaymentInsertRequest);
            paymentEntity.OnlinePayment = _mapper.Map<Common.Data.DataModels.OnlinePayment>(onlinePaymentInsertRequest);

            return await _onlinePaymentRepository.InsertOnlinePaymentAsync(paymentEntity, cancellationToken);
        }

        public async Task UpdateOnlinePaymentAsync(Guid externalPaymentId, OnlinePaymentUpdateRequestDto onlinePaymentUpdateRequest, CancellationToken cancellationToken)
        {
            var entity = await _onlinePaymentRepository.GetOnlinePaymentByExternalPaymentIdAsync(externalPaymentId, cancellationToken);

            _mapper.Map(onlinePaymentUpdateRequest, entity);
            _mapper.Map(onlinePaymentUpdateRequest, entity.OnlinePayment);
            await _onlinePaymentRepository.UpdateOnlinePayment(entity, cancellationToken);
        }

        public async Task<int> GetOnlinePaymentStatusCountAsync(CancellationToken cancellationToken)
        {
            return await _onlinePaymentRepository.GetPaymentStatusCount(cancellationToken);
        }

        public async Task<OnlinePaymentResponseDto> GetOnlinePaymentByExternalPaymentIdAsync(Guid externalPaymentId, CancellationToken cancellationToken)
        {
            var entity = await _onlinePaymentRepository.GetOnlinePaymentByExternalPaymentIdAsync(externalPaymentId, cancellationToken);
            return _mapper.Map<OnlinePaymentResponseDto>(entity);
        }

    }
}
