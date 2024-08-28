using EPR.Payment.Service.Common.Constants.RegistrationFees;
using EPR.Payment.Service.Common.Data.Interfaces;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
using Microsoft.EntityFrameworkCore;

namespace EPR.Payment.Service.Common.Data.Repositories.RegistrationFees
{
    public class ProducerFeesRepository : IProducerFeesRepository
    {
        private readonly IAppDbContext _dataContext;

        public ProducerFeesRepository(IAppDbContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<decimal> GetBaseFeeAsync(string producerType, string regulator, CancellationToken cancellationToken)
        {
            var currentDate = DateTime.UtcNow;

            var fee = await _dataContext.RegistrationFees
                .Where(r => r.Group.Type == GroupTypeConstants.ProducerType
                            && r.SubGroup.Description.ToLower() == producerType.ToLower()
                            && r.Regulator.Description == regulator
                            && r.EffectiveFrom <= currentDate
                            && r.EffectiveTo >= currentDate)
                .OrderByDescending(r => r.EffectiveFrom)  // Ensure the most recent EffectiveFrom is selected
                .Select(r => r.Amount)
                .FirstOrDefaultAsync(cancellationToken);

            if (fee == 0)
                throw new KeyNotFoundException(string.Format(ProducerFeesRepositoryConstants.InvalidProducerTypeOrRegulatorError, producerType, regulator));

            return fee;
        }

        public async Task<decimal> GetFirst20SubsidiariesFeeAsync(string regulator, CancellationToken cancellationToken)
        {
            var currentDate = DateTime.UtcNow;

            var fee = await _dataContext.RegistrationFees
                .Where(r => r.Group.Type == GroupTypeConstants.ProducerSubsidiaries
                            && r.SubGroup.Description == SubsidiariesConstants.UpTo20
                            && r.Regulator.Description == regulator
                            && r.EffectiveFrom <= currentDate
                            && r.EffectiveTo >= currentDate)
                .OrderByDescending(r => r.EffectiveFrom) // Ensure the most recent EffectiveFrom is selected
                .Select(r => r.Amount)
                .FirstOrDefaultAsync(cancellationToken);

            if (fee == 0)
                throw new KeyNotFoundException(string.Format(ProducerFeesRepositoryConstants.InvalidSubsidiariesFeeOrRegulatorError, SubsidiariesConstants.UpTo20, regulator));

            return fee;
        }

        public async Task<decimal> GetAdditionalSubsidiariesFeeAsync(string regulator, CancellationToken cancellationToken)
        {
            var currentDate = DateTime.UtcNow;

            var fee = await _dataContext.RegistrationFees
                .Where(r => r.Group.Type == GroupTypeConstants.ProducerSubsidiaries
                            && r.SubGroup.Description == SubsidiariesConstants.MoreThan20
                            && r.Regulator.Description == regulator
                            && r.EffectiveFrom <= currentDate
                            && r.EffectiveTo >= currentDate)
                .OrderByDescending(r => r.EffectiveFrom) // Ensure the most recent EffectiveFrom is selected
                .Select(r => r.Amount)
                .FirstOrDefaultAsync(cancellationToken);

            if (fee == 0)
                throw new KeyNotFoundException(string.Format(ProducerFeesRepositoryConstants.InvalidSubsidiariesFeeOrRegulatorError, SubsidiariesConstants.MoreThan20, regulator));

            return fee;
        }

    }
}
