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
            ComplianceSchemeFeesRequestDto complianceSchemeFeesRequestDto,
            DateTimeOffset invoicePeriod,
            int payerTypeId,
            ComplianceSchemeFeesResponseDto calculationResponse,
            DateTimeOffset? invoiceDate = null)
        {
            var lines = new List<FeeSummaryLineRequest>();

            if (calculationResponse?.ComplianceSchemeRegistrationFee > 0)
            {
                lines.Add(new FeeSummaryLineRequest
                {
                    FeeTypeId = (int)FeeTypeIds.ComplianceSchemeRegistrationFee,
                    UnitPrice = calculationResponse.ComplianceSchemeRegistrationFee,
                    Quantity = 1,
                    Amount = calculationResponse.ComplianceSchemeRegistrationFee
                });
            }
            foreach (var m in calculationResponse?.ComplianceSchemeMembersWithFees ?? Enumerable.Empty<ComplianceSchemeMembersWithFeesDto>())
            {
                if (m.MemberRegistrationFee > 0)
                {
                    lines.Add(new FeeSummaryLineRequest
                    {
                        FeeTypeId = (int)FeeTypeIds.MemberRegistrationFee,
                        UnitPrice = m.MemberRegistrationFee,
                        Quantity = 1,
                        Amount = m.MemberRegistrationFee
                    });
                }

                if (m.MemberOnlineMarketPlaceFee > 0)
                {
                    lines.Add(new FeeSummaryLineRequest
                    {
                        FeeTypeId = (int)FeeTypeIds.UnitOmpFee,
                        UnitPrice = m.MemberOnlineMarketPlaceFee,
                        Quantity = 1,
                        Amount = m.MemberOnlineMarketPlaceFee
                    });
                }

                if (m.MemberLateRegistrationFee > 0)
                {
                    lines.Add(new FeeSummaryLineRequest
                    {
                        FeeTypeId = (int)FeeTypeIds.MemberLateRegistrationFee,
                        UnitPrice = m.MemberLateRegistrationFee,
                        Quantity = 1,
                        Amount = m.MemberLateRegistrationFee
                    });
                }

                var s = m.SubsidiariesFeeBreakdown;
                if (s != null)
                {
                    foreach (var b in s.FeeBreakdowns)
                    {
                        if (b.UnitCount > 0 && b.UnitPrice > 0)
                        {
                            lines.Add(new FeeSummaryLineRequest
                            {
                                FeeTypeId = (int)FeeTypeIds.SubsidiaryFee,
                                UnitPrice = b.UnitPrice,
                                Quantity = b.UnitCount,
                                Amount = b.TotalPrice
                            });
                        }
                    }

                    if (s.TotalSubsidiariesOMPFees > 0 && s.UnitOMPFees > 0 && s.CountOfOMPSubsidiaries > 0)
                    {
                        lines.Add(new FeeSummaryLineRequest
                        {
                            FeeTypeId = (int)FeeTypeIds.SubsidiaryFee,
                            UnitPrice = s.UnitOMPFees,
                            Quantity = s.CountOfOMPSubsidiaries,
                            Amount = s.TotalSubsidiariesOMPFees
                        });
                    }
                }
            }

            return new FeeSummarySaveRequest
            {
                FileId = complianceSchemeFeesRequestDto.FileId!.Value,
                ExternalId = complianceSchemeFeesRequestDto.ExternalId!.Value,
                ApplicationReferenceNumber = complianceSchemeFeesRequestDto.ApplicationReferenceNumber,
                InvoiceDate = invoiceDate ?? DateTimeOffset.UtcNow,
                InvoicePeriod = invoicePeriod,
                PayerTypeId = payerTypeId,
                PayerId = complianceSchemeFeesRequestDto.PayerId!.Value,
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
            var lineAmount = result.TotalResubmissionFee;

            return new FeeSummarySaveRequest
            {
                FileId = req.FileId!.Value,
                ExternalId = req.ExternalId!.Value,
                ApplicationReferenceNumber = req.ReferenceNumber,
                InvoiceDate = invoiceDate ?? DateTimeOffset.UtcNow,
                InvoicePeriod = invoicePeriod,
                PayerTypeId = payerTypeId,
                PayerId = req.PayerId!.Value,
                Lines = new List<FeeSummaryLineRequest>
                {
                    new FeeSummaryLineRequest
                    {
                        FeeTypeId = resubmissionFeeTypeId,
                        UnitPrice = lineAmount,
                        Quantity  = 1,
                        Amount    = lineAmount
                    }
                }
            };
        }
    }
}