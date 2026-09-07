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
        private readonly IBaseSubsidiariesFeeCalculationStrategy<ProducerRegistrationFeesRequestDto, SubsidiariesFeeBreakdown> _subsidiariesFeeCalculationStrategy;
        private readonly IValidator<ProducerRegistrationFeesRequestDto> _validator;
        private readonly IOnlineMarketCalculationStrategy<ProducerRegistrationFeesRequestDto, decimal> _onlineMarketCalculationStrategy;
        private readonly ILateFeeCalculationStrategy<ProducerRegistrationFeesRequestDto, decimal> _lateFeeCalculationStrategy;
        private readonly ISubsidiaryLateFeeCalculationStrategy<ProducerRegistrationFeesRequestDto, decimal> _subsidiaryLateFeeCalculationStrategy;
        private readonly IPaymentsService _paymentsService;
        private readonly IClosedLoopRecyclingCalculationStrategy<ProducerRegistrationFeesRequestDto, decimal> _closedLoopRecyclingCalculationStrategy;

        public ProducerFeesCalculatorService(
            IBaseFeeCalculationStrategy<ProducerRegistrationFeesRequestDto, decimal> baseFeeCalculationStrategy,
            IBaseSubsidiariesFeeCalculationStrategy<ProducerRegistrationFeesRequestDto, SubsidiariesFeeBreakdown> subsidiariesFeeCalculationStrategy,
            IValidator<ProducerRegistrationFeesRequestDto> validator,
            IOnlineMarketCalculationStrategy<ProducerRegistrationFeesRequestDto, decimal> onlineMarketCalculationStrategy,
            ILateFeeCalculationStrategy<ProducerRegistrationFeesRequestDto, decimal> lateFeeCalculationStrategy,
            ISubsidiaryLateFeeCalculationStrategy<ProducerRegistrationFeesRequestDto, decimal> subsidiaryLateFeeCalculationStrategy,
            IPaymentsService paymentsService,
            IClosedLoopRecyclingCalculationStrategy<ProducerRegistrationFeesRequestDto, decimal> closedLoopRecyclingCalculationStrategy)
        {
            _baseFeeCalculationStrategy = baseFeeCalculationStrategy ?? throw new ArgumentNullException(nameof(baseFeeCalculationStrategy));
            _subsidiariesFeeCalculationStrategy = subsidiariesFeeCalculationStrategy ?? throw new ArgumentNullException(nameof(subsidiariesFeeCalculationStrategy));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _onlineMarketCalculationStrategy = onlineMarketCalculationStrategy ?? throw new ArgumentNullException(nameof(onlineMarketCalculationStrategy));
            _lateFeeCalculationStrategy = lateFeeCalculationStrategy ?? throw new ArgumentNullException(nameof(lateFeeCalculationStrategy));
            _subsidiaryLateFeeCalculationStrategy = subsidiaryLateFeeCalculationStrategy ?? throw new ArgumentNullException(nameof(subsidiaryLateFeeCalculationStrategy));
            _paymentsService = paymentsService ?? throw new ArgumentNullException(nameof(paymentsService));
            _closedLoopRecyclingCalculationStrategy = closedLoopRecyclingCalculationStrategy ?? throw new ArgumentNullException(nameof(closedLoopRecyclingCalculationStrategy));
        }

        public async Task<RegistrationFeesResponseDto> CalculateFeesAsync(ProducerRegistrationFeesRequestDto request, CancellationToken cancellationToken)
        {
            ValidateRequest(request);
            decimal orgLateFee = await _lateFeeCalculationStrategy.CalculateFeeAsync(request, cancellationToken);
            decimal subLateUnit = await _subsidiaryLateFeeCalculationStrategy.CalculateFeeAsync(request, cancellationToken);
            decimal subLateTotal = request.NumberOfLateSubsidiaries * subLateUnit;

            var response = new RegistrationFeesResponseDto
            {
                ProducerRegistrationFee = await _baseFeeCalculationStrategy.CalculateFeeAsync(request, cancellationToken),
                ProducerOnlineMarketPlaceFee = await _onlineMarketCalculationStrategy.CalculateFeeAsync(request, cancellationToken),
                ProducerClosedLoopRecyclingFee = await _closedLoopRecyclingCalculationStrategy.CalculateFeeAsync(request, cancellationToken),
                ProducerLateRegistrationFee = orgLateFee,
                SubsidiariesFeeBreakdown = await _subsidiariesFeeCalculationStrategy.CalculateFeeAsync(request, cancellationToken)
            };

            response.SubsidiariesFeeBreakdown.CountOfLateSubsidiaries = request.NumberOfLateSubsidiaries;
            response.SubsidiariesFeeBreakdown.UnitSubsidiaryLateFee = subLateUnit;
            response.SubsidiariesFeeBreakdown.TotalSubsidiariesLateFees = subLateTotal;

            response.SubsidiariesFee = response.SubsidiariesFeeBreakdown.TotalSubsidiariesOMPFees
                                       + response.SubsidiariesFeeBreakdown.TotalSubsidiariesClosedLoopRecyclingFees
                                       + response.SubsidiariesFeeBreakdown.TotalSubsidiariesLateFees
                                       + response.SubsidiariesFeeBreakdown.FeeBreakdowns.Select(i => i.TotalPrice).Sum();
            response.TotalFee = response.ProducerRegistrationFee + response.ProducerOnlineMarketPlaceFee + response.ProducerClosedLoopRecyclingFee + response.SubsidiariesFee + response.ProducerLateRegistrationFee;
            response.PreviousPayment = await _paymentsService.GetPreviousPaymentsByReferenceAsync(request.ApplicationReferenceNumber, cancellationToken);
            response.OutstandingPayment = response.TotalFee - response.PreviousPayment;

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