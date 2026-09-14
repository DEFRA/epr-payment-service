using EPR.Payment.Service.Common.Data.DataModels;
using EPR.Payment.Service.Services.RegistrationSubmission;

namespace EPR.Payment.Service.Services.Interfaces.RegistrationSubmission
{
    public interface IRegistrationFeeSnapshotHandler
    {
        Task HandleAsync(
            RegistrationSubmissionData latestRecord,
            DateTime submissionDate,
            SubmissionLifecycle lifecycle,
            CancellationToken cancellationToken);
    }
}
