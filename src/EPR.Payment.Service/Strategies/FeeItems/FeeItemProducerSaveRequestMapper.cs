using EPR.Payment.Service.Common.Dtos.FeeItems;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Common.Dtos.Request.ResubmissionFees.Producer;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.Producer;
using EPR.Payment.Service.Common.Dtos.Response.ResubmissionFees.Producer;
using EPR.Payment.Service.Common.Enums;
using EPR.Payment.Service.Strategies.Interfaces.FeeItems;

namespace EPR.Payment.Service.Strategies.FeeItems
{
    public class FeeItemProducerSaveRequestMapper : IFeeItemProducerSaveRequestMapper
    {
        public FeeSummarySaveRequest BuildRegistrationFeeSummaryRecord(
          ProducerRegistrationFeesRequestV2Dto dto,
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
            if (resp?.ProducerRegistrationFee > 0)
            {
                lines.Add(new FeeSummaryLineRequest
                {
                    FeeTypeId = (int)FeeTypeIds.ProducerRegistrationFee,
                    UnitPrice = resp.ProducerRegistrationFee,
                    Quantity = 1,
                    Amount = resp.ProducerRegistrationFee
                });
            }

            void Sum(FeeTypeIds type, decimal amount)
            {
                if (amount > 0)
                {
                    lines.Add(new FeeSummaryLineRequest { FeeTypeId = (int)type, Amount = amount });
                }
            }

            var s = resp.SubsidiariesFeeBreakdown;
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

            Sum(FeeTypeIds.MemberRegistrationFee, memberRegistrationFee);
            Sum(FeeTypeIds.MemberLateRegistrationFee, memberLateRegistrationFee);
            Sum(FeeTypeIds.UnitOnlineMarketplaceFee, unitOmpFee);
            Sum(FeeTypeIds.SubsidiaryFee, subsidiaryFee);

            return new FeeSummarySaveRequest
            {
                FileId = dto.FileId ?? Guid.NewGuid(),
                ExternalId = dto.ExternalId ?? Guid.NewGuid(),
                ApplicationReferenceNumber = dto.ApplicationReferenceNumber,
                InvoiceDate = invoiceDate ?? DateTimeOffset.UtcNow,
                InvoicePeriod = invoicePeriod,
                PayerTypeId = payerTypeId,
                PayerId = dto.PayerId ?? 0,
                Lines = lines
            };
        }

        public FeeSummarySaveRequest BuildRegistrationResubmissionFeeSummaryRecord(
            ProducerResubmissionFeeRequestDto req,
            ProducerResubmissionFeeResponseDto result,
            int resubmissionFeeTypeId,
            DateTimeOffset invoicePeriod,
            int payerTypeId,
            DateTimeOffset? invoiceDate = null)
        {
            return new FeeSummarySaveRequest
            {
                FileId = req.FileId ?? Guid.NewGuid(),
                ExternalId = req.ExternalId ?? Guid.NewGuid(),
                ApplicationReferenceNumber = req.ReferenceNumber,
                InvoicePeriod = invoicePeriod,
                InvoiceDate = invoiceDate ?? DateTimeOffset.UtcNow,
                PayerTypeId = payerTypeId,
                PayerId = req.PayerId ?? 0,
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