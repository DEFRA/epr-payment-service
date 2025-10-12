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
    public class FeeSummarySaveRequestMapperTests
    {
        [TestMethod]
        public void BuildComplianceSchemeRegistrationFeeSummaryRecord_FullHappyPath_WithBands_NoOMPSubs_SetsHeadersAndLines()
        {
            // Arrange
            var mapper = new FeeItemSaveRequestMapper();

            var submitDate = new DateTime(2025, 09, 26);
            var req = CreateV2Dto(submissionDate: submitDate, payerId: 10, appRef: "CS-2025-0003");

            var resp = new ComplianceSchemeFeesResponseDto
            {
                ComplianceSchemeRegistrationFee = 1_380_400,
                PreviousPayment = 0,
                ComplianceSchemeMembersWithFees = new List<ComplianceSchemeMembersWithFeesDto>
                {
                    CreateMemberDto(
                        reg: 168_500,
                        late: 132_800,
                        memberOmp: 257_900,
                        subsFee: 167_400 + 14_000 + 2_000,
                        breakdown: CreateSubsidiariesFeeBreakdown(new []
                        {
                            CreateBand(1, 3, 55_800, 167_400),
                            CreateBand(2, 1, 14_000, 14_000),
                            CreateBand(3, 2, 1_000,  2_000)
                        }))
                }
            };

            // Act
            var before = DateTimeOffset.UtcNow;
            var result = mapper.BuildComplianceSchemeRegistrationFeeSummaryRecord(req, resp);
            var after = DateTimeOffset.UtcNow;

            // Assert
            result.FileId.Should().Be(req.FileId);
            result.ExternalId.Should().Be(req.ExternalId);
            result.ApplicationReferenceNumber.Should().Be(req.ApplicationReferenceNumber);
            result.InvoicePeriod.Should().Be(new DateTimeOffset(req.SubmissionDate, TimeSpan.Zero));
            result.InvoiceDate.Should().BeOnOrAfter(before).And.BeOnOrBefore(after);
            result.PayerTypeId.Should().Be((int)PayerTypeIds.ComplianceScheme);
            result.PayerId.Should().Be(req.PayerId);

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
            var req = CreateV2Dto();

            var resp = new ComplianceSchemeFeesResponseDto
            {
                ComplianceSchemeRegistrationFee = 0,
                ComplianceSchemeMembersWithFees = null!
            };

            // Act
            var result = mapper.BuildComplianceSchemeRegistrationFeeSummaryRecord(req, resp);

            // Assert
            result.Lines.Should().NotBeNull().And.BeEmpty();
        }

        [TestMethod]
        public void BuildComplianceSchemeRegistrationFeeSummaryRecord_WithSubsidiaryOMP_AddsUnitOMPLine()
        {
            // Arrange
            var mapper = new FeeItemSaveRequestMapper();
            var req = CreateV2Dto(appRef: "CS-2025-OMP", payerId: 55);

            var resp = new ComplianceSchemeFeesResponseDto
            {
                ComplianceSchemeRegistrationFee = 100,
                ComplianceSchemeMembersWithFees = new List<ComplianceSchemeMembersWithFeesDto>
                {
                    CreateMemberDto(
                        reg: 0, late: 0, memberOmp: 0, subsFee: 900,
                        breakdown: CreateSubsidiariesFeeBreakdown(
                            feeBreakdowns: new []
                            {
                                CreateBand(1, 1, 100, 100),
                                CreateBand(2, 2, 200, 400),
                                CreateBand(3, 1, 100, 100)
                            },
                            ompCount: 3, ompUnit: 100, ompTotal: 300))
                }
            };

            // Act
            var result = mapper.BuildComplianceSchemeRegistrationFeeSummaryRecord(req, resp);

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
            var req = CreateV2Dto();

            var resp = new ComplianceSchemeFeesResponseDto
            {
                ComplianceSchemeRegistrationFee = 0,
                ComplianceSchemeMembersWithFees = new List<ComplianceSchemeMembersWithFeesDto>
                {
                    CreateMemberDto(
                        reg: 0, late: 0, memberOmp: 0, subsFee: 50,
                        breakdown: CreateSubsidiariesFeeBreakdown(new [] { CreateBand(99, 5, 10, 50) } ))
                }
            };

            // Act
            var result = mapper.BuildComplianceSchemeRegistrationFeeSummaryRecord(req, resp);

            // Assert
            result.Lines.Should().HaveCount(1);
            var line = result.Lines.Single();
            line.FeeTypeId.Should().Be((int)FeeTypeIds.SubsidiaryFee);
            line.UnitPrice.Should().Be(10);
            line.Quantity.Should().Be(5);
            line.Amount.Should().Be(50);
        }

        [TestMethod]
        public void BuildComplianceSchemeRegistrationFeeSummaryRecord_InvalidBreakdowns_AreSkipped()
        {
            // Arrange
            var mapper = new FeeItemSaveRequestMapper();
            var req = CreateV2Dto();

            var resp = new ComplianceSchemeFeesResponseDto
            {
                ComplianceSchemeRegistrationFee = 0,
                ComplianceSchemeMembersWithFees = new List<ComplianceSchemeMembersWithFeesDto>
                {
                    CreateMemberDto(
                        subsFee: 50,
                        breakdown: CreateSubsidiariesFeeBreakdown(new []
                        {
                            CreateBand(1, 0, 10, 0),
                            CreateBand(2, 5, 10, 50)
                        }))
                }
            };

            // Act
            var result = mapper.BuildComplianceSchemeRegistrationFeeSummaryRecord(req, resp);

            // Assert
            result.Lines.Should().HaveCount(1);
            var line = result.Lines.Single();
            line.FeeTypeId.Should().Be((int)FeeTypeIds.BandNumber2);
            line.UnitPrice.Should().Be(10);
            line.Quantity.Should().Be(5);
            line.Amount.Should().Be(50);
        }

        [TestMethod]
        public void BuildComplianceSchemeResubmissionFeeSummaryRecord_SingleLine_AndHeaders()
        {
            // Arrange
            var mapper = new FeeItemSaveRequestMapper();

            var subDate = new DateTime(2025, 07, 01);

            var req = new ComplianceSchemeResubmissionFeeRequestV2Dto
            {
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "REF-999",
                SubmissionDate = subDate,

                FileId = Guid.NewGuid(),
                ExternalId = Guid.NewGuid(),
                InvoicePeriod = new DateTimeOffset(subDate, TimeSpan.Zero),
                PayerTypeId = (int)PayerTypeIds.ComplianceScheme,
                PayerId = 999
            };

            var resultDto = new ComplianceSchemeResubmissionFeeResult
            {
                TotalResubmissionFee = 432.10m
            };

            var before = DateTimeOffset.UtcNow;

            // Act
            var result = mapper.BuildComplianceSchemeResubmissionFeeSummaryRecord(
                req, resultDto, (int)FeeTypeIds.ComplianceSchemeResubmissionFee);

            var after = DateTimeOffset.UtcNow;

            // Assert
            result.FileId.Should().Be(req.FileId);
            result.ExternalId.Should().Be(req.ExternalId);
            result.ApplicationReferenceNumber.Should().Be(req.ApplicationReferenceNumber);
            result.InvoicePeriod.Should().Be(new DateTimeOffset(req.SubmissionDate, TimeSpan.Zero));
            result.InvoiceDate.Should().BeOnOrAfter(before).And.BeOnOrBefore(after);
            result.PayerTypeId.Should().Be((int)PayerTypeIds.ComplianceScheme);
            result.PayerId.Should().Be(req.PayerId);

            result.Lines.Should().HaveCount(1);
            var line = result.Lines.Single();
            line.FeeTypeId.Should().Be((int)FeeTypeIds.ComplianceSchemeResubmissionFee);
            line.UnitPrice.Should().Be(432.10m);
            line.Quantity.Should().Be(1);
            line.Amount.Should().Be(432.10m);
        }

        [TestMethod]
        public void BuildComplianceSchemeResubmissionFeeSummaryRecord_SetsInvoiceDateAndPeriod()
        {
            // Arrange
            var mapper = new FeeItemSaveRequestMapper();

            var subDate = new DateTime(2026, 01, 02);

            var req = new ComplianceSchemeResubmissionFeeRequestV2Dto
            {
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "REF-CHK",
                SubmissionDate = subDate,

                FileId = Guid.NewGuid(),
                ExternalId = Guid.NewGuid(),
                InvoicePeriod = new DateTimeOffset(subDate, TimeSpan.Zero),
                PayerTypeId = (int)PayerTypeIds.ComplianceScheme,
                PayerId = 123
            };

            var resultDto = new ComplianceSchemeResubmissionFeeResult { TotalResubmissionFee = 99m };

            var before = DateTimeOffset.UtcNow;

            // Act
            var result = mapper.BuildComplianceSchemeResubmissionFeeSummaryRecord(
                req, resultDto, (int)FeeTypeIds.ProducerResubmissionFee);

            var after = DateTimeOffset.UtcNow;

            // Assert
            result.InvoiceDate.Should().BeOnOrAfter(before).And.BeOnOrBefore(after);
            result.InvoicePeriod.Should().Be(new DateTimeOffset(req.SubmissionDate, TimeSpan.Zero));

            var line = result.Lines.Single();
            line.FeeTypeId.Should().Be((int)FeeTypeIds.ProducerResubmissionFee);
            line.UnitPrice.Should().Be(99m);
            line.Amount.Should().Be(99m);
            line.Quantity.Should().Be(1);
        }


        [TestMethod]
        public void BuildComplianceSchemeRegistrationFeeSummaryRecord_NullCalc_ProducesNoLines()
        {
            // Arrange
            var mapper = new FeeItemSaveRequestMapper();
            var req = CreateV2Dto();
            ComplianceSchemeFeesResponseDto? resp = null;

            // Act
            var result = mapper.BuildComplianceSchemeRegistrationFeeSummaryRecord(req, resp!);

            // Assert
            result.Lines.Should().BeEmpty();
        }

        [TestMethod]
        public void BuildComplianceSchemeRegistrationFeeSummaryRecord_ProvidedIds_AreUsed_NoDefaults()
        {
            // Arrange
            var mapper = new FeeItemSaveRequestMapper();
            var subDate = new DateTime(2025, 09, 26);

            var expectedFileId = Guid.NewGuid();
            var expectedExternalId = Guid.NewGuid();
            var expectedPayerId = 777;

            var req = new ComplianceSchemeFeesRequestV2Dto
            {
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "APP-IDS",
                SubmissionDate = subDate,
                FileId = expectedFileId,          
                ExternalId = expectedExternalId,  
                InvoicePeriod = new DateTimeOffset(subDate, TimeSpan.Zero),
                PayerTypeId = (int)PayerTypeIds.ComplianceScheme,
                PayerId = expectedPayerId,        
                ComplianceSchemeMembers = new()
            };

            var resp = new ComplianceSchemeFeesResponseDto
            {
                ComplianceSchemeRegistrationFee = 0,
                ComplianceSchemeMembersWithFees = null
            };

            // Act
            var result = mapper.BuildComplianceSchemeRegistrationFeeSummaryRecord(req, resp);

            // Assert
            result.FileId.Should().Be(expectedFileId);
            result.ExternalId.Should().Be(expectedExternalId);
            result.PayerId.Should().Be(expectedPayerId);
            result.Lines.Should().BeEmpty();
        }

        [TestMethod]
        public void BuildComplianceSchemeResubmissionFeeSummaryRecord_ProvidedIds_AreUsed_NoDefaults()
        {
            // Arrange
            var mapper = new FeeItemSaveRequestMapper();
            var subDate = new DateTime(2026, 03, 05);

            var expectedFileId = Guid.NewGuid();
            var expectedExternalId = Guid.NewGuid();
            var expectedPayerId = 4242;

            var req = new ComplianceSchemeResubmissionFeeRequestV2Dto
            {
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "REF-IDS",
                SubmissionDate = subDate,
                FileId = expectedFileId,          
                ExternalId = expectedExternalId,  
                InvoicePeriod = new DateTimeOffset(subDate, TimeSpan.Zero),
                PayerTypeId = (int)PayerTypeIds.ComplianceScheme,
                PayerId = expectedPayerId         
            };

            var resultDto = new ComplianceSchemeResubmissionFeeResult { TotalResubmissionFee = 10m };

            // Act
            var result = mapper.BuildComplianceSchemeResubmissionFeeSummaryRecord(
                req, resultDto, (int)FeeTypeIds.ComplianceSchemeResubmissionFee);

            // Assert
            result.FileId.Should().Be(expectedFileId);
            result.ExternalId.Should().Be(expectedExternalId);
            result.PayerId.Should().Be(expectedPayerId);
            result.Lines.Should().HaveCount(1);
        }

        [TestMethod]
        public void BuildComplianceSchemeRegistrationFeeSummaryRecord_NullSubsidiaryBreakdown_NoSubsidiaryLines()
        {
            // Arrange
            var mapper = new FeeItemSaveRequestMapper();
            var req = CreateV2Dto();

            var member = CreateMemberDto(reg: 100, late: 50, memberOmp: 25, subsFee: 0, breakdown: null);
            var resp = new ComplianceSchemeFeesResponseDto
            {
                ComplianceSchemeRegistrationFee = 0,
                ComplianceSchemeMembersWithFees = new List<ComplianceSchemeMembersWithFeesDto> { member }
            };

            // Act
            var result = mapper.BuildComplianceSchemeRegistrationFeeSummaryRecord(req, resp);

            // Assert
            result.Lines.Should().HaveCount(3);
            result.Lines.Should().OnlyContain(l =>
                l.FeeTypeId == (int)FeeTypeIds.MemberRegistrationFee ||
                l.FeeTypeId == (int)FeeTypeIds.MemberLateRegistrationFee ||
                l.FeeTypeId == (int)FeeTypeIds.MemberOnlineMarketplaceFee);
        }

        [TestMethod]
        public void BuildComplianceSchemeRegistrationFeeSummaryRecord_ZeroUnitPrice_SkipsBandLine()
        {
            // Arrange
            var mapper = new FeeItemSaveRequestMapper();
            var req = CreateV2Dto();

            var resp = new ComplianceSchemeFeesResponseDto
            {
                ComplianceSchemeRegistrationFee = 0,
                ComplianceSchemeMembersWithFees = new List<ComplianceSchemeMembersWithFeesDto>
                {
                    CreateMemberDto(
                        subsFee: 0,
                        breakdown: CreateSubsidiariesFeeBreakdown(new []
                        {
                            CreateBand(1, units: 3, unitPrice: 0, total: 0) 
                        }))
                }
            };

            // Act
            var result = mapper.BuildComplianceSchemeRegistrationFeeSummaryRecord(req, resp);

            // Assert
            result.Lines.Should().BeEmpty();
        }

        [TestMethod]
        public void BuildComplianceSchemeRegistrationFeeSummaryRecord_OmpGuard_NotAdded_WhenAnyOperandZero()
        {
            // Arrange
            var mapper = new FeeItemSaveRequestMapper();
            var req = CreateV2Dto();

            var resp = new ComplianceSchemeFeesResponseDto
            {
                ComplianceSchemeRegistrationFee = 0,
                ComplianceSchemeMembersWithFees = new List<ComplianceSchemeMembersWithFeesDto>
                {
                    CreateMemberDto(
                        reg: 0, late: 0, memberOmp: 0, subsFee: 0,
                        breakdown: CreateSubsidiariesFeeBreakdown(
                            feeBreakdowns: Enumerable.Empty<FeeBreakdown>(),
                            ompCount: 0, ompUnit: 100, ompTotal: 0))
                }
            };

            // Act
            var result = mapper.BuildComplianceSchemeRegistrationFeeSummaryRecord(req, resp);

            // Assert
            result.Lines.Should().BeEmpty();
        }


        private static ComplianceSchemeFeesRequestV2Dto CreateV2Dto(
              DateTime? submissionDate = null,
              Guid? fileId = null,
              Guid? externalId = null,
              string appRef = "APP-123",
              int payerId = 42,
              int? payerTypeId = null,
              string regulator = "GB-ENG")
        {
            var subDate = submissionDate ?? DateTime.UtcNow.Date;
            return new ComplianceSchemeFeesRequestV2Dto
            {
                Regulator = regulator,
                ApplicationReferenceNumber = appRef,
                SubmissionDate = subDate,

                FileId = fileId ?? Guid.NewGuid(),
                ExternalId = externalId ?? Guid.NewGuid(),
                InvoicePeriod = new DateTimeOffset(subDate, TimeSpan.Zero),
                PayerTypeId = payerTypeId ?? (int)PayerTypeIds.ComplianceScheme,
                PayerId = payerId,

                ComplianceSchemeMembers = new()
            };
        }

        private static ComplianceSchemeMembersWithFeesDto CreateMemberDto(
            decimal reg = 0, decimal late = 0, decimal memberOmp = 0,
            decimal subsFee = 0, SubsidiariesFeeBreakdown? breakdown = null)
        {
            return new ComplianceSchemeMembersWithFeesDto()
            {
                MemberId = Guid.NewGuid().ToString(),
                MemberRegistrationFee = reg,
                MemberLateRegistrationFee = late,
                MemberOnlineMarketPlaceFee = memberOmp,
                SubsidiariesFee = subsFee,
                SubsidiariesFeeBreakdown = breakdown ?? new SubsidiariesFeeBreakdown()
            };
        }

        private static FeeBreakdown CreateBand(int band, int units, decimal unitPrice, decimal total)
        {
            return new FeeBreakdown() { BandNumber = band, UnitCount = units, UnitPrice = unitPrice, TotalPrice = total };
        }

        private static SubsidiariesFeeBreakdown CreateSubsidiariesFeeBreakdown(
            IEnumerable<FeeBreakdown>? feeBreakdowns = null,
            int ompCount = 0, decimal ompUnit = 0, decimal ompTotal = 0)
        {
            return new SubsidiariesFeeBreakdown()
            {
                FeeBreakdowns = feeBreakdowns?.ToList() ?? new List<FeeBreakdown>(),
                CountOfOMPSubsidiaries = ompCount,
                UnitOMPFees = ompUnit,
                TotalSubsidiariesOMPFees = ompTotal
            };
        }
    }
}
