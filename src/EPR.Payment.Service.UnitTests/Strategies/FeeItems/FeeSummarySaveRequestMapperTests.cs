using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Request.ResubmissionFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Response.ResubmissionFees.ComplianceScheme;
using EPR.Payment.Service.Common.Enums;
using EPR.Payment.Service.Strategies.FeeItems;
using FluentAssertions;

namespace EPR.Payment.Service.UnitTests.Strategies.FeeItems
{
    [TestClass]
    public class FeeItemSaveRequestMapperTests
    {
        private static ComplianceSchemeFeesRequestDto MakeBaseReq(
            Guid? fileId = null, Guid? externalId = null, string appRef = "APP-123", int? payerId = 42)
            => new()
            {
                Regulator = "GB-ENG",
                FileId = fileId,
                ExternalId = externalId,
                ApplicationReferenceNumber = appRef,
                PayerId = payerId
            };

        private static ComplianceSchemeMembersWithFeesDto Member(
            decimal reg = 0, decimal late = 0, decimal memberOmp = 0,
            decimal subsFee = 0, SubsidiariesFeeBreakdown? breakdown = null)
            => new()
            {
                MemberId = Guid.NewGuid().ToString(),
                MemberRegistrationFee = reg,
                MemberLateRegistrationFee = late,
                MemberOnlineMarketPlaceFee = memberOmp,
                SubsidiariesFee = subsFee,
                SubsidiariesFeeBreakdown = breakdown ?? new SubsidiariesFeeBreakdown()
            };

        private static FeeBreakdown Band(int band, int units, decimal unitPrice, decimal total)
            => new() { BandNumber = band, UnitCount = units, UnitPrice = unitPrice, TotalPrice = total };

        private static SubsidiariesFeeBreakdown Subs(
            IEnumerable<FeeBreakdown>? feeBreakdowns = null,
            int ompCount = 0, decimal ompUnit = 0, decimal ompTotal = 0)
            => new()
            {
                FeeBreakdowns = feeBreakdowns?.ToList() ?? new List<FeeBreakdown>(),
                CountOfOMPSubsidiaries = ompCount,
                UnitOMPFees = ompUnit,
                TotalSubsidiariesOMPFees = ompTotal
            };

        [TestMethod]
        public void BuildComplianceSchemeRegistrationFeeSummaryRecord_FullHappyPath_WithBands_NoOMPSubs_SetsHeadersAndLines()
        {
            // Arrange
            var mapper = new FeeItemSaveRequestMapper();

            var req = MakeBaseReq(fileId: Guid.NewGuid(), externalId: Guid.NewGuid(), appRef: "CS-2025-0003", payerId: 10);

            var resp = new ComplianceSchemeFeesResponseDto
            {
                ComplianceSchemeRegistrationFee = 1_380_400,
                PreviousPayment = 0,
                ComplianceSchemeMembersWithFees = new List<ComplianceSchemeMembersWithFeesDto>
                {
                    Member(
                        reg: 168_500,
                        late: 132_800,
                        memberOmp: 257_900,
                        subsFee: 167_400 + 14_000 + 2_000,
                        breakdown: Subs(new []
                        {
                            Band(1, 3, 55_800, 167_400),
                            Band(2, 1, 14_000, 14_000),
                            Band(3, 2, 1_000,  2_000)
                        }))
                }
            };

            var invoicePeriod = new DateTimeOffset(2025, 09, 26, 0, 0, 0, TimeSpan.Zero);
            var invoiceDate = invoicePeriod.AddDays(2);
            var payerTypeId = 2;

            // Act
            var result = mapper.BuildComplianceSchemeRegistrationFeeSummaryRecord(req, invoicePeriod, payerTypeId, resp, invoiceDate);

            // Assert
            result.FileId.Should().Be(req.FileId!.Value);
            result.ExternalId.Should().Be(req.ExternalId!.Value);
            result.ApplicationReferenceNumber.Should().Be(req.ApplicationReferenceNumber);
            result.InvoicePeriod.Should().Be(invoicePeriod);
            result.InvoiceDate.Should().Be(invoiceDate);
            result.PayerTypeId.Should().Be(payerTypeId);
            result.PayerId.Should().Be(req.PayerId!.Value);

            result.Lines.Should().HaveCount(7);

            result.Lines.Should().ContainSingle(l =>
                l.FeeTypeId == (int)FeeTypeIds.ComplianceSchemeRegistrationFee &&
                l.UnitPrice == 1_380_400 &&
                l.Quantity == 1 &&
                l.Amount == 1_380_400);

            result.Lines.Should().ContainSingle(l =>
                l.FeeTypeId == (int)FeeTypeIds.MemberRegistrationFee &&
                l.UnitPrice == 168_500 &&
                l.Quantity == 1 &&
                l.Amount == 168_500);

            result.Lines.Should().ContainSingle(l =>
                l.FeeTypeId == (int)FeeTypeIds.MemberOnlineMarketplaceFee &&
                l.UnitPrice == 257_900 &&
                l.Quantity == 1 &&
                l.Amount == 257_900);

            result.Lines.Should().ContainSingle(l =>
                l.FeeTypeId == (int)FeeTypeIds.MemberLateRegistrationFee &&
                l.UnitPrice == 132_800 &&
                l.Quantity == 1 &&
                l.Amount == 132_800);

            var band1 = result.Lines.Single(l => l.FeeTypeId == (int)FeeTypeIds.BandNumber1);
            band1.UnitPrice.Should().Be(55_800);
            band1.Quantity.Should().Be(3);
            band1.Amount.Should().Be(167_400);

            var band2 = result.Lines.Single(l => l.FeeTypeId == (int)FeeTypeIds.BandNumber2);
            band2.UnitPrice.Should().Be(14_000);
            band2.Quantity.Should().Be(1);
            band2.Amount.Should().Be(14_000);

            var band3 = result.Lines.Single(l => l.FeeTypeId == (int)FeeTypeIds.BandNumber3);
            band3.UnitPrice.Should().Be(1_000);
            band3.Quantity.Should().Be(2);
            band3.Amount.Should().Be(2_000);

            result.Lines.Should().NotContain(l => l.FeeTypeId == (int)FeeTypeIds.UnitOnlineMarketplaceFee);
        }

        [TestMethod]
        public void BuildComplianceSchemeRegistrationFeeSummaryRecord_ZeroRegFee_And_NullMembers_YieldsNoLines()
        {
            // Arrange
            var mapper = new FeeItemSaveRequestMapper();
            var req = MakeBaseReq();
            var resp = new ComplianceSchemeFeesResponseDto
            {
                ComplianceSchemeRegistrationFee = 0,
                ComplianceSchemeMembersWithFees = null!
            };

            // Act
            var result = mapper.BuildComplianceSchemeRegistrationFeeSummaryRecord(
                req,
                invoicePeriod: new DateTimeOffset(2025, 08, 01, 0, 0, 0, TimeSpan.Zero),
                payerTypeId: 3,
                calculationResponse: resp,
                invoiceDate: new DateTimeOffset(2025, 08, 31, 0, 0, 0, TimeSpan.Zero));

            // Assert
            result.Lines.Should().NotBeNull().And.BeEmpty();
        }

        [TestMethod]
        public void BuildComplianceSchemeRegistrationFeeSummaryRecord_WithSubsidiaryOMP_AddsUnitOMPLine()
        {
            // Arrange
            var mapper = new FeeItemSaveRequestMapper();

            var req = MakeBaseReq(fileId: Guid.NewGuid(), externalId: Guid.NewGuid(), appRef: "CS-2025-OMP", payerId: 55);

            var resp = new ComplianceSchemeFeesResponseDto
            {
                ComplianceSchemeRegistrationFee = 100,
                ComplianceSchemeMembersWithFees = new List<ComplianceSchemeMembersWithFeesDto>
                {
                    Member(
                        reg: 0, late: 0, memberOmp: 0, subsFee: 900,
                        breakdown: Subs(
                            feeBreakdowns: new []
                            {
                                Band(1, 1, 100, 100),
                                Band(2, 2, 200, 400),
                                Band(3, 1, 100, 100)
                            },
                            ompCount: 3, ompUnit: 100, ompTotal: 300))
                }
            };

            // Act
            var result = mapper.BuildComplianceSchemeRegistrationFeeSummaryRecord(
                req,
                invoicePeriod: new DateTimeOffset(2025, 10, 01, 0, 0, 0, TimeSpan.Zero),
                payerTypeId: 2,
                calculationResponse: resp,
                invoiceDate: new DateTimeOffset(2025, 10, 02, 0, 0, 0, TimeSpan.Zero));

            // Assert
            result.Lines.Should().HaveCount(5);
            result.Lines.Count(l => l.FeeTypeId == (int)FeeTypeIds.BandNumber1).Should().Be(1);
            result.Lines.Count(l => l.FeeTypeId == (int)FeeTypeIds.BandNumber2).Should().Be(1);
            result.Lines.Count(l => l.FeeTypeId == (int)FeeTypeIds.BandNumber3).Should().Be(1);

            var ompLine = result.Lines.Single(l => l.FeeTypeId == (int)FeeTypeIds.UnitOnlineMarketplaceFee);
            ompLine.UnitPrice.Should().Be(100);
            ompLine.Quantity.Should().Be(3);
            ompLine.Amount.Should().Be(300);
        }

        [TestMethod]
        public void BuildComplianceSchemeRegistrationFeeSummaryRecord_UnknownBand_FallsBackToSubsidiaryFee()
        {
            // Arrange
            var mapper = new FeeItemSaveRequestMapper();

            var req = MakeBaseReq();
            var resp = new ComplianceSchemeFeesResponseDto
            {
                ComplianceSchemeRegistrationFee = 0,
                ComplianceSchemeMembersWithFees = new List<ComplianceSchemeMembersWithFeesDto>
                {
                    Member(
                        reg: 0, late: 0, memberOmp: 0, subsFee: 50,
                        breakdown: Subs(new [] { Band(99, 5, 10, 50) }))
                }
            };

            // Act
            var result = mapper.BuildComplianceSchemeRegistrationFeeSummaryRecord(
                req,
                invoicePeriod: new DateTimeOffset(2025, 11, 01, 0, 0, 0, TimeSpan.Zero),
                payerTypeId: 2,
                calculationResponse: resp,
                invoiceDate: new DateTimeOffset(2025, 11, 02, 0, 0, 0, TimeSpan.Zero));

            // Assert
            result.Lines.Should().HaveCount(1);
            var line = result.Lines.Single();
            line.FeeTypeId.Should().Be((int)FeeTypeIds.SubsidiaryFee);
            line.UnitPrice.Should().Be(10);
            line.Quantity.Should().Be(5);
            line.Amount.Should().Be(50);
        }

        [TestMethod]
        public void BuildComplianceSchemeRegistrationFeeSummaryRecord_FallbackIdsWhenMissingInRequest()
        {
            // Arrange
            var mapper = new FeeItemSaveRequestMapper();

            var req = MakeBaseReq(fileId: null, externalId: null, payerId: null);
            var resp = new ComplianceSchemeFeesResponseDto
            {
                ComplianceSchemeRegistrationFee = 250,
                ComplianceSchemeMembersWithFees = new List<ComplianceSchemeMembersWithFeesDto>()
            };

            // Act
            var result = mapper.BuildComplianceSchemeRegistrationFeeSummaryRecord(
                req,
                invoicePeriod: new DateTimeOffset(2025, 12, 01, 0, 0, 0, TimeSpan.Zero),
                payerTypeId: 1,
                calculationResponse: resp,
                invoiceDate: new DateTimeOffset(2025, 12, 02, 0, 0, 0, TimeSpan.Zero));

            // Assert
            result.FileId.Should().NotBe(Guid.Empty);
            result.ExternalId.Should().NotBe(Guid.Empty);
            result.PayerId.Should().Be(0);
        }

        [TestMethod]
        public void BuildComplianceSchemeResubmissionFeeSummaryRecord_SingleLine_AndHeaders()
        {
            // Arrange
            var mapper = new FeeItemSaveRequestMapper();

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
                TotalResubmissionFee = 432.10m
            };

            var resubmissionFeeTypeId = (int)FeeTypeIds.ComplianceSchemeResubmissionFee; // use real enum
            var invoicePeriod = new DateTimeOffset(2025, 07, 01, 0, 0, 0, TimeSpan.Zero);
            var invoiceDate = new DateTimeOffset(2025, 07, 15, 0, 0, 0, TimeSpan.Zero);
            var payerTypeId = 5;

            // Act
            var result = mapper.BuildComplianceSchemeResubmissionFeeSummaryRecord(
                req, resultDto, resubmissionFeeTypeId, invoicePeriod, payerTypeId, invoiceDate);

            // Assert
            result.FileId.Should().Be(req.FileId!.Value);
            result.ExternalId.Should().Be(req.ExternalId!.Value);
            result.ApplicationReferenceNumber.Should().Be(req.ReferenceNumber);
            result.InvoicePeriod.Should().Be(invoicePeriod);
            result.InvoiceDate.Should().Be(invoiceDate);
            result.PayerTypeId.Should().Be(payerTypeId);
            result.PayerId.Should().Be(req.PayerId!.Value);

            result.Lines.Should().HaveCount(1);
            var line = result.Lines.Single();
            line.FeeTypeId.Should().Be(resubmissionFeeTypeId);
            line.UnitPrice.Should().Be(432.10m);
            line.Quantity.Should().Be(1);
            line.Amount.Should().Be(432.10m);
        }

        [TestMethod]
        public void BuildComplianceSchemeResubmissionFeeSummaryRecord_FallbackIds_And_DefaultInvoiceDate()
        {
            // Arrange
            var mapper = new FeeItemSaveRequestMapper();

            var req = new ComplianceSchemeResubmissionFeeRequestDto
            {
                Regulator = "GB-ENG",
                FileId = null,
                ExternalId = null,
                ReferenceNumber = "REF-NULLS",
                PayerId = null
            };

            var resultDto = new ComplianceSchemeResubmissionFeeResult { TotalResubmissionFee = 99m };
            var payerTypeId = 2;
            var invoicePeriod = new DateTimeOffset(2026, 01, 01, 0, 0, 0, TimeSpan.Zero);

            var before = DateTimeOffset.UtcNow;

            // Act
            var result = mapper.BuildComplianceSchemeResubmissionFeeSummaryRecord(
                req, resultDto, (int)FeeTypeIds.ProducerResubmissionFee, invoicePeriod, payerTypeId, invoiceDate: null);

            var after = DateTimeOffset.UtcNow;

            // Assert
            result.FileId.Should().NotBe(Guid.Empty);
            result.ExternalId.Should().NotBe(Guid.Empty);
            result.PayerId.Should().Be(0);

            result.InvoiceDate.Should().BeOnOrAfter(before).And.BeOnOrBefore(after);

            var line = result.Lines.Single();
            line.FeeTypeId.Should().Be((int)FeeTypeIds.ProducerResubmissionFee);
            line.UnitPrice.Should().Be(99m);
            line.Amount.Should().Be(99m);
            line.Quantity.Should().Be(1);
        }
    }
}
