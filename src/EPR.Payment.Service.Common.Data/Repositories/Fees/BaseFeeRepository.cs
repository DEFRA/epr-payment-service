using EPR.Payment.Service.Common.Data.Interfaces;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using Microsoft.EntityFrameworkCore;

namespace EPR.Payment.Service.Common.Data.Repositories.RegistrationFees
{
    public abstract class BaseFeeRepository
    {
        protected readonly IAppDbContext _dataContext;

        protected BaseFeeRepository(IAppDbContext dataContext)
        {
            _dataContext = dataContext;
        }
        protected async Task<decimal> GetFeeAsync(string groupType, string subGroupType, RegulatorType regulator, DateTime? submissionDate, CancellationToken cancellationToken)
        {
            var checkingDate = submissionDate.HasValue ? submissionDate.Value : DateTime.UtcNow.Date;

            var fee = await _dataContext.RegistrationFees
                .Where(r => r.Group.Type.ToLower() == groupType.ToLower() &&
                            r.SubGroup.Type.ToLower() == subGroupType.ToLower() &&
                            r.Regulator.Type.ToLower() == regulator.Value.ToLower() &&
                            r.EffectiveFrom.Date <= checkingDate &&
                            r.EffectiveTo.Date >= checkingDate)
                .OrderByDescending(r => r.EffectiveFrom)
                .Select(r => r.Amount)
                .FirstOrDefaultAsync(cancellationToken);

            return fee;
        }

        protected static void ValidateFee(decimal fee, string errorMessage)
        {
            if (fee == 0)
            {
                throw new KeyNotFoundException(errorMessage);
            }
        }
    }
}
