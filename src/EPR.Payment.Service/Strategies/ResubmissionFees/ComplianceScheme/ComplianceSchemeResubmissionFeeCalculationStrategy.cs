using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.ResubmissionFees.ComplianceScheme;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using EPR.Payment.Service.Strategies.Interfaces.ResubmissionFees.ComplianceScheme;

namespace EPR.Payment.Service.Strategies.ResubmissionFees.ComplianceScheme
{
    public class ComplianceSchemeResubmissionFeeCalculationStrategy : IComplianceSchemeResubmissionFeeCalculationStrategy<ComplianceSchemeResubmissionFeeRequestDto, decimal>
    {
        private readonly IComplianceSchemeFeesRepository _feesRepository;

        public ComplianceSchemeResubmissionFeeCalculationStrategy(IComplianceSchemeFeesRepository feesRepository)
        {
            _feesRepository = feesRepository ?? throw new ArgumentNullException(nameof(feesRepository));
        }

        public async Task<decimal> CalculateFeeAsync(ComplianceSchemeResubmissionFeeRequestDto request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Regulator))
            {
                throw new ArgumentException("Regulator cannot be null or empty");
            }

            var regulatorType = RegulatorType.Create(request.Regulator);

            var fee = await _feesRepository.GetResubmissionFeeAsync(regulatorType, cancellationToken);

            if (fee == 0)
            {
                throw new KeyNotFoundException($"No resubmission fee found for regulator '{request.Regulator}'.");
            }

            return fee;
        }
    }
}