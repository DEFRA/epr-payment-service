using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees.ComplianceScheme;

namespace EPR.Payment.Service.Strategies.RegistrationFees.ComplianceScheme
{
    public class CSLateFeeCalculationStrategy : ICSLateFeeCalculationStrategy<ComplianceSchemeLateFeeRequestDto, decimal>
    {
        private readonly IComplianceSchemeFeesRepository _feesRepository;

        public CSLateFeeCalculationStrategy(IComplianceSchemeFeesRepository feesRepository)
        {
            _feesRepository = feesRepository ?? throw new ArgumentNullException(nameof(feesRepository));
        }

        public async Task<decimal> CalculateFeeAsync(ComplianceSchemeLateFeeRequestDto request, CancellationToken cancellationToken)
        {
            // If Late Fee is false, return zero
            if (!request.IsLateFeeApplicable)
                return 0m;

            return await _feesRepository.GetLateFeeAsync(request.Regulator, request.SubmissionDate, cancellationToken);
        }
    }
}
