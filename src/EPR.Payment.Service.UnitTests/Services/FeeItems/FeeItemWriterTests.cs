using EPR.Payment.Service.Common.Data.DataModels;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories.FeeItems;
using EPR.Payment.Service.Common.Dtos.FeeItems;
using EPR.Payment.Service.Services.FeeItems;
using FluentAssertions;
using Moq;
using Microsoft.Extensions.Logging;

namespace EPR.Payment.Service.UnitTests.Services.FeeItems
{
    [TestClass]
    public class FeeItemWriterTests
    {
        private Mock<IFeeItemRepository> _repo = null!;
        private Mock<ILogger<FeeItemWriter>> _logger = null!;
        private FeeItemWriter _sut = null!;

        [TestInitialize]
        public void Setup()
        {
            _repo = new Mock<IFeeItemRepository>(MockBehavior.Strict);
            _logger = new Mock<ILogger<FeeItemWriter>>();
            _sut = new FeeItemWriter(_repo.Object, _logger.Object);
        }

        [TestMethod]
        public async Task Save_Throws_WhenRequestIsNull()
        {
            // Arrange
            FeeSummarySaveRequest? req = null;

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

            var request = new FeeSummarySaveRequest
            {
                ExternalId = externalId,
                FileId = fileId,
                ApplicationReferenceNumber = appRef,
                InvoicePeriod = invoicePeriod,
                InvoiceDate = invoiceDate,
                PayerTypeId = payerTypeId,
                PayerId = payerId,
                Lines = new List<FeeSummaryLineRequest>
                {
                    new() { FeeTypeId = 10, UnitPrice = 2.5m, Quantity = null, Amount = 2.5m }, 
                    new() { FeeTypeId = 11, UnitPrice = 3m,   Quantity = 3,    Amount = 9m   }
                }
            };

            Guid capturedExternalId = Guid.Empty;
            string? capturedAppRef = null;
            DateTimeOffset capturedInvoiceDate = default;
            DateTimeOffset capturedInvoicePeriod = default;
            int capturedPayerTypeId = default;
            int capturedPayerId = default;
            Guid capturedFileId = Guid.Empty;
            IEnumerable<FeeItem>? capturedItems = null;
            CancellationToken capturedToken = default;

            _repo.Setup(r => r.UpsertAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTimeOffset>(),
                    It.IsAny<DateTimeOffset>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<Guid>(),
                    It.IsAny<IEnumerable<FeeItem>>(),
                    It.IsAny<CancellationToken>()))
                .Callback((Guid ex, string ar, DateTimeOffset id, DateTimeOffset ip, int pt, int p, Guid f, IEnumerable<FeeItem> it, CancellationToken ct) =>
                {
                    capturedExternalId = ex;
                    capturedAppRef = ar;
                    capturedInvoiceDate = id;
                    capturedInvoicePeriod = ip;
                    capturedPayerTypeId = pt;
                    capturedPayerId = p;
                    capturedFileId = f;
                    capturedItems = it?.ToList();
                    capturedToken = ct;
                })
                .Returns(Task.CompletedTask);

            // Act
            await _sut.Save(request, CancellationToken.None);

            // Assert 
            capturedExternalId.Should().Be(externalId);
            capturedAppRef.Should().Be(appRef);
            capturedInvoiceDate.Should().Be(invoiceDate);
            capturedInvoicePeriod.Should().Be(invoicePeriod);
            capturedPayerTypeId.Should().Be(payerTypeId);
            capturedPayerId.Should().Be(payerId);
            capturedFileId.Should().Be(fileId);
            capturedToken.Should().Be(CancellationToken.None);

            // Assert
            capturedItems.Should().NotBeNull();
            var list = capturedItems!.ToList();
            list.Should().HaveCount(2);

            var i0 = list[0];
            i0.FeeTypeId.Should().Be(10);
            i0.UnitPrice.Should().Be(2.5m);
            i0.Quantity.Should().Be(1, "quantity defaults to 1 when null");
            i0.Amount.Should().Be(2.5m);

            var i1 = list[1];
            i1.FeeTypeId.Should().Be(11);
            i1.UnitPrice.Should().Be(3m);
            i1.Quantity.Should().Be(3);
            i1.Amount.Should().Be(9m);

            _repo.VerifyAll();
        }

        [TestMethod]
        public async Task Save_UsesUtcNow_WhenInvoiceDateIsNull()
        {
            // Arrange
            var request = new FeeSummarySaveRequest
            {
                ExternalId = Guid.NewGuid(),
                FileId = Guid.NewGuid(),
                ApplicationReferenceNumber = "APP-UTC",
                InvoicePeriod = new DateTimeOffset(2026, 01, 01, 0, 0, 0, TimeSpan.Zero),
                InvoiceDate = null,
                PayerTypeId = 9,
                PayerId = 123,
                Lines = new List<FeeSummaryLineRequest>
                {
                    new() { FeeTypeId = 1, UnitPrice = 10m, Quantity = null, Amount = 10m }
                }
            };

            DateTimeOffset before = DateTimeOffset.UtcNow;
            DateTimeOffset capturedInvoiceDate = default;

            _repo.Setup(r => r.UpsertAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTimeOffset>(),
                    It.IsAny<DateTimeOffset>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<Guid>(),
                    It.Is<IEnumerable<FeeItem>>(it => it.Count() == 1 && it.First().Quantity == 1),
                    It.IsAny<CancellationToken>()))
                .Callback((Guid _, string __, DateTimeOffset id, DateTimeOffset ___, int ____, int _____, Guid ______, IEnumerable<FeeItem> _______, CancellationToken ________) =>
                {
                    capturedInvoiceDate = id;
                })
                .Returns(Task.CompletedTask);

            // Act
            await _sut.Save(request, CancellationToken.None);

            // Assert
            DateTimeOffset after = DateTimeOffset.UtcNow;
            capturedInvoiceDate.Should().BeOnOrAfter(before).And.BeOnOrBefore(after);

            _repo.VerifyAll();
        }
    }
}
