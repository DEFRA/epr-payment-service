using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.Dtos.Responses;

namespace EPR.Payment.Service.Common.Data.Interfaces.Repositories
{
    public interface IAccreditationFeesRepository
    {
        Task<List<AccreditationFees>> GetAllFeesAsync();

        Task<decimal?> GetFeesAmountAsync(bool isLarge, string regulator);

        Task<AccreditationFees?> GetFeesAsync(bool isLarge, string regulator);

        Task<int> GetFeesCount();
    }
}