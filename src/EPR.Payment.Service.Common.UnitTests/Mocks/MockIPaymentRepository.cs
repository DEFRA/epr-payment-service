using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using Moq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace EPR.Payment.Service.Common.UnitTests.Mocks
{
    public static class MockIPaymentRepository
    {
        public static Mock<DbSet<Data.DataModels.Payment>> GetPaymentMock()
        {
            var paymentMock = new Mock<DbSet<Data.DataModels.Payment>>();

            var paymentMockData = new List<Data.DataModels.Payment>()
            {
                new Data.DataModels.Payment()
                {
                    Id = 1,
                    ExternalPaymentId = Guid.Parse("d0f74b07-42e1-43a7-ae9d-0e279f213278"),
                    UserId = Guid.NewGuid(),
                    Regulator = "Test 1 Regulator",
                    Reference = "Test 1 Reference",
                    InternalStatusId = Data.Enums.Status.Success,
                    Amount = 10.0M,
                    ReasonForPayment = "Test 1",
                    CreatedDate = new DateTime(),
                    UpdatedByUserId = Guid.NewGuid(),
                    UpdatedDate = new DateTime(),
                    OnlinePayment = new Data.DataModels.OnlinePayment()
                    {
                        Id = 1,
                        PaymentId = 1,
                        UpdatedByOrgId = Guid.NewGuid(),
                        OrganisationId = Guid.NewGuid()
                    }
                },
               new Data.DataModels.Payment()
                {
                    Id = 2,
                    ExternalPaymentId = Guid.Parse("dab3d8e1-409b-4b40-a610-1b41843e4710"),
                    UserId = Guid.NewGuid(),
                    Regulator = "Test 2 Regulator",
                    Reference = "Test 2 Reference",
                    InternalStatusId = Data.Enums.Status.Success,
                    Amount = 10.0M,
                    ReasonForPayment = "Test 2",
                    CreatedDate = new DateTime(),
                    UpdatedByUserId = Guid.NewGuid(),
                    UpdatedDate = new DateTime(),
                    OfflinePayment = new Data.DataModels.OfflinePayment()
                    {
                        Id = 1,
                        PaymentId = 2,
                        PaymentDate = new DateTime()
                    }
                },
               new Data.DataModels.Payment()
                {
                    Id = 3,
                    ExternalPaymentId = Guid.Parse("5b44fe7c-f63d-40ae-a28a-97c4680e082c"),
                    UserId = Guid.NewGuid(),
                    Regulator = "Test Regulator",
                    Reference = "Test Reference",
                    InternalStatusId = Data.Enums.Status.Success,
                    Amount = 30.0M,
                    ReasonForPayment = "Test",
                    CreatedDate = new DateTime(),
                    UpdatedByUserId = Guid.NewGuid(),
                    UpdatedDate = new DateTime(),
                    OfflinePayment = new Data.DataModels.OfflinePayment()
                    {
                        Id = 2,
                        PaymentId = 3,
                        PaymentDate = new DateTime()
                    }
                },
               new Data.DataModels.Payment()
                {
                    Id = 4,
                    ExternalPaymentId = Guid.Parse("5c81a6b3-3669-413a-a961-f072e18c6025"),
                    UserId = Guid.NewGuid(),
                    Regulator = "Test Regulator",
                    Reference = "Test Reference",
                    InternalStatusId = Data.Enums.Status.Success,
                    Amount = 20.0M,
                    ReasonForPayment = "Test",
                    CreatedDate = new DateTime(),
                    UpdatedByUserId = Guid.NewGuid(),
                    UpdatedDate = new DateTime(),
                    OfflinePayment = new Data.DataModels.OfflinePayment()
                    {
                        Id = 3,
                        PaymentId = 4,
                        PaymentDate = new DateTime()
                    }
                },
               //Next three records are added for negative payments. A permitted scenario
               new Data.DataModels.Payment()
                {
                    Id = 5,
                    ExternalPaymentId = Guid.Parse("482963c7-57da-4758-b630-f25737c18793"),
                    UserId = Guid.NewGuid(),
                    Regulator = "Test Regulator For Negative",
                    Reference = "Test Reference For Negative",
                    InternalStatusId = Data.Enums.Status.Success,
                    Amount = -10.0M,
                    ReasonForPayment = "Test 1",
                    CreatedDate = new DateTime(),
                    UpdatedByUserId = Guid.NewGuid(),
                    UpdatedDate = new DateTime(),
                    OnlinePayment = new Data.DataModels.OnlinePayment()
                    {
                        Id = 2,
                        PaymentId = 5,
                        UpdatedByOrgId = Guid.NewGuid(),
                        OrganisationId = Guid.NewGuid()
                    }
                },
                new Data.DataModels.Payment()
                {
                    Id = 6,
                    ExternalPaymentId = Guid.Parse("47bb50c4-c2cd-4791-95e8-f31159411eb4"),
                    UserId = Guid.NewGuid(),
                    Regulator = "Test Regulator For Negative",
                    Reference = "Test Reference For Negative",
                    InternalStatusId = Data.Enums.Status.Success,
                    Amount = 50.0M,
                    ReasonForPayment = "Test 2",
                    CreatedDate = new DateTime(),
                    UpdatedByUserId = Guid.NewGuid(),
                    UpdatedDate = new DateTime(),
                    OfflinePayment = new Data.DataModels.OfflinePayment()
                    {
                        Id = 4,
                        PaymentId = 6,
                        PaymentDate = new DateTime()
                    }
                },
                new Data.DataModels.Payment()
                {
                    Id = 7,
                    ExternalPaymentId = Guid.Parse("5a8fad6e-5570-48b3-83b9-174195b0f0b0"),
                    UserId = Guid.NewGuid(),
                    Regulator = "Test Regulator For Negative",
                    Reference = "Test Reference For Negative",
                    InternalStatusId = Data.Enums.Status.Success,
                    Amount = 100.0M,
                    ReasonForPayment = "Test 2",
                    CreatedDate = new DateTime(),
                    UpdatedByUserId = Guid.NewGuid(),
                    UpdatedDate = new DateTime(),
                    OfflinePayment = new Data.DataModels.OfflinePayment()
                    {
                        Id = 5,
                        PaymentId = 7,
                        PaymentDate = new DateTime()
                    }
                },
               new Data.DataModels.Payment()
                {
                    Id = 3,
                    ExternalPaymentId = Guid.Parse("dab3d8e1-409b-4b40-a610-1b41843e4710"),
                    UserId = Guid.NewGuid(),
                    Regulator = "Test Regulator",
                    Reference = "Test Reference",
                    InternalStatusId = Data.Enums.Status.Initiated,
                    Amount = 30.0M,
                    ReasonForPayment = "Test",
                    CreatedDate = new DateTime(),
                    UpdatedByUserId = Guid.NewGuid(),
                    UpdatedDate = new DateTime(),
                    OfflinePayment = new Data.DataModels.OfflinePayment()
                    {
                        Id = 2,
                        PaymentId = 3,
                        PaymentDate = new DateTime()
                    }
                },
               new Data.DataModels.Payment()
                {
                    Id = 4,
                    ExternalPaymentId = Guid.Parse("dab3d8e1-409b-4b40-a610-1b41843e4710"),
                    UserId = Guid.NewGuid(),
                    Regulator = "Test Regulator",
                    Reference = "Test Reference",
                    InternalStatusId = Data.Enums.Status.Initiated,
                    Amount = 20.0M,
                    ReasonForPayment = "Test",
                    CreatedDate = new DateTime(),
                    UpdatedByUserId = Guid.NewGuid(),
                    UpdatedDate = new DateTime(),
                    OfflinePayment = new Data.DataModels.OfflinePayment()
                    {
                        Id = 3,
                        PaymentId = 4,
                        PaymentDate = new DateTime()
                    }
                },
                new Data.DataModels.Payment()
                {
                    Id = 5,
                    ExternalPaymentId = Guid.Parse("dab3d8e1-409b-4b40-a610-1b41843e4712"),
                    UserId = Guid.NewGuid(),
                    Regulator = "Test Regulator",
                    Reference = "Test Reference With Offline Payment",
                    InternalStatusId = Data.Enums.Status.Success,
                    Amount = 20.0M,
                    ReasonForPayment = "Test",
                    CreatedDate = new DateTime(),
                    UpdatedByUserId = Guid.NewGuid(),
                    UpdatedDate = new DateTime(),
                    OfflinePayment = new Data.DataModels.OfflinePayment()
                    {
                        Id = 3,
                        PaymentId = 4,
                        PaymentDate = new DateTime()
                    }
                },
                new Data.DataModels.Payment()
                {
                    Id = 6,
                    ExternalPaymentId = Guid.Parse("dab3d8e1-409b-4b40-a610-1b41843e4713"),
                    UserId = Guid.NewGuid(),
                    Regulator = "Test Regulator",
                    Reference = "Test Reference With Online Payment",
                    InternalStatusId = Data.Enums.Status.Success,
                    Amount = 20.0M,
                    ReasonForPayment = "Test",
                    CreatedDate = new DateTime(),
                    UpdatedByUserId = Guid.NewGuid(),
                    UpdatedDate = new DateTime(),
                    OnlinePayment = new Data.DataModels.OnlinePayment()
                    {
                        Id = 2,
                        PaymentId = 5,
                        UpdatedByOrgId = Guid.NewGuid(),
                        OrganisationId = Guid.NewGuid()
                    }
                }
            }.AsQueryable();

            paymentMock.As<IDbAsyncEnumerable<Data.DataModels.Payment>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestHelperDbAsyncEnumerator<Data.DataModels.Payment>(paymentMockData.GetEnumerator()));

            paymentMock.As<IQueryable<Data.DataModels.Payment>>()
                .Setup(m => m.Provider)
                .Returns(new TestHelperDbAsyncQueryProvider<Data.DataModels.Payment>(paymentMockData.Provider));

            paymentMock.As<IQueryable<Data.DataModels.Payment>>().Setup(m => m.Expression).Returns(paymentMockData.Expression);
            paymentMock.As<IQueryable<Data.DataModels.Payment>>().Setup(m => m.ElementType).Returns(paymentMockData.ElementType);
            paymentMock.As<IQueryable<Data.DataModels.Payment>>().Setup(m => m.GetEnumerator()).Returns(() => paymentMockData.GetEnumerator());

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
