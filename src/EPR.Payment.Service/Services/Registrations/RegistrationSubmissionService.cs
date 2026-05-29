using EPR.Payment.Service.Common.Data.Interfaces.Repositories.Registrations;
using EPR.Payment.Service.Services.Interfaces.Registrations;

namespace EPR.Payment.Service.Services.Registrations
{
    public class RegistrationSubmissionService : IRegistrationSubmissionService
    {
        private readonly IRegistrationSubmissionRepository _repository;

        public RegistrationSubmissionService(IRegistrationSubmissionRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<bool> SubmissionExistsAsync(Guid submissionId, CancellationToken cancellationToken)
        {
            return await _repository.SubmissionExistsAsync(submissionId, cancellationToken);
        }
    }
}
