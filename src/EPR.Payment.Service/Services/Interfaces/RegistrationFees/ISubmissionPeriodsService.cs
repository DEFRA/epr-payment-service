using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees;

namespace EPR.Payment.Service.Services.Interfaces.RegistrationFees
{
    public interface ISubmissionPeriodsService
    {
        Task<IReadOnlyList<SubmissionPeriodResponseDto>> GetAllAsync(CancellationToken cancellationToken);
    }
}
