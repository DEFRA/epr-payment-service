using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.Producer;
using EPR.Payment.Service.Services.Interfaces.Payments;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.Producer;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees.Producer;
using FluentValidation;

namespace EPR.Payment.Service.Services.RegistrationFees.Producer
{
    public class ProducerFeesCalculatorService : IProducerFeesCalculatorService
    {
        private readonly IBaseFeeCalculationStrategy<ProducerRegistrationFeesRequestDto, decimal> _baseFeeCalculationStrategy;
        private readonly IBaseFeeCalculationStrategy<ProducerRegistrationFeesRequestV2Dto, decimal> _baseFeeCalculationStrategyV2;
        private readonly IBaseSubsidiariesFeeCalculationStrategy<ProducerRegistrationFeesRequestDto, SubsidiariesFeeBreakdown> _subsidiariesFeeCalculationStrategy;
        private readonly IValidator<ProducerRegistrationFeesRequestDto> _validator;
        private readonly IOnlineMarketCalculationStrategy<ProducerRegistrationFeesRequestDto, decimal> _onlineMarketCalculationStrategy;
        private readonly ILateFeeCalculationStrategy<ProducerRegistrationFeesRequestDto, decimal> _lateFeeCalculationStrategy;
        private readonly IPaymentsService _paymentsService;

        public ProducerFeesCalculatorService(
            IBaseFeeCalculationStrategy<ProducerRegistrationFeesRequestDto, decimal> baseFeeCalculationStrategy,
            IBaseSubsidiariesFeeCalculationStrategy<ProducerRegistrationFeesRequestDto, SubsidiariesFeeBreakdown> subsidiariesFeeCalculationStrategy,
            IValidator<ProducerRegistrationFeesRequestDto> validator,
            IOnlineMarketCalculationStrategy<ProducerRegistrationFeesRequestDto, decimal> onlineMarketCalculationStrategy,
            ILateFeeCalculationStrategy<ProducerRegistrationFeesRequestDto, decimal> lateFeeCalculationStrategy,
            IPaymentsService paymentsService)
        {
            _baseFeeCalculationStrategy = baseFeeCalculationStrategy ?? throw new ArgumentNullException(nameof(baseFeeCalculationStrategy));
            _subsidiariesFeeCalculationStrategy = subsidiariesFeeCalculationStrategy ?? throw new ArgumentNullException(nameof(subsidiariesFeeCalculationStrategy));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _onlineMarketCalculationStrategy = onlineMarketCalculationStrategy ?? throw new ArgumentNullException(nameof(onlineMarketCalculationStrategy));
            _lateFeeCalculationStrategy = lateFeeCalculationStrategy ?? throw new ArgumentNullException(nameof(lateFeeCalculationStrategy));
            _paymentsService = paymentsService ?? throw new ArgumentNullException(nameof(paymentsService));
        }

        public async Task<RegistrationFeesResponseDto> CalculateFeesAsync(ProducerRegistrationFeesRequestDto request, CancellationToken cancellationToken)
        {
            ValidateRequest(request);
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
            response.PreviousPayment = await _paymentsService.GetPreviousPaymentsByReferenceAsync(request.ApplicationReferenceNumber, cancellationToken);
            response.OutstandingPayment = response.TotalFee - response.PreviousPayment;

            return response;
        }

      /*  public async Task<RegistrationFeesResponseDto> CalculateFeesAsync(ProducerRegistrationFeesRequestV2Dto request, CancellationToken cancellationToken)
        {
            ValidateRequest(request);
            decimal lateFee = await _baseFeeCalculationStrategyV2.CalculateFeeAsync(request, cancellationToken);
            decimal subsidiariesLateFee = request.NumberOfSubsidiaries * lateFee;
            var response = new RegistrationFeesResponseDto
            {
                ProducerRegistrationFee = await _baseFeeCalculationStrategyV2.CalculateFeeAsync(request, cancellationToken),
                ProducerOnlineMarketPlaceFee = await _onlineMarketCalculationStrategy.CalculateFeeAsync(request, cancellationToken),
                ProducerLateRegistrationFee = lateFee + subsidiariesLateFee,
                SubsidiariesFeeBreakdown = await _subsidiariesFeeCalculationStrategy.CalculateFeeAsync(request, cancellationToken)
            };

            response.SubsidiariesFee = response.SubsidiariesFeeBreakdown.TotalSubsidiariesOMPFees + response.SubsidiariesFeeBreakdown.FeeBreakdowns.Select(i => i.TotalPrice).Sum();
            response.TotalFee = response.ProducerRegistrationFee + response.ProducerOnlineMarketPlaceFee + response.SubsidiariesFee + response.ProducerLateRegistrationFee;
            response.PreviousPayment = await _paymentsService.GetPreviousPaymentsByReferenceAsync(request.ApplicationReferenceNumber, cancellationToken);
            response.OutstandingPayment = response.TotalFee - response.PreviousPayment;

            return response;
        }*/

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