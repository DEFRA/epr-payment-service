using EPR.Payment.Service.Common.Dtos.Request.RegistrationSubmission;

namespace EPR.Payment.Service.Common.Services.Interfaces.RegistrationSubmission
{
    public interface IRegulatorRegistrationDecisionHandler
    {
        Task HandleAsync(RegulatorRegistrationDecisionRequest request, CancellationToken cancellationToken);
    }
}
