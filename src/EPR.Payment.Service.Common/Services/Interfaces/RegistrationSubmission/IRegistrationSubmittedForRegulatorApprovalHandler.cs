using EPR.Payment.Service.Common.Dtos.Request.RegistrationSubmission;

namespace EPR.Payment.Service.Common.Services.Interfaces.RegistrationSubmission
{
    public interface IRegistrationSubmittedForRegulatorApprovalHandler
    {
        Task HandleAsync(RegistrationSubmittedForRegulatorApprovalRequest request, CancellationToken cancellationToken);
    }
}
