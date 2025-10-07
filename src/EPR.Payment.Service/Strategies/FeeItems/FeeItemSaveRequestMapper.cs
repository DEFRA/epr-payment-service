using EPR.Payment.Service.Common.Dtos.FeeItems;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Request.ResubmissionFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Response.ResubmissionFees.ComplianceScheme;
using EPR.Payment.Service.Common.Enums;
using EPR.Payment.Service.Strategies.Interfaces.FeeItems;

namespace EPR.Payment.Service.Strategies.FeeItems
{
    public class FeeItemSaveRequestMapper : IFeeItemSaveRequestMapper
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
                        FeeTypeId = (int)FeeTypeIds.MemberOnlineMarketplaceFee,
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
                if (s?.FeeBreakdowns != null)
                {
                    foreach (var b in s.FeeBreakdowns)
                    {
                        if (b.UnitCount > 0 && b.UnitPrice > 0)
                        {
                            var feeTypeForBand = b.BandNumber switch
                            {
                                1 => FeeTypeIds.BandNumber1,
                                2 => FeeTypeIds.BandNumber2,
                                3 => FeeTypeIds.BandNumber3,
                                _ => FeeTypeIds.SubsidiaryFee
                            };

                            lines.Add(new FeeSummaryLineRequest
                            {
                                FeeTypeId = (int)feeTypeForBand,
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
                            FeeTypeId = (int)FeeTypeIds.UnitOnlineMarketplaceFee,
                            UnitPrice = s.UnitOMPFees,
                            Quantity = s.CountOfOMPSubsidiaries,
                            Amount = s.TotalSubsidiariesOMPFees
                        });
                    }
                }
            }

            var fileId = complianceSchemeFeesRequestDto.FileId ?? Guid.NewGuid();
            var externalId = complianceSchemeFeesRequestDto.ExternalId ?? Guid.NewGuid();
            var payerId = complianceSchemeFeesRequestDto.PayerId ?? 0;

            return new FeeSummarySaveRequest
            {
                FileId = fileId,
                ExternalId = externalId,
                ApplicationReferenceNumber = complianceSchemeFeesRequestDto.ApplicationReferenceNumber,
                InvoiceDate = invoiceDate ?? DateTimeOffset.UtcNow,
                InvoicePeriod = invoicePeriod,
                PayerTypeId = payerTypeId,
                PayerId = payerId,
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

            var fileId = req.FileId ?? Guid.NewGuid();
            var externalId = req.ExternalId ?? Guid.NewGuid();
            var payerId = req.PayerId ?? 0;

            return new FeeSummarySaveRequest
            {
                FileId = fileId,
                ExternalId = externalId,
                ApplicationReferenceNumber = req.ReferenceNumber,
                InvoiceDate = invoiceDate ?? DateTimeOffset.UtcNow,
                InvoicePeriod = invoicePeriod,
                PayerTypeId = payerTypeId,
                PayerId = payerId,
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
