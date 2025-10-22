using EPR.Payment.Service.Common.Dtos.FeeSummaries;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Request.ResubmissionFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Response.ResubmissionFees.ComplianceScheme;

namespace EPR.Payment.Service.Strategies.Interfaces.FeeSummary
{

    public interface IFeeSummarySaveRequestMapper
    {
        FeeSummarySaveRequest BuildComplianceSchemeRegistrationFeeSummaryRecord(
            ComplianceSchemeFeesRequestDto dto,
            DateTimeOffset invoicePeriod,
            int payerTypeId,
            ComplianceSchemeFeesResponseDto resp,
            DateTimeOffset? invoiceDate = null);

        FeeSummarySaveRequest BuildComplianceSchemeResubmissionFeeSummaryRecord(
            ComplianceSchemeResubmissionFeeRequestDto req,
            ComplianceSchemeResubmissionFeeResult result,
            int resubmissionFeeTypeId,
            DateTimeOffset invoicePeriod,
            int payerTypeId,
            DateTimeOffset? invoiceDate = null);
    }
}