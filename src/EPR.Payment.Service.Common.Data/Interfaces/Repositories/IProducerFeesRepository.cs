using EPR.Payment.Service.Common.Dtos.Requests;
using EPR.Payment.Service.Common.Dtos.Responses;

namespace EPR.Payment.Service.Common.Data.Interfaces.Repositories
{
    public interface IProducerFeesRepository
    {
        Task<decimal?> GetProducerFeesAmountAsync(ProducerRegistrationRequestDto request);
        Task<decimal?> GetProducerSubsFeesAmountAsync(ProducerRegistrationRequestDto request);
        Task<int> GetProducerRegitrationFeesCount();
    }
}