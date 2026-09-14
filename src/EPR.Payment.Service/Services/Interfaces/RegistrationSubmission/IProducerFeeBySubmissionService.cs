using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.Producer;

namespace EPR.Payment.Service.Services.Interfaces.RegistrationSubmission
{
    public interface IProducerFeeBySubmissionService
    {
        Task<RegistrationFeesResponseDto?> GetFeesAsync(Guid submissionId, bool requireSubmittedForApproval, CancellationToken cancellationToken);
    }
}
