using EPR.Payment.Service.Common.Data.Enums;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using Moq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace EPR.Payment.Service.Common.UnitTests.Mocks
{
    public class MockIRegistrationFeesRepository
    {
        public static Mock<DbSet<Data.DataModels.Lookups.RegistrationFees>> GetRegistrationFeesMock()
        {
            var registrationFeesMock = new Mock<DbSet<Data.DataModels.Lookups.RegistrationFees>>();

            var registrationFeesMockData = new List<Data.DataModels.Lookups.RegistrationFees>()
            {
                new Data.DataModels.Lookups.RegistrationFees()
                {
                    Id = 1,
                    GroupId = (int)Group.ProducerResubmission,
                    SubGroupId = (int)SubGroup.ReSubmitting,
                    RegulatorId = 1,
                    Amount = 100,
                    EffectiveFrom = DateTime.Now,
                    EffectiveTo = DateTime.Now,
                    Regulator = new Data.DataModels.Lookups.Regulator { Id = 1, Type = "Test-Regulator-1", Description = "Test Regulator 1" }
                },
               new Data.DataModels.Lookups.RegistrationFees()
                {
                    Id = 2,
                    GroupId = (int)Group.ProducerType,
                    SubGroupId = (int)SubGroup.Small,
                    RegulatorId = 2,
                    Amount = 100,
                    EffectiveFrom = DateTime.Now,
                    EffectiveTo = DateTime.Now,
                    Regulator = new Data.DataModels.Lookups.Regulator { Id = 2, Type = "Test-Regulator-2", Description = "Test Regulator 2" }
                }
            }.AsQueryable();

            registrationFeesMock.As<IDbAsyncEnumerable<Data.DataModels.Lookups.RegistrationFees>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestHelperDbAsyncEnumerator<Data.DataModels.Lookups.RegistrationFees>(registrationFeesMockData.GetEnumerator()));

            registrationFeesMock.As<IQueryable<Data.DataModels.Lookups.RegistrationFees>>()
                .Setup(m => m.Provider)
                .Returns(new TestHelperDbAsyncQueryProvider<Data.DataModels.Lookups.RegistrationFees>(registrationFeesMockData.Provider));

            registrationFeesMock.As<IQueryable<Data.DataModels.Lookups.RegistrationFees>>().Setup(m => m.Expression).Returns(registrationFeesMockData.Expression);
            registrationFeesMock.As<IQueryable<Data.DataModels.Lookups.RegistrationFees>>().Setup(m => m.ElementType).Returns(registrationFeesMockData.ElementType);
            registrationFeesMock.As<IQueryable<Data.DataModels.Lookups.RegistrationFees>>().Setup(m => m.GetEnumerator()).Returns(() => registrationFeesMockData.GetEnumerator());

            // Setup the mock
            return registrationFeesMock;
        }
    }
}
