using EPR.Payment.Service.Common.Constants.RegistrationFees;
using EPR.Payment.Service.Common.Data.Interfaces;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
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

        public async Task<decimal> GetBaseFeeAsync(string producer, RegulatorType regulator, CancellationToken cancellationToken)
        {
            var currentDate = DateTime.UtcNow;

            var fee = await _dataContext.RegistrationFees
                .Where(r => r.Group.Type.ToLower() == GroupTypeConstants.ProducerType.ToLower() &&
                            r.SubGroup.Type.ToLower() == producer.ToLower() &&
                            r.Regulator.Type.ToLower() == regulator.Value.ToLower() &&
                            r.EffectiveFrom <= currentDate &&
                            r.EffectiveTo >= currentDate)
                .OrderByDescending(r => r.EffectiveFrom) // Ensure the most recent EffectiveFrom is selected
                .Select(r => r.Amount)
                .FirstOrDefaultAsync(cancellationToken);

            if (fee == 0)
                throw new KeyNotFoundException(string.Format(ProducerFeesRepositoryConstants.InvalidProducerTypeOrRegulatorError, producer, regulator.Value));

            return fee;
        }

        public async Task<decimal> GetFirst20SubsidiariesFeeAsync(RegulatorType regulator, CancellationToken cancellationToken)
        {
            var currentDate = DateTime.UtcNow;

            var fee = await _dataContext.RegistrationFees
                .Where(r => r.Group.Type.ToLower() == GroupTypeConstants.ProducerSubsidiaries.ToLower() &&
                            r.SubGroup.Type.ToLower() == SubsidiariesConstants.UpTo20.ToLower() &&
                            r.Regulator.Type.ToLower() == regulator.Value.ToLower() &&
                            r.EffectiveFrom <= currentDate &&
                            r.EffectiveTo >= currentDate)
                .OrderByDescending(r => r.EffectiveFrom) // Ensure the most recent EffectiveFrom is selected
                .Select(r => r.Amount)
                .FirstOrDefaultAsync(cancellationToken);

            if (fee == 0)
                throw new KeyNotFoundException(string.Format(ProducerFeesRepositoryConstants.InvalidSubsidiariesFeeOrRegulatorError, SubsidiariesConstants.UpTo20, regulator.Value));

            return fee;
        }

        public async Task<decimal> GetAdditionalSubsidiariesFeeAsync(RegulatorType regulator, CancellationToken cancellationToken)
        {
            var currentDate = DateTime.UtcNow;

            var fee = await _dataContext.RegistrationFees
                .Where(r => r.Group.Type.ToLower() == GroupTypeConstants.ProducerSubsidiaries.ToLower() &&
                            r.SubGroup.Type.ToLower() == SubsidiariesConstants.MoreThan20.ToLower() &&
                            r.Regulator.Type.ToLower() == regulator.Value.ToLower() &&
                            r.EffectiveFrom <= currentDate &&
                            r.EffectiveTo >= currentDate)
                .OrderByDescending(r => r.EffectiveFrom) // Ensure the most recent EffectiveFrom is selected
                .Select(r => r.Amount)
                .FirstOrDefaultAsync(cancellationToken);

            if (fee == 0)
                throw new KeyNotFoundException(string.Format(ProducerFeesRepositoryConstants.InvalidSubsidiariesFeeOrRegulatorError, SubsidiariesConstants.MoreThan20, regulator.Value));

            return fee;
        }

        public async Task<decimal?> GetProducerResubmissionAmountByRegulatorAsync(string regulator, CancellationToken cancellationToken)
        {
            var fee = await _dataContext.RegistrationFees.
                Where(a =>
                          a.Group.Type.ToLower() == GroupTypeConstants.ProducerResubmission.ToLower() &&
                          a.SubGroup.Type.ToLower() == ProducerResubmissionConstants.ReSubmitting.ToLower() &&
                          a.Regulator.Type.ToLower() == regulator.ToLower())
                .OrderByDescending(r => r.EffectiveFrom) 
                .Select(r => r.Amount)
                .FirstOrDefaultAsync(cancellationToken);

            if (fee == 0)
            {
                throw new KeyNotFoundException($"{ProducerResubmissionConstants.RecordNotFoundProducerResubmissionFeeError}: {regulator}");
            }

            return fee;
        }
    }
}
