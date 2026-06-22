using EPR.Payment.Service.Common.Dtos.Request.RegistrationSubmission;

namespace EPR.Payment.Service.Common.Services.Interfaces.RegistrationSubmission
{
    public interface IRegistrationSubmissionDataHandler
    {
        Task<Guid> HandleAsync(CreateRegistrationSubmissionDataRequest request, CancellationToken cancellationToken);
    }
}
