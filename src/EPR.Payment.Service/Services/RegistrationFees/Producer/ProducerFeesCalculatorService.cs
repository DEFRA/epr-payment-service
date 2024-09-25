using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.Producer;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees.Producer;
using FluentValidation;

namespace EPR.Payment.Service.Services.RegistrationFees.Producer
{
    public class ProducerFeesCalculatorService : IProducerFeesCalculatorService
    {
        private readonly IBaseFeeCalculationStrategy<ProducerRegistrationFeesRequestDto, decimal> _baseFeeCalculationStrategy;
        private readonly ISubsidiariesFeeCalculationStrategy<ProducerRegistrationFeesRequestDto, SubsidiariesFeeBreakdown> _subsidiariesFeeCalculationStrategy;
        private readonly IValidator<ProducerRegistrationFeesRequestDto> _validator;
        private readonly IOnlineMarketCalculationStrategy<ProducerRegistrationFeesRequestDto, decimal> _onlineMarketCalculationStrategy;

        public ProducerFeesCalculatorService(
            IBaseFeeCalculationStrategy<ProducerRegistrationFeesRequestDto, decimal> baseFeeCalculationStrategy,
            ISubsidiariesFeeCalculationStrategy<ProducerRegistrationFeesRequestDto, SubsidiariesFeeBreakdown> subsidiariesFeeCalculationStrategy,
            IValidator<ProducerRegistrationFeesRequestDto> validator,
            IOnlineMarketCalculationStrategy<ProducerRegistrationFeesRequestDto, decimal> onlineMarketCalculationStrategy)
        {
            _baseFeeCalculationStrategy = baseFeeCalculationStrategy ?? throw new ArgumentNullException(nameof(baseFeeCalculationStrategy));
            _subsidiariesFeeCalculationStrategy = subsidiariesFeeCalculationStrategy ?? throw new ArgumentNullException(nameof(subsidiariesFeeCalculationStrategy));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _onlineMarketCalculationStrategy = onlineMarketCalculationStrategy ?? throw new ArgumentNullException(nameof(onlineMarketCalculationStrategy));
        }

        public async Task<RegistrationFeesResponseDto> CalculateFeesAsync(ProducerRegistrationFeesRequestDto request, CancellationToken cancellationToken)
        {
            var response = new RegistrationFeesResponseDto();
            ValidateRequest(request);

            try
            {
                response.ProducerRegistrationFee = await _baseFeeCalculationStrategy.CalculateFeeAsync(request, cancellationToken);
                response.ProducerOnlineMarketPlaceFee = await _onlineMarketCalculationStrategy.CalculateFeeAsync(request, cancellationToken);
                response.SubsidiariesFeeBreakdown = await _subsidiariesFeeCalculationStrategy.CalculateFeeAsync(request, cancellationToken);
                response.SubsidiariesFee = response.SubsidiariesFeeBreakdown.TotalSubsidiariesOMPFees + response.SubsidiariesFeeBreakdown.FeeBreakdowns.Select(i => i.TotalPrice).Sum();
                response.TotalFee = response.ProducerRegistrationFee + response.ProducerOnlineMarketPlaceFee + response.SubsidiariesFee;
                response.PreviousPayment = 0;
                response.OutstandingPayment = response.TotalFee - response.PreviousPayment;
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