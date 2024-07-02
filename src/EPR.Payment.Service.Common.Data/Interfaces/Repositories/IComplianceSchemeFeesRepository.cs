using EPR.Payment.Service.Common.Dtos.Requests;
using EPR.Payment.Service.Common.Dtos.Responses;

namespace EPR.Payment.Service.Common.Data.Interfaces.Repositories
{
    public interface IComplianceSchemeFeesRepository
    {
        Task<RegistrationFeeResponseDto?> CalculateFeesAsync(ComplianceSchemeRegistrationRequestDto request);
    }
}
