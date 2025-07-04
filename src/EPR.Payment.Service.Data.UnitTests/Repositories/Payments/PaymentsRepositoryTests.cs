﻿using AutoFixture.MSTest;
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
    public class PaymentsRepositoryTests
    {
        private Mock<DbSet<Common.Data.DataModels.Payment>> _paymentMock = null!;
        private CancellationToken _cancellationToken;

        [TestInitialize]
        public void TestInitialize()
        {
            _paymentMock = MockIPaymentRepository.GetPaymentMock();
            _cancellationToken = new CancellationToken();
        }

        [TestMethod, AutoMoqData]
        public async Task GetPreviousPaymentByReferenceAsync_SinglePaymentExist_ShouldReturnPreviousAmounts(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] PaymentsRepository _mockPaymentsRepository)
        {
            //Arrange
            _dataContextMock.Setup(i => i.Payment).ReturnsDbSet(_paymentMock.Object);
            _mockPaymentsRepository = new PaymentsRepository(_dataContextMock.Object);

            var reference = "Test 1 Reference";

            //Act
            var result = await _mockPaymentsRepository.GetPreviousPaymentsByReferenceAsync(reference, _cancellationToken);

            //Assert
            result.Should().Be(10.0m);
        }

        [TestMethod, AutoMoqData]
        public async Task GetPreviousPaymentsByReferenceAsync_MultiplePaymentsExist_ShouldReturnPreviousAmounts(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] PaymentsRepository _mockPaymentsRepository)
        {
            //Arrange
            _dataContextMock.Setup(i => i.Payment).ReturnsDbSet(_paymentMock.Object);
            _mockPaymentsRepository = new PaymentsRepository(_dataContextMock.Object);

            var reference = "Test Reference";

            //Act
            var result = await _mockPaymentsRepository.GetPreviousPaymentsByReferenceAsync(reference, _cancellationToken);

            //Assert
            result.Should().Be(50.0m);
        }

        [TestMethod, AutoMoqData]
        public async Task GetPreviousPaymentsByReferenceAsync_PaymentDoesNotExist_ShouldReturnZeroAmount(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] PaymentsRepository _mockPaymentsRepository)
        {
            //Arrange
            _dataContextMock.Setup(i => i.Payment).ReturnsDbSet(_paymentMock.Object);
            _mockPaymentsRepository = new PaymentsRepository(_dataContextMock.Object);

            var reference = "Reference";

            //Act
            var result = await _mockPaymentsRepository.GetPreviousPaymentsByReferenceAsync(reference, _cancellationToken);

            //Assert
            result.Should().Be(0.0m);
        }

        [TestMethod, AutoMoqData]
        public async Task GetPreviousPaymentsByReferenceAsync_PaymentCanBeNegative_ShouldReturnAmount(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] PaymentsRepository _mockPaymentsRepository)
        {
            //Arrange
            _dataContextMock.Setup(i => i.Payment).ReturnsDbSet(_paymentMock.Object);
            _mockPaymentsRepository = new PaymentsRepository(_dataContextMock.Object);

            var reference = "Test Reference For Negative";

            //Act
            var result = await _mockPaymentsRepository.GetPreviousPaymentsByReferenceAsync(reference, _cancellationToken);

            //Assert
            result.Should().Be(140.0m);
        }


        [TestMethod, AutoMoqData]
        public async Task GetPreviousPaymentIncludeChildrenByReferenceAsync_SinglePaymentExist_ShouldReturnPreviousAmounts_ShouldHaveOnlinePayment(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] PaymentsRepository _mockPaymentsRepository)
        {
            //Arrange
            _dataContextMock.Setup(i => i.Payment).ReturnsDbSet(_paymentMock.Object);
            _mockPaymentsRepository = new PaymentsRepository(_dataContextMock.Object);

            var reference = "Test Reference With Online Payment";

            //Act
            var result = await _mockPaymentsRepository.GetPreviousPaymentIncludeChildrenByReferenceAsync(reference, _cancellationToken);
            
            // Assert
            using (new AssertionScope())
            {   
                result?.Amount.Should().Be(20.0m);
                result?.OnlinePayment.Should().NotBeNull();
                result?.OfflinePayment.Should().BeNull();
            }
        }

        [TestMethod, AutoMoqData]
        public async Task GetPreviousPaymentIncludeChildrenByReferenceAsync_SinglePaymentExist_ShouldReturnPreviousAmounts_ShouldHaveOfflinePayment(
           [Frozen] Mock<IAppDbContext> _dataContextMock,
           [Greedy] PaymentsRepository _mockPaymentsRepository)
        {
            //Arrange
            _dataContextMock.Setup(i => i.Payment).ReturnsDbSet(_paymentMock.Object);
            _mockPaymentsRepository = new PaymentsRepository(_dataContextMock.Object);

            var reference = "Test Reference With Offline Payment";

            //Act
            var result = await _mockPaymentsRepository.GetPreviousPaymentIncludeChildrenByReferenceAsync(reference, _cancellationToken);

            // Assert
            using (new AssertionScope())
            {
                result?.Amount.Should().Be(20.0m);
                result?.OfflinePayment.Should().NotBeNull();
                result?.OnlinePayment.Should().BeNull();
            }
        }

        [TestMethod, AutoMoqData]
        public async Task GetPreviousPaymentIncludeChildrenByReferenceAsync_PaymentDoesNotExist_ShouldReturnNull(
            [Frozen] Mock<IAppDbContext> _dataContextMock,
            [Greedy] PaymentsRepository _mockPaymentsRepository)
        {
            //Arrange
            _dataContextMock.Setup(i => i.Payment).ReturnsDbSet(_paymentMock.Object);
            _mockPaymentsRepository = new PaymentsRepository(_dataContextMock.Object);

            var reference = "Reference";

            //Act
            var result = await _mockPaymentsRepository.GetPreviousPaymentIncludeChildrenByReferenceAsync(reference, _cancellationToken);

            //Assert            
            Assert.IsNull(result);            
        }        
    }
}
