using EPR.Payment.Service.Common.Services.Interfaces.RegistrationSubmission;
using EPR.Payment.Service.Services.Interfaces.RegistrationSubmission;
using EPR.Payment.Service.Services.RegistrationSubmission;

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
    /// CreatedDate ordering and the CalcDate/lateness fallback use the historic event time.
    /// </summary>
    public class RegistrationReplayHandlerFactory : IRegistrationReplayHandlerFactory
    {
        public ReplayHandlers Create(IServiceProvider scopedProvider, TimeProvider clock)
        {
            ArgumentNullException.ThrowIfNull(scopedProvider);
            ArgumentNullException.ThrowIfNull(clock);

            IRegistrationFeeSnapshotHandler snapshotHandler =
                ActivatorUtilities.CreateInstance<RegistrationFeeSnapshotHandler>(scopedProvider, clock);

            return new ReplayHandlers(
                ActivatorUtilities.CreateInstance<RegistrationSubmissionDataHandler>(scopedProvider, clock),
                ActivatorUtilities.CreateInstance<RegistrationSubmittedForRegulatorApprovalHandler>(scopedProvider, snapshotHandler, clock),
                scopedProvider.GetRequiredService<IRegulatorRegistrationDecisionHandler>());
        }
    }
}
