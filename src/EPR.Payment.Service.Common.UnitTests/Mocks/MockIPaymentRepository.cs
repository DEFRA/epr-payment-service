using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using Moq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace EPR.Payment.Service.Common.UnitTests.Mocks
{
    public static class MockIPaymentRepository
    {
        public static Mock<DbSet<Data.DataModels.OnlinePayment>> GetPaymentMock()
        {
            var paymentMock = new Mock<DbSet<Data.DataModels.OnlinePayment>>();

            var paymentMockData = new List<Data.DataModels.OnlinePayment>()
            {
                new Data.DataModels.OnlinePayment()
                {
                    Id = 1,
                    ExternalPaymentId = Guid.Parse("d0f74b07-42e1-43a7-ae9d-0e279f213278"),
                    OrganisationId = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    Regulator = "Test 1 Regulator",
                    Reference = "Test 1 Reference",
                    InternalStatusId = Data.Enums.Status.Initiated,
                    Amount = 10.0M,
                    ReasonForPayment = "Test 1",
                    CreatedDate = new DateTime(),
                    UpdatedByOrganisationId = Guid.NewGuid(),
                    UpdatedByUserId = Guid.NewGuid(),
                    UpdatedDate = new DateTime()
                },
               new Data.DataModels.OnlinePayment()
                {
                    Id = 2,
                    ExternalPaymentId = Guid.Parse("dab3d8e1-409b-4b40-a610-1b41843e4710"),
                    OrganisationId = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    Regulator = "Test 2 Regulator",
                    Reference = "Test 2 Reference",
                    InternalStatusId = Data.Enums.Status.Initiated,
                    Amount = 10.0M,
                    ReasonForPayment = "Test 2",
                    CreatedDate = new DateTime(),
                    UpdatedByOrganisationId = Guid.NewGuid(),
                    UpdatedByUserId = Guid.NewGuid(),
                    UpdatedDate = new DateTime()
                }
            }.AsQueryable();

            paymentMock.As<IDbAsyncEnumerable<Data.DataModels.OnlinePayment>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestHelperDbAsyncEnumerator<Data.DataModels.OnlinePayment>(paymentMockData.GetEnumerator()));

            paymentMock.As<IQueryable<Data.DataModels.OnlinePayment>>()
                .Setup(m => m.Provider)
                .Returns(new TestHelperDbAsyncQueryProvider<Data.DataModels.OnlinePayment>(paymentMockData.Provider));

            paymentMock.As<IQueryable<Data.DataModels.OnlinePayment>>().Setup(m => m.Expression).Returns(paymentMockData.Expression);
            paymentMock.As<IQueryable<Data.DataModels.OnlinePayment>>().Setup(m => m.ElementType).Returns(paymentMockData.ElementType);
            paymentMock.As<IQueryable<Data.DataModels.OnlinePayment>>().Setup(m => m.GetEnumerator()).Returns(() => paymentMockData.GetEnumerator());

            return paymentMock;
        }

        public static Mock<DbSet<Data.DataModels.Lookups.PaymentStatus>> GetPaymentStatusMock(bool returnResults)
        {
            var paymentStatusMock = new Mock<DbSet<Data.DataModels.Lookups.PaymentStatus>>();

            var paymentStatusMockData = new List<Data.DataModels.Lookups.PaymentStatus>()
            {
                new Data.DataModels.Lookups.PaymentStatus()
                {
                    Id = Data.Enums.Status.Initiated,
                    Status = "Initiated"
                },
               new Data.DataModels.Lookups.PaymentStatus()
                {
                    Id = Data.Enums.Status.InProgress,
                    Status = "InProgress"
                },
               new Data.DataModels.Lookups.PaymentStatus()
                {
                    Id = Data.Enums.Status.Success,
                    Status = "Success"
                }
            }.AsQueryable();

            if (!returnResults)
            {
                paymentStatusMockData = new List<Data.DataModels.Lookups.PaymentStatus>().AsQueryable();
            }

            paymentStatusMock.As<IDbAsyncEnumerable<Data.DataModels.Lookups.PaymentStatus>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestHelperDbAsyncEnumerator<Data.DataModels.Lookups.PaymentStatus>(paymentStatusMockData.GetEnumerator()));

            paymentStatusMock.As<IQueryable<Data.DataModels.Lookups.PaymentStatus>>()
                .Setup(m => m.Provider)
                .Returns(new TestHelperDbAsyncQueryProvider<Data.DataModels.Lookups.PaymentStatus>(paymentStatusMockData.Provider));

            paymentStatusMock.As<IQueryable<Data.DataModels.Lookups.PaymentStatus>>().Setup(m => m.Expression).Returns(paymentStatusMockData.Expression);
            paymentStatusMock.As<IQueryable<Data.DataModels.Lookups.PaymentStatus>>().Setup(m => m.ElementType).Returns(paymentStatusMockData.ElementType);
            paymentStatusMock.As<IQueryable<Data.DataModels.Lookups.PaymentStatus>>().Setup(m => m.GetEnumerator()).Returns(() => paymentStatusMockData.GetEnumerator());

            return paymentStatusMock;
        }
    }
}
