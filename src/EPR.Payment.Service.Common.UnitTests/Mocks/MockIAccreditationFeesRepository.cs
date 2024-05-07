using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Data.Entity.Infrastructure;
using System.Net.Sockets;
using System.Reflection.Metadata;

namespace EPR.Payment.Service.Common.UnitTests.Mocks
{
    public class MockIAccreditationFeesRepository
    {
        public static Mock<DbSet<AccreditationFees>> GetMock()
        {
            var accreditationFeesMock = new Mock<DbSet<AccreditationFees>>();

            var accreditationFeesData = new List<AccreditationFees>()
            {
                new AccreditationFees()
                {
                    Id = 1,
                    Amount = 10.0M, 
                    Large = true,
                    Regulator = "GB-ENG", 
                    EffectiveFrom = new DateTime(2024, 3, 31),
                    EffectiveTo = null
                }
            }.AsQueryable();

            accreditationFeesMock.As<IDbAsyncEnumerable<AccreditationFees>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestHelperDbAsyncEnumerator<AccreditationFees>(accreditationFeesData.GetEnumerator()));

            accreditationFeesMock.As<IQueryable<AccreditationFees>>()
                .Setup(m => m.Provider)
                .Returns(new TestHelperDbAsyncQueryProvider<AccreditationFees>(accreditationFeesData.Provider));

            accreditationFeesMock.As<IQueryable<AccreditationFees>>().Setup(m => m.Expression).Returns(accreditationFeesData.Expression);
            accreditationFeesMock.As<IQueryable<AccreditationFees>>().Setup(m => m.ElementType).Returns(accreditationFeesData.ElementType);
            accreditationFeesMock.As<IQueryable<AccreditationFees>>().Setup(m => m.GetEnumerator()).Returns(() => accreditationFeesData.GetEnumerator());

            //accreditationFeesMock.As<IQueryable<AccreditationFees>>().Setup(m => m.Provider).Returns(accreditationFeesData.Provider);
            //accreditationFeesMock.As<IQueryable<AccreditationFees>>().Setup(m => m.Expression).Returns(accreditationFeesData.Expression);
            //accreditationFeesMock.As<IQueryable<AccreditationFees>>().Setup(m => m.ElementType).Returns(accreditationFeesData.ElementType);
            //accreditationFeesMock.As<IQueryable<AccreditationFees>>().Setup(m => m.GetEnumerator()).Returns(() => accreditationFeesData.GetEnumerator());

            //mock.Setup(m => m.GetAllFeesAsync()).Returns(() => accreditationFeesResponse);
            //mock.Setup(m => m.GetFeesAmountAsync(It.IsAny<bool>(), It.IsAny<string>())).Returns(() => accreditationFeesResponse[0].Amount);
            //mock.Setup(m => m.GetFeesAsync(It.IsAny<bool>(), It.IsAny<string>())).Returns(() => accreditationFeesResponse[0]);
            //mock.Setup(m => m.GetFeesCount()).Returns(() => accreditationFeesResponse.Count);

            // Setup the mock
            return accreditationFeesMock;
        }
    }
}
