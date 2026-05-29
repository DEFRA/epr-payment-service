namespace EPR.Payment.Service.Services.Interfaces.Registrations
{
    public interface IRegistrationSubmissionService
    {
        Task<bool> SubmissionExistsAsync(Guid submissionId, CancellationToken cancellationToken);
    }
}
