using EPR.Payment.Service.Common.Data.Interfaces;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.Fees;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using Microsoft.EntityFrameworkCore;

namespace EPR.Payment.Service.Common.Data.Repositories.Fees
{
    public class ReprocessorOrExporterFeeRepository(IAppDbContext dataContext) : IReprocessorOrExporterFeeRepository
    {
        public async Task<DataModels.Lookups.RegistrationFees?> GetFeeAsync(int groupId, int subgroupId, RegulatorType regulator, DateTime submissionDate, CancellationToken cancellationToken)
        {
            return await dataContext.RegistrationFees
                .Where(r => 
                    r.GroupId == groupId &&
                    r.SubGroupId == subgroupId &&
                    r.Regulator.Type.Equals(regulator.Value, StringComparison.CurrentCultureIgnoreCase) &&
                    r.EffectiveFrom <= submissionDate &&
                    r.EffectiveTo > submissionDate)
               .SingleOrDefaultAsync(cancellationToken);
        }
    }
}
