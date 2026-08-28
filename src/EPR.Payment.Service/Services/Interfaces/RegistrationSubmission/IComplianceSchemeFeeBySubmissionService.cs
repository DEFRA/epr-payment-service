using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.ComplianceScheme;

namespace EPR.Payment.Service.Services.Interfaces.RegistrationSubmission
{
    public interface IComplianceSchemeFeeBySubmissionService
    {
        Task<ComplianceSchemeFeesResponseDto?> GetFeesAsync(Guid submissionId, bool requireSubmittedForApproval, CancellationToken cancellationToken);
    }
}
