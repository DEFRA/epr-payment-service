using EPR.Payment.Service.Common.Data.Dtos;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.FeeItems;
using EPR.Payment.Service.Common.Dtos.FeeItems;
using EPR.Payment.Service.Services.FeeItems;
using FluentAssertions;
using Moq;

namespace EPR.Payment.Service.UnitTests.Services.FeeItems
{
    [TestClass]
    public class FeeItemWriterTests
    {
        private Mock<IFeeItemRepository> _repo = null!;
        private FeeItemWriter _sut = null!;

        [TestInitialize]
        public void Setup()
        {
            _repo = new Mock<IFeeItemRepository>(MockBehavior.Strict);
            _sut = new FeeItemWriter(_repo.Object);
        }

        [TestMethod]
        public void Constructor_Throws_WhenRepositoryIsNull()
        {
            // Arrange
            IFeeItemRepository? repo = null;

            // Act
            Action act = () => new FeeItemWriter(repo!);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithParameterName("repository");
        }

        [TestMethod]
        public async Task Save_Throws_WhenRequestIsNull()
        {
            // Arrange
            FeeItemSaveRequest? req = null;

            // Act
            var act = async () => await _sut.Save(req!, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>()
                .WithParameterName("request");
        }

        [TestMethod]
        public async Task Save_MapsLinesAndPassesHeaders_UsingProvidedInvoiceDate()
        {
            // Arrange
            var externalId = Guid.NewGuid();
            var fileId = Guid.NewGuid();
            var appRef = "APP-123";
            var invoicePeriod = new DateTimeOffset(2025, 09, 01, 0, 0, 0, TimeSpan.Zero);
            var invoiceDate = new DateTimeOffset(2025, 09, 15, 12, 34, 56, TimeSpan.Zero);
            var payerTypeId = 2;
            var payerId = 77;

            var request = new FeeItemSaveRequest
            {
                ExternalId = externalId,
                FileId = fileId,
                ApplicationReferenceNumber = appRef,
                InvoicePeriod = invoicePeriod,
                InvoiceDate = invoiceDate,
                PayerTypeId = payerTypeId,
                PayerId = payerId,
                Lines = new List<FeeItemLine>
                {
                    new() { FeeTypeId = 10, UnitPrice = 2.5m, Quantity = null, Amount = 2.5m },
                    new() { FeeTypeId = 11, UnitPrice = 3m, Quantity = 3, Amount = 9m }
                }
            };

            FeeItemMappedRequest? capturedRequest = null;

            _repo.Setup(r => r.UpsertAsync(
                    It.IsAny<FeeItemMappedRequest>(),
                    It.IsAny<CancellationToken>()))
                .Callback((FeeItemMappedRequest req, CancellationToken ct) =>
                {
                    capturedRequest = req;
                })
                .Returns(Task.CompletedTask);

            // Act
            await _sut.Save(request, CancellationToken.None);

            // Assert
            capturedRequest.Should().NotBeNull();
            capturedRequest!.ExternalId.Should().Be(externalId);
            capturedRequest.AppRefNo.Should().Be(appRef);
            capturedRequest.InvoiceDate.Should().Be(invoiceDate);
            capturedRequest.InvoicePeriod.Should().Be(invoicePeriod);
            capturedRequest.PayerTypeId.Should().Be(payerTypeId);
            capturedRequest.PayerId.Should().Be(payerId);
            capturedRequest.FileId.Should().Be(fileId);
            capturedRequest.Items.Should().NotBeNull();

            var items = capturedRequest.Items.ToList();
            items.Should().HaveCount(2);

            var item0 = items[0];
            item0.FeeTypeId.Should().Be(10);
            item0.UnitPrice.Should().Be(2.5m);
            item0.Quantity.Should().Be(1, "quantity defaults to 1 when null");
            item0.Amount.Should().Be(2.5m);

            var item1 = items[1];
            item1.FeeTypeId.Should().Be(11);
            item1.UnitPrice.Should().Be(3m);
            item1.Quantity.Should().Be(3);
            item1.Amount.Should().Be(9m);

            _repo.VerifyAll();
        }

        [TestMethod]
        public async Task Save_UsesUtcNow_WhenInvoiceDateIsNull()
        {
            // Arrange
            var externalId = Guid.NewGuid();
            var fileId = Guid.NewGuid();
            var appRef = "APP-UTC";
            var invoicePeriod = new DateTimeOffset(2026, 01, 01, 0, 0, 0, TimeSpan.Zero);
            var payerTypeId = 9;
            var payerId = 123;

            var request = new FeeItemSaveRequest
            {
                ExternalId = externalId,
                FileId = fileId,
                ApplicationReferenceNumber = appRef,
                InvoicePeriod = invoicePeriod,
                InvoiceDate = null,
                PayerTypeId = payerTypeId,
                PayerId = payerId,
                Lines = new List<FeeItemLine>
                {
                    new() { FeeTypeId = 1, UnitPrice = 10m, Quantity = null, Amount = 10m }
                }
            };

            DateTimeOffset before = DateTimeOffset.UtcNow;
            FeeItemMappedRequest? capturedRequest = null;

            _repo.Setup(r => r.UpsertAsync(
                    It.IsAny<FeeItemMappedRequest>(),
                    It.IsAny<CancellationToken>()))
                .Callback((FeeItemMappedRequest req, CancellationToken ct) =>
                {
                    capturedRequest = req;
                })
                .Returns(Task.CompletedTask);

            // Act
            await _sut.Save(request, CancellationToken.None);

            // Assert
            DateTimeOffset after = DateTimeOffset.UtcNow;
            capturedRequest.Should().NotBeNull();
            capturedRequest!.InvoiceDate.Should().BeOnOrAfter(before).And.BeOnOrBefore(after);
            capturedRequest.ExternalId.Should().Be(externalId);
            capturedRequest.AppRefNo.Should().Be(appRef);
            capturedRequest.InvoicePeriod.Should().Be(invoicePeriod);
            capturedRequest.PayerTypeId.Should().Be(payerTypeId);
            capturedRequest.PayerId.Should().Be(payerId);
            capturedRequest.FileId.Should().Be(fileId);

            var items = capturedRequest.Items.ToList();
            items.Should().HaveCount(1);
            items[0].FeeTypeId.Should().Be(1);
            items[0].UnitPrice.Should().Be(10m);
            items[0].Quantity.Should().Be(1, "quantity defaults to 1 when null");
            items[0].Amount.Should().Be(10m);

            _repo.VerifyAll();
        }

        [TestMethod]
        public async Task Save_HandlesEmptyLinesCollection()
        {
            // Arrange
            var externalId = Guid.NewGuid();
            var fileId = Guid.NewGuid();
            var appRef = "APP-EMPTY";
            var invoicePeriod = new DateTimeOffset(2026, 01, 01, 0, 0, 0, TimeSpan.Zero);
            var invoiceDate = new DateTimeOffset(2026, 01, 02, 0, 0, 0, TimeSpan.Zero);
            var payerTypeId = 5;
            var payerId = 456;

            var request = new FeeItemSaveRequest
            {
                ExternalId = externalId,
                FileId = fileId,
                ApplicationReferenceNumber = appRef,
                InvoicePeriod = invoicePeriod,
                InvoiceDate = invoiceDate,
                PayerTypeId = payerTypeId,
                PayerId = payerId,
                Lines = new List<FeeItemLine>()
            };

            FeeItemMappedRequest? capturedRequest = null;

            _repo.Setup(r => r.UpsertAsync(
                    It.IsAny<FeeItemMappedRequest>(),
                    It.IsAny<CancellationToken>()))
                .Callback((FeeItemMappedRequest req, CancellationToken ct) =>
                {
                    capturedRequest = req;
                })
                .Returns(Task.CompletedTask);

            // Act
            await _sut.Save(request, CancellationToken.None);

            // Assert
            capturedRequest.Should().NotBeNull();
            capturedRequest!.ExternalId.Should().Be(externalId);
            capturedRequest.AppRefNo.Should().Be(appRef);
            capturedRequest.InvoiceDate.Should().Be(invoiceDate);
            capturedRequest.InvoicePeriod.Should().Be(invoicePeriod);
            capturedRequest.PayerTypeId.Should().Be(payerTypeId);
            capturedRequest.PayerId.Should().Be(payerId);
            capturedRequest.FileId.Should().Be(fileId);
            capturedRequest.Items.Should().BeEmpty();

            _repo.VerifyAll();
        }
    }
}