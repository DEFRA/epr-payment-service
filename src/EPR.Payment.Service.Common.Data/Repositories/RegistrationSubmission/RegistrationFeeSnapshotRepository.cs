using EPR.Payment.Service.Common.Data.DataModels;
using EPR.Payment.Service.Common.Data.Interfaces;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationSubmission;
using Microsoft.EntityFrameworkCore;

namespace EPR.Payment.Service.Common.Data.Repositories.RegistrationSubmission
{
    public class RegistrationFeeSnapshotRepository : IRegistrationFeeSnapshotRepository
    {
        private readonly IAppDbContext _dataContext;

        public RegistrationFeeSnapshotRepository(IAppDbContext dataContext)
        {
            _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
        }

        public async Task<Guid> CreateAsync(RegistrationFeeSnapshot snapshot, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(snapshot);

            _dataContext.RegistrationFeeSnapshot.Add(snapshot);
            await _dataContext.SaveChangesAsync(cancellationToken);
            return snapshot.Id;
        }

        public Task<RegistrationFeeSnapshot?> GetByRegistrationSubmissionDataIdAsync(Guid registrationSubmissionDataId, CancellationToken cancellationToken)
        {
            return _dataContext.RegistrationFeeSnapshot
                .AsNoTracking()
                .Include(s => s.LineItems)
                .FirstOrDefaultAsync(s => s.RegistrationSubmissionDataId == registrationSubmissionDataId, cancellationToken);
        }
    }
}
