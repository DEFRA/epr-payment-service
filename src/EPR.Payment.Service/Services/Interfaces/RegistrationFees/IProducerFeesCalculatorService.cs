using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees;

namespace EPR.Payment.Service.Services.Interfaces.RegistrationFees
{
    public interface IProducerFeesCalculatorService
    {
        Task<RegistrationFeesResponseDto> CalculateFeesAsync(ProducerRegistrationFeesRequestDto request, CancellationToken cancellationToken);
        Task<decimal?> GetProducerResubmissionAmountByRegulatorAsync(string regulator, CancellationToken cancellationToken);
    }
}
