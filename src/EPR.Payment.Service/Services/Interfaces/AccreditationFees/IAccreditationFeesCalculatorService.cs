using EPR.Payment.Service.Common.Dtos.Request.AccreditationFees;
using EPR.Payment.Service.Common.Dtos.Response.AccreditationFees;

namespace EPR.Payment.Service.Services.Interfaces.AccreditationFees
{
    public interface IAccreditationFeesCalculatorService
    {
        Task<ReprocessorOrExporterAccreditationFeesResponseDto?> CalculateFeesAsync(ReprocessorOrExporterAccreditationFeesRequestDto request, CancellationToken cancellationToken);
    }
}