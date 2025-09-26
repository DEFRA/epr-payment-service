using EPR.Payment.Service.Common.Dtos.FeeSummaries;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Common.Dtos.Request.ResubmissionFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Request.ResubmissionFees.Producer;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.Producer;
using EPR.Payment.Service.Common.Dtos.Response.ResubmissionFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Response.ResubmissionFees.Producer;
using EPR.Payment.Service.Common.Enums;
using EPR.Payment.Service.Strategies.Interfaces.FeeSummary;

namespace EPR.Payment.Service.Strategies.FeeSummary
{
    public class FeeSummarySaveProducerRequestMapper : IFeeSummarySaveProducerRequestMapper
    {
        public FeeSummarySaveRequest BuildRegistrationFeeSummaryRecord(
            ProducerRegistrationFeesRequestDto dto, 
            DateTimeOffset invoicePeriod, 
            int payerTypeId, 
            RegistrationFeesResponseDto resp, 
            DateTimeOffset? invoiceDate = null)
        {
            decimal memberRegistrationFee = 0, memberLateRegistrationFee = 0, unitOmpFee = 0, subsidiaryFee = 0;
            memberRegistrationFee += resp.MemberRegistrationFee;
            memberLateRegistrationFee += resp.MemberLateRegistrationFee;
            unitOmpFee += resp.MemberOnlineMarketPlaceFee;
            subsidiaryFee += resp.SubsidiariesFee;

            var lines = new List<FeeSummaryLineRequest>();

            void Sum(FeeTypeIds type, decimal amount)
            {
                if (amount >= 0)
                {
                    lines.Add(new FeeSummaryLineRequest { FeeTypeId = (int)type, Amount = amount });
                }
            }
            Sum(FeeTypeIds.MemberRegistrationFee, memberRegistrationFee);
            Sum(FeeTypeIds.MemberLateRegistrationFee, memberLateRegistrationFee);
            Sum(FeeTypeIds.UnitOmpFee, unitOmpFee);
            Sum(FeeTypeIds.SubsidiaryFee, subsidiaryFee);

            return new FeeSummarySaveRequest
            {
                FileId = dto.FileId.Value,
                ExternalId = dto.ExternalId.Value,
                ApplicationReferenceNumber = dto.ApplicationReferenceNumber,
                InvoiceDate = invoiceDate ?? DateTimeOffset.UtcNow,
                InvoicePeriod = invoicePeriod,
                PayerTypeId = payerTypeId,
                PayerId = dto.PayerId.Value,
                Lines = lines
            };
        }

        public FeeSummarySaveRequest BuildRegistrationResubmissionFeeSummaryRecord(
            ProducerResubmissionFeeRequestDto req, 
            ProducerResubmissionFeeResponseDto result, 
            int resubmissionFeeTypeId, 
            DateTimeOffset invoicePeriod, 
            int payerTypeId, 
            DateTimeOffset? invoiceDate)
        {
            return new FeeSummarySaveRequest
            {
                FileId = req.FileId.Value,
                ExternalId = req.ExternalId.Value,
                ApplicationReferenceNumber = req.ReferenceNumber,
                InvoicePeriod = invoicePeriod,
                InvoiceDate = invoiceDate ?? DateTimeOffset.UtcNow,
                PayerTypeId = payerTypeId,
                PayerId = req.PayerId.Value,
                Lines = new[]
                {
                    new FeeSummaryLineRequest
                    {
                        FeeTypeId = resubmissionFeeTypeId,
                        UnitPrice  = result.TotalResubmissionFee,
                        Quantity   = 1,
                        Amount     = result.TotalResubmissionFee
                    }
                }
            };
        }       
    }    
}