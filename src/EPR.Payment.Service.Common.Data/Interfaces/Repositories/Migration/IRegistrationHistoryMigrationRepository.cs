using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.Data.DataModels.Migration;
using Microsoft.EntityFrameworkCore.Storage;

namespace EPR.Payment.Service.Common.Data.Interfaces.Repositories.Migration
{
    public interface IRegistrationHistoryMigrationRepository
    {
        /// <summary>Session-scoped SQL app lock so only one replica runs the migration; held until the scope is disposed.</summary>
        Task<bool> TryAcquireRunLockAsync(CancellationToken cancellationToken);

        Task ReleaseRunLockAsync(CancellationToken cancellationToken);

        Task<MigrationRun?> GetRunAsync(string runKey, CancellationToken cancellationToken);

        Task<MigrationRun> CreateRunAsync(string runKey, string instanceId, DateTimeOffset startedAt, CancellationToken cancellationToken);

        Task SaveRunAsync(MigrationRun run, CancellationToken cancellationToken);

        Task ResetStagingAsync(Guid runId, CancellationToken cancellationToken);

        Task AddStagingBatchAsync(IReadOnlyCollection<RegistrationHistoryStaging> rows, CancellationToken cancellationToken);

        Task<int> CreatePendingSubmissionsAsync(Guid runId, CancellationToken cancellationToken);

        Task<IReadOnlyList<RegistrationSubmissionMigration>> GetPendingSubmissionsAsync(Guid runId, CancellationToken cancellationToken);

        Task<IReadOnlyList<RegistrationHistoryStaging>> GetStagingForSubmissionAsync(Guid runId, string submissionId, CancellationToken cancellationToken);

        Task UpdateSubmissionAsync(RegistrationSubmissionMigration submission, CancellationToken cancellationToken);

        Task<MigrationStatusCounts> GetStatusCountsAsync(Guid runId, CancellationToken cancellationToken);

        Task<IReadOnlyList<SubmissionPeriod>> GetSubmissionPeriodsAsync(CancellationToken cancellationToken);

        Task<bool> SubmissionExistsAsync(Guid submissionId, CancellationToken cancellationToken);

        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);

        Task AddMigratedEntityAsync(MigratedEntity entity, CancellationToken cancellationToken);

        Task<ClearDownResult> ClearDownAsync(Guid runId, CancellationToken cancellationToken);
    }
}
