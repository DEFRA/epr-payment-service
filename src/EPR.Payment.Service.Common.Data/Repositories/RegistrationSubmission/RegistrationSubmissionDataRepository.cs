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

        public Task<RegistrationSubmissionData?> GetByRegistrationBlobNameAsync(string registrationBlobName, CancellationToken cancellationToken)
        {
            return _dataContext.RegistrationSubmissionData
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.RegistrationBlobName == registrationBlobName, cancellationToken);
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

        public async Task<IReadOnlyList<RegistrationSubmissionData>> GetAllForSubmissionAsync(Guid submissionId, CancellationToken cancellationToken)
        {
            return await _dataContext.RegistrationSubmissionData
                .AsNoTracking()
                .Include(r => r.Producers)
                    .ThenInclude(p => p.Subsidiaries)
                .Include(r => r.Events)
                .Include(r => r.SubmissionPeriodWindow)
                .Where(r => r.SubmissionId == submissionId)
                .OrderBy(r => r.CreatedDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<Guid> CreateAsync(RegistrationSubmissionData entity, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(entity);

            _dataContext.RegistrationSubmissionData.Add(entity);
            await _dataContext.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }

        public async Task<Guid?> GetLatestIdByApplicationReferenceNumberAsync(string applicationReferenceNumber, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(applicationReferenceNumber))
            {
                return null;
            }

            var latest = await _dataContext.RegistrationSubmissionData
                .AsNoTracking()
                .Where(r => r.ApplicationReferenceNumber == applicationReferenceNumber)
                .OrderByDescending(r => r.CreatedDate)
                .Select(r => (Guid?)r.Id)
                .FirstOrDefaultAsync(cancellationToken);

            return latest;
        }
    }
}
