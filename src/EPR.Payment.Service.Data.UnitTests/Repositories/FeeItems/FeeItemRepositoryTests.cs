using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Data.DataModels;
using EPR.Payment.Service.Common.Data.Dtos;
using EPR.Payment.Service.Common.Data.Interfaces;
using EPR.Payment.Service.Common.Data.Repositories.FeeItems;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.EntityFrameworkCore;

namespace EPR.Payment.Service.Data.UnitTests.Repositories.FeeItems
{
    [TestClass]
    public class FeeItemRepositoryTests
    {
        [TestMethod, AutoMoqData]
        public async Task UpsertAsync_WhenExistingItem_UpdatesAmountsAndUpdatedDate(
            [Frozen] Mock<IAppDbContext> db)
        {
            // Arrange
            var externalId = Guid.NewGuid();
            var appRefNo = "APP-002";
            var invoiceDate = new DateTimeOffset(2025, 09, 21, 0, 0, 0, TimeSpan.Zero);
            var invoicePeriod = new DateTimeOffset(2025, 09, 01, 0, 0, 0, TimeSpan.Zero);
            var payerTypeId = 2;
            var payerId = 456;
            var fileId = Guid.NewGuid();

            var existing = new FeeItem
            {
                Id = 101,
                ExternalId = externalId,
                AppRefNo = appRefNo,
                InvoiceDate = invoiceDate,
                InvoicePeriod = invoicePeriod,
                PayerTypeId = payerTypeId,
                PayerId = payerId,
                FileId = fileId,
                FeeTypeId = 7,
                UnitPrice = 10m,
                Quantity = 2,
                Amount = 20m,
                CreatedDate = DateTimeOffset.UtcNow
            };

            var feeItems = new List<FeeItem> { existing };
            db.Setup(d => d.FeeItems).ReturnsDbSet(feeItems);
            db.Setup(d => d.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var repo = new FeeItemRepository(db.Object);

            var incoming = new FeeItem
            {
                FeeTypeId = 7, 
                UnitPrice = 50m,
                Quantity = 3,
                Amount = 150m
            };

            var request = new FeeItemMappedRequest
            {
                ExternalId = externalId,
                AppRefNo = appRefNo,
                InvoiceDate = invoiceDate,
                InvoicePeriod = invoicePeriod,
                PayerTypeId = payerTypeId,
                PayerId = payerId,
                FileId = fileId,
                Items = new[] { incoming }
            };

            // Act
            await repo.UpsertAsync(request, CancellationToken.None);

            // Assert
            feeItems.Should().HaveCount(1);
            existing.UnitPrice.Should().Be(50m);
            existing.Quantity.Should().Be(3);
            existing.Amount.Should().Be(150m);
            existing.UpdatedDate.Should().NotBeNull();

            db.Verify(d => d.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod, AutoMoqData]
        public async Task UpsertAsync_WhenExistingItem_SameCompositeKey_DoesNotDuplicate_InsertsOrUpdatesOnce(
            [Frozen] Mock<IAppDbContext> db)
        {
            // Arrange
            var externalId = Guid.NewGuid();
            var appRefNo = "APP-003";
            var invoiceDate = new DateTimeOffset(2025, 09, 21, 0, 0, 0, TimeSpan.Zero);
            var invoicePeriod = new DateTimeOffset(2025, 09, 01, 0, 0, 0, TimeSpan.Zero);
            var payerTypeId = 2;
            var payerId = 789;
            var fileId = Guid.NewGuid();

            var existing = new FeeItem
            {
                Id = 202,
                ExternalId = externalId,
                AppRefNo = appRefNo,
                InvoiceDate = invoiceDate,
                InvoicePeriod = invoicePeriod,
                PayerTypeId = payerTypeId,
                PayerId = payerId,
                FileId = fileId,
                FeeTypeId = 5,
                UnitPrice = 20m,
                Quantity = 1,
                Amount = 20m,
                CreatedDate = DateTimeOffset.UtcNow
            };

            var feeItems = new List<FeeItem> { existing };
            db.Setup(d => d.FeeItems).ReturnsDbSet(feeItems);
            db.Setup(d => d.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var repo = new FeeItemRepository(db.Object);

            var incoming = new FeeItem
            {
                FeeTypeId = 5,    
                UnitPrice = 75m,
                Quantity = 2,
                Amount = 150m
            };

            var request = new FeeItemMappedRequest
            {
                ExternalId = externalId,
                AppRefNo = appRefNo,
                InvoiceDate = invoiceDate,
                InvoicePeriod = invoicePeriod,
                PayerTypeId = payerTypeId,
                PayerId = payerId,
                FileId = fileId,
                Items = new[] { incoming }
            };

            // Act
            await repo.UpsertAsync(request, CancellationToken.None);

            // Assert
            feeItems.Should().HaveCount(1);
            var updated = feeItems.Single();

            updated.AppRefNo.Should().Be(appRefNo);
            updated.InvoicePeriod.Should().Be(invoicePeriod);
            updated.FeeTypeId.Should().Be(5);
            updated.PayerTypeId.Should().Be(payerTypeId);
            updated.PayerId.Should().Be(payerId);
            updated.FileId.Should().Be(fileId);

            updated.UnitPrice.Should().Be(75m);
            updated.Quantity.Should().Be(2);
            updated.Amount.Should().Be(150m);
            updated.UpdatedDate.Should().NotBeNull();

            db.Verify(d => d.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
