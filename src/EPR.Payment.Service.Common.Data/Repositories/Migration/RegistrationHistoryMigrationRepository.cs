using System.Data;
using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.Data.DataModels.Migration;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.Migration;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EPR.Payment.Service.Common.Data.Repositories.Migration
{
    [ExcludeFromCodeCoverage(Justification = "Raw SQL, app locks, bulk updates and transactions; covered by integration tests against SQL Server.")]
    public class RegistrationHistoryMigrationRepository : IRegistrationHistoryMigrationRepository
    {
        private const string AcquireAppLockSql =
            "EXEC @result = sp_getapplock @Resource = N'EPR.Payment.Service.RegistrationHistoryMigration', @LockMode = 'Exclusive', @LockOwner = 'Session', @LockTimeout = 0;";

        private const string ReleaseAppLockSql =
            "EXEC sp_releaseapplock @Resource = N'EPR.Payment.Service.RegistrationHistoryMigration', @LockOwner = 'Session';";

        private const int ClearDownChunkSize = 1000;

        private readonly AppDbContext _context;

        public RegistrationHistoryMigrationRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> TryAcquireRunLockAsync(CancellationToken cancellationToken)
        {
            await _context.Database.OpenConnectionAsync(cancellationToken);

            var result = new SqlParameter("@result", SqlDbType.Int) { Direction = ParameterDirection.Output };
            await _context.Database.ExecuteSqlRawAsync(AcquireAppLockSql, new object[] { result }, cancellationToken);

            return result.Value is int code && code >= 0;
        }

        public async Task ReleaseRunLockAsync(CancellationToken cancellationToken)
        {
            await _context.Database.ExecuteSqlRawAsync(ReleaseAppLockSql, cancellationToken);
            await _context.Database.CloseConnectionAsync();
        }

        public Task<MigrationRun?> GetRunAsync(string runKey, CancellationToken cancellationToken) =>
            _context.MigrationRun.SingleOrDefaultAsync(r => r.RunKey == runKey, cancellationToken);

        public async Task<MigrationRun> CreateRunAsync(string runKey, string instanceId, DateTimeOffset startedAt, CancellationToken cancellationToken)
        {
            var run = new MigrationRun
            {
                Id = Guid.NewGuid(),
                RunKey = runKey,
                InstanceId = instanceId,
                StartedAt = startedAt,
                Status = MigrationRunStatus.Running,
            };

            _context.MigrationRun.Add(run);
            await _context.SaveChangesAsync(cancellationToken);
            return run;
        }

        public Task SaveRunAsync(MigrationRun run, CancellationToken cancellationToken)
        {
            if (_context.Entry(run).State == EntityState.Detached)
            {
                _context.MigrationRun.Update(run);
            }

            return _context.SaveChangesAsync(cancellationToken);
        }

        public async Task ResetStagingAsync(Guid runId, CancellationToken cancellationToken)
        {
            await _context.RegistrationSubmissionMigration.Where(s => s.RunId == runId).ExecuteDeleteAsync(cancellationToken);
            await _context.RegistrationHistoryStaging.Where(s => s.RunId == runId).ExecuteDeleteAsync(cancellationToken);
        }

        public async Task AddStagingBatchAsync(IReadOnlyCollection<RegistrationHistoryStaging> rows, CancellationToken cancellationToken)
        {
            _context.RegistrationHistoryStaging.AddRange(rows);
            await _context.SaveChangesAsync(cancellationToken);

            foreach (var row in rows)
            {
                _context.Entry(row).State = EntityState.Detached;
            }
        }

        public Task<int> CreatePendingSubmissionsAsync(Guid runId, CancellationToken cancellationToken) =>
            _context.Database.ExecuteSqlInterpolatedAsync(
                $@"INSERT INTO [migration].[RegistrationSubmissionMigration] ([Id], [RunId], [SubmissionId], [Status], [FilesMigrated], [EventsReplayed], [EventsSkipped])
                   SELECT NEWID(), {runId}, s.[SubmissionId], {nameof(SubmissionMigrationStatus.Pending)}, 0, 0, 0
                   FROM (SELECT DISTINCT [SubmissionId] FROM [migration].[RegistrationHistoryStaging]
                         WHERE [RunId] = {runId} AND [SubmissionId] IS NOT NULL) s;",
                cancellationToken);

        public async Task<IReadOnlyList<RegistrationSubmissionMigration>> GetPendingSubmissionsAsync(Guid runId, CancellationToken cancellationToken) =>
            await _context.RegistrationSubmissionMigration
                .AsNoTracking()
                .Where(s => s.RunId == runId && s.Status == SubmissionMigrationStatus.Pending)
                .OrderBy(s => s.SubmissionId)
                .ToListAsync(cancellationToken);

        public async Task<IReadOnlyList<RegistrationHistoryStaging>> GetStagingForSubmissionAsync(Guid runId, string submissionId, CancellationToken cancellationToken) =>
            await _context.RegistrationHistoryStaging
                .AsNoTracking()
                .Where(s => s.RunId == runId && s.SubmissionId == submissionId)
                .OrderBy(s => s.ReplayTs)
                .ThenBy(s => s.EventTypeOrder)
                .ThenBy(s => s.Id)
                .ToListAsync(cancellationToken);

        public async Task UpdateSubmissionAsync(RegistrationSubmissionMigration submission, CancellationToken cancellationToken)
        {
            _context.RegistrationSubmissionMigration.Update(submission);
            await _context.SaveChangesAsync(cancellationToken);
            _context.Entry(submission).State = EntityState.Detached;
        }

        public async Task<MigrationStatusCounts> GetStatusCountsAsync(Guid runId, CancellationToken cancellationToken)
        {
            var counts = await _context.RegistrationSubmissionMigration
                .AsNoTracking()
                .Where(s => s.RunId == runId)
                .GroupBy(s => s.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync(cancellationToken);

            int CountOf(SubmissionMigrationStatus status) => counts.Where(c => c.Status == status).Sum(c => c.Count);

            return new MigrationStatusCounts(
                counts.Sum(c => c.Count),
                CountOf(SubmissionMigrationStatus.Pending),
                CountOf(SubmissionMigrationStatus.Succeeded),
                CountOf(SubmissionMigrationStatus.Failed),
                CountOf(SubmissionMigrationStatus.NotRequired));
        }

        public async Task<IReadOnlyList<SubmissionPeriod>> GetSubmissionPeriodsAsync(CancellationToken cancellationToken) =>
            await _context.SubmissionPeriod.AsNoTracking().ToListAsync(cancellationToken);

        public Task<bool> SubmissionExistsAsync(Guid submissionId, CancellationToken cancellationToken) =>
            _context.RegistrationSubmissionData.AsNoTracking().AnyAsync(r => r.SubmissionId == submissionId, cancellationToken);

        public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken) =>
            _context.Database.BeginTransactionAsync(cancellationToken);

        public async Task AddMigratedEntityAsync(MigratedEntity entity, CancellationToken cancellationToken)
        {
            _context.MigratedEntity.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<ClearDownResult> ClearDownAsync(Guid runId, CancellationToken cancellationToken)
        {
            var dataIds = await _context.MigratedEntity
                .AsNoTracking()
                .Where(m => m.RunId == runId)
                .Select(m => m.RegistrationSubmissionDataId)
                .Distinct()
                .ToListAsync(cancellationToken);

            var unlinkedPaymentIds = new List<int>();
            var deleted = 0;

            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            foreach (var chunk in dataIds.Chunk(ClearDownChunkSize))
            {
                var linkedPayments = _context.Payment.Where(p => p.RegistrationSubmissionDataId != null && chunk.Contains(p.RegistrationSubmissionDataId.Value));

                unlinkedPaymentIds.AddRange(await linkedPayments.Select(p => p.Id).ToListAsync(cancellationToken));
                await linkedPayments.ExecuteUpdateAsync(s => s.SetProperty(p => p.RegistrationSubmissionDataId, (Guid?)null), cancellationToken);

                // Producers, subsidiaries, events, snapshots and line items are removed by ON DELETE CASCADE.
                deleted += await _context.RegistrationSubmissionData.Where(r => chunk.Contains(r.Id)).ExecuteDeleteAsync(cancellationToken);
            }

            await _context.RegistrationSubmissionMigration
                .Where(s => s.RunId == runId && s.Status == SubmissionMigrationStatus.Succeeded)
                .ExecuteUpdateAsync(s => s.SetProperty(x => x.Status, SubmissionMigrationStatus.ClearedDown), cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return new ClearDownResult(deleted, unlinkedPaymentIds);
        }
    }
}
