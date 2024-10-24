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
    public class OnlinePaymentsRepositoryTests
    {
        private Mock<DbSet<Common.Data.DataModels.OnlinePayment>> _onlinePaymentMock = null!;
        private Mock<DbSet<Common.Data.DataModels.Lookups.PaymentStatus>> _paymentStatusMock = null!;
        private CancellationToken _cancellationToken;

        [TestInitialize]
        public void TestInitialize()
        {
            _onlinePaymentMock = MockIPaymentRepository.GetPaymentMock();
            _paymentStatusMock = MockIPaymentRepository.GetPaymentStatusMock(true);
            _cancellationToken = new CancellationToken();
        }

        [TestMethod]
        [AutoMoqData]
        public async Task InsertPaymentStatusAsync_ValidInput_ShouldReturnGuid(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] OnlinePaymentsRepository _mockOnlinePaymentsRepository,
            [Frozen] Guid newId,
            [Frozen] Guid userId,
            [Frozen] Guid organisationId)
        {
            //Arrange
            _dataContextMock.Setup(i => i.OnlinePayment).ReturnsDbSet(_onlinePaymentMock.Object);
            _mockOnlinePaymentsRepository = new OnlinePaymentsRepository(_dataContextMock.Object);

            var request = new Common.Data.DataModels.OnlinePayment
            {
                UserId = userId,
                OrganisationId = organisationId,
                ExternalPaymentId = newId
            };

            //Act
            Guid result = await _mockOnlinePaymentsRepository.InsertOnlinePaymentAsync(request, _cancellationToken);


            //Assert
            using (new AssertionScope())
            {
                result.Should().NotBe(Guid.Empty);
                _dataContextMock.Verify(c => c.OnlinePayment.Add(It.Is<Common.Data.DataModels.OnlinePayment>(s => s.UserId == userId && s.OrganisationId == organisationId)), Times.Once());
                _dataContextMock.Verify(c => c.SaveChangesAsync(default), Times.Once);
            }
        }

        [TestMethod]
        [AutoMoqData]
        public async Task InsertPaymentStatusAsync_TwoRecordsWithSameUserIdAndOrgId_VerifyBothRecorded(
           [Frozen] Mock<IAppDbContext> _dataContextMock,
           [Greedy] OnlinePaymentsRepository _mockOnlinePaymentsRepository,
           [Frozen] Guid firstId,
           [Frozen] Guid secondId,
           [Frozen] Guid userId,
           [Frozen] Guid organisationId)
        {
            //Arrange
            _dataContextMock.Setup(i => i.OnlinePayment).ReturnsDbSet(_onlinePaymentMock.Object);
            _mockOnlinePaymentsRepository = new OnlinePaymentsRepository(_dataContextMock.Object);

            var firstRequest = new Common.Data.DataModels.OnlinePayment
            {
                ExternalPaymentId = firstId,
                UserId = userId,
                OrganisationId = organisationId,
            };

            var secondRequest = new Common.Data.DataModels.OnlinePayment
            {
                ExternalPaymentId = secondId,
                UserId = userId,
                OrganisationId = organisationId,
            };

            //Act
            await _mockOnlinePaymentsRepository.InsertOnlinePaymentAsync(firstRequest, _cancellationToken);
            await _mockOnlinePaymentsRepository.InsertOnlinePaymentAsync(secondRequest, _cancellationToken);


            //Assert
            using (new AssertionScope())
            {
                _dataContextMock.Verify(m => m.OnlinePayment.Add(It.IsAny<Common.Data.DataModels.OnlinePayment>()), Times.Exactly(2));
                _dataContextMock.Verify(c => c.SaveChangesAsync(default), Times.Exactly(2));
            }
        }

        [TestMethod]
        [AutoMoqData]
        public async Task InsertPaymentStatusAsync_NullEntity_ShouldThrowArgumentException(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] OnlinePaymentsRepository _mockOnlinePaymentsRepository)
        {
            //Arrange
            _dataContextMock.Setup(i => i.OnlinePayment).ReturnsDbSet(_onlinePaymentMock.Object);
            _mockOnlinePaymentsRepository = new OnlinePaymentsRepository(_dataContextMock.Object);

            Common.Data.DataModels.OnlinePayment? request = null;


            //Act & Assert
            await _mockOnlinePaymentsRepository.Invoking(async x => await x.InsertOnlinePaymentAsync(request, _cancellationToken))
                .Should().ThrowAsync<ArgumentException>();
        }

        [TestMethod]
        [AutoMoqData]
        public async Task UpdatePaymentStatusAsync_ValidInput_ShouldComplete(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] OnlinePaymentsRepository _mockOnlinePaymentsRepository,
            [Frozen] Guid newId,
            [Frozen] Guid userId,
            [Frozen] Guid organisationId)
        {
            //Arrange
            _dataContextMock.Setup(i => i.OnlinePayment).ReturnsDbSet(_onlinePaymentMock.Object);
            _mockOnlinePaymentsRepository = new OnlinePaymentsRepository(_dataContextMock.Object);

            var request = new Common.Data.DataModels.OnlinePayment
            {
                ExternalPaymentId = newId,
                UserId = userId,
                OrganisationId = organisationId,
            };

            //Act
            await _mockOnlinePaymentsRepository.UpdateOnlinePaymentAsync(request, _cancellationToken);


            //Assert
            using (new AssertionScope())
            {
                _dataContextMock.Verify(c => c.OnlinePayment.Update(It.Is<Common.Data.DataModels.OnlinePayment>(s => s.UserId == userId && s.OrganisationId == organisationId)), Times.Once());
                _dataContextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Exactly(1));
            }
        }

        [TestMethod]
        [AutoMoqData]
        public async Task UpdatePaymentStatusAsync_NullEntity_ShouldThrowArgumentException(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] OnlinePaymentsRepository _mockOnlinePaymentsRepository)
        {
            //Arrange
            _dataContextMock.Setup(i => i.OnlinePayment).ReturnsDbSet(_onlinePaymentMock.Object);
            _mockOnlinePaymentsRepository = new OnlinePaymentsRepository(_dataContextMock.Object);


            Common.Data.DataModels.OnlinePayment? request = null;


            //Act & Assert
            await _mockOnlinePaymentsRepository.Invoking(async x => await x.UpdateOnlinePaymentAsync(request, _cancellationToken))
                .Should().ThrowAsync<ArgumentException>();
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetPaymentByIdAsync_PaymentExist_ShouldReturnPayment(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] OnlinePaymentsRepository _mockOnlinePaymentsRepository)
        {
            //Arrange
            _dataContextMock.Setup(i => i.OnlinePayment).ReturnsDbSet(_onlinePaymentMock.Object);
            _mockOnlinePaymentsRepository = new OnlinePaymentsRepository(_dataContextMock.Object);

            var externalPaymentId = Guid.Parse("d0f74b07-42e1-43a7-ae9d-0e279f213278");

            //Act
            var result = await _mockOnlinePaymentsRepository.GetOnlinePaymentByExternalPaymentIdAsync(externalPaymentId, _cancellationToken);

            //Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result!.ExternalPaymentId.Should().Be(externalPaymentId);
                result!.Amount.Should().Be(10.0M);
                result!.Regulator.Should().Be("Test 1 Regulator");
                result!.Reference.Should().Be("Test 1 Reference");
            }
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetPaymentByIdAsync_PaymentDoesNotExist_ShouldThrowKeyNotFoundException(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] OnlinePaymentsRepository _mockOnlinePaymentsRepository)
        {
            //Arrange
            _dataContextMock.Setup(i => i.OnlinePayment).ReturnsDbSet(_onlinePaymentMock.Object);
            _mockOnlinePaymentsRepository = new OnlinePaymentsRepository(_dataContextMock.Object);

            var externalPaymenId = Guid.Parse("c8459886-7a06-412c-9ca9-c9e9ab0aa72c");

            //Act & Assert
            await _mockOnlinePaymentsRepository.Invoking(async x => await x.GetOnlinePaymentByExternalPaymentIdAsync(externalPaymenId, _cancellationToken))
                .Should().ThrowAsync<KeyNotFoundException>();
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetPaymentStatusCount_PaymentStatusExists_ShouldReturnCountOfPaymentStatusRecords(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] OnlinePaymentsRepository _mockOnlinePaymentsRepository)
        {
            //Arrange
            _dataContextMock.Setup(i => i.PaymentStatus).ReturnsDbSet(_paymentStatusMock.Object);
            _mockOnlinePaymentsRepository = new OnlinePaymentsRepository(_dataContextMock.Object);

            //Act
            var result = await _mockOnlinePaymentsRepository.GetPaymentStatusCount(_cancellationToken);

            //Assert
            result.Should().Be(3);
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetPaymentStatusCount_PaymentStatusDoesNotExist_ShouldReturnZeroCountOfPaymentStatusRecords(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] OnlinePaymentsRepository _mockOnlinePaymentsRepository)
        {
            //Arrange
            _paymentStatusMock = MockIPaymentRepository.GetPaymentStatusMock(false);
            _dataContextMock.Setup(i => i.PaymentStatus).ReturnsDbSet(_paymentStatusMock.Object);
            _mockOnlinePaymentsRepository = new OnlinePaymentsRepository(_dataContextMock.Object);

            //Act
            var result = await _mockOnlinePaymentsRepository.GetPaymentStatusCount(_cancellationToken);

            //Assert
            result.Should().Be(0);
        }
    }
}
