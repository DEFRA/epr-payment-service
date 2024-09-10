using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees.Producer;
using EPR.Payment.Service.Utilities.RegistrationFees.Interfaces;
using FluentValidation;

namespace EPR.Payment.Service.Services.RegistrationFees
{
    public class ProducerFeesCalculatorService : IProducerFeesCalculatorService
    {
        private readonly IBaseFeeCalculationStrategy<ProducerRegistrationFeesRequestDto> _baseFeeCalculationStrategy;
        private readonly ISubsidiariesFeeCalculationStrategy<ProducerRegistrationFeesRequestDto> _subsidiariesFeeCalculationStrategy;
        private readonly IValidator<ProducerRegistrationFeesRequestDto> _validator;
        private readonly IFeeBreakdownGenerator<ProducerRegistrationFeesRequestDto, RegistrationFeesResponseDto> _feeBreakdownGenerator;

        public ProducerFeesCalculatorService(
            IBaseFeeCalculationStrategy<ProducerRegistrationFeesRequestDto> baseFeeCalculationStrategy,
            ISubsidiariesFeeCalculationStrategy<ProducerRegistrationFeesRequestDto> subsidiariesFeeCalculationStrategy,
            IValidator<ProducerRegistrationFeesRequestDto> validator,
            IFeeBreakdownGenerator<ProducerRegistrationFeesRequestDto, RegistrationFeesResponseDto> feeBreakdownGenerator)
        {
            _baseFeeCalculationStrategy = baseFeeCalculationStrategy ?? throw new ArgumentNullException(nameof(baseFeeCalculationStrategy));
            _subsidiariesFeeCalculationStrategy = subsidiariesFeeCalculationStrategy ?? throw new ArgumentNullException(nameof(subsidiariesFeeCalculationStrategy));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _feeBreakdownGenerator = feeBreakdownGenerator ?? throw new ArgumentNullException(nameof(feeBreakdownGenerator));
        }

        public async Task<RegistrationFeesResponseDto> CalculateFeesAsync(ProducerRegistrationFeesRequestDto request, CancellationToken cancellationToken)
        {
            var response = new RegistrationFeesResponseDto();
            ValidateRequest(request);

            try
            {
                response.BaseFee = await _baseFeeCalculationStrategy.CalculateFeeAsync(request, cancellationToken);
                response.SubsidiariesFee = await _subsidiariesFeeCalculationStrategy.CalculateFeeAsync(request, cancellationToken);
                response.TotalFee = response.BaseFee + response.SubsidiariesFee;

                await _feeBreakdownGenerator.GenerateFeeBreakdownAsync(response, request, cancellationToken);
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException(ProducerFeesCalculationExceptions.FeeCalculationError, ex);
            }

            return response;
        }

        private void ValidateRequest(ProducerRegistrationFeesRequestDto request)
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
        }
    }
}