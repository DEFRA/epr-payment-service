using EPR.Payment.Service.Common.Dtos.Responses;

namespace EPR.Payment.Service.Common.Data.Interfaces.Repositories
{
    public interface IAccreditationFeesRepository
    {
        Task<List<GetAccreditationFeesResponse>> GetAllFeesAsync();

        Task<decimal?> GetFeesAmountAsync(bool isLarge, string regulator);

        Task<GetAccreditationFeesResponse?> GetFeesAsync(bool isLarge, string regulator);

        Task<int> GetFeesCount();
    }
}