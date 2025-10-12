using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Dtos.FeeItems;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Request.ResubmissionFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Response.ResubmissionFees.ComplianceScheme;
using EPR.Payment.Service.Common.Enums;
using EPR.Payment.Service.Strategies.Interfaces.FeeItems;

namespace EPR.Payment.Service.Strategies.FeeItems
{
    public class FeeItemSaveRequestMapper : IFeeItemSaveRequestMapper
    {
        public FeeItemSaveRequest BuildComplianceSchemeRegistrationFeeSummaryRecord(
            ComplianceSchemeFeesRequestV2Dto req,
            ComplianceSchemeFeesResponseDto calc)
        {
            var lines = new List<FeeItemLine>();

            AddCsRegistrationLine(lines, calc);

            AddMemberLines(lines, calc?.ComplianceSchemeMembersWithFees);

            return BuildHeader(
                applicationReferenceNumber: req.ApplicationReferenceNumber,
                fileId: req.FileId,
                externalId: req.ExternalId,
                payerId: req.PayerId,
                payerTypeId: (int)PayerTypeIds.ComplianceScheme,
                invoicePeriod: new(req.SubmissionDate, TimeSpan.Zero),
                invoiceDate: DateTimeOffset.UtcNow,
                lines: lines);
        }

        public FeeItemSaveRequest BuildComplianceSchemeResubmissionFeeSummaryRecord(
            ComplianceSchemeResubmissionFeeRequestV2Dto req,
            ComplianceSchemeResubmissionFeeResult result,
            int resubmissionFeeTypeId)
        {
            var amount = result.TotalResubmissionFee;

            var lines = new List<FeeItemLine>
            {
                new()
                {
                    FeeTypeId = resubmissionFeeTypeId,
                    UnitPrice = amount,
                    Quantity  = 1,
                    Amount    = amount
                }
            };

            return BuildHeader(
                applicationReferenceNumber: req.ApplicationReferenceNumber,
                fileId: req.FileId,
                externalId: req.ExternalId,
                payerId: req.PayerId,
                payerTypeId: (int)PayerTypeIds.ComplianceScheme,
                invoicePeriod: new(req.SubmissionDate, TimeSpan.Zero),
                invoiceDate: DateTimeOffset.UtcNow,
                lines: lines);
        }

        [ExcludeFromCodeCoverage]
        private static FeeItemSaveRequest BuildHeader(
            string applicationReferenceNumber,
            Guid? fileId,
            Guid? externalId,
            int? payerId,
            int payerTypeId,
            DateTimeOffset invoicePeriod,
            DateTimeOffset invoiceDate,
            List<FeeItemLine> lines)
        {
            return new FeeItemSaveRequest
            {
                FileId = fileId ?? Guid.NewGuid(),
                ExternalId = externalId ?? Guid.NewGuid(),
                ApplicationReferenceNumber = applicationReferenceNumber,
                InvoiceDate = invoiceDate,
                InvoicePeriod = invoicePeriod,
                PayerTypeId = payerTypeId,
                PayerId = payerId ?? 0,
                Lines = lines
            };
        }

        private static void AddCsRegistrationLine(
            IList<FeeItemLine> lines,
            ComplianceSchemeFeesResponseDto? calc)
        {
            var regFee = calc?.ComplianceSchemeRegistrationFee ?? 0m;
            if (regFee > 0)
            {
                lines.Add(new FeeItemLine
                {
                    FeeTypeId = (int)FeeTypeIds.ComplianceSchemeRegistrationFee,
                    UnitPrice = regFee,
                    Quantity = 1,
                    Amount = regFee
                });
            }
        }

        private static void AddMemberLines(
            IList<FeeItemLine> lines,
            IEnumerable<ComplianceSchemeMembersWithFeesDto>? members)
        {
            foreach (var m in members ?? Enumerable.Empty<ComplianceSchemeMembersWithFeesDto>())
            {
                AddMemberFixedFee(lines, FeeTypeIds.MemberRegistrationFee, m.MemberRegistrationFee);
                AddMemberFixedFee(lines, FeeTypeIds.MemberOnlineMarketplaceFee, m.MemberOnlineMarketPlaceFee);
                AddMemberFixedFee(lines, FeeTypeIds.MemberLateRegistrationFee, m.MemberLateRegistrationFee);

                AddSubsidiaryBands(lines, m.SubsidiariesFeeBreakdown);
            }
        }

        private static void AddMemberFixedFee(
            IList<FeeItemLine> lines,
            FeeTypeIds feeType,
            decimal amount)
        {
            if (amount > 0)
            {
                lines.Add(new FeeItemLine
                {
                    FeeTypeId = (int)feeType,
                    UnitPrice = amount,
                    Quantity = 1,
                    Amount = amount
                });
            }
        }

        private static void AddSubsidiaryBands(
            IList<FeeItemLine> lines,
            SubsidiariesFeeBreakdown? s)
        {
            foreach (var b in s?.FeeBreakdowns ?? Enumerable.Empty<FeeBreakdown>())
            {
                if (b.UnitCount > 0 && b.UnitPrice > 0)
                {
                    lines.Add(new FeeItemLine
                    {
                        FeeTypeId = (int)MapBandFeeType(b.BandNumber),
                        UnitPrice = b.UnitPrice,
                        Quantity = b.UnitCount,
                        Amount = b.TotalPrice
                    });
                }
            }

            AddSubsidiaryOmp(lines, s);
        }

        [ExcludeFromCodeCoverage] 
        private static void AddSubsidiaryOmp(
            IList<FeeItemLine> lines,
            SubsidiariesFeeBreakdown? s)
        {
            if (s is null) return;

            if (s.TotalSubsidiariesOMPFees > 0 && s.UnitOMPFees > 0 && s.CountOfOMPSubsidiaries > 0)
            {
                lines.Add(new FeeItemLine
                {
                    FeeTypeId = (int)FeeTypeIds.UnitOnlineMarketplaceFee,
                    UnitPrice = s.UnitOMPFees,
                    Quantity = s.CountOfOMPSubsidiaries,
                    Amount = s.TotalSubsidiariesOMPFees
                });
            }
        }

        private static FeeTypeIds MapBandFeeType(int band) => band switch
        {
            1 => FeeTypeIds.BandNumber1,
            2 => FeeTypeIds.BandNumber2,
            3 => FeeTypeIds.BandNumber3,
            _ => FeeTypeIds.SubsidiaryFee
        };
    }
}
