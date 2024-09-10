using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees;

namespace EPR.Payment.Service.Services.Interfaces.RegistrationFees.Producer
{
    public interface IProducerFeesCalculatorService
    {
        Task<RegistrationFeesResponseDto> CalculateFeesAsync(ProducerRegistrationFeesRequestDto request, CancellationToken cancellationToken);
    }
}
