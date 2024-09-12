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
            // Base Fee Breakdown
            if (response.BaseFee > 0)
            {
                response.FeeBreakdowns.Add(new FeeBreakdown
                {
                    Description = $"Base Fee (£{Math.Truncate(response.BaseFee / 100m)})", // Convert to pounds and truncate decimals
                    Amount = response.BaseFee
                });
            }

            // Online Market Fee Breakdown
            if (request.IsOnlineMarketplace)
            {
                response.FeeBreakdowns.Add(new FeeBreakdown
                {
                    Description = $"Online Market Fee (£{Math.Truncate(response.OnlineMarket / 100m)})", // Convert to pounds and truncate decimals
                    Amount = response.OnlineMarket
                });
            }

            // Subsidiaries Fee Breakdown
            if (request.NumberOfSubsidiaries > 0)
            {
                var regulator = RegulatorType.Create(request.Regulator);
                var first20SubsidiaryRate = await _feesRepository.GetFirst20SubsidiariesFeeAsync(regulator, cancellationToken);
                var additionalUpto100SubsidiaryRate = await _feesRepository.GetAdditionalUpTo100SubsidiariesFeeAsync(regulator, cancellationToken);
                var additionalMoreThan100SubsidiaryRate = await _feesRepository.GetAdditionalMoreThan100SubsidiariesFeeAsync(regulator, cancellationToken);

                var first20SubsidiariesCount = Math.Min(request.NumberOfSubsidiaries, 20);
                var additionalUpto100SubsidiariesCount = request.NumberOfSubsidiaries > 100 ? 80 : Math.Max(0, request.NumberOfSubsidiaries - 20);
                var additionalMoreThan100SubsidiariesCount = request.NumberOfSubsidiaries > 100 ? Math.Max(0, request.NumberOfSubsidiaries - 100) : 0;

                if (first20SubsidiariesCount > 0)
                {
                    var first20Fee = first20SubsidiariesCount * first20SubsidiaryRate;
                    response.FeeBreakdowns.Add(new FeeBreakdown
                    {
                        Description = $"First {first20SubsidiariesCount} Subsidiaries Fee (£{Math.Truncate(first20SubsidiaryRate / 100m)} each)", // Convert to pounds and truncate decimals
                        Amount = first20Fee
                    });
                }

                if (additionalUpto100SubsidiariesCount > 0)
                {
                    var additionalFee = additionalUpto100SubsidiariesCount * additionalUpto100SubsidiaryRate;
                    response.FeeBreakdowns.Add(new FeeBreakdown
                    {
                        Description = $"21st to {(additionalMoreThan100SubsidiariesCount > 0 ? 100 : request.NumberOfSubsidiaries)}th Subsidiaries Fee (£{Math.Truncate(additionalUpto100SubsidiaryRate / 100m)} each)", // Convert to pounds and truncate decimals
                        Amount = additionalFee
                    });
                }

                if (additionalMoreThan100SubsidiariesCount > 0)
                {
                    var additionalFee = additionalMoreThan100SubsidiariesCount * additionalMoreThan100SubsidiaryRate;
                    response.FeeBreakdowns.Add(new FeeBreakdown
                    {
                        Description = $"101st to {request.NumberOfSubsidiaries}th Subsidiaries Fee (£{Math.Truncate(additionalMoreThan100SubsidiaryRate / 100m)} each)", // Convert to pounds and truncate decimals
                        Amount = additionalFee
                    });
                }
            }
        }
    }
}
