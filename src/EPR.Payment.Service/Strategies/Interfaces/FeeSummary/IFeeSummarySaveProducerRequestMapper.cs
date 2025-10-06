using EPR.Payment.Service.Common.Dtos.FeeSummaries;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Common.Dtos.Request.ResubmissionFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Request.ResubmissionFees.Producer;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.Producer;
using EPR.Payment.Service.Common.Dtos.Response.ResubmissionFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Response.ResubmissionFees.Producer;

namespace EPR.Payment.Service.Strategies.Interfaces.FeeSummary
{
    public interface IFeeSummarySaveProducerRequestMapper
    {
        FeeSummarySaveRequest BuildRegistrationFeeSummaryRecord(
            ProducerRegistrationFeesRequestV2Dto dto,
            DateTimeOffset invoicePeriod,
            int payerTypeId,
            RegistrationFeesResponseDto resp,
            DateTimeOffset? invoiceDate = null);

        FeeSummarySaveRequest BuildRegistrationResubmissionFeeSummaryRecord(
            ProducerResubmissionFeeRequestDto req,
            ProducerResubmissionFeeResponseDto result,
            int resubmissionFeeTypeId,
            DateTimeOffset invoicePeriod,
            int payerTypeId,
            DateTimeOffset? invoiceDate = null);
    }
}