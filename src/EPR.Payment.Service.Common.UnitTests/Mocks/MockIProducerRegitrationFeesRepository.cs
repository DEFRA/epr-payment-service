using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Data.Entity.Infrastructure;

namespace EPR.Payment.Service.Common.UnitTests.Mocks
{
    public class MockIProducerRegitrationFeesRepository
    {
        public static Mock<DbSet<ProducerRegitrationFees>> GetMock(bool returnResults)
        {
            var FeesMock = new Mock<DbSet<ProducerRegitrationFees>>();


            var FeesData = new List<ProducerRegitrationFees>()
            {
                new ProducerRegitrationFees()
                {
                    Id = 1,
                    Amount = 10.0M, 
                    ProducerType = "L",
                    Country = "GB-ENG", 
                    EffectiveFrom = new DateTime(2024, 3, 31),
                    EffectiveTo = new DateTime(2024, 12, 31),
                },
                new ProducerRegitrationFees()
                {
                    Id = 2,
                    Amount = 20.0M,
                    ProducerType = "S",
                    Country = "GB-SCT",
                    EffectiveFrom = new DateTime(2024, 3, 31),
                    EffectiveTo = new DateTime(2024, 12, 31),
                }
            }.AsQueryable();

            if (!returnResults)
            {
                FeesData = new List<ProducerRegitrationFees>().AsQueryable();
            }

            FeesMock.As<IDbAsyncEnumerable<ProducerRegitrationFees>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestHelperDbAsyncEnumerator<ProducerRegitrationFees>(FeesData.GetEnumerator()));

            FeesMock.As<IQueryable<ProducerRegitrationFees>>()
                .Setup(m => m.Provider)
                .Returns(new TestHelperDbAsyncQueryProvider<ProducerRegitrationFees>(FeesData.Provider));

            FeesMock.As<IQueryable<ProducerRegitrationFees>>().Setup(m => m.Expression).Returns(FeesData.Expression);
            FeesMock.As<IQueryable<ProducerRegitrationFees>>().Setup(m => m.ElementType).Returns(FeesData.ElementType);
            FeesMock.As<IQueryable<ProducerRegitrationFees>>().Setup(m => m.GetEnumerator()).Returns(() => FeesData.GetEnumerator());

            // Setup the mock
            return FeesMock;
        }
    }
}
