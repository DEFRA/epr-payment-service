using EPR.Payment.Service.Common.Constants.RegistrationFees;
using EPR.Payment.Service.Common.Constants.RegistrationFees.LookUps;
using EPR.Payment.Service.Common.Data.Helper;
using EPR.Payment.Service.Common.Data.Interfaces;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using Microsoft.EntityFrameworkCore;

namespace EPR.Payment.Service.Common.Data.Repositories.RegistrationFees
{
    public abstract class BaseFeeRepository
    {
        protected readonly IAppDbContext _dataContext;
        private readonly FeesKeyValueStore _keyValueStore;

        protected BaseFeeRepository(IAppDbContext dataContext, FeesKeyValueStore keyValueStore)
        {
            _dataContext = dataContext;
            _keyValueStore = keyValueStore;
        }

        protected async Task<decimal> GetFeeAsync(
            string groupType,
            string subGroupType,
            RegulatorType regulator,
            DateTime submissionDate,
            CancellationToken cancellationToken)
        {
            decimal fee = 0;
            string inMemoryKey = GetInMemoryKey(groupType, subGroupType, regulator);
            var cachedFee = _keyValueStore.Get(inMemoryKey);
            if (cachedFee != null)
            {
                return (decimal)cachedFee;
            }

            var registrationFees = await _dataContext.RegistrationFees
                .Where(r => r.Group.Type.ToLower() == groupType.ToLower() &&
                r.SubGroup.Type.ToLower() == subGroupType.ToLower() &&
                r.Regulator.Type.ToLower() == regulator.Value.ToLower())
               .ToListAsync(cancellationToken);

            if (!registrationFees.Any())
            {
                _keyValueStore.Add(inMemoryKey, fee); 
                return fee;  
            }

            fee = registrationFees
                .Where(r => submissionDate >= r.EffectiveFrom && submissionDate <= r.EffectiveTo)
                .OrderByDescending(r => r.EffectiveFrom)  
                .Select(r => r.Amount)
                .FirstOrDefault();

            if (fee == 0)
            {
                throw new ArgumentException(subGroupType == SubGroupTypeConstants.ReSubmitting ? ValidationMessages.ResubmissionDateIsNotInRange : ValidationMessages.SubmissionDateIsNotInRange);
            }

            _keyValueStore.Add(inMemoryKey, fee);

            return fee;
        }

        protected static void ValidateFee(decimal fee, string errorMessage)
        {
            if (fee == 0)
            {
                throw new KeyNotFoundException(errorMessage);
            }
        }
        private static string GetInMemoryKey(string groupType, string subGroupType, RegulatorType regulator)
        {
            return string.Concat(groupType, subGroupType, regulator);
        }
    }
}
