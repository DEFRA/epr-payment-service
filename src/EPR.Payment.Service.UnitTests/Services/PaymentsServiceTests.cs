using AutoMapper;
using EPR.Payment.Service.Common.Data.Interfaces.Repositories;
using EPR.Payment.Service.Common.Dtos.Request;
using EPR.Payment.Service.Services;
using FluentAssertions;
using Moq;

namespace EPR.Payment.Service.UnitTests.Services
{
    [TestClass]
    public class PaymentsServiceTests
    {
        private readonly Mock<IPaymentsRepository> _paymentsRepositoryMock = new Mock<IPaymentsRepository>();
        private readonly Mock<IMapper> mapperMock = new Mock<IMapper>();

        [TestMethod]
        public async Task InsertPaymentStatus_WhenInitialisedWithNullPaymentId_ThrowsArgumentNullException()
        {

            //Act
            var service = new PaymentsService(mapperMock.Object, _paymentsRepositoryMock.Object);
            var status = new PaymentStatusInsertRequestDto { Status = "Inserted" };

            //Assert
            await service.Invoking(async x => await x.InsertPaymentStatusAsync(null, status))
                .Should().ThrowAsync<ArgumentException>();
        }

        [TestMethod]
        public async Task InsertPaymentStatus_WhenInitialisedWithNullStatus_ThrowsArgumentNullException()
        {

            //Act
            var service = new PaymentsService(mapperMock.Object, _paymentsRepositoryMock.Object);
            var status = new PaymentStatusInsertRequestDto { Status = null };

            //Assert
            await service.Invoking(async x => await x.InsertPaymentStatusAsync("123", status))
                .Should().ThrowAsync<ArgumentException>();
        }

        [TestMethod]
        public async Task InsertPaymentStatus_Success()
        {
            // Arrange
            var service = new PaymentsService(mapperMock.Object, _paymentsRepositoryMock.Object);
            var status = new PaymentStatusInsertRequestDto { Status = "Inserted" };
            var paymentId = "123";

            // Act & Assert
            Func<Task> action = async () => await service.InsertPaymentStatusAsync(paymentId, status);

            // Assert
            await action.Should().NotThrowAsync();
        }

    }
}
