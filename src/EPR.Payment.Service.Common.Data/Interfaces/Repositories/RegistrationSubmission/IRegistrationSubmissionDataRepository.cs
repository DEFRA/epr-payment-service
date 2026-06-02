using EPR.Payment.Service.Common.Data.DataModels;

namespace EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationSubmission
{
    public interface IRegistrationSubmissionDataRepository
    {
        Task<RegistrationSubmissionData?> GetBySubmissionAndFileAsync(Guid submissionId, Guid fileId, CancellationToken cancellationToken);

        Task<RegistrationSubmissionData?> GetLatestWithProducersAndSubsidiariesAsync(Guid submissionId, CancellationToken cancellationToken);

        Task<Guid> CreateAsync(RegistrationSubmissionData entity, CancellationToken cancellationToken);
    }
}
