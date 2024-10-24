using EPR.Payment.Service.Common.Constants.RegistrationFees.LookUps;
using EPR.Payment.Service.Common.UnitTests.TestHelpers;
using Moq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace EPR.Payment.Service.Common.UnitTests.Mocks
{
    public static class MockIRegistrationFeesRepository
    {
        public static Mock<DbSet<Common.Data.DataModels.Lookups.RegistrationFees>> GetRegistrationFeesMock()
        {
            var registrationFeesMock = new Mock<DbSet<Common.Data.DataModels.Lookups.RegistrationFees>>();

            var registrationFeesData = new List<Common.Data.DataModels.Lookups.RegistrationFees>
        {
                // PRODUCER FEES
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
            // Additional More Than 100 Subsidiaries - Fee (Currently Active)
            new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.ProducerSubsidiaries, Description = "Producer Subsidiaries" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Type = SubsidiariesConstants.MoreThan100, Description = "More than 100" },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Type="GB-ENG", Description = "England" },
                Amount = 1, // £0.01 represented in pence per additional subsidiary (1 pence)
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
            // Late Fee
            new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.ProducerType, Description = "Producer Type" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Type = SubGroupTypeConstants.LateFee, Description = "Late Fee" },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Type = "GB-ENG", Description = "England" },
                Amount = 33200m, // £332 represented in pence (33200 pence)
                EffectiveFrom = DateTime.UtcNow.AddDays(-10), // Effective 10 days ago
                EffectiveTo = DateTime.UtcNow.AddDays(10) // Expires in 10 days
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
            },


                  // COMPLIANCE SCHEME FEES
             // Compliance Scheme - Base Fee (Currently Active)
            new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.ComplianceScheme, Description = "Compliance Scheme" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Type = ComplianceSchemeConstants.Registration, Description = "Registration" },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Type = "GB-ENG", Description = "England" },
                Amount = 1380400m, // £13,804 represented in pence (1380400 pence)
                EffectiveFrom = DateTime.UtcNow.AddDays(-10), // Effective 10 days ago
                EffectiveTo = DateTime.UtcNow.AddDays(10) // Expires in 10 days
            },
            // Compliance Scheme - Overlapping Effective Dates (Higher Amount)
            new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.ComplianceScheme, Description = "Compliance Scheme" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Type = ComplianceSchemeConstants.Registration, Description = "Registration" },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Type = "GB-ENG", Description = "England" },
                Amount = 1500000m, // £15,000 represented in pence (1500000 pence)
                EffectiveFrom = DateTime.UtcNow.AddDays(-5), // More recent effective date
                EffectiveTo = DateTime.UtcNow.AddDays(10) // Still within the valid period
            },
            // Compliance Scheme - Base Fee (Expired)
            new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.ComplianceScheme, Description = "Compliance Scheme" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Type = ComplianceSchemeConstants.Registration, Description = "Registration" },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Type = "GB-ENG", Description = "England" },
                Amount = 1280400m, // £12,804 represented in pence (1280400 pence)
                EffectiveFrom = DateTime.UtcNow.AddDays(-30), // Effective 30 days ago
                EffectiveTo = DateTime.UtcNow.AddDays(-15) // Expired 15 days ago
            },
            // Compliance Scheme - Base Fee (Future Effective)
            new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.ComplianceScheme, Description = "Compliance Scheme" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Type = ComplianceSchemeConstants.Registration, Description = "Registration" },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Type = "GB-ENG", Description = "England" },
                Amount = 1480400m, // £14,804 represented in pence (1480400 pence)
                EffectiveFrom = DateTime.UtcNow.AddDays(5), // Not effective yet, future record
                EffectiveTo = DateTime.UtcNow.AddDays(20) // Future expiration
            },
            // Large Member Type Compliance Scheme (Currently Active)
            new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.ComplianceScheme, Description = "Compliance Scheme" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Type="Large", Description = "Large producer" },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Type="GB-ENG", Description = "England" },
                Amount = 165800m, // £1,658 represented in pence (165800 pence)
                EffectiveFrom = DateTime.UtcNow.AddDays(-10), // Effective 10 days ago
                EffectiveTo = DateTime.UtcNow.AddDays(10) // Expires in 10 days
            },
            // Large Member Type Compliance Scheme (Expired)
            new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.ComplianceScheme, Description = "Compliance Scheme" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Type="Large", Description = "Large producer" },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Type="GB-ENG", Description = "England" },
                Amount = 240000m, // £2,400 represented in pence (240000 pence)
                EffectiveFrom = DateTime.UtcNow.AddDays(-30), // Effective 30 days ago
                EffectiveTo = DateTime.UtcNow.AddDays(-15) // Expired 15 days ago
            },
            // Small Member Type Compliance Scheme (Currently Active)
            new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.ComplianceScheme, Description = "Compliance Scheme" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Type= "Small", Description = "Small producer" },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Type="GB-ENG", Description = "England" },
                Amount = 63100m, // £631 represented in pence (63100 pence)
                EffectiveFrom = DateTime.UtcNow.AddDays(-10), // Effective 10 days ago
                EffectiveTo = DateTime.UtcNow.AddDays(10) // Expires in 10 days
            },
            // Online Market Compliance Scheme
            new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.ComplianceScheme, Description = "Compliance Scheme" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Type=SubGroupTypeConstants.OnlineMarket, Description = "Online Market" },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Type="GB-ENG", Description = "England" },
                Amount = 257900m, // £2,579 represented in pence (257900 pence)
                EffectiveFrom = DateTime.UtcNow.AddDays(-10), // Effective 10 days ago
                EffectiveTo = DateTime.UtcNow.AddDays(10) // Expires in 10 days
            },
            // Online Market Compliance Scheme
            new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.ComplianceScheme, Description = "Compliance Scheme" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Type=SubGroupTypeConstants.OnlineMarket, Description = "Online Market" },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Type="GB-ENG", Description = "England" },
                Amount = 257900m, // £2,579 represented in pence (257900 pence)
                EffectiveFrom = DateTime.UtcNow.AddDays(-10), // Effective 10 days ago
                EffectiveTo = DateTime.UtcNow.AddDays(10) // Expires in 10 days
            },
            // First 20 Subsidiaries - Fee (Currently Active)
            new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.ComplianceSchemeSubsidiaries, Description = "Compliance Scheme Subsidiaries" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Type=SubsidiariesConstants.UpTo20, Description = "Up to 20" },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Type="GB-ENG", Description = "England" },
                Amount = 55800m, // £558 represented in pence per subsidiary (55800 pence)
                EffectiveFrom = DateTime.UtcNow.AddDays(-10), // Effective 10 days ago
                EffectiveTo = DateTime.UtcNow.AddDays(10) // Expires in 10 days
            },
            // Additional More Than 100 Subsidiaries - Fee (Currently Active)
            new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.ComplianceSchemeSubsidiaries, Description = "Compliance Scheme Subsidiaries" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Type = SubsidiariesConstants.MoreThan100, Description = "More than 100" },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Type="GB-ENG", Description = "England" },
                Amount = 1, // £0.01 represented in pence per additional subsidiary (1 pence)
                EffectiveFrom = DateTime.UtcNow.AddDays(-10), // Effective 10 days ago
                EffectiveTo = DateTime.UtcNow.AddDays(10) // Expires in 10 days
            },
            // Additional Subsidiaries - Fee (Currently Active)
            new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.ComplianceSchemeSubsidiaries, Description = "Compliance Scheme Subsidiaries" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Type = SubsidiariesConstants.MoreThan20, Description = "More than 20" },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Type="GB-ENG", Description = "England" },
                Amount = 14000m, // £140 represented in pence per additional subsidiary (14000 pence)
                EffectiveFrom = DateTime.UtcNow.AddDays(-10), // Effective 10 days ago
                EffectiveTo = DateTime.UtcNow.AddDays(10) // Expires in 10 days
            },
            // Compliance Scheme Resubmission Fee (Currently Active)
            new Common.Data.DataModels.Lookups.RegistrationFees
            {
                Group = new Common.Data.DataModels.Lookups.Group { Type = GroupTypeConstants.ComplianceSchemeResubmission, Description = "Compliance Scheme Re-submission" },
                SubGroup = new Common.Data.DataModels.Lookups.SubGroup { Type = SubGroupTypeConstants.ReSubmitting, Description = "Re-submitting a report" },
                Regulator = new Common.Data.DataModels.Lookups.Regulator { Type = "GB-ENG", Description = "England" },
                Amount = 43000m, // The fee for resubmission, represented in pence
                EffectiveFrom = DateTime.UtcNow.AddDays(-10), // Effective 10 days ago
                EffectiveTo = DateTime.UtcNow.AddDays(10) // Expires in 10 days
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
