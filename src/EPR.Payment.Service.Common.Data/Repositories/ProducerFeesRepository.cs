using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories;
using EPR.Payment.Service.Common.Dtos.Requests;
using EPR.Payment.Service.Common.Dtos.Responses;
using Microsoft.EntityFrameworkCore;

namespace EPR.Payment.Service.Common.Data.Repositories
{
    public class ProducerFeesRepository : IProducerFeesRepository
    {
        private readonly FeesPaymentDataContext _feePaymentDataContext;

        public ProducerFeesRepository(FeesPaymentDataContext feePaymentDataContext)
        {
            _feePaymentDataContext = feePaymentDataContext;
        }

        public async Task<decimal?> GetProducerFeesAmountAsync(ProducerRegistrationRequestDto request)
        {
            return await
                _feePaymentDataContext.ProducerRegitrationFees
                .Where(i => i.ProducerType == request.ProducerType && i.Country == request.Country)
                .Select(i => (decimal?)i.Amount).FirstOrDefaultAsync();
        }

        public async Task<decimal?> GetProducerSubsFeesAmountAsync(ProducerRegistrationRequestDto request)
        {
            return await
                _feePaymentDataContext.Subsidiaries
                    .Where(j => j.MinSub <= request.NumberOfSubsidiaries && j.MaxSub >= request.NumberOfSubsidiaries && j.Country == request.Country)
                    .Select(i => (decimal?)i.Amount).FirstOrDefaultAsync();
        }

        public async Task<int> GetProducerRegitrationFeesCount()
        {
            return await _feePaymentDataContext.ProducerRegitrationFees.CountAsync();
        }
    }
}