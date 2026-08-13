using EPR.Payment.Service.Common.Data.DataModels;
using EPR.Payment.Service.Common.Data.Interfaces;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationSubmission;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EPR.Payment.Service.Common.Data.Repositories.RegistrationSubmission
{
    public class RegistrationSubmissionDataEventRepository : IRegistrationSubmissionDataEventRepository
    {
        private readonly IAppDbContext _dataContext;
        private readonly ILogger<RegistrationSubmissionDataEventRepository> _logger;

        public RegistrationSubmissionDataEventRepository(
            IAppDbContext dataContext,
            ILogger<RegistrationSubmissionDataEventRepository> logger)
        {
            _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Guid?> AddEventForLatestSubmissionAsync(
            Guid submissionId,
            string eventName,
            DateTime eventDate,
            CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(eventName);

            var latest = await _dataContext.RegistrationSubmissionData
                .AsNoTracking()
                .Where(r => r.SubmissionId == submissionId)
                .OrderByDescending(r => r.CreatedDate)
                .Select(r => new { r.Id })
                .FirstOrDefaultAsync(cancellationToken);

            if (latest is null)
            {
                return null;
            }

            var entity = new RegistrationSubmissionDataEvent
            {
                RegistrationSubmissionDataId = latest.Id,
                EventName = eventName,
                EventDate = eventDate,
                CreatedDate = DateTimeOffset.UtcNow,
            };

            _dataContext.RegistrationSubmissionDataEvents.Add(entity);

            try
            {
                await _dataContext.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex) when (IsUniqueConstraintViolation(ex))
            {
                _logger.LogDebug(
                    ex,
                    "Duplicate {EventName} event ignored for RegistrationSubmissionDataId {RegistrationSubmissionDataId} at {EventDate}",
                    eventName,
                    latest.Id,
                    eventDate);

                return latest.Id;
            }

            return entity.Id;
        }

        private static bool IsUniqueConstraintViolation(DbUpdateException ex)
        {
            var message = ex.InnerException?.Message ?? ex.Message;
            return message.Contains("unique", StringComparison.OrdinalIgnoreCase)
                || message.Contains("duplicate key", StringComparison.OrdinalIgnoreCase);
        }
    }
}
