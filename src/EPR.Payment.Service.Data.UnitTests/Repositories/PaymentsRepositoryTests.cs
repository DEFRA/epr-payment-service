using AutoFixture.MSTest;
using EPR.Payment.Service.Common.Data.Interfaces;
using EPR.Payment.Service.Common.Data.Repositories;
using EPR.Payment.Service.Common.UnitTests.Mocks;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.EntityFrameworkCore;
using System.Data.Entity;

namespace EPR.Payment.Service.Data.UnitTests.Repositories
{
    [TestClass]
    public class PaymentsRepositoryTests
    {
        private Mock<DbSet<Common.Data.DataModels.Payment>> _paymentMock = null!;
        private Mock<DbSet<Common.Data.DataModels.Lookups.PaymentStatus>> _paymentStatusMock = null!;
        private CancellationToken _cancellationToken;

        [TestInitialize]
        public void TestInitialize()
        {
            _paymentMock = MockIPaymentRepository.GetPaymentMock();
            _paymentStatusMock = MockIPaymentRepository.GetPaymentStatusMock(true);
            _cancellationToken = new CancellationToken();
        }

        [TestMethod]
        [AutoMoqData]
        public async Task InsertPaymentStatusAsync_ValidInput_ShouldReturnGuid(
            [Frozen] Mock<IAppDbContext> _dataContextMock, 
            [Greedy] PaymentsRepository _mockPaymentsRepository,
            [Frozen] Guid newId,
            [Frozen] Guid userId,
            [Frozen] Guid organisationId)
        {
            //Arrange
            _dataContextMock.Setup(i => i.Payment).ReturnsDbSet(_paymentMock.Object);
            _mockPaymentsRepository = new PaymentsRepository(_dataContextMock.Object);

            var request = new Common.Data.DataModels.Payment
            {
                Id = newId,
                UserId = userId,
                OrganisationId = organisationId,
            };

            //Act
            Guid result = await _mockPaymentsRepository.InsertPaymentStatusAsync(request, _cancellationToken);


            //Assert
            using (new AssertionScope())
            {
                result.Should().NotBe(Guid.Empty);
                _dataContextMock.Verify(c => c.Payment.Add(It.Is<Common.Data.DataModels.Payment>(s => s.UserId == userId && s.OrganisationId == organisationId)), Times.Once());
                _dataContextMock.Verify(c => c.SaveChangesAsync(default), Times.Once);
            }
        }

        [TestMethod]
        [AutoMoqData]
        public async Task InsertPaymentStatusAsync_TwoRecordsWithSameUserIdAndOrgId_VerifyBothRecorded(
           [Frozen] Mock<IAppDbContext> _dataContextMock,
           [Greedy] PaymentsRepository _mockPaymentsRepository,
           [Frozen] Guid firstId,
           [Frozen] Guid secondId,
           [Frozen] Guid userId,
           [Frozen] Guid organisationId)
        {
            //Arrange
            _dataContextMock.Setup(i => i.Payment).ReturnsDbSet(_paymentMock.Object);
            _mockPaymentsRepository = new PaymentsRepository(_dataContextMock.Object);

            var firstRequest = new Common.Data.DataModels.Payment
            {
                Id = firstId,
                UserId = userId,
                OrganisationId = organisationId,
            };

            var secondRequest = new Common.Data.DataModels.Payment
            {
                Id = secondId,
                UserId = userId,
                OrganisationId = organisationId,
            };

            //Act
            await _mockPaymentsRepository.InsertPaymentStatusAsync(firstRequest, _cancellationToken);
            await _mockPaymentsRepository.InsertPaymentStatusAsync(secondRequest, _cancellationToken);


            //Assert
            _dataContextMock.Verify(m => m.Payment.Add(It.IsAny<Common.Data.DataModels.Payment>()), Times.Exactly(2));
            _dataContextMock.Verify(c => c.SaveChangesAsync(default), Times.Exactly(2));
        }

        [TestMethod]
        [AutoMoqData]
        public async Task InsertPaymentStatusAsync_NullEntity_ShouldThrowArgumentException(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] PaymentsRepository _mockPaymentsRepository)
        {
            //Arrange
            _dataContextMock.Setup(i => i.Payment).ReturnsDbSet(_paymentMock.Object);
            _mockPaymentsRepository = new PaymentsRepository(_dataContextMock.Object);

            Common.Data.DataModels.Payment? request = null;


            //Act & Assert
            await _mockPaymentsRepository.Invoking(async x => await x.InsertPaymentStatusAsync(request, _cancellationToken))
                .Should().ThrowAsync<ArgumentException>();
        }

        [TestMethod]
        [AutoMoqData]
        public async Task UpdatePaymentStatusAsync_ValidInput_ShouldComplete(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] PaymentsRepository _mockPaymentsRepository,
            [Frozen] Guid newId,
            [Frozen] Guid userId,
            [Frozen] Guid organisationId)
        {
            //Arrange
            _dataContextMock.Setup(i => i.Payment).ReturnsDbSet(_paymentMock.Object);
            _mockPaymentsRepository = new PaymentsRepository(_dataContextMock.Object);

            var request = new Common.Data.DataModels.Payment
            {
                Id = newId,
                UserId = userId,
                OrganisationId = organisationId,
            };

            //Act
            await _mockPaymentsRepository.UpdatePaymentStatusAsync(request, _cancellationToken);


            //Assert
            using (new AssertionScope())
            {
                _dataContextMock.Verify(c => c.Payment.Update(It.Is<Common.Data.DataModels.Payment>(s => s.UserId == userId && s.OrganisationId == organisationId)), Times.Once());
                _dataContextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Exactly(1));
            }
        }

        [TestMethod]
        [AutoMoqData]
        public async Task UpdatePaymentStatusAsync_NullEntity_ShouldThrowArgumentException(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] PaymentsRepository _mockPaymentsRepository)
        {
            //Arrange
            _dataContextMock.Setup(i => i.Payment).ReturnsDbSet(_paymentMock.Object);
            _mockPaymentsRepository = new PaymentsRepository(_dataContextMock.Object);


            Common.Data.DataModels.Payment? request = null;


            //Act & Assert
            await _mockPaymentsRepository.Invoking(async x => await x.UpdatePaymentStatusAsync(request, _cancellationToken))
                .Should().ThrowAsync<ArgumentException>();
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetPaymentByIdAsync_PaymentExist_ShouldReturnPayment(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] PaymentsRepository _mockPaymentsRepository)
        {
            //Arrange
            _dataContextMock.Setup(i => i.Payment).ReturnsDbSet(_paymentMock.Object);
            _mockPaymentsRepository = new PaymentsRepository(_dataContextMock.Object);

            var id = Guid.Parse("fe40c799-c027-4a8b-9172-3343c701b4db");

            //Act
            var result = await _mockPaymentsRepository.GetPaymentByIdAsync(id, _cancellationToken);

            //Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.Id.Should().Be(id);
                result.Amount.Should().Be(10.0M);
                result.Regulator.Should().Be("Test 1 Regulator");
                result.Reference.Should().Be("Test 1 Reference");
            }
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetPaymentByIdAsync_PaymentDoesNotExist_ShouldThrowKeyNotFoundException(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] PaymentsRepository _mockPaymentsRepository)
        {
            //Arrange
            _dataContextMock.Setup(i => i.Payment).ReturnsDbSet(_paymentMock.Object);
            _mockPaymentsRepository = new PaymentsRepository(_dataContextMock.Object);

            var id = Guid.Parse("c8459886-7a06-412c-9ca9-c9e9ab0aa72c");

            //Act & Assert
            await _mockPaymentsRepository.Invoking(async x => await x.GetPaymentByIdAsync(id, _cancellationToken))
                .Should().ThrowAsync<KeyNotFoundException>();
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetPaymentStatusCount_PaymentStatusExists_ShouldReturnCountOfPaymentStatusRecords(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] PaymentsRepository _mockPaymentsRepository)
        {
            //Arrange
            _dataContextMock.Setup(i => i.PaymentStatus).ReturnsDbSet(_paymentStatusMock.Object);
            _mockPaymentsRepository = new PaymentsRepository(_dataContextMock.Object);

            //Act
            var result = await _mockPaymentsRepository.GetPaymentStatusCount(_cancellationToken);

            //Assert
            result.Should().Be(3);
        }

        [TestMethod]
        [AutoMoqData]
        public async Task GetPaymentStatusCount_PaymentStatusDoesNotExist_ShouldReturnZeroCountOfPaymentStatusRecords(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] PaymentsRepository _mockPaymentsRepository)
        {
            //Arrange
            _paymentStatusMock = MockIPaymentRepository.GetPaymentStatusMock(false);
            _dataContextMock.Setup(i => i.PaymentStatus).ReturnsDbSet(_paymentStatusMock.Object);
            _mockPaymentsRepository = new PaymentsRepository(_dataContextMock.Object);

            //Act
            var result = await _mockPaymentsRepository.GetPaymentStatusCount(_cancellationToken);

            //Assert
            result.Should().Be(0);
        }
    }
}
