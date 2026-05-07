using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees.Producer;

namespace EPR.Payment.Service.Strategies.RegistrationFees.Producer
{
    public class ClosedLoopRecyclingCalculationStrategy : IClosedLoopRecyclingCalculationStrategy<ProducerRegistrationFeesRequestDto, decimal>
    {
        private readonly IProducerFeesRepository _feesRepository;

        public ClosedLoopRecyclingCalculationStrategy(IProducerFeesRepository feesRepository)
        {
            _feesRepository = feesRepository ?? throw new ArgumentNullException(nameof(feesRepository));
        }

        public async Task<decimal> CalculateFeeAsync(ProducerRegistrationFeesRequestDto request, CancellationToken cancellationToken)
        {
            if (!request.IsClosedLoopRecycling)
                return 0m;

            if (string.IsNullOrEmpty(request.Regulator))
                throw new ArgumentException(ProducerFeesCalculationExceptions.RegulatorMissing);

            var regulator = RegulatorType.Create(request.Regulator);
            return await _feesRepository.GetClosedLoopRecyclingFeeAsync(regulator, request.SubmissionDate, cancellationToken);
        }
    }
}
