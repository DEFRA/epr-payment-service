using EPR.Payment.Service.Common.Dtos.Requests;
using EPR.Payment.Service.Common.Dtos.Responses;

namespace EPR.Payment.Service.Services.Interfaces
{
    public interface IProducerFeesService
    {
        Task<RegistrationFeeResponseDto> CalculateFeesAsync(ProducerRegistrationRequestDto request);
        Task<int> GetProducerRegitrationFeesCount();
    }
}