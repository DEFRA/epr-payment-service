using EPR.Payment.Service.Common.Constants.RegistrationFees.LookUps;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using Moq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace EPR.Payment.Service.Common.UnitTests.Mocks
{
    public static class MockIAccreditationFeeRepository
    {
        public static Mock<DbSet<Common.Data.DataModels.Lookups.AccreditationFee>> GetAccreditationFeesMock()
        {
            var accreditationFeesMock = new Mock<DbSet<Common.Data.DataModels.Lookups.AccreditationFee>>();

            var accreditationFeesData = new List<Common.Data.DataModels.Lookups.AccreditationFee>
        {   
            new() {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.Exporters, Description = "Exporter" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Type="Aluminium", Description = "Aluminium" },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Type="GB-ENG", Description = "England" },
                TonnesOver = 0,
                TonnesUpTo = 500,
                Amount = 50000m, // £500 represented in pence (50000 pence)
                EffectiveFrom = DateTime.UtcNow.AddDays(-10), // Effective 10 days ago
                EffectiveTo = DateTime.UtcNow.AddDays(10) // Expires in 10 days
            },            
             new() {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.Reprocessors, Description = "Reprocessors" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Type="Aluminium", Description = "Aluminium" },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Type="GB-ENG", Description = "England" },
                TonnesOver = 0,
                TonnesUpTo = 500,
                Amount = 50000m, // £500 represented in pence (50000 pence)
                EffectiveFrom = DateTime.UtcNow.AddDays(-10), // Effective 10 days ago
                EffectiveTo = DateTime.UtcNow.AddDays(10) // Expires in 10 days
            },


        }.AsQueryable();

            accreditationFeesMock.As<IDbAsyncEnumerable<Common.Data.DataModels.Lookups.AccreditationFee>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestHelperDbAsyncEnumerator<Common.Data.DataModels.Lookups.AccreditationFee>(accreditationFeesData.GetEnumerator()));

            accreditationFeesMock.As<IQueryable<Common.Data.DataModels.Lookups.AccreditationFee>>()
                .Setup(m => m.Provider)
                .Returns(new TestHelperDbAsyncQueryProvider<Common.Data.DataModels.Lookups.AccreditationFee>(accreditationFeesData.Provider));

            accreditationFeesMock.As<IQueryable<Common.Data.DataModels.Lookups.AccreditationFee>>().Setup(m => m.Expression).Returns(accreditationFeesData.Expression);
            accreditationFeesMock.As<IQueryable<Common.Data.DataModels.Lookups.AccreditationFee>>().Setup(m => m.ElementType).Returns(accreditationFeesData.ElementType);
            accreditationFeesMock.As<IQueryable<Common.Data.DataModels.Lookups.AccreditationFee>>().Setup(m => m.GetEnumerator()).Returns(() => accreditationFeesData.GetEnumerator());

            return accreditationFeesMock;
        }

        public static Mock<DbSet<Common.Data.DataModels.Lookups.AccreditationFee>> GetEmptyAccreditationFeesMock()
        {
            var accreditationFeesMock = new Mock<DbSet<Common.Data.DataModels.Lookups.AccreditationFee>>();

            var emptyData = new List<Common.Data.DataModels.Lookups.AccreditationFee>().AsQueryable();

            accreditationFeesMock.As<IDbAsyncEnumerable<Common.Data.DataModels.Lookups.AccreditationFee>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestHelperDbAsyncEnumerator<Common.Data.DataModels.Lookups.AccreditationFee>(emptyData.GetEnumerator()));

            accreditationFeesMock.As<IQueryable<Common.Data.DataModels.Lookups.AccreditationFee>>()
                .Setup(m => m.Provider)
                .Returns(new TestHelperDbAsyncQueryProvider<Common.Data.DataModels.Lookups.AccreditationFee>(emptyData.Provider));

            accreditationFeesMock.As<IQueryable<Common.Data.DataModels.Lookups.AccreditationFee>>().Setup(m => m.Expression).Returns(emptyData.Expression);
            accreditationFeesMock.As<IQueryable<Common.Data.DataModels.Lookups.AccreditationFee>>().Setup(m => m.ElementType).Returns(emptyData.ElementType);
            accreditationFeesMock.As<IQueryable<Common.Data.DataModels.Lookups.AccreditationFee>>().Setup(m => m.GetEnumerator()).Returns(() => emptyData.GetEnumerator());

            return accreditationFeesMock;
        }
    }
}
