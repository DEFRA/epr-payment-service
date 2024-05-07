using EPR.Payment.Service.Common.Dtos.Responses;

namespace EPR.Payment.Service.Services.Interfaces
{
    public interface IAccreditationFeesService
    {
        Task<GetAccreditationFeesResponse?> GetFees(bool isLarge, string regulator);

        Task<decimal?> GetFeesAmount(bool isLarge, string regulator);

        Task<int> GetFeesCount();
    }
}