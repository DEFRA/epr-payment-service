using EPR.Payment.Service.Common.Dtos.Request.AccreditationFees;
using EPR.Payment.Service.Common.Dtos.Response.AccreditationFees;

namespace EPR.Payment.Service.Services.Interfaces.AccreditationFees
{
    public interface IAccreditationFeesCalculatorService
    {
        Task<AccreditationFeesResponseDto> CalculateFeesAsync(AccreditationFeesRequestDto request, CancellationToken cancellationToken);
    }
}