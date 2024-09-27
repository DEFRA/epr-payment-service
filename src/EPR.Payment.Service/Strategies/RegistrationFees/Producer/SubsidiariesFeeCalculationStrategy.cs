using EPR.Payment.Service.Common.Constants.RegistrationFees.Exceptions;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees.Producer;

namespace EPR.Payment.Service.Strategies.RegistrationFees.Producer
{
    public class SubsidiariesFeeCalculationStrategy : ISubsidiariesFeeCalculationStrategy<ProducerRegistrationFeesRequestDto, SubsidiariesFeeBreakdown>
    {
        private readonly IProducerFeesRepository _feesRepository;

        public SubsidiariesFeeCalculationStrategy(IProducerFeesRepository feesRepository)
        {
            _feesRepository = feesRepository ?? throw new ArgumentNullException(nameof(feesRepository));
        }

        public async Task<SubsidiariesFeeBreakdown> CalculateFeeAsync(ProducerRegistrationFeesRequestDto request, CancellationToken cancellationToken)
        {
            ValidateRequest(request);

            var regulator = RegulatorType.Create(request.Regulator);
            var unitOMPFees = await _feesRepository.GetOnlineMarketFeeAsync(regulator, cancellationToken);

            var subsidiariesFeeBreakdown = new SubsidiariesFeeBreakdown
            {
                CountOfOMPSubsidiaries = request.NoOfSubsidiariesOnlineMarketplace,
                UnitOMPFees = unitOMPFees,
                TotalSubsidiariesOMPFees = request.NoOfSubsidiariesOnlineMarketplace * unitOMPFees,
                FeeBreakdowns = new List<FeeBreakdown>()
            };

            var firstBandCount = Math.Min(request.NumberOfSubsidiaries, 20);
            var secondBandCount = request.NumberOfSubsidiaries > 100 ? 80 : Math.Max(0, request.NumberOfSubsidiaries - 20);
            var thirdBandCount = request.NumberOfSubsidiaries > 100 ? Math.Max(0, request.NumberOfSubsidiaries - 100) : 0;

            var firstBandFee = await _feesRepository.GetFirst20SubsidiariesFeeAsync(regulator, cancellationToken);
            AddFeeBreakdown(subsidiariesFeeBreakdown.FeeBreakdowns, 1, firstBandCount, firstBandFee);

            var secondBandFee = await _feesRepository.GetAdditionalUpTo100SubsidiariesFeeAsync(regulator, cancellationToken);
            AddFeeBreakdown(subsidiariesFeeBreakdown.FeeBreakdowns, 2, secondBandCount, secondBandFee);

            var thirdBandFee = await _feesRepository.GetAdditionalMoreThan100SubsidiariesFeeAsync(regulator, cancellationToken);
            AddFeeBreakdown(subsidiariesFeeBreakdown.FeeBreakdowns, 3, thirdBandCount, thirdBandFee);

            return subsidiariesFeeBreakdown;
        }

        private static void ValidateRequest(ProducerRegistrationFeesRequestDto request)
        {
            if (request.NumberOfSubsidiaries < 0 || string.IsNullOrEmpty(request.Regulator))
            {
                throw new ArgumentException(ProducerFeesCalculationExceptions.InvalidSubsidiariesNumber);
            }
        }
        private static void AddFeeBreakdown(List<FeeBreakdown> feeBreakdowns, int bandNumber, int unitCount, decimal unitPrice)
        {
            feeBreakdowns.Add(new FeeBreakdown
            {
                BandNumber = bandNumber,
                UnitCount = unitCount,
                UnitPrice = unitPrice,
                TotalPrice = unitCount * unitPrice
            });
        }

    }

}
