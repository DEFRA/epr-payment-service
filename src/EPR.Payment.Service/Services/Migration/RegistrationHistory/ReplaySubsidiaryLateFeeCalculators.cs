using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.Producer;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.Producer;

namespace EPR.Payment.Service.Services.Migration.RegistrationHistory
{
    /// <summary>
    /// Replay-only calculator: no subsidiary late fees for applications submitted before the fee was
    /// actually charged, as the live service did not charge them at the time.
    /// </summary>
    public sealed class ReplayProducerFeesCalculator(
        IProducerFeesCalculatorService inner,
        TimeProvider clock,
        DateTime subsidiaryLateFeeChargedFrom) : IProducerFeesCalculatorService
    {
        public Task<RegistrationFeesResponseDto> CalculateFeesAsync(ProducerRegistrationFeesRequestDto request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            if (clock.GetUtcNow().UtcDateTime < subsidiaryLateFeeChargedFrom)
            {
                request.NumberOfLateSubsidiaries = 0;
            }

            return inner.CalculateFeesAsync(request, cancellationToken);
        }
    }

    /// <inheritdoc cref="ReplayProducerFeesCalculator"/>
    public sealed class ReplayComplianceSchemeCalculator(
        IComplianceSchemeCalculatorService inner,
        TimeProvider clock,
        DateTime subsidiaryLateFeeChargedFrom) : IComplianceSchemeCalculatorService
    {
        public Task<ComplianceSchemeFeesResponseDto> CalculateFeesAsync(ComplianceSchemeFeesRequestDto request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            if (clock.GetUtcNow().UtcDateTime < subsidiaryLateFeeChargedFrom)
            {
                foreach (var member in request.ComplianceSchemeMembers)
                {
                    member.NumberOfLateSubsidiaries = 0;
                }
            }

            return inner.CalculateFeesAsync(request, cancellationToken);
        }
    }
}
