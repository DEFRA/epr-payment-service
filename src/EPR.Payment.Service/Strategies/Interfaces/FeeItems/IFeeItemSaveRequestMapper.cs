using EPR.Payment.Service.Common.Dtos.FeeItems;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Request.ResubmissionFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Response.ResubmissionFees.ComplianceScheme;

namespace EPR.Payment.Service.Strategies.Interfaces.FeeItems
{
    public interface IFeeItemSaveRequestMapper
    {
        FeeSummarySaveRequest BuildComplianceSchemeRegistrationFeeSummaryRecord(
            ComplianceSchemeFeesRequestDto complianceSchemeFeesRequestDto,
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
