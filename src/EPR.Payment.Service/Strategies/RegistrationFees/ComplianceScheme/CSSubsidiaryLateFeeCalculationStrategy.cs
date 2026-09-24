using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees.ComplianceScheme;

namespace EPR.Payment.Service.Strategies.RegistrationFees.ComplianceScheme
{
    public class CSSubsidiaryLateFeeCalculationStrategy : ICSSubsidiaryLateFeeCalculationStrategy<ComplianceSchemeLateFeeRequestDto, decimal>
    {
        private readonly IComplianceSchemeFeesRepository _feesRepository;

        public CSSubsidiaryLateFeeCalculationStrategy(IComplianceSchemeFeesRepository feesRepository)
        {
            _feesRepository = feesRepository ?? throw new ArgumentNullException(nameof(feesRepository));
        }

        public async Task<decimal> CalculateFeeAsync(ComplianceSchemeLateFeeRequestDto request, CancellationToken cancellationToken)
        {
            return await _feesRepository.GetSubsidiaryLateFeeAsync(request.Regulator, request.SubmissionDate, cancellationToken);
        }
    }
}
