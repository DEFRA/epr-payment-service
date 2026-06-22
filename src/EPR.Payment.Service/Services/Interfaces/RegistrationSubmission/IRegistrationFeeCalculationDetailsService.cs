using EPR.Payment.Service.Common.Dtos.Response.RegistrationSubmission;

namespace EPR.Payment.Service.Services.Interfaces.RegistrationSubmission
{
    public interface IRegistrationFeeCalculationDetailsService
    {
        Task<IReadOnlyList<RegistrationFeeCalculationDetailsDto>> GetAsync(Guid submissionId, CancellationToken cancellationToken);
    }
}
