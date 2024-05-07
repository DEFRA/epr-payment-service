using AutoMapper;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories;
using EPR.Payment.Service.Common.Dtos.Responses;
using Microsoft.EntityFrameworkCore;

namespace EPR.Payment.Service.Common.Data.Repositories
{
    public class AccreditationFeesRepository : IAccreditationFeesRepository
    {
        private IMapper _mapper;
        private readonly FeePaymentDataContext _feePaymentDataContext;

        public AccreditationFeesRepository(IMapper mapper, FeePaymentDataContext feePaymentDataContext)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _feePaymentDataContext = feePaymentDataContext;
        }

        public async Task<List<GetAccreditationFeesResponse>> GetAllFeesAsync()
        {
            return await _feePaymentDataContext.AccreditationFees
                .Select(i => _mapper.Map<GetAccreditationFeesResponse>(i))
                .ToListAsync();
        }

        public async Task<decimal?> GetFeesAmountAsync(bool isLarge, string regulator)
        {
            return await _feePaymentDataContext.AccreditationFees
                .Where(i => i.Large == isLarge && i.Regulator == regulator)
                .Select(i => (decimal?)i.Amount)
                .FirstOrDefaultAsync();
        }

        public async Task<GetAccreditationFeesResponse?> GetFeesAsync(bool isLarge, string regulator)
        {
            return await _feePaymentDataContext.AccreditationFees
                .Where(i => i.Large == isLarge && i.Regulator == regulator)
                .Select(i => _mapper.Map<GetAccreditationFeesResponse>(i))
                .FirstOrDefaultAsync();
        }

        public async Task<int> GetFeesCount()
        {
            return await _feePaymentDataContext.AccreditationFees.CountAsync();
        }
    }
}