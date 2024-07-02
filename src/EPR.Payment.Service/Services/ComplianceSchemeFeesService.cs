using EPR.Payment.Service.Common.Data.Interfaces.Repositories;
using EPR.Payment.Service.Common.Data.Repositories;
using EPR.Payment.Service.Common.Dtos.Requests;
using EPR.Payment.Service.Common.Dtos.Responses;
using EPR.Payment.Service.Services.Interfaces;

namespace EPR.Payment.Service.Services
{
    public class ComplianceSchemeFeesService : IComplianceSchemeFeesService
    {
        private readonly IComplianceSchemeFeesRepository _feesRepository;
        public ComplianceSchemeFeesService(IComplianceSchemeFeesRepository feesRepository)
        {
            _feesRepository = feesRepository;
        }

        public async Task<RegistrationFeeResponseDto?> CalculateFeesAsync(ComplianceSchemeRegistrationRequestDto request)
        {
            return await _feesRepository.CalculateFeesAsync(request);
        }
    }
}
