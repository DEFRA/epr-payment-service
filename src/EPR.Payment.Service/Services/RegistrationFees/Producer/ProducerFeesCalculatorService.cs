using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.Producer;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.Producer;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees.Producer;
using EPR.Payment.Service.Strategies.RegistrationFees;
using FluentValidation;

namespace EPR.Payment.Service.Services.RegistrationFees.Producer
{
    public class ProducerFeesCalculatorService : IProducerFeesCalculatorService
    {
        private readonly IBaseFeeCalculationStrategy<ProducerRegistrationFeesRequestDto, decimal> _baseFeeCalculationStrategy;
        private readonly IBaseSubsidiariesFeeCalculationStrategy<ProducerRegistrationFeesRequestDto, SubsidiariesFeeBreakdown> _subsidiariesFeeCalculationStrategy;
        private readonly IValidator<ProducerRegistrationFeesRequestDto> _validator;
        private readonly IOnlineMarketCalculationStrategy<ProducerRegistrationFeesRequestDto, decimal> _onlineMarketCalculationStrategy;
        private readonly ILateFeeCalculationStrategy<ProducerRegistrationFeesRequestDto, decimal> _lateFeeCalculationStrategy;

        public ProducerFeesCalculatorService(
            IBaseFeeCalculationStrategy<ProducerRegistrationFeesRequestDto, decimal> baseFeeCalculationStrategy,
            IBaseSubsidiariesFeeCalculationStrategy<ProducerRegistrationFeesRequestDto, SubsidiariesFeeBreakdown> subsidiariesFeeCalculationStrategy,
            IValidator<ProducerRegistrationFeesRequestDto> validator,
            IOnlineMarketCalculationStrategy<ProducerRegistrationFeesRequestDto, decimal> onlineMarketCalculationStrategy,
            ILateFeeCalculationStrategy<ProducerRegistrationFeesRequestDto, decimal> lateFeeCalculationStrategy)
        {
            _baseFeeCalculationStrategy = baseFeeCalculationStrategy ?? throw new ArgumentNullException(nameof(baseFeeCalculationStrategy));
            _subsidiariesFeeCalculationStrategy = subsidiariesFeeCalculationStrategy ?? throw new ArgumentNullException(nameof(subsidiariesFeeCalculationStrategy));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _onlineMarketCalculationStrategy = onlineMarketCalculationStrategy ?? throw new ArgumentNullException(nameof(onlineMarketCalculationStrategy));
            _lateFeeCalculationStrategy = lateFeeCalculationStrategy ?? throw new ArgumentNullException(nameof(lateFeeCalculationStrategy));
        }

        public async Task<RegistrationFeesResponseDto> CalculateFeesAsync(ProducerRegistrationFeesRequestDto request, CancellationToken cancellationToken)
        {
            ValidateRequest(request);

            try
            {
                decimal lateFee = await _lateFeeCalculationStrategy.CalculateFeeAsync(request, cancellationToken);
                decimal subsidiariesLateFee = request.NumberOfSubsidiaries * lateFee;
                var response = new RegistrationFeesResponseDto
                {
                    ProducerRegistrationFee = await _baseFeeCalculationStrategy.CalculateFeeAsync(request, cancellationToken),
                    ProducerOnlineMarketPlaceFee = await _onlineMarketCalculationStrategy.CalculateFeeAsync(request, cancellationToken),
                    ProducerLateRegistrationFee = lateFee + subsidiariesLateFee,
                    SubsidiariesFeeBreakdown = await _subsidiariesFeeCalculationStrategy.CalculateFeeAsync(request, cancellationToken)
                };

                response.SubsidiariesFee = response.SubsidiariesFeeBreakdown.TotalSubsidiariesOMPFees + response.SubsidiariesFeeBreakdown.FeeBreakdowns.Select(i => i.TotalPrice).Sum();
                response.TotalFee = response.ProducerRegistrationFee + response.ProducerOnlineMarketPlaceFee + response.SubsidiariesFee + response.ProducerLateRegistrationFee;
                response.PreviousPayment = 0;// TODO: This will not be 0 but calculated once the database schema is changes are ready
                response.OutstandingPayment = response.TotalFee - response.PreviousPayment;

                return response;
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException(ProducerFeesCalculationExceptions.FeeCalculationError, ex);
            }
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