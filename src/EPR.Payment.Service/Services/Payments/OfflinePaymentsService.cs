using AutoMapper;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.Payments;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationSubmission;
using EPR.Payment.Service.Common.Dtos.Request.Payments;
using EPR.Payment.Service.Services.Interfaces.Payments;

namespace EPR.Payment.Service.Services.Payments
{
    public class OfflinePaymentsService : IOfflinePaymentsService
    {
        private readonly IOfflinePaymentsRepository _OfflinePaymentRepository;
        private readonly IRegistrationSubmissionDataRepository _registrationSubmissionDataRepository;
        private readonly IMapper _mapper;

        public OfflinePaymentsService(
            IMapper mapper,
            IOfflinePaymentsRepository offlinePaymentRepository,
            IRegistrationSubmissionDataRepository registrationSubmissionDataRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _OfflinePaymentRepository = offlinePaymentRepository ?? throw new ArgumentNullException(nameof(offlinePaymentRepository));
            _registrationSubmissionDataRepository = registrationSubmissionDataRepository ?? throw new ArgumentNullException(nameof(registrationSubmissionDataRepository));
        }

        public async Task InsertOfflinePaymentAsync(
            OfflinePaymentInsertRequestDto paymentInsertRequest,
            CancellationToken cancellationToken)
        {
            var paymentEntity = _mapper.Map<Common.Data.DataModels.Payment>(paymentInsertRequest);
            paymentEntity.OfflinePayment = _mapper.Map<Common.Data.DataModels.OfflinePayment>(paymentInsertRequest);

            paymentEntity.RegistrationSubmissionDataId = await _registrationSubmissionDataRepository
                .GetLatestIdByApplicationReferenceNumberAsync(paymentEntity.Reference, cancellationToken);

            await _OfflinePaymentRepository.InsertOfflinePaymentAsync(paymentEntity, cancellationToken);
        }
    }
}
