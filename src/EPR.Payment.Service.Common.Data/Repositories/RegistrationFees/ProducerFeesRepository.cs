using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Common.Constants.RegistrationFees.LookUps;
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
            var currentDate = DateTime.UtcNow.Date; // Only the date part, time is set to 00:00:00

            var fee = await _dataContext.RegistrationFees
                .Where(r => r.Group.Type.ToLower() == GroupTypeConstants.ProducerType.ToLower() &&
                            r.SubGroup.Type.ToLower() == producer.ToLower() &&
                            r.Regulator.Type.ToLower() == regulator.Value.ToLower() &&
                            r.EffectiveFrom.Date <= currentDate &&
                            r.EffectiveTo.Date >= currentDate)
                .OrderByDescending(r => r.EffectiveFrom) // Ensure the most recent EffectiveFrom is selected
                .Select(r => r.Amount)
                .FirstOrDefaultAsync(cancellationToken);

            if (fee == 0)
                throw new KeyNotFoundException(string.Format(ProducerFeesRepositoryConstants.InvalidProducerTypeOrRegulatorError, producer, regulator.Value));

            return fee;
        }

        public async Task<decimal> GetFirst20SubsidiariesFeeAsync(RegulatorType regulator, CancellationToken cancellationToken)
        {
            var currentDate = DateTime.UtcNow.Date; // Only the date part, time is set to 00:00:00

            var fee = await _dataContext.RegistrationFees
                .Where(r => r.Group.Type.ToLower() == GroupTypeConstants.ProducerSubsidiaries.ToLower() &&
                            r.SubGroup.Type.ToLower() == SubsidiariesConstants.UpTo20.ToLower() &&
                            r.Regulator.Type.ToLower() == regulator.Value.ToLower() &&
                            r.EffectiveFrom.Date <= currentDate &&
                            r.EffectiveTo.Date >= currentDate)
                .OrderByDescending(r => r.EffectiveFrom) // Ensure the most recent EffectiveFrom is selected
                .Select(r => r.Amount)
                .FirstOrDefaultAsync(cancellationToken);

            if (fee == 0)
                throw new KeyNotFoundException(string.Format(ProducerFeesRepositoryConstants.InvalidSubsidiariesFeeOrRegulatorError, SubsidiariesConstants.UpTo20, regulator.Value));

            return fee;
        }

        public async Task<decimal> GetAdditionalUpTo100SubsidiariesFeeAsync(RegulatorType regulator, CancellationToken cancellationToken)
        {
            var currentDate = DateTime.UtcNow.Date; // Only the date part, time is set to 00:00:00

            var fee = await _dataContext.RegistrationFees
                .Where(r => r.Group.Type.ToLower() == GroupTypeConstants.ProducerSubsidiaries.ToLower() &&
                            r.SubGroup.Type.ToLower() == SubsidiariesConstants.MoreThan20.ToLower() &&
                            r.Regulator.Type.ToLower() == regulator.Value.ToLower() &&
                            r.EffectiveFrom.Date <= currentDate &&
                            r.EffectiveTo.Date >= currentDate)
                .OrderByDescending(r => r.EffectiveFrom) // Ensure the most recent EffectiveFrom is selected
                .Select(r => r.Amount)
                .FirstOrDefaultAsync(cancellationToken);

            if (fee == 0)
                throw new KeyNotFoundException(string.Format(ProducerFeesRepositoryConstants.InvalidSubsidiariesFeeOrRegulatorError, SubsidiariesConstants.MoreThan20, regulator.Value));

            return fee;
        }

        public async Task<decimal> GetAdditionalMoreThan100SubsidiariesFeeAsync(RegulatorType regulator, CancellationToken cancellationToken)
        {
            var currentDate = DateTime.UtcNow.Date; // Only the date part, time is set to 00:00:00

            var fee = await _dataContext.RegistrationFees
                .Where(r => r.Group.Type.ToLower() == GroupTypeConstants.ProducerSubsidiaries.ToLower() &&
                            r.SubGroup.Type.ToLower() == SubsidiariesConstants.MoreThan100.ToLower() &&
                            r.Regulator.Type.ToLower() == regulator.Value.ToLower() &&
                            r.EffectiveFrom.Date <= currentDate &&
                            r.EffectiveTo.Date >= currentDate)
                .OrderByDescending(r => r.EffectiveFrom) // Ensure the most recent EffectiveFrom is selected
                .Select(r => r.Amount)
                .FirstOrDefaultAsync(cancellationToken);

            return fee;//return even if zero
        }

        public async Task<decimal> GetOnlineMarketFeeAsync(RegulatorType regulator, CancellationToken cancellationToken)
        {
            var currentDate = DateTime.UtcNow.Date; // Only the date part, time is set to 00:00:00

            var fee = await _dataContext.RegistrationFees
                .Where(r => r.Group.Type.ToLower() == GroupTypeConstants.ProducerType.ToLower() &&
                            r.SubGroup.Type.ToLower() == SubGroupTypeConstants.OnlineMarket.ToLower() &&
                            r.Regulator.Type.ToLower() == regulator.Value.ToLower() &&
                            r.EffectiveFrom.Date <= currentDate &&
                            r.EffectiveTo.Date >= currentDate)
                .OrderByDescending(r => r.EffectiveFrom) // Ensure the most recent EffectiveFrom is selected
                .Select(r => r.Amount)
                .FirstOrDefaultAsync(cancellationToken);

            if (fee == 0)
                throw new KeyNotFoundException($"{ProducerFeesRepositoryConstants.InvalidOnlineMarketRegulatorError}: {regulator}");

            return fee;
        }

        public async Task<decimal> GetLateFeeAsync(RegulatorType regulator, CancellationToken cancellationToken)
        {
            var currentDate = DateTime.UtcNow.Date; // Only the date part, time is set to 00:00:00

            var fee = await _dataContext.RegistrationFees
                .Where(r => r.Group.Type.ToLower() == GroupTypeConstants.ProducerType.ToLower() &&
                            r.SubGroup.Type.ToLower() == SubGroupTypeConstants.LateFee.ToLower() &&
                            r.Regulator.Type.ToLower() == regulator.Value.ToLower() &&
                            r.EffectiveFrom.Date <= currentDate &&
                            r.EffectiveTo.Date >= currentDate)
                .OrderByDescending(r => r.EffectiveFrom) // Ensure the most recent EffectiveFrom is selected
                .Select(r => r.Amount)
                .FirstOrDefaultAsync(cancellationToken);

            if (fee == 0)
                throw new KeyNotFoundException($"{ProducerFeesRepositoryConstants.InvalidLateFeeError}: {regulator}");

            return fee;
        }

        public async Task<decimal> GetResubmissionAsync(RegulatorType regulator, CancellationToken cancellationToken)
        {
            var currentDate = DateTime.UtcNow.Date; // Only the date part, time is set to 00:00:00

            var fee = await _dataContext.RegistrationFees.
                Where(a =>
                          a.Group.Type.ToLower() == GroupTypeConstants.ProducerResubmission.ToLower() &&
                          a.SubGroup.Type.ToLower() == SubGroupTypeConstants.ReSubmitting.ToLower() &&
                          a.Regulator.Type.ToLower() == regulator.Value.ToLower() &&
                          a.EffectiveFrom.Date <= currentDate &&
                          a.EffectiveTo.Date >= currentDate)
                .OrderByDescending(r => r.EffectiveFrom)
                .Select(r => r.Amount)
                .FirstOrDefaultAsync(cancellationToken);

            if (fee == 0)
            {
                throw new KeyNotFoundException($"{ProducerResubmissionExceptions.RecordNotFoundProducerResubmissionFeeError}: {regulator}");
            }

            return fee;
        }
    }
}
