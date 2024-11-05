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
        private Mock<DbSet<Common.Data.DataModels.Payment>> _offlinePaymentMock = null!;
        private CancellationToken _cancellationToken;

        [TestInitialize]
        public void TestInitialize()
        {
            _offlinePaymentMock = MockIPaymentRepository.GetPaymentMock();
            _cancellationToken = new CancellationToken();
        }

        [TestMethod]
        [AutoMoqData]
        public async Task InsertOfflinePaymentStatusAsync_ValidInput_ShouldReturnGuid(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] OfflinePaymentsRepository _mockOfflinePaymentsRepository,
            [Frozen] Guid newId,
            [Frozen] Guid userId,
            [Frozen] string comments)
        {
            //Arrange
            _dataContextMock.Setup(i => i.Payment).ReturnsDbSet(_offlinePaymentMock.Object);
            _mockOfflinePaymentsRepository = new OfflinePaymentsRepository(_dataContextMock.Object);

            var request = new Common.Data.DataModels.Payment()
            {
                ExternalPaymentId = newId,
                UserId = userId,
                OfflinePayment = new Common.Data.DataModels.OfflinePayment()
                {
                    Comments = comments
                }
            };

            //Act
            await _mockOfflinePaymentsRepository.InsertOfflinePaymentAsync(request, _cancellationToken);


            //Assert
            using (new AssertionScope())
            {
                _dataContextMock.Verify(c => c.Payment.Add(It.Is<Common.Data.DataModels.Payment>(s => s.UserId == userId && s.OfflinePayment.Comments == comments)), Times.Once());
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
           [Frozen] string comments)
        {
            //Arrange
            _dataContextMock.Setup(i => i.Payment).ReturnsDbSet(_offlinePaymentMock.Object);
            _mockOfflinePaymentsRepository = new OfflinePaymentsRepository(_dataContextMock.Object);

            var firstRequest = new Common.Data.DataModels.Payment()
            {
                ExternalPaymentId = firstId,
                UserId = userId,
                OfflinePayment = new Common.Data.DataModels.OfflinePayment()
                {
                    Comments = comments
                }
            };

            var secondRequest = new Common.Data.DataModels.Payment()
            {
                ExternalPaymentId = secondId,
                UserId = userId,
                OfflinePayment = new Common.Data.DataModels.OfflinePayment()
                {
                    Comments = comments
                }
            };

            //Act
            await _mockOfflinePaymentsRepository.InsertOfflinePaymentAsync(firstRequest, _cancellationToken);
            await _mockOfflinePaymentsRepository.InsertOfflinePaymentAsync(secondRequest, _cancellationToken);


            //Assert
            using (new AssertionScope())
            {
                _dataContextMock.Verify(m => m.Payment.Add(It.IsAny<Common.Data.DataModels.Payment>()), Times.Exactly(2));
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
            _dataContextMock.Setup(i => i.Payment).ReturnsDbSet(_offlinePaymentMock.Object);
            _mockOfflinePaymentsRepository = new OfflinePaymentsRepository(_dataContextMock.Object);

            Common.Data.DataModels.Payment? request = null;


            //Act & Assert
            await _mockOfflinePaymentsRepository.Invoking(async x => await x.InsertOfflinePaymentAsync(request, _cancellationToken))
                .Should().ThrowAsync<ArgumentException>();
        }
    }
}
