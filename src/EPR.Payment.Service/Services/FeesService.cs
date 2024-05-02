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
            var fees = await _feesRepository.GetFeesAsync(isLarge, regulator);

            return fees;
        }

        public async Task<decimal?> GetFeesAmount(bool isLarge, string regulator)
        {
            var fees = await _feesRepository.GetFeesAmountAsync(isLarge, regulator);

            return fees;
        }

    }
}
