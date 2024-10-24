using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Common.Constants.RegistrationFees.LookUps;
using EPR.Payment.Service.Common.Data.Interfaces;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;

namespace EPR.Payment.Service.Common.Data.Repositories.RegistrationFees
{
    public class ProducerFeesRepository : BaseFeeRepository, IProducerFeesRepository
    {
        public ProducerFeesRepository(IAppDbContext dataContext) : base(dataContext) { }

        public async Task<decimal> GetBaseFeeAsync(string producer, RegulatorType regulator, DateTime submissionDate, CancellationToken cancellationToken)
        {
            var fee = await GetFeeAsync(GroupTypeConstants.ProducerType, producer, regulator, submissionDate, cancellationToken);
            ValidateFee(fee, string.Format(ProducerFeesRepositoryConstants.InvalidProducerTypeOrRegulatorError, producer, regulator.Value));
            return fee;
        }

        public async Task<decimal> GetFirstBandFeeAsync(RegulatorType regulator, DateTime submissionDate, CancellationToken cancellationToken)
        {
            var fee = await GetFeeAsync(GroupTypeConstants.ProducerSubsidiaries, SubsidiariesConstants.UpTo20, regulator, submissionDate, cancellationToken);
            ValidateFee(fee, string.Format(ProducerFeesRepositoryConstants.InvalidSubsidiariesFeeOrRegulatorError, SubsidiariesConstants.UpTo20, regulator.Value));
            return fee;
        }

        public async Task<decimal> GetSecondBandFeeAsync(RegulatorType regulator, DateTime submissionDate, CancellationToken cancellationToken)
        {
            var fee = await GetFeeAsync(GroupTypeConstants.ProducerSubsidiaries, SubsidiariesConstants.MoreThan20, regulator, submissionDate, cancellationToken);
            ValidateFee(fee, string.Format(ProducerFeesRepositoryConstants.InvalidSubsidiariesFeeOrRegulatorError, SubsidiariesConstants.MoreThan20, regulator.Value));
            return fee;
        }

        public async Task<decimal> GetThirdBandFeeAsync(RegulatorType regulator, DateTime submissionDate, CancellationToken cancellationToken)
        {
            return await GetFeeAsync(GroupTypeConstants.ProducerSubsidiaries, SubsidiariesConstants.MoreThan100, regulator, submissionDate, cancellationToken);
        }

        public async Task<decimal> GetOnlineMarketFeeAsync(RegulatorType regulator, DateTime submissionDate, CancellationToken cancellationToken)
        {
            var fee = await GetFeeAsync(GroupTypeConstants.ProducerType, SubGroupTypeConstants.OnlineMarket, regulator, submissionDate, cancellationToken);
            ValidateFee(fee, string.Format(ProducerFeesRepositoryConstants.InvalidOnlineMarketRegulatorError, regulator.Value));
            return fee;
        }

        public async Task<decimal> GetLateFeeAsync(RegulatorType regulator, DateTime submissionDate, CancellationToken cancellationToken)
        {
            var fee = await GetFeeAsync(GroupTypeConstants.ProducerType, SubGroupTypeConstants.LateFee, regulator, submissionDate, cancellationToken);
            ValidateFee(fee, string.Format(ProducerFeesRepositoryConstants.InvalidLateFeeError, regulator.Value));
            return fee;
        }

        public async Task<decimal> GetResubmissionAsync(RegulatorType regulator, CancellationToken cancellationToken)
        {
            var fee = await GetFeeAsync(GroupTypeConstants.ProducerResubmission, SubGroupTypeConstants.ReSubmitting, regulator, DateTime.Now, cancellationToken);
            ValidateFee(fee, string.Format(ProducerResubmissionExceptions.RecordNotFoundProducerResubmissionFeeError, regulator.Value));
            return fee;
        }
    }
}
