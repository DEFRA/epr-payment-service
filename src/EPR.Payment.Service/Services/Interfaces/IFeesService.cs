using EPR.Payment.Service.Common.Dtos.Responses;

namespace EPR.Payment.Service.Services.Interfaces
{
    public interface IFeesService
    {
        Task<GetFeesResponse?> GetFees(bool isLarge, string regulator);
        Task<decimal?> GetFeesAmount(bool isLarge, string regulator);
    }
}
