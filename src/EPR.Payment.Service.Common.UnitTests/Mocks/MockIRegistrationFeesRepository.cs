using EPR.Payment.Service.Common.Constants.RegistrationFees.LookUps;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using Moq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace EPR.Payment.Service.Common.UnitTests.Mocks
{
    public class MockIRegistrationFeesRepository
    {
        public static Mock<DbSet<Common.Data.DataModels.Lookups.RegistrationFees>> GetRegistrationFeesMock()
        {
            var registrationFeesMock = new Mock<DbSet<Common.Data.DataModels.Lookups.RegistrationFees>>();

            var registrationFeesData = new List<Common.Data.DataModels.Lookups.RegistrationFees>
        {
            // Large Producer - Base Fee (Currently Active)
            new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.ProducerType, Description = "Producer Type" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Type="Large", Description = "Large producer" },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Type="GB-ENG", Description = "England" },
                Amount = 262000m, // £2,620 represented in pence (262000 pence)
                EffectiveFrom = DateTime.UtcNow.AddDays(-10), // Effective 10 days ago
                EffectiveTo = DateTime.UtcNow.AddDays(10) // Expires in 10 days
            },
            // Large Producer - Base Fee (Expired)
            new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.ProducerType, Description = "Producer Type" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Type="Large", Description = "Large producer" },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Type="GB-ENG", Description = "England" },
                Amount = 240000m, // £2,400 represented in pence (240000 pence)
                EffectiveFrom = DateTime.UtcNow.AddDays(-30), // Effective 30 days ago
                EffectiveTo = DateTime.UtcNow.AddDays(-15) // Expired 15 days ago
            },
            // Small Producer - Base Fee (Currently Active)
            new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.ProducerType, Description = "Producer Type" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Type= "Small", Description = "Small producer" },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Type="GB-ENG", Description = "England" },
                Amount = 121600m, // £1,216 represented in pence (121600 pence)
                EffectiveFrom = DateTime.UtcNow.AddDays(-10), // Effective 10 days ago
                EffectiveTo = DateTime.UtcNow.AddDays(10) // Expires in 10 days
            },
            // First 20 Subsidiaries - Fee (Currently Active)
            new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.ProducerSubsidiaries, Description = "Producer Subsidiaries" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Type=SubsidiariesConstants.UpTo20, Description = "Up to 20" },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Type="GB-ENG", Description = "England" },
                Amount = 55800m, // £558 represented in pence per subsidiary (55800 pence)
                EffectiveFrom = DateTime.UtcNow.AddDays(-10), // Effective 10 days ago
                EffectiveTo = DateTime.UtcNow.AddDays(10) // Expires in 10 days
            },
            // Additional Subsidiaries - Fee (Currently Active)
            new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.ProducerSubsidiaries, Description = "Producer Subsidiaries" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Type = SubsidiariesConstants.MoreThan20, Description = "More than 20" },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Type="GB-ENG", Description = "England" },
                Amount = 14000m, // £140 represented in pence per additional subsidiary (14000 pence)
                EffectiveFrom = DateTime.UtcNow.AddDays(-10), // Effective 10 days ago
                EffectiveTo = DateTime.UtcNow.AddDays(10) // Expires in 10 days
            },
            // Large Producer - Base Fee (Future Effective)
            new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.ProducerType, Description = "Producer Type" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Type="Large", Description = "Large producer" },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Type="GB-ENG", Description = "England" },
                Amount = 300000m, // £3,000 represented in pence (300000 pence)
                EffectiveFrom = DateTime.UtcNow.AddDays(5), // Not effective yet, future record
                EffectiveTo = DateTime.UtcNow.AddDays(20) // Future expiration
            },
            // Online Market Producer
            new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.ProducerType, Description = "Producer Type" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Type=SubGroupTypeConstants.OnlineMarket, Description = "Online Market" },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Type="GB-ENG", Description = "England" },
                Amount = 257900m, // £2,579 represented in pence (257900 pence)
                EffectiveFrom = DateTime.UtcNow.AddDays(-10), // Effective 10 days ago
                EffectiveTo = DateTime.UtcNow.AddDays(10) // Expires in 10 days
            },
            new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.ProducerResubmission, Description = "Producer re-submitting a report" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Type="ReSubmitting", Description = "Re-submitting a report" },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Type="GB-ENG", Description = "England" },
                Amount = 100m, 
                EffectiveFrom = DateTime.UtcNow.AddDays(-20), // Not effective yet, future record
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
