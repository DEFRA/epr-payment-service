using EPR.Payment.Service.Common.Data.Interfaces;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.Registrations;
using Microsoft.EntityFrameworkCore;

namespace EPR.Payment.Service.Common.Data.Repositories.Registrations
{
    public class RegistrationSubmissionRepository : IRegistrationSubmissionRepository
    {
        private readonly IAppDbContext _dataContext;

        public RegistrationSubmissionRepository(IAppDbContext dataContext)
        {
            _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
        }

        public async Task<bool> SubmissionExistsAsync(Guid submissionId, CancellationToken cancellationToken)
        {
            return await _dataContext.RegistrationSubmissionData
                .AnyAsync(r => r.SubmissionId == submissionId, cancellationToken);
        }
    }
}
