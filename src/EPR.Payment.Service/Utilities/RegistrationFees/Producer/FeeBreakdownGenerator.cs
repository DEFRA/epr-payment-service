using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees;
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

            // Subsidiaries Fee Breakdown
            if (request.NumberOfSubsidiaries > 0)
            {
                var regulator = RegulatorType.Create(request.Regulator);
                var first20SubsidiaryRate = await _feesRepository.GetFirst20SubsidiariesFeeAsync(regulator, cancellationToken);
                var additionalSubsidiaryRate = await _feesRepository.GetAdditionalSubsidiariesFeeAsync(regulator, cancellationToken);

                var first20SubsidiariesCount = Math.Min(request.NumberOfSubsidiaries, 20);
                var additionalSubsidiariesCount = Math.Max(0, request.NumberOfSubsidiaries - 20);

                if (first20SubsidiariesCount > 0)
                {
                    var first20Fee = first20SubsidiariesCount * first20SubsidiaryRate;
                    response.FeeBreakdowns.Add(new FeeBreakdown
                    {
                        Description = $"First {first20SubsidiariesCount} Subsidiaries Fee (£{Math.Truncate(first20SubsidiaryRate / 100m)} each)", // Convert to pounds and truncate decimals
                        Amount = first20Fee
                    });
                }

                if (additionalSubsidiariesCount > 0)
                {
                    var additionalFee = additionalSubsidiariesCount * additionalSubsidiaryRate;
                    response.FeeBreakdowns.Add(new FeeBreakdown
                    {
                        Description = $"Next {additionalSubsidiariesCount} Subsidiaries Fee (£{Math.Truncate(additionalSubsidiaryRate / 100m)} each)", // Convert to pounds and truncate decimals
                        Amount = additionalFee
                    });
                }
            }
        }
    }
}