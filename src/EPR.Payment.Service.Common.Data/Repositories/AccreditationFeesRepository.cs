using AutoMapper;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories;
using EPR.Payment.Service.Common.Dtos.Responses;
using Microsoft.EntityFrameworkCore;

namespace EPR.Payment.Service.Common.Data.Repositories
{
    public class AccreditationFeesRepository : IAccreditationFeesRepository
    {
        private readonly FeesPaymentDataContext _feePaymentDataContext;

        public AccreditationFeesRepository(FeesPaymentDataContext feePaymentDataContext)
        {
            _feePaymentDataContext = feePaymentDataContext;
        }

        public async Task<List<AccreditationFees>> GetAllFeesAsync()
        {
            return await _feePaymentDataContext.AccreditationFees
                .ToListAsync();
        }

        public async Task<decimal?> GetFeesAmountAsync(bool isLarge, string regulator)
        {
            return await _feePaymentDataContext.AccreditationFees
                .Where(i => i.Large == isLarge && i.Regulator == regulator)
                .Select(i => (decimal?)i.Amount)
                .FirstOrDefaultAsync();
        }

        public async Task<AccreditationFees?> GetFeesAsync(bool isLarge, string regulator)
        {
            return await _feePaymentDataContext.AccreditationFees
                .Where(i => i.Large == isLarge && i.Regulator == regulator)
                .FirstOrDefaultAsync();
        }

        public async Task<int> GetFeesCount()
        {
            return await _feePaymentDataContext.AccreditationFees.CountAsync();
        }
    }
}