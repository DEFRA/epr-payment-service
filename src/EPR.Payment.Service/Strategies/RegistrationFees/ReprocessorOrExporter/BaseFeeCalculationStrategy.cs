using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.Fees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ReprocessorOrExporter;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees.ReprocessorOrExporter;

namespace EPR.Payment.Service.Strategies.RegistrationFees.ReprocessorOrExporter
{
    public class BaseFeeCalculationStrategy : IBaseFeeCalculationStrategy
    {
        private readonly IReprocessorOrExporterFeeRepository _feeRepository;

        public BaseFeeCalculationStrategy(IReprocessorOrExporterFeeRepository feeRepository)
        {
            _feeRepository = feeRepository ?? throw new ArgumentNullException(nameof(feeRepository));
        }

        public async Task<decimal> CalculateFeeAsync(ReprocessorOrExporterRegistrationFeesRequestDto request, CancellationToken cancellationToken)
        {
            // Ensure Regulator is not null or empty
            if (string.IsNullOrEmpty(request.Regulator))
                throw new ArgumentException(ReprocessorOrExporterFeesCalculationExceptions.RegulatorMissing);

            var regulator = RegulatorType.Create(request.Regulator);
            return await _feeRepository.GetBaseFeeAsync(request.RequestorType, request.MaterialType, regulator, request.SubmissionDate, cancellationToken);
        }
    }
}
