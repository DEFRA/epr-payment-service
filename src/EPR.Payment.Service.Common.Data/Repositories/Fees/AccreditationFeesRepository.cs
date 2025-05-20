using EPR.Payment.Service.Common.Constants.RegistrationFees;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.Data.Interfaces;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.Fees;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using Microsoft.EntityFrameworkCore;

namespace EPR.Payment.Service.Common.Data.Repositories.Fees
{
    public class AccreditationFeesRepository : IAccreditationFeesRepository
    {
        private readonly IAppDbContext _dataContext;
        
        public AccreditationFeesRepository(IAppDbContext dataContext)
        {
            _dataContext = dataContext;
        }        

        public async Task<AccreditationFee?> GetFeeAsync(string groupType, string subGroupType, int tonnesOver, int tonnesUpto, RegulatorType regulator, DateTime submissionDate, CancellationToken cancellationToken)
        {
            return await _dataContext.AccreditationFees
                .Where(r => r.Group.Type.ToLower() == groupType.ToLower() &&
                r.SubGroup.Type.ToLower() == subGroupType.ToLower() &&
                r.Regulator.Type.ToLower() == regulator.Value.ToLower() &&
                r.TonnesOver == tonnesOver && r.TonnesUpTo == tonnesUpto &&
                submissionDate >= r.EffectiveFrom && submissionDate <= r.EffectiveTo).FirstOrDefaultAsync(cancellationToken);
        }
    }
}