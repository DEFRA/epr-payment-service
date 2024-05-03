using EPR.Payment.Service.Common.Data.Interfaces.Repositories;
using EPR.Payment.Service.Common.Dtos.Responses;
using EPR.Payment.Service.Services.Interfaces;

namespace EPR.Payment.Service.Services
{
    public class FeesService : IFeesService
    {
        private readonly IFeesRepository _feesRepository;

        public FeesService(IFeesRepository feesRepository)
        {
            _feesRepository = feesRepository ?? throw new ArgumentNullException(nameof(feesRepository)); ;
        }

        public async Task<GetFeesResponse?> GetFees(bool isLarge, string regulator)
        {
            return await _feesRepository.GetFeesAsync(isLarge, regulator);
        }

        public async Task<decimal?> GetFeesAmount(bool isLarge, string regulator)
        {
            return await _feesRepository.GetFeesAmountAsync(isLarge, regulator);
        }

        public async Task<int> GetFeesCount()
        {
            return await _feesRepository.GetFeesCount();
        }

    }
}
