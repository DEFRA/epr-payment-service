using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees.Producer;

namespace EPR.Payment.Service.Strategies.RegistrationFees.Producer
{
    public class LateSubsidiariesFeeCalculationStrategy : ILateSubsidiariesFeeCalculationStrategy<ProducerRegistrationFeesRequestDto, decimal>
    {
        private readonly IProducerFeesRepository _feesRepository;

        public LateSubsidiariesFeeCalculationStrategy(IProducerFeesRepository feesRepository)
        {
            _feesRepository = feesRepository ?? throw new ArgumentNullException(nameof(feesRepository));
        }
        public async Task<decimal> CalculateFeeAsync(ProducerRegistrationFeesRequestDto request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            if (request.IsLateFeeApplicable)
                return 0m;

            var lateSubsidiaryCount = request.NumberOfLateSubsidiaries;
            if (lateSubsidiaryCount <= 0)
                return 0m;

            if (string.IsNullOrWhiteSpace(request.Regulator))
                throw new ArgumentException(ProducerFeesCalculationExceptions.RegulatorMissing, nameof(request.Regulator));

            var regulator = RegulatorType.Create(request.Regulator);
            var perSubsidiaryLateFee = await _feesRepository
                .GetLateFeeAsync(regulator, request.SubmissionDate, cancellationToken)
                .ConfigureAwait(false);

            return perSubsidiaryLateFee * lateSubsidiaryCount;
        }
    }
}
