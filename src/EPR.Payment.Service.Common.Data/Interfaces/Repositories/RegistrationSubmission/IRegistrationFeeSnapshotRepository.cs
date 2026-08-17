using EPR.Payment.Service.Common.Data.DataModels;

namespace EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationSubmission
{
    public interface IRegistrationFeeSnapshotRepository
    {
        Task<Guid> CreateAsync(RegistrationFeeSnapshot snapshot, CancellationToken cancellationToken);

        Task<RegistrationFeeSnapshot?> GetByRegistrationSubmissionDataIdAsync(Guid registrationSubmissionDataId, CancellationToken cancellationToken);
    }
}
