using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees.ComplianceScheme;

namespace EPR.Payment.Service.Strategies.RegistrationFees.ComplianceScheme
{
    public class CSLateSubsidiariesFeeCalculationStrategy : ICSLateSubsidiariesFeeCalculationStrategy<ComplianceSchemeMemberWithRegulatorDto, decimal>
    {
        private readonly IComplianceSchemeFeesRepository _feesRepository;

        public CSLateSubsidiariesFeeCalculationStrategy(IComplianceSchemeFeesRepository feesRepository)
        {
            _feesRepository = feesRepository ?? throw new ArgumentNullException(nameof(feesRepository));
        }

        public async Task<decimal> CalculateFeeAsync(ComplianceSchemeMemberWithRegulatorDto request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            if (request.IsLateFeeApplicable)
                return 0m;

            var lateSubsidiaryCount = request.NumberOfLateSubsidiaries;
            if (lateSubsidiaryCount <= 0)
                return 0m;
                      

            var perSubsidiaryLateFee = await _feesRepository.GetLateFeeAsync(request.Regulator, request.SubmissionDate, cancellationToken).ConfigureAwait(false); 
            return perSubsidiaryLateFee * lateSubsidiaryCount;

        }
    }
}
