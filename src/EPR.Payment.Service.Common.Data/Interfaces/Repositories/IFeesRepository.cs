using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.Dtos.Responses;

namespace EPR.Payment.Service.Common.Data.Interfaces.Repositories
{
    public interface IFeesRepository
    {
        Task<List<GetFeesResponse>> GetAllFeesAsync();

        Task<decimal?> GetFeesAmountAsync(bool isLarge, string regulator);

        Task<GetFeesResponse?> GetFeesAsync(bool isLarge, string regulator);
    }
}