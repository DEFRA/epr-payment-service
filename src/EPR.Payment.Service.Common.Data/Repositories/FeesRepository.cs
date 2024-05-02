using AutoMapper;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories;
using EPR.Payment.Service.Common.Dtos.Responses;
using Microsoft.EntityFrameworkCore;

namespace EPR.Payment.Service.Common.Data.Repositories
{
    public class FeesRepository : IFeesRepository
    {
        private IMapper _mapper;
        private readonly FeePaymentDataContext _feePaymentDataContext;

        public FeesRepository(IMapper mapper, FeePaymentDataContext feePaymentDataContext)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _feePaymentDataContext = feePaymentDataContext;
        }

        public async Task<List<GetFeesResponse>> GetAllFeesAsync()
        {
            return await _feePaymentDataContext.Fees
                .Select(i => _mapper.Map<GetFeesResponse>(i))
                .ToListAsync();
        }

        public async Task<decimal?> GetFeesAmountAsync(bool isLarge, string regulator)
        {
            return await _feePaymentDataContext.Fees
                .Where(i => i.Large == isLarge && i.Regulator == regulator)
                .Select(i => (decimal?)i.Amount)
                .FirstOrDefaultAsync();
        }

        public async Task<GetFeesResponse?> GetFeesAsync(bool isLarge, string regulator)
        {
            return await _feePaymentDataContext.Fees
                .Where(i => i.Large == isLarge && i.Regulator == regulator)
                .Select(i => _mapper.Map<GetFeesResponse>(i))
                .FirstOrDefaultAsync();
        }
    }
}