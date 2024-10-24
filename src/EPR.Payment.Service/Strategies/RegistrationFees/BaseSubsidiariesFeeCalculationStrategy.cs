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


        protected virtual Task<decimal> GetFirstBandFeeAsync(RegulatorType regulator, DateTime submissionDate, CancellationToken cancellationToken)
        {
            return _feesRepository.GetFirstBandFeeAsync(regulator, submissionDate, cancellationToken);
        }

        protected virtual Task<decimal> GetSecondBandFeeAsync(RegulatorType regulator, DateTime submissionDate, CancellationToken cancellationToken)
        {
            return _feesRepository.GetSecondBandFeeAsync(regulator, submissionDate, cancellationToken);
        }

        protected virtual Task<decimal> GetThirdBandFeeAsync(RegulatorType regulator, DateTime submissionDate, CancellationToken cancellationToken)
        {
            return _feesRepository.GetThirdBandFeeAsync(regulator, submissionDate, cancellationToken);
        }

        protected virtual Task<decimal> GetOnlineMarketFeeAsync(RegulatorType regulator, DateTime submissionDate, CancellationToken cancellationToken)
        {
            return _feesRepository.GetOnlineMarketFeeAsync(regulator, submissionDate, cancellationToken);
        }

        protected abstract int GetNoOfSubsidiaries(TRequestDto request);
        protected abstract int GetNoOfOMPSubsidiaries(TRequestDto request);
        protected abstract RegulatorType GetRegulator(TRequestDto request);
        protected abstract DateTime GetSubmissionDate(TRequestDto request);

        public async Task<SubsidiariesFeeBreakdown> CalculateFeeAsync(TRequestDto request, CancellationToken cancellationToken)
        {
            var regulator = GetRegulator(request);
            var submissionDate = GetSubmissionDate(request);
            var unitOMPFees = await GetOnlineMarketFeeAsync(regulator, submissionDate, cancellationToken);

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
            var firstBandFee = await GetFirstBandFeeAsync(regulator, submissionDate, cancellationToken);
            var secondBandFee = await GetSecondBandFeeAsync(regulator, submissionDate, cancellationToken);
            var thirdBandFee = await GetThirdBandFeeAsync(regulator, submissionDate, cancellationToken);

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
