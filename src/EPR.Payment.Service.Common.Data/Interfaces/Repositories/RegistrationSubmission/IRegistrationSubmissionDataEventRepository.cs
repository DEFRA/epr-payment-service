namespace EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationSubmission
{
    public interface IRegistrationSubmissionDataEventRepository
    {
        Task<Guid?> AddEventForLatestSubmissionAsync(
            Guid submissionId,
            string eventName,
            DateTime eventDate,
            CancellationToken cancellationToken);
    }
}
