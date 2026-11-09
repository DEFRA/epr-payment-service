using EPR.Payment.Service.Common.Services.Interfaces.RegistrationSubmission;
using EPR.Payment.Service.Options;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.Producer;
using EPR.Payment.Service.Services.Interfaces.RegistrationSubmission;
using EPR.Payment.Service.Services.RegistrationSubmission;
using Microsoft.Extensions.Options;

namespace EPR.Payment.Service.Services.Migration.RegistrationHistory
{
    public sealed record ReplayHandlers(
        IRegistrationSubmissionDataHandler SubmissionData,
        IRegistrationSubmittedForRegulatorApprovalHandler ApplicationSubmitted,
        IRegulatorRegistrationDecisionHandler Decision);

    public interface IRegistrationReplayHandlerFactory
    {
        ReplayHandlers Create(IServiceProvider scopedProvider, TimeProvider clock);
    }

    /// <summary>
    /// Builds the live registration handlers with the replay clock in place of TimeProvider.System, so
    /// CreatedDate ordering and the CalcDate/lateness fallback use the historic event time. The fee
    /// calculators are wrapped so subsidiary late fees are only charged from SubsidiaryLateFeeChargedFrom.
    /// </summary>
    public class RegistrationReplayHandlerFactory : IRegistrationReplayHandlerFactory
    {
        private readonly RegistrationHistoryMigrationOptions _options;

        public RegistrationReplayHandlerFactory(IOptions<RegistrationHistoryMigrationOptions> options)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public ReplayHandlers Create(IServiceProvider scopedProvider, TimeProvider clock)
        {
            ArgumentNullException.ThrowIfNull(scopedProvider);
            ArgumentNullException.ThrowIfNull(clock);

            IProducerFeesCalculatorService producerCalculator = new ReplayProducerFeesCalculator(
                scopedProvider.GetRequiredService<IProducerFeesCalculatorService>(), clock, _options.SubsidiaryLateFeeChargedFrom);
            IComplianceSchemeCalculatorService complianceSchemeCalculator = new ReplayComplianceSchemeCalculator(
                scopedProvider.GetRequiredService<IComplianceSchemeCalculatorService>(), clock, _options.SubsidiaryLateFeeChargedFrom);

            IRegistrationFeeSnapshotHandler snapshotHandler =
                ActivatorUtilities.CreateInstance<RegistrationFeeSnapshotHandler>(scopedProvider, producerCalculator, complianceSchemeCalculator, clock);

            return new ReplayHandlers(
                ActivatorUtilities.CreateInstance<RegistrationSubmissionDataHandler>(scopedProvider, clock),
                ActivatorUtilities.CreateInstance<RegistrationSubmittedForRegulatorApprovalHandler>(scopedProvider, snapshotHandler, clock),
                scopedProvider.GetRequiredService<IRegulatorRegistrationDecisionHandler>());
        }
    }
}
