using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.Data.Extensions;
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

        public async Task<AccreditationFee?> GetFeeAsync(
            int groupId,
            int subGroupId,
            int tonnageBandId, 
            RegulatorType regulator, 
            DateTime submissionDate, 
            CancellationToken cancellationToken)
        {
            return await _dataContext.AccreditationFees
                .Where(r => r.GroupId == groupId &&
                r.SubGroupId == subGroupId &&                
                r.Regulator.Type.ToLower().Equals(regulator.Value.ToLower()) &&
                r.TonnageBandId == tonnageBandId &&
                submissionDate >= r.EffectiveFrom && submissionDate <= r.EffectiveTo).FirstOrDefaultAsync(cancellationToken);
        }
    }
}