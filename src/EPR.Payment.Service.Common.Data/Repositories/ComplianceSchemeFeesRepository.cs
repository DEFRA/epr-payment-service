using EPR.Payment.Service.Common.Data.Interfaces.Repositories;
using EPR.Payment.Service.Common.Dtos.Requests;
using EPR.Payment.Service.Common.Dtos.Responses;

namespace EPR.Payment.Service.Common.Data.Repositories
{
    public class ComplianceSchemeFeesRepository : IComplianceSchemeFeesRepository
    {
        private readonly FeesPaymentDataContext _feePaymentDataContext;
        public ComplianceSchemeFeesRepository(FeesPaymentDataContext feePaymentDataContext)
        {
            _feePaymentDataContext = feePaymentDataContext;
        }
        public Task<RegistrationFeeResponseDto?> CalculateFeesAsync(ComplianceSchemeRegistrationRequestDto request)
        {
            throw new NotImplementedException();
        }
    }
}
