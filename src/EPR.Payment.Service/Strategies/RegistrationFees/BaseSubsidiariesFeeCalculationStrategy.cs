using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using EPR.Payment.Service.Strategies.Interfaces.RegistrationFees;

namespace EPR.Payment.Service.Strategies.RegistrationFees
{
    public abstract class BaseSubsidiariesFeeCalculationStrategy<TRequestDto> : IBaseSubsidiariesFeeCalculationStrategy<TRequestDto, SubsidiariesFeeBreakdown>
    {
        private readonly IFeeRepository _feesRepository;

        protected BaseSubsidiariesFeeCalculationStrategy(IFeeRepository feesRepository)
        {
            _feesRepository = feesRepository ?? throw new ArgumentNullException(nameof(feesRepository));
        }

        private const int FirstBandLimit = 20;
        private const int SecondBandLimit = 100;
        private const int SecondBandSize = 80;


        protected virtual Task<decimal> GetFirstBandFeeAsync(RegulatorType regulator, CancellationToken cancellationToken)
        {
            return _feesRepository.GetFirstBandFeeAsync(regulator, cancellationToken);
        }

        protected virtual Task<decimal> GetSecondBandFeeAsync(RegulatorType regulator, CancellationToken cancellationToken)
        {
            return _feesRepository.GetSecondBandFeeAsync(regulator, cancellationToken);
        }

        protected virtual Task<decimal> GetThirdBandFeeAsync(RegulatorType regulator, CancellationToken cancellationToken)
        {
            return _feesRepository.GetThirdBandFeeAsync(regulator, cancellationToken);
        }

        protected virtual Task<decimal> GetOnlineMarketFeeAsync(RegulatorType regulator, CancellationToken cancellationToken)
        {
            return _feesRepository.GetOnlineMarketFeeAsync(regulator, cancellationToken);
        }

        protected abstract int GetNoOfSubsidiaries(TRequestDto request);
        protected abstract int GetNoOfOMPSubsidiaries(TRequestDto request);
        protected abstract RegulatorType GetRegulator(TRequestDto request);

        public async Task<SubsidiariesFeeBreakdown> CalculateFeeAsync(TRequestDto request, CancellationToken cancellationToken)
        {
            var regulator = GetRegulator(request);
            var unitOMPFees = await GetOnlineMarketFeeAsync(regulator, cancellationToken);

            // Fee breakdown initialization
            var subsidiariesFeeBreakdown = new SubsidiariesFeeBreakdown
            {
                CountOfOMPSubsidiaries = GetNoOfOMPSubsidiaries(request),
                UnitOMPFees = unitOMPFees,
                TotalSubsidiariesOMPFees = GetNoOfOMPSubsidiaries(request) * unitOMPFees,
                FeeBreakdowns = new List<FeeBreakdown>()
            };

            // Calculate subsidiary band counts
            (int firstBandCount, int secondBandCount, int thirdBandCount) = CalculateBandCounts(GetNoOfSubsidiaries(request));

            // Fetch fees in parallel
            var firstBandFee = await GetFirstBandFeeAsync(regulator, cancellationToken);
            var secondBandFee = await GetSecondBandFeeAsync(regulator, cancellationToken);
            var thirdBandFee = await GetThirdBandFeeAsync(regulator, cancellationToken);

            // Adding Fee breakdowns
            AddFeeBreakdown(subsidiariesFeeBreakdown.FeeBreakdowns, 1, firstBandCount, firstBandFee);
            AddFeeBreakdown(subsidiariesFeeBreakdown.FeeBreakdowns, 2, secondBandCount, secondBandFee);
            AddFeeBreakdown(subsidiariesFeeBreakdown.FeeBreakdowns, 3, thirdBandCount, thirdBandFee);

            return subsidiariesFeeBreakdown;
        }

        private static (int firstBandCount, int secondBandCount, int thirdBandCount) CalculateBandCounts(int numberOfSubsidiaries)
        {
            var firstBandCount = Math.Min(numberOfSubsidiaries, FirstBandLimit);
            var secondBandCount = numberOfSubsidiaries > SecondBandLimit ? SecondBandSize : Math.Max(0, numberOfSubsidiaries - FirstBandLimit);
            var thirdBandCount = numberOfSubsidiaries > SecondBandLimit ? Math.Max(0, numberOfSubsidiaries - SecondBandLimit) : 0;

            return (firstBandCount, secondBandCount, thirdBandCount);
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
