using EPR.Payment.Service.Common.Dtos.Requests;
using EPR.Payment.Service.Common.Dtos.Responses;

namespace EPR.Payment.Service.Services.Interfaces
{
    public interface IComplianceSchemeFeesService
    {
        Task<RegistrationFeeResponseDto?> CalculateFeesAsync(ComplianceSchemeRegistrationRequestDto request);
    }
}
