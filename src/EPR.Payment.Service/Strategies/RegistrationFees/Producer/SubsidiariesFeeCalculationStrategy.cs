using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees.Producer;

namespace EPR.Payment.Service.Strategies.RegistrationFees.Producer
{
    public class SubsidiariesFeeCalculationStrategy : ISubsidiariesFeeCalculationStrategy<ProducerRegistrationFeesRequestDto, SubsidiariesFeeBreakdown>
    {
        private const int FirstBandLimit = 20;
        private const int SecondBandLimit = 100;
        private const int SecondBandSize = 80;
        private readonly IProducerFeesRepository _feesRepository;

        public SubsidiariesFeeCalculationStrategy(IProducerFeesRepository feesRepository)
        {
            _feesRepository = feesRepository ?? throw new ArgumentNullException(nameof(feesRepository));
        }

        public async Task<SubsidiariesFeeBreakdown> CalculateFeeAsync(ProducerRegistrationFeesRequestDto request, CancellationToken cancellationToken)
        {
            var regulator = RegulatorType.Create(request.Regulator);
            var unitOMPFees = await _feesRepository.GetOnlineMarketFeeAsync(regulator, cancellationToken);

            // Fee breakdown initialization
            var subsidiariesFeeBreakdown = new SubsidiariesFeeBreakdown
            {
                CountOfOMPSubsidiaries = request.NoOfSubsidiariesOnlineMarketplace,
                UnitOMPFees = unitOMPFees,
                TotalSubsidiariesOMPFees = request.NoOfSubsidiariesOnlineMarketplace * unitOMPFees,
                FeeBreakdowns = new List<FeeBreakdown>()
            };

            // Calculate subsidiary band counts
            (int firstBandCount, int secondBandCount, int thirdBandCount) = CalculateBandCounts(request.NumberOfSubsidiaries);

            // Fetch fees in parallel
            var firstBandFeeTask = await _feesRepository.GetFirst20SubsidiariesFeeAsync(regulator, cancellationToken);
            var secondBandFeeTask = await _feesRepository.GetAdditionalUpTo100SubsidiariesFeeAsync(regulator, cancellationToken);
            var thirdBandFeeTask = await _feesRepository.GetAdditionalMoreThan100SubsidiariesFeeAsync(regulator, cancellationToken);

            // Adding Fee breakdowns
            AddFeeBreakdown(subsidiariesFeeBreakdown.FeeBreakdowns, 1, firstBandCount, firstBandFeeTask);
            AddFeeBreakdown(subsidiariesFeeBreakdown.FeeBreakdowns, 2, secondBandCount, secondBandFeeTask);
            AddFeeBreakdown(subsidiariesFeeBreakdown.FeeBreakdowns, 3, thirdBandCount, thirdBandFeeTask);

            return subsidiariesFeeBreakdown;
        }

        private (int firstBandCount, int secondBandCount, int thirdBandCount) CalculateBandCounts(int numberOfSubsidiaries)
        {
            var firstBandCount = Math.Min(numberOfSubsidiaries, FirstBandLimit);
            var secondBandCount = numberOfSubsidiaries > SecondBandLimit ? SecondBandSize : Math.Max(0, numberOfSubsidiaries - FirstBandLimit);
            var thirdBandCount = numberOfSubsidiaries > SecondBandLimit ? Math.Max(0, numberOfSubsidiaries - SecondBandLimit) : 0;

            return (firstBandCount, secondBandCount, thirdBandCount);
        }

        private void AddFeeBreakdown(List<FeeBreakdown> feeBreakdowns, int bandNumber, int unitCount, decimal unitPrice)
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
