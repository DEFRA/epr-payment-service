using EPR.Payment.Service.Common.Constants.RegistrationFees;
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
               new Data.DataModels.Payment()
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

        public static Mock<DbSet<Common.Data.DataModels.Lookups.RegistrationFees>> GetRegistrationFeesMock()
        {
            var registrationFeesMock = new Mock<DbSet<Common.Data.DataModels.Lookups.RegistrationFees>>();

            var registrationFeesData = new List<Common.Data.DataModels.Lookups.RegistrationFees>
        {
            // Large Producer - Base Fee (Currently Active)
            new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.ProducerType, Description = "large" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Description = "large" },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Description = "GB-ENG" },
                Amount = 262000m, // £2,620 represented in pence (262000 pence)
                EffectiveFrom = DateTime.UtcNow.AddDays(-10), // Effective 10 days ago
                EffectiveTo = DateTime.UtcNow.AddDays(10) // Expires in 10 days
            },
            // Large Producer - Base Fee (Expired)
            new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.ProducerType, Description = "large" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Description = "large" },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Description = "GB-ENG" },
                Amount = 240000m, // £2,400 represented in pence (240000 pence)
                EffectiveFrom = DateTime.UtcNow.AddDays(-30), // Effective 30 days ago
                EffectiveTo = DateTime.UtcNow.AddDays(-15) // Expired 15 days ago
            },
            // Small Producer - Base Fee (Currently Active)
            new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.ProducerType, Description = "small" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Description = "small" },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Description = "GB-ENG" },
                Amount = 121600m, // £1,216 represented in pence (121600 pence)
                EffectiveFrom = DateTime.UtcNow.AddDays(-10), // Effective 10 days ago
                EffectiveTo = DateTime.UtcNow.AddDays(10) // Expires in 10 days
            },
            // First 20 Subsidiaries - Fee (Currently Active)
            new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.ProducerSubsidiaries, Description = "ProducerSubsidiaries" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Description = SubsidiariesConstants.UpTo20 },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Description = "GB-ENG" },
                Amount = 55800m, // £558 represented in pence per subsidiary (55800 pence)
                EffectiveFrom = DateTime.UtcNow.AddDays(-10), // Effective 10 days ago
                EffectiveTo = DateTime.UtcNow.AddDays(10) // Expires in 10 days
            },
            // Additional Subsidiaries - Fee (Currently Active)
            new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.ProducerSubsidiaries, Description = "ProducerSubsidiaries" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Description = SubsidiariesConstants.MoreThan20 },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Description = "GB-ENG" },
                Amount = 14000m, // £140 represented in pence per additional subsidiary (14000 pence)
                EffectiveFrom = DateTime.UtcNow.AddDays(-10), // Effective 10 days ago
                EffectiveTo = DateTime.UtcNow.AddDays(10) // Expires in 10 days
            },
            // Large Producer - Base Fee (Future Effective)
            new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.ProducerType, Description = "large" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Description = "large" },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Description = "GB-ENG" },
                Amount = 300000m, // £3,000 represented in pence (300000 pence)
                EffectiveFrom = DateTime.UtcNow.AddDays(5), // Not effective yet, future record
                EffectiveTo = DateTime.UtcNow.AddDays(20) // Future expiration
            }
        }.AsQueryable();

            registrationFeesMock.As<IDbAsyncEnumerable<Common.Data.DataModels.Lookups.RegistrationFees>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestHelperDbAsyncEnumerator<Common.Data.DataModels.Lookups.RegistrationFees>(registrationFeesData.GetEnumerator()));

            registrationFeesMock.As<IQueryable<Common.Data.DataModels.Lookups.RegistrationFees>>()
                .Setup(m => m.Provider)
                .Returns(new TestHelperDbAsyncQueryProvider<Common.Data.DataModels.Lookups.RegistrationFees>(registrationFeesData.Provider));

            registrationFeesMock.As<IQueryable<Common.Data.DataModels.Lookups.RegistrationFees>>().Setup(m => m.Expression).Returns(registrationFeesData.Expression);
            registrationFeesMock.As<IQueryable<Common.Data.DataModels.Lookups.RegistrationFees>>().Setup(m => m.ElementType).Returns(registrationFeesData.ElementType);
            registrationFeesMock.As<IQueryable<Common.Data.DataModels.Lookups.RegistrationFees>>().Setup(m => m.GetEnumerator()).Returns(() => registrationFeesData.GetEnumerator());

            return registrationFeesMock;
        }

        public static Mock<DbSet<Common.Data.DataModels.Lookups.RegistrationFees>> GetEmptyRegistrationFeesMock()
        {
            var registrationFeesMock = new Mock<DbSet<Common.Data.DataModels.Lookups.RegistrationFees>>();

            var emptyData = new List<Common.Data.DataModels.Lookups.RegistrationFees>().AsQueryable();

            registrationFeesMock.As<IDbAsyncEnumerable<Common.Data.DataModels.Lookups.RegistrationFees>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestHelperDbAsyncEnumerator<Common.Data.DataModels.Lookups.RegistrationFees>(emptyData.GetEnumerator()));

            registrationFeesMock.As<IQueryable<Common.Data.DataModels.Lookups.RegistrationFees>>()
                .Setup(m => m.Provider)
                .Returns(new TestHelperDbAsyncQueryProvider<Common.Data.DataModels.Lookups.RegistrationFees>(emptyData.Provider));

            registrationFeesMock.As<IQueryable<Common.Data.DataModels.Lookups.RegistrationFees>>().Setup(m => m.Expression).Returns(emptyData.Expression);
            registrationFeesMock.As<IQueryable<Common.Data.DataModels.Lookups.RegistrationFees>>().Setup(m => m.ElementType).Returns(emptyData.ElementType);
            registrationFeesMock.As<IQueryable<Common.Data.DataModels.Lookups.RegistrationFees>>().Setup(m => m.GetEnumerator()).Returns(() => emptyData.GetEnumerator());

            return registrationFeesMock;
        }


    }
}
