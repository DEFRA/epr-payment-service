using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using Moq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace EPR.Payment.Service.Common.UnitTests.Mocks
{
    public static class MockIOfflinePaymentRepository
    {
        public static Mock<DbSet<Data.DataModels.OfflinePayment>> GetPaymentMock()
        {
            var paymentMock = new Mock<DbSet<Data.DataModels.OfflinePayment>>();

            var paymentMockData = new List<Data.DataModels.OfflinePayment>()
            {
                new Data.DataModels.OfflinePayment()
                {
                    Id = 1,
                    ExternalPaymentId = Guid.Parse("d0f74b07-42e1-43a7-ae9d-0e279f213278"),
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
               new Data.DataModels.OfflinePayment()
                {
                    Id = 2,
                    ExternalPaymentId = Guid.Parse("dab3d8e1-409b-4b40-a610-1b41843e4710"),
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

            paymentMock.As<IDbAsyncEnumerable<Data.DataModels.OfflinePayment>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestHelperDbAsyncEnumerator<Data.DataModels.OfflinePayment>(paymentMockData.GetEnumerator()));

            paymentMock.As<IQueryable<Data.DataModels.OfflinePayment>>()
                .Setup(m => m.Provider)
                .Returns(new TestHelperDbAsyncQueryProvider<Data.DataModels.OfflinePayment>(paymentMockData.Provider));

            paymentMock.As<IQueryable<Data.DataModels.OfflinePayment>>().Setup(m => m.Expression).Returns(paymentMockData.Expression);
            paymentMock.As<IQueryable<Data.DataModels.OfflinePayment>>().Setup(m => m.ElementType).Returns(paymentMockData.ElementType);
            paymentMock.As<IQueryable<Data.DataModels.OfflinePayment>>().Setup(m => m.GetEnumerator()).Returns(() => paymentMockData.GetEnumerator());

            return paymentMock;
        }
    }
}
