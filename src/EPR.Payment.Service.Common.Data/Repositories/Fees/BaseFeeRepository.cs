using EPR.Payment.Service.Common.Constants.RegistrationFees;
using EPR.Payment.Service.Common.Constants.RegistrationFees.LookUps;
using EPR.Payment.Service.Common.Data.Extensions;
using EPR.Payment.Service.Common.Data.Helper;
using EPR.Payment.Service.Common.Data.Interfaces;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using Microsoft.EntityFrameworkCore;
using RegistrationFeesEntity = EPR.Payment.Service.Common.Data.DataModels.Lookups.RegistrationFees;

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
            string inMemoryKey = GetInMemoryKey(groupType, subGroupType, regulator);

            List<RegistrationFeesEntity> registrationFees;
            if (_keyValueStore.Get(inMemoryKey) is List<RegistrationFeesEntity> cachedRows)
            {
                registrationFees = cachedRows;
            }
            else
            {
                registrationFees = await _dataContext.RegistrationFees
                    .Where(r => r.Group.Type.ToLower().Equals(groupType.ToLower()) &&
                                r.SubGroup.Type.ToLower().Equals(subGroupType.ToLower()) &&
                                r.Regulator.Type.ToLower().Equals(regulator.Value.ToLower()))
                    .ToListAsync(cancellationToken);
                _keyValueStore.Add(inMemoryKey, registrationFees);
            }

            if (registrationFees.Count == 0)
            {
                return 0;
            }

            var submissionDateOnly = submissionDate.Date;
            var fee = registrationFees
                .Where(r => submissionDateOnly >= r.EffectiveFrom.Date && submissionDateOnly <= r.EffectiveTo.Date)
                .OrderByDescending(r => r.EffectiveFrom)
                .Select(r => r.Amount)
                .FirstOrDefault();

            if (fee == 0)
            {
                throw new ArgumentException(subGroupType == SubGroupTypeConstants.ReSubmitting ? ValidationMessages.ResubmissionDateIsNotInRange : ValidationMessages.SubmissionDateIsNotInRange);
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
        private static string GetInMemoryKey(string groupType, string subGroupType, RegulatorType regulator)
        {
            return string.Concat(groupType, subGroupType, regulator);
        }
    }
}