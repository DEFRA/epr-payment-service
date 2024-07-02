using AutoMapper;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories;
using EPR.Payment.Service.Common.Dtos.Requests;
using EPR.Payment.Service.Common.Dtos.Responses;
using EPR.Payment.Service.Services.Interfaces;

namespace EPR.Payment.Service.Services
{
    public class ProducerFeesService : IProducerFeesService
    {
        private readonly IProducerFeesRepository _producerFeesRepository;
        private IMapper _mapper;

        public ProducerFeesService(IMapper mapper, IProducerFeesRepository producerFeesRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _producerFeesRepository = producerFeesRepository ?? throw new ArgumentNullException(nameof(producerFeesRepository));
        }

        public async Task<RegistrationFeeResponseDto> CalculateFeesAsync(ProducerRegistrationRequestDto request)
        {
            var producerFees = await _producerFeesRepository.GetProducerFeesAmountAsync(request);
            var producerSubsidiariesFees = await _producerFeesRepository.GetProducerSubsFeesAmountAsync(request);

            return new RegistrationFeeResponseDto
            {
                BaseFee = request.PayBaseFee ? producerFees : null,
                ProducersFee = producerFees,
                SubsidiariesFee = producerSubsidiariesFees,
                TotalFee = (producerFees ?? 0) + (producerSubsidiariesFees ?? 0)
            };
        }

        public async Task<int> GetProducerRegitrationFeesCount()
        {
            return await _producerFeesRepository.GetProducerRegitrationFeesCount();
        }
    }
}
