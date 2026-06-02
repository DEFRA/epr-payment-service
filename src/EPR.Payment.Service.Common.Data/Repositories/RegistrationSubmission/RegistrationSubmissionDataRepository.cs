using EPR.Payment.Service.Common.Data.DataModels;
using EPR.Payment.Service.Common.Data.Interfaces;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationSubmission;
using Microsoft.EntityFrameworkCore;

namespace EPR.Payment.Service.Common.Data.Repositories.RegistrationSubmission
{
    public class RegistrationSubmissionDataRepository : IRegistrationSubmissionDataRepository
    {
        private readonly IAppDbContext _dataContext;

        public RegistrationSubmissionDataRepository(IAppDbContext dataContext)
        {
            _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
        }

        public Task<RegistrationSubmissionData?> GetBySubmissionAndFileAsync(Guid submissionId, Guid fileId, CancellationToken cancellationToken)
        {
            return _dataContext.RegistrationSubmissionData
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.SubmissionId == submissionId && r.FileId == fileId, cancellationToken);
        }

        public Task<RegistrationSubmissionData?> GetLatestWithProducersAndSubsidiariesAsync(Guid submissionId, CancellationToken cancellationToken)
        {
            return _dataContext.RegistrationSubmissionData
                .AsNoTracking()
                .Include(r => r.Producers)
                    .ThenInclude(p => p.Subsidiaries)
                .Where(r => r.SubmissionId == submissionId)
                .OrderByDescending(r => r.CreatedDate)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Guid> CreateAsync(RegistrationSubmissionData entity, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(entity);

            _dataContext.RegistrationSubmissionData.Add(entity);
            await _dataContext.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }
    }
}
