using EPR.Payment.Service.Common.Dtos.Response.SubmissionPeriods;

namespace EPR.Payment.Service.Services.Interfaces.SubmissionPeriods
{
    public interface ISubmissionPeriodsService
    {
        Task<IReadOnlyList<SubmissionPeriodResponseDto>> GetAllAsync(CancellationToken cancellationToken);
    }
}
