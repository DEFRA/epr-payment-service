using EPR.Payment.Service.Common.Dtos.FeeItems;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Request.ResubmissionFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Response.ResubmissionFees.ComplianceScheme;

namespace EPR.Payment.Service.Strategies.Interfaces.FeeItems
{
    public interface IFeeItemSaveRequestMapper
    {
        FeeItemSaveRequest BuildComplianceSchemeRegistrationFeeSummaryRecord(
            ComplianceSchemeFeesRequestV2Dto req,            
            ComplianceSchemeFeesResponseDto calc);

        FeeItemSaveRequest BuildComplianceSchemeResubmissionFeeSummaryRecord(
            ComplianceSchemeResubmissionFeeRequestV2Dto req,
            ComplianceSchemeResubmissionFeeResult result,
            int resubmissionFeeTypeId);
    }
}
