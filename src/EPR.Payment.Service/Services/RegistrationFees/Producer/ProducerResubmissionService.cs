using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.Producer;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees.Producer;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace EPR.Payment.Service.Services.RegistrationFees.Producer
{
    public class ProducerResubmissionService : IProducerResubmissionService
    {
        private readonly IResubmissionAmountStrategy _resubmissionAmountStrategy;
        private readonly IValidator<RegulatorDto> _producerResubmissionRequestValidator;

        public ProducerResubmissionService(IResubmissionAmountStrategy resubmissionAmountStrategy, IValidator<RegulatorDto> producerResubmissionRequestValidator)
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