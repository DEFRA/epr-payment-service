using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Data.Interfaces;
using EPR.Payment.Service.Common.Data.Repositories.Payments;
using EPR.Payment.Service.Common.UnitTests.Mocks;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.EntityFrameworkCore;
using System.Data.Entity;

namespace EPR.Payment.Service.Data.UnitTests.Repositories.Payments
{
    [TestClass]
    public class OfflinePaymentsRepositoryTests
    {
        private Mock<DbSet<Common.Data.DataModels.OfflinePayment>> _offlinePaymentMock = null!;
        private Mock<DbSet<Common.Data.DataModels.Lookups.PaymentStatus>> _paymentStatusMock = null!;
        private CancellationToken _cancellationToken;

        [TestInitialize]
        public void TestInitialize()
        {
            _offlinePaymentMock = MockIOfflinePaymentRepository.GetPaymentMock();
            //_paymentStatusMock = MockIPaymentRepository.GetPaymentStatusMock(true);
            _cancellationToken = new CancellationToken();
        }

        [TestMethod]
        [AutoMoqData]
        public async Task InsertOfflinePaymentStatusAsync_ValidInput_ShouldReturnGuid(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] OfflinePaymentsRepository _mockOfflinePaymentsRepository,
            [Frozen] Guid newId,
            [Frozen] Guid userId,
            [Frozen] Guid organisationId)
        {
            //Arrange
            _dataContextMock.Setup(i => i.OfflinePayment).ReturnsDbSet(_offlinePaymentMock.Object);
            _mockOfflinePaymentsRepository = new OfflinePaymentsRepository(_dataContextMock.Object);

            var request = new Common.Data.DataModels.OfflinePayment
            {
                UserId = userId,
                ExternalPaymentId = newId
            };

            //Act
            Guid result = await _mockOfflinePaymentsRepository.InsertOfflinePaymentAsync(request, _cancellationToken);


            //Assert
            using (new AssertionScope())
            {
                result.Should().NotBe(Guid.Empty);
                _dataContextMock.Verify(c => c.OfflinePayment.Add(It.Is<Common.Data.DataModels.OfflinePayment>(s => s.UserId == userId)), Times.Once());
                _dataContextMock.Verify(c => c.SaveChangesAsync(default), Times.Once);
            }
        }

        [TestMethod]
        [AutoMoqData]
        public async Task InsertOfflinePaymentStatusAsync_TwoRecordsWithSameUserId_VerifyBothRecorded(
           [Frozen] Mock<IAppDbContext> _dataContextMock,
           [Greedy] OfflinePaymentsRepository _mockOfflinePaymentsRepository,
           [Frozen] Guid firstId,
           [Frozen] Guid secondId,
           [Frozen] Guid userId,
           [Frozen] Guid organisationId)
        {
            //Arrange
            _dataContextMock.Setup(i => i.OfflinePayment).ReturnsDbSet(_offlinePaymentMock.Object);
            _mockOfflinePaymentsRepository = new OfflinePaymentsRepository(_dataContextMock.Object);

            var firstRequest = new Common.Data.DataModels.OfflinePayment
            {
                ExternalPaymentId = firstId,
                UserId = userId
            };

            var secondRequest = new Common.Data.DataModels.OfflinePayment
            {
                ExternalPaymentId = secondId,
                UserId = userId
            };

            //Act
            await _mockOfflinePaymentsRepository.InsertOfflinePaymentAsync(firstRequest, _cancellationToken);
            await _mockOfflinePaymentsRepository.InsertOfflinePaymentAsync(secondRequest, _cancellationToken);


            //Assert
            using (new AssertionScope())
            {
                _dataContextMock.Verify(m => m.OfflinePayment.Add(It.IsAny<Common.Data.DataModels.OfflinePayment>()), Times.Exactly(2));
                _dataContextMock.Verify(c => c.SaveChangesAsync(default), Times.Exactly(2));
            }
        }

        [TestMethod]
        [AutoMoqData]
        public async Task InsertOfflinePaymentStatusAsync_NullEntity_ShouldThrowArgumentException(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] OfflinePaymentsRepository _mockOfflinePaymentsRepository)
        {
            //Arrange
            _dataContextMock.Setup(i => i.OfflinePayment).ReturnsDbSet(_offlinePaymentMock.Object);
            _mockOfflinePaymentsRepository = new OfflinePaymentsRepository(_dataContextMock.Object);

            Common.Data.DataModels.OfflinePayment? request = null;


            //Act & Assert
            await _mockOfflinePaymentsRepository.Invoking(async x => await x.InsertOfflinePaymentAsync(request, _cancellationToken))
                .Should().ThrowAsync<ArgumentException>();
        }
    }
}
