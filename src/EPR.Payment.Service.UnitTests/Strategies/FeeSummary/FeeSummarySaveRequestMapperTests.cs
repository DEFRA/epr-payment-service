using System;
using System.Collections.Generic;
using System.Linq;
using EPR.Payment.Service.Common.Dtos.FeeSummaries;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationFees.Producer;
using EPR.Payment.Service.Common.Dtos.Request.ResubmissionFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Common.Dtos.Response.RegistrationFees.Producer;
using EPR.Payment.Service.Common.Dtos.Response.ResubmissionFees.ComplianceScheme;
using EPR.Payment.Service.Common.Enums;
using EPR.Payment.Service.Common.ValueObjects.RegistrationFees;
using EPR.Payment.Service.Strategies.FeeSummary;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
                ComplianceSchemeRegistrationFee = 1250m,
                ComplianceSchemeMembersWithFees = new List<ComplianceSchemeMembersWithFeesDto>
                {
                    new ComplianceSchemeMembersWithFeesDto
                    {
                        MemberId = "M-001",
                        SubsidiariesFeeBreakdown = null!,
                        MemberRegistrationFee = 100m,
                        MemberLateRegistrationFee = 10m,
                        MemberOnlineMarketPlaceFee = 5m,
                        SubsidiariesFee = 20m
                    },
                    new ComplianceSchemeMembersWithFeesDto
                    {
                        MemberId = "M-002",
                        SubsidiariesFeeBreakdown = null!,
                        MemberRegistrationFee = 200m,
                        MemberLateRegistrationFee = 30m,
                        MemberOnlineMarketPlaceFee = 15m,
                        SubsidiariesFee = 40m
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
            result.FileId.Should().Be(dto.FileId.Value);
            result.ExternalId.Should().Be(dto.ExternalId.Value);
            result.ApplicationReferenceNumber.Should().Be(dto.ApplicationReferenceNumber);
            result.InvoicePeriod.Should().Be(invoicePeriod);
            result.InvoiceDate.Should().Be(invoiceDate);
            result.PayerTypeId.Should().Be(payerTypeId);
            result.PayerId.Should().Be(dto.PayerId.Value);

            result.Lines.Should().NotBeNull();
            result.Lines.Should().HaveCount(5);

            result.Lines.Should().ContainSingle(l => l.FeeTypeId == (int)FeeTypeIds.ComplianceSchemeRegistrationFee && l.Amount == 1250m);
            result.Lines.Should().ContainSingle(l => l.FeeTypeId == (int)FeeTypeIds.MemberRegistrationFee && l.Amount == 300m);
            result.Lines.Should().ContainSingle(l => l.FeeTypeId == (int)FeeTypeIds.MemberLateRegistrationFee && l.Amount == 40m);
            result.Lines.Should().ContainSingle(l => l.FeeTypeId == (int)FeeTypeIds.UnitOmpFee && l.Amount == 20m);
            result.Lines.Should().ContainSingle(l => l.FeeTypeId == (int)FeeTypeIds.SubsidiaryFee && l.Amount == 60m);
        }

        [TestMethod]
        public void BuildComplianceSchemeRegistrationFeeSummaryRecord_NullMembers_YieldsZeroMemberLines()
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
                ComplianceSchemeRegistrationFee = 0m,
                ComplianceSchemeMembersWithFees = null
            };

            var invoicePeriod = new DateTimeOffset(2025, 08, 01, 0, 0, 0, TimeSpan.Zero);
            var invoiceDate = new DateTimeOffset(2025, 08, 31, 0, 0, 0, TimeSpan.Zero);
            var payerTypeId = 3;

            // Act
            var result = mapper.BuildComplianceSchemeRegistrationFeeSummaryRecord(
                dto, invoicePeriod, payerTypeId, resp, invoiceDate);

            // Assert
            result.Should().NotBeNull();
            result.Lines.Should().HaveCount(5);

            result.Lines.Should().ContainSingle(l => l.FeeTypeId == (int)FeeTypeIds.ComplianceSchemeRegistrationFee && l.Amount == 0m);
            result.Lines.Should().ContainSingle(l => l.FeeTypeId == (int)FeeTypeIds.MemberRegistrationFee && l.Amount == 0m);
            result.Lines.Should().ContainSingle(l => l.FeeTypeId == (int)FeeTypeIds.MemberLateRegistrationFee && l.Amount == 0m);
            result.Lines.Should().ContainSingle(l => l.FeeTypeId == (int)FeeTypeIds.UnitOmpFee && l.Amount == 0m);
            result.Lines.Should().ContainSingle(l => l.FeeTypeId == (int)FeeTypeIds.SubsidiaryFee && l.Amount == 0m);
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
                TotalResubmissionFee = 432.10m
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
            result.FileId.Should().Be(req.FileId.Value);
            result.ExternalId.Should().Be(req.ExternalId.Value);
            result.ApplicationReferenceNumber.Should().Be(req.ReferenceNumber);
            result.InvoicePeriod.Should().Be(invoicePeriod);
            result.InvoiceDate.Should().Be(invoiceDate);
            result.PayerTypeId.Should().Be(payerTypeId);
            result.PayerId.Should().Be(req.PayerId.Value);

            result.Lines.Should().NotBeNull();
            result.Lines.Should().HaveCount(1);

            var line = result.Lines.Single();
            line.FeeTypeId.Should().Be(resubmissionFeeTypeId);
            line.UnitPrice.Should().Be(432.10m);
            line.Quantity.Should().Be(1);
            line.Amount.Should().Be(432.10m);
        }

        public void BuildProducerRegistrationFeeSummaryRecord_SumsMembers()
        {
            // Arrange
            var mapper = new FeeSummarySaveProducerRequestMapper();

            var dto = new ProducerRegistrationFeesRequestDto
            {
                ProducerType = "Large",
                IsLateFeeApplicable = true,
                Regulator = "GB-ENG",
                ApplicationReferenceNumber = "A123",
                SubmissionDate = DateTime.UtcNow,
                FileId = Guid.NewGuid(),
                ExternalId = Guid.NewGuid(),
                PayerId = 42
            };

            var resp = new RegistrationFeesResponseDto
            {
                ProducerRegistrationFee = 1250m,
                MemberId = "M-001",
                SubsidiariesFeeBreakdown = null!,
                MemberRegistrationFee = 100m,
                MemberLateRegistrationFee = 10m,
                MemberOnlineMarketPlaceFee = 5m,
                SubsidiariesFee = 20m
            };

            var regulator = RegulatorType.Create("GB-ENG");
            var invoicePeriod = new DateTimeOffset(2025, 09, 01, 0, 0, 0, TimeSpan.Zero);
            var invoiceDate = new DateTimeOffset(2025, 09, 21, 0, 0, 0, TimeSpan.Zero);
            var payerTypeId = 2;

            // Act
            var result = mapper.BuildRegistrationFeeSummaryRecord(
                dto, invoicePeriod, payerTypeId, resp, invoiceDate);

            // Assert
            result.Should().NotBeNull();
            result.FileId.Should().Be(dto.FileId.Value);
            result.ExternalId.Should().Be(dto.ExternalId.Value);
            result.ApplicationReferenceNumber.Should().Be(dto.ApplicationReferenceNumber);
            result.InvoicePeriod.Should().Be(invoicePeriod);
            result.InvoiceDate.Should().Be(invoiceDate);
            result.PayerTypeId.Should().Be(payerTypeId);
            result.PayerId.Should().Be(dto.PayerId.Value);

            result.Lines.Should().NotBeNull();
            result.Lines.Should().HaveCount(5);
            result.Lines.Should().ContainSingle(l => l.FeeTypeId == (int)FeeTypeIds.ProducerRegistrationFee && l.Amount == 1250m);
            result.Lines.Should().ContainSingle(l => l.FeeTypeId == (int)FeeTypeIds.UnitOmpFee && l.Amount == 20m);
            result.Lines.Should().ContainSingle(l => l.FeeTypeId == (int)FeeTypeIds.SubsidiaryFee && l.Amount == 60m);
        }
    }
}
