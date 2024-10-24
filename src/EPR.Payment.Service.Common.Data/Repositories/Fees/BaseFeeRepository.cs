using EPR.Payment.Service.Common.Data.Interfaces;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using Microsoft.EntityFrameworkCore;
using EPR.Payment.Service.Common.Constants.RegistrationFees;

namespace EPR.Payment.Service.Common.Data.Repositories.RegistrationFees
{
    public abstract class BaseFeeRepository
    {
        protected readonly IAppDbContext _dataContext;

        protected BaseFeeRepository(IAppDbContext dataContext)
        {
            _dataContext = dataContext;
        }
        protected async Task<decimal> GetFeeAsync(string groupType, string subGroupType, RegulatorType regulator, DateTime submissionDate, CancellationToken cancellationToken)
        {
            var registrationFees = await _dataContext.RegistrationFees
                .Where(r => r.Group.Type.ToLower() == groupType.ToLower() &&
                r.SubGroup.Type.ToLower() == subGroupType.ToLower() &&
                r.Regulator.Type.ToLower() == regulator.Value.ToLower())
               .ToListAsync(cancellationToken);

            // Return 0 if no matching records based on groupType, subGroupType, and regulator
            if (registrationFees == null || registrationFees.Count == 0)
            {
                return 0;
            }

            // Now apply date filtering in memory
            var fee = registrationFees
                .Where(r => submissionDate >= r.EffectiveFrom && submissionDate <= r.EffectiveTo)
                .OrderByDescending(r => r.EffectiveFrom)
                .Select(r => r.Amount)
                .FirstOrDefault();

            if (fee == 0)
            {
                throw new ArgumentException(ValidationMessages.SubmissionDateIsNotInRange);
            }

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