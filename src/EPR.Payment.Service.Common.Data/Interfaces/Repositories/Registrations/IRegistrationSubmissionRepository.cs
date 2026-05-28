namespace EPR.Payment.Service.Common.Data.Interfaces.Repositories.Registrations
{
    public interface IRegistrationSubmissionRepository
    {
        Task<bool> SubmissionExistsAsync(Guid submissionId, CancellationToken cancellationToken);
    }
}
