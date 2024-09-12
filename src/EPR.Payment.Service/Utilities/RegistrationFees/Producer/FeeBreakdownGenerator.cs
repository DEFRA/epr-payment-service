using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using EPR.Payment.Service.Utilities.RegistrationFees.Interfaces;

namespace EPR.Payment.Service.Utilities.RegistrationFees.Producer
{
    public class FeeBreakdownGenerator : IFeeBreakdownGenerator<ProducerRegistrationFeesRequestDto, RegistrationFeesResponseDto>
    {
        private readonly IProducerFeesRepository _feesRepository;

        public FeeBreakdownGenerator(IProducerFeesRepository feesRepository)
        {
            _feesRepository = feesRepository ?? throw new ArgumentNullException(nameof(feesRepository));
        }

        public async Task GenerateFeeBreakdownAsync(RegistrationFeesResponseDto response, ProducerRegistrationFeesRequestDto request, CancellationToken cancellationToken)
        {
            AddBaseFeeBreakdown(response);
            AddOnlineMarketFeeBreakdown(response, request);
            await AddSubsidiariesFeeBreakdown(response, request, cancellationToken);
        }

        private void AddBaseFeeBreakdown(RegistrationFeesResponseDto response)
        {
            if (response.BaseFee <= 0) return;

            response.FeeBreakdowns.Add(CreateFeeBreakdown(
                $"Base Fee (£{Math.Truncate(response.BaseFee / 100m)})",
                response.BaseFee));
        }

        private void AddOnlineMarketFeeBreakdown(RegistrationFeesResponseDto response, ProducerRegistrationFeesRequestDto request)
        {
            if (!request.IsOnlineMarketplace) return;

            response.FeeBreakdowns.Add(CreateFeeBreakdown(
                $"Online Market Fee (£{Math.Truncate(response.OnlineMarket / 100m)})",
                response.OnlineMarket));
        }

        private async Task AddSubsidiariesFeeBreakdown(
            RegistrationFeesResponseDto response,
            ProducerRegistrationFeesRequestDto request,
            CancellationToken cancellationToken)
        {
            if (request.NumberOfSubsidiaries <= 0) return;

            var regulator = RegulatorType.Create(request.Regulator);
            var (first20Rate, additionalUpTo100Rate, additionalMoreThan100Rate) = await GetSubsidiaryRates(regulator, cancellationToken);

            AddSubsidiariesFee(response, request.NumberOfSubsidiaries, first20Rate, additionalUpTo100Rate, additionalMoreThan100Rate);
        }

        private async Task<(decimal first20Rate, decimal additionalUpTo100Rate, decimal additionalMoreThan100Rate)> GetSubsidiaryRates(
            RegulatorType regulator,
            CancellationToken cancellationToken)
        {
            var first20Rate = await _feesRepository.GetFirst20SubsidiariesFeeAsync(regulator, cancellationToken);
            var additionalUpTo100Rate = await _feesRepository.GetAdditionalUpTo100SubsidiariesFeeAsync(regulator, cancellationToken);
            var additionalMoreThan100Rate = await _feesRepository.GetAdditionalMoreThan100SubsidiariesFeeAsync(regulator, cancellationToken);

            return (first20Rate, additionalUpTo100Rate, additionalMoreThan100Rate);
        }

        private void AddSubsidiariesFee(
            RegistrationFeesResponseDto response,
            int numberOfSubsidiaries,
            decimal first20Rate,
            decimal additionalUpTo100Rate,
            decimal additionalMoreThan100Rate)
        {
            var first20Count = Math.Min(numberOfSubsidiaries, 20);
            var additionalUpTo100Count = numberOfSubsidiaries > 100 ? 80 : Math.Max(0, numberOfSubsidiaries - 20);
            var additionalMoreThan100Count = numberOfSubsidiaries > 100 ? Math.Max(0, numberOfSubsidiaries - 100) : 0;

            AddFeeBreakdown(response, first20Count, first20Rate, $"First {first20Count} Subsidiaries Fee (£{Math.Truncate(first20Rate / 100m)} each)");

            if (additionalUpTo100Count > 0)
            {
                var endRange = additionalMoreThan100Count > 0 ? 100 : numberOfSubsidiaries;
                AddFeeBreakdown(response, additionalUpTo100Count, additionalUpTo100Rate, $"21st to {endRange}th Subsidiaries Fee (£{Math.Truncate(additionalUpTo100Rate / 100m)} each)");
            }

            if (additionalMoreThan100Count > 0)
            {
                AddFeeBreakdown(response, additionalMoreThan100Count, additionalMoreThan100Rate, $"101st to {numberOfSubsidiaries}th Subsidiaries Fee (£{Math.Truncate(additionalMoreThan100Rate / 100m)} each)");
            }
        }

        private void AddFeeBreakdown(RegistrationFeesResponseDto response, int count, decimal rate, string description)
        {
            if (count <= 0) return;

            var amount = count * rate;
            response.FeeBreakdowns.Add(CreateFeeBreakdown(description, amount));
        }

        private static FeeBreakdown CreateFeeBreakdown(string description, decimal amount) => new FeeBreakdown
        {
            Description = description,
            Amount = amount
        };
    }

}
