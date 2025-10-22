using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Request.ResubmissionFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Response.ResubmissionFees.ComplianceScheme;
using EPR.Payment.Service.Common.Enums;
using EPR.Payment.Service.Strategies.FeeSummary;
using FluentAssertions;

namespace EPR.Payment.Service.UnitTests.Strategies.FeeSummary
{
    [TestClass]
    public class FeeSummarySaveRequestMapperTests
    {
        [TestMethod]
        public void BuildComplianceSchemeRegistrationFeeSummaryRecord_SumsMembersAndSetsHeaders()
        {
            // Arrange
            var mapper = new FeeSummarySaveRequestMapper();

            var dto = new ComplianceSchemeFeesRequestDto
            {
                Regulator = "GB-ENG",
                FileId = Guid.NewGuid(),
                ExternalId = Guid.NewGuid(),
                ApplicationReferenceNumber = "APP-123",
                PayerId = 42
            };

            var resp = new ComplianceSchemeFeesResponseDto
            {
                ComplianceSchemeRegistrationFee = 1250,
                ComplianceSchemeMembersWithFees = new List<ComplianceSchemeMembersWithFeesDto>
                {
                    new ComplianceSchemeMembersWithFeesDto
                    {
                        MemberId = "M-001",
                        SubsidiariesFeeBreakdown = null!,
                        MemberRegistrationFee = 100,
                        MemberLateRegistrationFee = 10,
                        MemberOnlineMarketPlaceFee = 5,
                        SubsidiariesFee = 0
                    },
                    new ComplianceSchemeMembersWithFeesDto
                    {
                        MemberId = "M-002",
                        SubsidiariesFeeBreakdown = null!,
                        MemberRegistrationFee = 200,
                        MemberLateRegistrationFee = 30,
                        MemberOnlineMarketPlaceFee = 15,
                        SubsidiariesFee = 0
                    }
                }
            };

            var invoicePeriod = new DateTimeOffset(2025, 09, 01, 0, 0, 0, TimeSpan.Zero);
            var invoiceDate = new DateTimeOffset(2025, 09, 21, 0, 0, 0, TimeSpan.Zero);
            var payerTypeId = 2;

            // Act
            var result = mapper.BuildComplianceSchemeRegistrationFeeSummaryRecord(
                dto, invoicePeriod, payerTypeId, resp, invoiceDate);

            // Assert
            result.Should().NotBeNull();
            result.FileId.Should().Be(dto.FileId!.Value);
            result.ExternalId.Should().Be(dto.ExternalId!.Value);
            result.ApplicationReferenceNumber.Should().Be(dto.ApplicationReferenceNumber);
            result.InvoicePeriod.Should().Be(invoicePeriod);
            result.InvoiceDate.Should().Be(invoiceDate);
            result.PayerTypeId.Should().Be(payerTypeId);
            result.PayerId.Should().Be(dto.PayerId!.Value);

            result.Lines.Should().NotBeNull();
            result.Lines.Should().HaveCount(7);

            result.Lines.Should().ContainSingle(l =>
                l.FeeTypeId == (int)FeeTypeIds.ComplianceSchemeRegistrationFee &&
                l.UnitPrice == 1250 &&
                l.Quantity == 1 &&
                l.Amount == 1250);

            var memberRegLines = result.Lines.Where(l => l.FeeTypeId == (int)FeeTypeIds.MemberRegistrationFee).ToList();
            memberRegLines.Should().HaveCount(2);
            memberRegLines.Sum(l => l.Amount).Should().Be(300);
            memberRegLines.All(l => l.Quantity == 1 && l.UnitPrice == l.Amount).Should().BeTrue();

            var memberLateLines = result.Lines.Where(l => l.FeeTypeId == (int)FeeTypeIds.MemberLateRegistrationFee).ToList();
            memberLateLines.Should().HaveCount(2);
            memberLateLines.Sum(l => l.Amount).Should().Be(40);
            memberLateLines.All(l => l.Quantity == 1 && l.UnitPrice == l.Amount).Should().BeTrue();

            var ompLines = result.Lines.Where(l => l.FeeTypeId == (int)FeeTypeIds.UnitOmpFee).ToList();
            ompLines.Should().HaveCount(2);
            ompLines.Sum(l => l.Amount).Should().Be(20);
            ompLines.All(l => l.Quantity == 1 && l.UnitPrice == l.Amount).Should().BeTrue();

            result.Lines.Should().NotContain(l => l.FeeTypeId == (int)FeeTypeIds.SubsidiaryFee);
        }

        [TestMethod]
        public void BuildComplianceSchemeRegistrationFeeSummaryRecord_NullMembers_YieldsNoLinesWhenRegFeeIsZero()
        {
            // Arrange
            var mapper = new FeeSummarySaveRequestMapper();

            var dto = new ComplianceSchemeFeesRequestDto
            {
                Regulator = "GB-ENG",
                FileId = Guid.NewGuid(),
                ExternalId = Guid.NewGuid(),
                ApplicationReferenceNumber = "APP-000",
                PayerId = 7
            };
            
            var resp = new ComplianceSchemeFeesResponseDto
            {
                ComplianceSchemeRegistrationFee = 0,
                ComplianceSchemeMembersWithFees = null!
            };

            var invoicePeriod = new DateTimeOffset(2025, 08, 01, 0, 0, 0, TimeSpan.Zero);
            var invoiceDate = new DateTimeOffset(2025, 08, 31, 0, 0, 0, TimeSpan.Zero);
            var payerTypeId = 3;

            // Act
            var result = mapper.BuildComplianceSchemeRegistrationFeeSummaryRecord(
                dto, invoicePeriod, payerTypeId, resp, invoiceDate);

            // Assert
            result.Should().NotBeNull();
            result.Lines.Should().NotBeNull();
            result.Lines.Should().HaveCount(0, "no registration fee and no members => no lines");
        }

        [TestMethod]
        public void BuildComplianceSchemeRegistrationFeeSummaryRecord_SplitsSubsidiaryBandsIntoSeparateLines()
        {
            // Arrange
            var mapper = new FeeSummarySaveRequestMapper();

            var dto = new ComplianceSchemeFeesRequestDto
            {
                Regulator = "GB-ENG",
                FileId = Guid.NewGuid(),
                ExternalId = Guid.NewGuid(),
                ApplicationReferenceNumber = "CS-2025-0003",
                PayerId = 10
            };

            var feeCalculationResponse = new ComplianceSchemeFeesResponseDto
            {
                ComplianceSchemeRegistrationFee = 1380400,
                PreviousPayment = 0,
                ComplianceSchemeMembersWithFees = new List<ComplianceSchemeMembersWithFeesDto>
                {
                    new ComplianceSchemeMembersWithFeesDto
                    {
                        MemberId = "MEM-0001",
                        MemberRegistrationFee = 168500,
                        MemberOnlineMarketPlaceFee = 257900,
                        MemberLateRegistrationFee = 132800,
                        SubsidiariesFee = 167400 + 14000 + 2000,
                        SubsidiariesFeeBreakdown = new SubsidiariesFeeBreakdown
                        {
                            TotalSubsidiariesOMPFees = 0,
                            CountOfOMPSubsidiaries = 0,
                            UnitOMPFees = 257_900,
                            FeeBreakdowns = new List<FeeBreakdown>
                            {
                                new FeeBreakdown { BandNumber = 1, UnitCount = 3, UnitPrice = 55800, TotalPrice = 167400 },
                                new FeeBreakdown { BandNumber = 2, UnitCount = 1, UnitPrice = 14000, TotalPrice = 14000 },
                                new FeeBreakdown { BandNumber = 3, UnitCount = 2, UnitPrice = 1000,  TotalPrice = 2000  }
                            }
                        }
                    }
                }
            };

            var m = feeCalculationResponse.ComplianceSchemeMembersWithFees[0];
            m.TotalMemberFee = m.MemberRegistrationFee
                             + m.MemberOnlineMarketPlaceFee
                             + m.MemberLateRegistrationFee
                             + m.SubsidiariesFee;

            feeCalculationResponse.TotalFee = feeCalculationResponse.ComplianceSchemeRegistrationFee + m.TotalMemberFee;
            feeCalculationResponse.OutstandingPayment = feeCalculationResponse.TotalFee - feeCalculationResponse.PreviousPayment;

            var invoicePeriod = new DateTimeOffset(2025, 09, 26, 0, 0, 0, TimeSpan.Zero);
            var invoiceDate = invoicePeriod.AddDays(2);
            var payerTypeId = 2;

            // Act
            var result = mapper.BuildComplianceSchemeRegistrationFeeSummaryRecord(
                dto, invoicePeriod, payerTypeId, feeCalculationResponse, invoiceDate);

            result.Should().NotBeNull();
            result.FileId.Should().Be(dto.FileId!.Value);
            result.ExternalId.Should().Be(dto.ExternalId!.Value);
            result.ApplicationReferenceNumber.Should().Be(dto.ApplicationReferenceNumber);
            result.InvoicePeriod.Should().Be(invoicePeriod);
            result.InvoiceDate.Should().Be(invoiceDate);
            result.PayerTypeId.Should().Be(payerTypeId);
            result.PayerId.Should().Be(dto.PayerId!.Value);

            result.Lines.Should().NotBeNull();
            result.Lines.Should().HaveCount(7);

            result.Lines.Should().ContainSingle(l =>
                l.FeeTypeId == (int)FeeTypeIds.ComplianceSchemeRegistrationFee &&
                l.UnitPrice == 1380400 &&
                l.Quantity == 1 &&
                l.Amount == 1380400);

            result.Lines.Should().ContainSingle(l =>
                l.FeeTypeId == (int)FeeTypeIds.MemberRegistrationFee &&
                l.UnitPrice == 168500 &&
                l.Quantity == 1 &&
                l.Amount == 168500);

            result.Lines.Should().ContainSingle(l =>
                l.FeeTypeId == (int)FeeTypeIds.UnitOmpFee &&
                l.UnitPrice == 257900 &&
                l.Quantity == 1 &&
                l.Amount == 257900);

            result.Lines.Should().ContainSingle(l =>
                l.FeeTypeId == (int)FeeTypeIds.MemberLateRegistrationFee &&
                l.UnitPrice == 132800 &&
                l.Quantity == 1 &&
                l.Amount == 132800);

            var subLines = result.Lines.Where(l => l.FeeTypeId == (int)FeeTypeIds.SubsidiaryFee).ToList();
            subLines.Should().HaveCount(3);

            subLines.Should().ContainSingle(l =>
                l.UnitPrice == 55800 && l.Quantity == 3 && l.Amount == 167400);
            subLines.Should().ContainSingle(l =>
                l.UnitPrice == 14000 && l.Quantity == 1 && l.Amount == 14000);
            subLines.Should().ContainSingle(l =>
                l.UnitPrice == 1000 && l.Quantity == 2 && l.Amount == 2000);
        }

        [TestMethod]
        public void BuildComplianceSchemeResubmissionFeeSummaryRecord_BuildsSingleLineAndSetsHeaders()
        {
            // Arrange
            var mapper = new FeeSummarySaveRequestMapper();

            var req = new ComplianceSchemeResubmissionFeeRequestDto
            {
                Regulator = "GB-ENG",
                FileId = Guid.NewGuid(),
                ExternalId = Guid.NewGuid(),
                ReferenceNumber = "REF-999",
                PayerId = 999
            };

            var resultDto = new ComplianceSchemeResubmissionFeeResult
            {
                TotalResubmissionFee = (decimal)432.10
            };

            var resubmissionFeeTypeId = 77;
            var invoicePeriod = new DateTimeOffset(2025, 07, 01, 0, 0, 0, TimeSpan.Zero);
            var invoiceDate = new DateTimeOffset(2025, 07, 15, 0, 0, 0, TimeSpan.Zero);
            var payerTypeId = 5;

            // Act
            var result = mapper.BuildComplianceSchemeResubmissionFeeSummaryRecord(
                req, resultDto, resubmissionFeeTypeId, invoicePeriod, payerTypeId, invoiceDate);

            // Assert
            result.Should().NotBeNull();
            result.FileId.Should().Be(req.FileId!.Value);
            result.ExternalId.Should().Be(req.ExternalId!.Value);
            result.ApplicationReferenceNumber.Should().Be(req.ReferenceNumber);
            result.InvoicePeriod.Should().Be(invoicePeriod);
            result.InvoiceDate.Should().Be(invoiceDate);
            result.PayerTypeId.Should().Be(payerTypeId);
            result.PayerId.Should().Be(req.PayerId!.Value);

            result.Lines.Should().NotBeNull();
            result.Lines.Should().HaveCount(1);

            var line = result.Lines.Single();
            line.FeeTypeId.Should().Be(resubmissionFeeTypeId);
            line.UnitPrice.Should().Be((decimal)432.10);
            line.Quantity.Should().Be(1);
            line.Amount.Should().Be((decimal)432.10);
        }
    }
}
