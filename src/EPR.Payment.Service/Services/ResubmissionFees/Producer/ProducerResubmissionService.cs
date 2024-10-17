using EPR.Payment.Service.Common.Dtos.Request.Common;
using EPR.Payment.Service.Services.Interfaces.ResubmissionFees.Producer;
using EPR.Payment.Service.Strategies.Interfaces.ResubmissionFees.Producer;
using FluentValidation;

namespace EPR.Payment.Service.Services.ResubmissionFees.Producer
{
    public class ProducerResubmissionService : IProducerResubmissionService
    {
        private readonly IResubmissionAmountStrategy<RegulatorDto, decimal> _resubmissionAmountStrategy;
        private readonly IValidator<RegulatorDto> _producerResubmissionRequestValidator;

        public ProducerResubmissionService(IResubmissionAmountStrategy<RegulatorDto, decimal> resubmissionAmountStrategy, IValidator<RegulatorDto> producerResubmissionRequestValidator)
        {
            _resubmissionAmountStrategy = resubmissionAmountStrategy ?? throw new ArgumentNullException(nameof(resubmissionAmountStrategy));
            _producerResubmissionRequestValidator = producerResubmissionRequestValidator ?? throw new ArgumentNullException(nameof(producerResubmissionRequestValidator));
        }

        public async Task<decimal?> GetResubmissionAsync(RegulatorDto request, CancellationToken cancellationToken)
        {
            var validatorResult = await _producerResubmissionRequestValidator.ValidateAsync(request, cancellationToken);

            if (!validatorResult.IsValid)
            {
                throw new ValidationException(validatorResult.Errors);
            }

            return await _resubmissionAmountStrategy.CalculateFeeAsync(request, cancellationToken);
        }
    }
}