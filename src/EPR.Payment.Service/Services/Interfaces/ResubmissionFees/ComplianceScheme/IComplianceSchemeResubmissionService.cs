using EPR.Payment.Service.Common.Dtos.Request.ResubmissionFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Response.ResubmissionFees.ComplianceScheme;

namespace EPR.Payment.Service.Services.Interfaces.ResubmissionFees.ComplianceScheme
{
    public interface IComplianceSchemeResubmissionService
    {
        Task<ComplianceSchemeResubmissionFeeResult> CalculateResubmissionFeeAsync(ComplianceSchemeResubmissionFeeRequestDto request, CancellationToken cancellationToken);
    }
}