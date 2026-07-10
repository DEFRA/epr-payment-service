using EPR.Payment.Service.Common.Data.Interfaces;
using EPR.Payment.Service.Common.Dtos.Response.SubmissionPeriods;
using EPR.Payment.Service.Services.Interfaces.SubmissionPeriods;
using Microsoft.EntityFrameworkCore;

namespace EPR.Payment.Service.Services.SubmissionPeriods
{
    public class SubmissionPeriodsService : ISubmissionPeriodsService
    {
        private readonly IAppDbContext _dbContext;

        public SubmissionPeriodsService(IAppDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IReadOnlyList<SubmissionPeriodResponseDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.SubmissionPeriod
                .AsNoTracking()
                .OrderBy(p => p.RegistrationYear)
                .ThenBy(p => p.WindowType)
                .Select(p => new SubmissionPeriodResponseDto
                {
                    Id = p.Id,
                    WindowType = p.WindowType,
                    RegistrationYear = p.RegistrationYear,
                    OpeningDate = p.OpeningDate,
                    DeadlineDate = p.DeadlineDate,
                    ClosingDate = p.ClosingDate,
                })
                .ToListAsync(cancellationToken);
        }
    }
}
