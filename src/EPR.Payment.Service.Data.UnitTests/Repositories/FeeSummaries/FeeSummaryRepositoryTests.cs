using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Data.DataModels;
using EPR.Payment.Service.Common.Data.Interfaces;
using EPR.Payment.Service.Common.Data.Repositories.FeeSummaries;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EPR.Payment.Service.Data.UnitTests.Repositories.FeeSummaries
{
    [TestClass]
    public class FeeSummaryRepositoryTests
    {
        [TestMethod, AutoMoqData]
        public async Task UpsertAsync_WhenNoExistingItem_InsertsAndLinksWithProvidedFileId(
            [Frozen] Mock<IAppDbContext> db)
        {
            // Arrange
            var feeSummaries = new List<FeeSummary>();
            db.Setup(d => d.FeeSummaries).ReturnsDbSet(feeSummaries);

            var linkSetMock = new Mock<DbSet<FileFeeSummaryConnection>>();
            FileFeeSummaryConnection? capturedLink = null;

            linkSetMock
                .Setup(s => s.AddAsync(It.IsAny<FileFeeSummaryConnection>(), It.IsAny<CancellationToken>()))
                .Callback<FileFeeSummaryConnection, CancellationToken>((e, _) => capturedLink = e)
                .Returns((FileFeeSummaryConnection _, CancellationToken __) =>
                    default(ValueTask<EntityEntry<FileFeeSummaryConnection>>));

            db.Setup(d => d.FileFeeSummaryConnections).Returns(linkSetMock.Object);
            db.Setup(d => d.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var repo = new FeeSummaryRepository(db.Object);

            var externalId    = Guid.NewGuid();
            var appRefNo      = "APP-001";
            var invoiceDate   = new DateTimeOffset(2025, 09, 21, 0, 0, 0, TimeSpan.Zero);
            var invoicePeriod = new DateTimeOffset(2025, 09, 01, 0, 0, 0, TimeSpan.Zero);
            var payerTypeId   = 2;
            var payerId       = 123;
            var fileId        = Guid.NewGuid();

            var line = new FeeSummary
            {
                FeeTypeId = 10,
                UnitPrice = 100m,
                Quantity  = 1,
                Amount    = 100m,
                FileFeeSummaryConnections = new List<FileFeeSummaryConnection>()
            };

            // Act
            await repo.UpsertAsync(
                externalId, appRefNo, invoiceDate, invoicePeriod,
                payerTypeId, payerId, fileId, new[] { line }, CancellationToken.None);

            // Assert
            line.ExternalId.Should().Be(externalId);
            line.AppRefNo.Should().Be(appRefNo);
            line.InvoiceDate.Should().Be(invoiceDate);
            line.InvoicePeriod.Should().Be(invoicePeriod);
            line.PayerTypeId.Should().Be(payerTypeId);
            line.PayerId.Should().Be(payerId);
            line.CreatedDate.Should().NotBe(default);

            capturedLink.Should().NotBeNull();
            capturedLink!.FileId.Should().Be(fileId);
            capturedLink.FeeSummary.Should().BeSameAs(line);

            linkSetMock.Verify(s => s.AddAsync(It.IsAny<FileFeeSummaryConnection>(), It.IsAny<CancellationToken>()), Times.Once);
            db.Verify(d => d.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod, AutoMoqData]
        public async Task UpsertAsync_WhenExistingItem_NoLink_AddsLinkAndUpdatesAmounts(
            [Frozen] Mock<IAppDbContext> db)
        {
            // Arrange
            var feeSummaries = new List<FeeSummary>();
            db.Setup(d => d.FeeSummaries).ReturnsDbSet(feeSummaries);

            var linkSetMock = new Mock<DbSet<FileFeeSummaryConnection>>();
            FileFeeSummaryConnection? capturedLink = null;

            linkSetMock
                .Setup(s => s.AddAsync(It.IsAny<FileFeeSummaryConnection>(), It.IsAny<CancellationToken>()))
                .Callback<FileFeeSummaryConnection, CancellationToken>((e, _) => capturedLink = e)
                .Returns((FileFeeSummaryConnection _, CancellationToken __) =>
                    default(ValueTask<EntityEntry<FileFeeSummaryConnection>>));

            db.Setup(d => d.FileFeeSummaryConnections).Returns(linkSetMock.Object);
            db.Setup(d => d.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var repo = new FeeSummaryRepository(db.Object);

            var externalId    = Guid.NewGuid();
            var appRefNo      = "APP-002";
            var invoiceDate   = new DateTimeOffset(2025, 09, 21, 0, 0, 0, TimeSpan.Zero);
            var invoicePeriod = new DateTimeOffset(2025, 09, 01, 0, 0, 0, TimeSpan.Zero);
            var payerTypeId   = 2;
            var payerId       = 456;
            var fileIdToLink  = Guid.NewGuid();

            var existing = new FeeSummary
            {
                Id            = 101,
                ExternalId    = externalId,
                AppRefNo      = appRefNo,
                InvoiceDate   = invoiceDate,
                InvoicePeriod = invoicePeriod,
                PayerTypeId   = payerTypeId,
                PayerId       = payerId,
                FeeTypeId     = 7,
                UnitPrice     = 10m,
                Quantity      = 2,
                Amount        = 20m,
                CreatedDate   = DateTimeOffset.UtcNow,
                FileFeeSummaryConnections = new List<FileFeeSummaryConnection>()
            };
            feeSummaries.Add(existing);

            var incoming = new FeeSummary
            {
                FeeTypeId = 7,
                UnitPrice = 50m,
                Quantity  = 3,
                Amount    = 150m,
            };

            // Act
            await repo.UpsertAsync(
                externalId, appRefNo, invoiceDate, invoicePeriod,
                payerTypeId, payerId, fileIdToLink, new[] { incoming }, CancellationToken.None);

            // Assert
            existing.UnitPrice.Should().Be(50m);
            existing.Quantity.Should().Be(3);
            existing.Amount.Should().Be(150m);
            existing.UpdatedDate.Should().NotBeNull();

            // Assert
            capturedLink.Should().NotBeNull();
            capturedLink!.FileId.Should().Be(fileIdToLink);
            capturedLink.FeeSummaryId.Should().Be(existing.Id);

            // Assert
            linkSetMock.Verify(s => s.AddAsync(It.IsAny<FileFeeSummaryConnection>(), It.IsAny<CancellationToken>()), Times.Once);
            db.Verify(d => d.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod, AutoMoqData]
        public async Task UpsertAsync_WhenExistingItem_WithLink_UpdatesButDoesNotDuplicateLink(
            [Frozen] Mock<IAppDbContext> db)
        {
            // Arrange
            var feeSummaries = new List<FeeSummary>();
            db.Setup(d => d.FeeSummaries).ReturnsDbSet(feeSummaries);

            var linkSetMock = new Mock<DbSet<FileFeeSummaryConnection>>();
            db.Setup(d => d.FileFeeSummaryConnections).Returns(linkSetMock.Object);

            db.Setup(d => d.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var repo = new FeeSummaryRepository(db.Object);

            var externalId    = Guid.NewGuid();
            var appRefNo      = "APP-003";
            var invoiceDate   = new DateTimeOffset(2025, 09, 21, 0, 0, 0, TimeSpan.Zero);
            var invoicePeriod = new DateTimeOffset(2025, 09, 01, 0, 0, 0, TimeSpan.Zero);
            var payerTypeId   = 2;
            var payerId       = 789;
            var fileId        = Guid.NewGuid();

            var existing = new FeeSummary
            {
                Id            = 202,
                ExternalId    = externalId,
                AppRefNo      = appRefNo,
                InvoiceDate   = invoiceDate,
                InvoicePeriod = invoicePeriod,
                PayerTypeId   = payerTypeId,
                PayerId       = payerId,
                FeeTypeId     = 5,
                UnitPrice     = 20m,
                Quantity      = 1,
                Amount        = 20m,
                CreatedDate   = DateTimeOffset.UtcNow,
                FileFeeSummaryConnections = new List<FileFeeSummaryConnection>
                {
                    new FileFeeSummaryConnection { FeeSummaryId = 202, FileId = fileId }
                }
            };
            feeSummaries.Add(existing);

            var incoming = new FeeSummary
            {
                FeeTypeId = 5,
                UnitPrice = 75m,
                Quantity  = 2,
                Amount    = 150m
            };

            // Act
            await repo.UpsertAsync(
                externalId, appRefNo, invoiceDate, invoicePeriod,
                payerTypeId, payerId, fileId, new[] { incoming }, CancellationToken.None);

            // Assert
            existing.UnitPrice.Should().Be(75m);
            existing.Quantity.Should().Be(2);
            existing.Amount.Should().Be(150m);

            // Assert
            linkSetMock.Verify(s => s.AddAsync(It.IsAny<FileFeeSummaryConnection>(), It.IsAny<CancellationToken>()), Times.Never);
            db.Verify(d => d.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
