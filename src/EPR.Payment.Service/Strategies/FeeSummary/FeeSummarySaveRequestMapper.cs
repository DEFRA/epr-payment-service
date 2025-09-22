using EPR.Payment.Service.Common.Dtos.FeeSummaries;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Request.ResubmissionFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Response.ResubmissionFees.ComplianceScheme;
using EPR.Payment.Service.Common.Enums;
using EPR.Payment.Service.Strategies.Interfaces.FeeSummary;

namespace EPR.Payment.Service.Strategies.FeeSummary
{
    public class FeeSummarySaveRequestMapper : IFeeSummarySaveRequestMapper
    {
        public FeeSummarySaveRequest BuildComplianceSchemeRegistrationFeeSummaryRecord(
            ComplianceSchemeFeesRequestDto dto,
            DateTimeOffset invoicePeriod,
            int payerTypeId,
            ComplianceSchemeFeesResponseDto resp,
            DateTimeOffset? invoiceDate = null)
        {
            var members = resp.ComplianceSchemeMembersWithFees ?? new List<ComplianceSchemeMembersWithFeesDto>(0);

            decimal memberRegistrationFee = 0, memberLateRegistrationFee = 0, unitOmpFee = 0, subsidiaryFee = 0;
            foreach (var m in members)
            {
                memberRegistrationFee += m.MemberRegistrationFee;
                memberLateRegistrationFee += m.MemberLateRegistrationFee;
                unitOmpFee += m.MemberOnlineMarketPlaceFee;
                subsidiaryFee += m.SubsidiariesFee;
            }

            var lines = new List<FeeSummaryLineRequest>();
            
            void Sum(FeeTypeIds type, decimal amount)
            {
                if (amount >= 0)
                {
                    lines.Add(new FeeSummaryLineRequest { FeeTypeId = (int)type,  Amount = amount });
                }
            }
            Sum(FeeTypeIds.ComplianceSchemeRegistrationFee, resp.ComplianceSchemeRegistrationFee);
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

        
        public FeeSummarySaveRequest BuildComplianceSchemeResubmissionFeeSummaryRecord(
            ComplianceSchemeResubmissionFeeRequestDto req,
            ComplianceSchemeResubmissionFeeResult result,
            int resubmissionFeeTypeId,
            DateTimeOffset invoicePeriod,
            int payerTypeId,
            DateTimeOffset? invoiceDate = null)
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