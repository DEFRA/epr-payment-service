using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Common.Constants.RegistrationFees.LookUps;
using EPR.Payment.Service.Common.Data.Interfaces;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;

namespace EPR.Payment.Service.Common.Data.Repositories.RegistrationFees
{
    public class ComplianceSchemeFeesRepository : BaseFeeRepository, IComplianceSchemeFeesRepository
    {
        public ComplianceSchemeFeesRepository(IAppDbContext dataContext) : base(dataContext) { }

        public async Task<decimal> GetBaseFeeAsync(RegulatorType regulator, CancellationToken cancellationToken)
        {
            var fee = await GetFeeAsync(GroupTypeConstants.ComplianceScheme, ComplianceSchemeConstants.Registration, regulator, cancellationToken);
            ValidateFee(fee, string.Format(ComplianceSchemeFeeCalculationExceptions.InvalidComplianceSchemeOrRegulatorError, regulator.Value));
            return fee;
        }

        public async Task<decimal> GetMemberFeeAsync(string memberType, RegulatorType regulator, CancellationToken cancellationToken)
        {
            var fee = await GetFeeAsync(GroupTypeConstants.ComplianceScheme, memberType, regulator, cancellationToken);
            ValidateFee(fee, string.Format(ComplianceSchemeFeeCalculationExceptions.InvalidMemberTypeOrRegulatorError, memberType, regulator.Value));
            return fee;
        }

        public async Task<decimal> GetFirstBandFeeAsync(RegulatorType regulator, CancellationToken cancellationToken)
        {
            var fee = await GetFeeAsync(GroupTypeConstants.ComplianceSchemeSubsidiaries, SubsidiariesConstants.UpTo20, regulator, cancellationToken);
            ValidateFee(fee, string.Format(ComplianceSchemeFeeCalculationExceptions.InvalidSubsidiariesFeeOrRegulatorError, SubsidiariesConstants.UpTo20, regulator.Value));
            return fee;
        }

        public async Task<decimal> GetSecondBandFeeAsync(RegulatorType regulator, CancellationToken cancellationToken)
        {
            var fee = await GetFeeAsync(GroupTypeConstants.ComplianceSchemeSubsidiaries, SubsidiariesConstants.MoreThan20, regulator, cancellationToken);
            ValidateFee(fee, string.Format(ComplianceSchemeFeeCalculationExceptions.InvalidSubsidiariesFeeOrRegulatorError, SubsidiariesConstants.MoreThan20, regulator.Value));
            return fee;
        }

        public async Task<decimal> GetThirdBandFeeAsync(RegulatorType regulator, CancellationToken cancellationToken)
        {
            return await GetFeeAsync(GroupTypeConstants.ComplianceSchemeSubsidiaries, SubsidiariesConstants.MoreThan100, regulator, cancellationToken);
        }

        public async Task<decimal> GetOnlineMarketFeeAsync(RegulatorType regulator, CancellationToken cancellationToken)
        {
            var fee = await GetFeeAsync(GroupTypeConstants.ComplianceScheme, SubGroupTypeConstants.OnlineMarket, regulator, cancellationToken);
            ValidateFee(fee, string.Format(ComplianceSchemeFeeCalculationExceptions.InvalidOnlineMarketPlaceError, regulator.Value));
            return fee;
        }

        public async Task<decimal> GetLateFeeAsync(RegulatorType regulator, CancellationToken cancellationToken)
        {
            var fee = await GetFeeAsync(GroupTypeConstants.ComplianceScheme, SubGroupTypeConstants.LateFee, regulator, cancellationToken);
            ValidateFee(fee, string.Format(ComplianceSchemeFeeCalculationExceptions.InvalidLateFeeError, regulator.Value));
            return fee;
        }
    }
}
