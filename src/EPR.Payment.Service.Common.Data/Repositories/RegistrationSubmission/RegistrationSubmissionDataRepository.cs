using EPR.Payment.Service.Common.Data.DataModels;
using EPR.Payment.Service.Common.Data.Interfaces;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationSubmission;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EPR.Payment.Service.Common.Data.Repositories.RegistrationSubmission
{
    public class RegistrationSubmissionDataRepository : IRegistrationSubmissionDataRepository
    {
        private const int SqlDuplicateKeyOnUniqueIndex = 2601;
        private const int SqlUniqueConstraintViolation = 2627;

        private readonly IAppDbContext _dataContext;
        private readonly ILogger<RegistrationSubmissionDataRepository> _logger;

        public RegistrationSubmissionDataRepository(
            IAppDbContext dataContext,
            ILogger<RegistrationSubmissionDataRepository> logger)
        {
            _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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

        public async Task<Guid> CreateAsync(RegistrationSubmissionData entity, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(entity);

            _dataContext.RegistrationSubmissionData.Add(entity);

            try
            {
                await _dataContext.SaveChangesAsync(cancellationToken);
                return entity.Id;
            }
            catch (DbUpdateException ex) when (IsUniqueConstraintViolation(ex))
            {
                var existing = await GetByRegistrationBlobNameAsync(entity.RegistrationBlobName, cancellationToken);
                if (existing is null)
                {
                    throw;
                }

                _logger.LogInformation(
                    "Concurrent write detected for RegistrationBlobName {RegistrationBlobName}; using winning snapshot {ExistingId}.",
                    entity.RegistrationBlobName,
                    existing.Id);

                return existing.Id;
            }
        }

        private static bool IsUniqueConstraintViolation(DbUpdateException ex) =>
            ex.InnerException is SqlException sqlEx
                && (sqlEx.Number == SqlDuplicateKeyOnUniqueIndex
                    || sqlEx.Number == SqlUniqueConstraintViolation);
    }
}
