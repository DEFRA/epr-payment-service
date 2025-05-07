using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using Microsoft.EntityFrameworkCore;

namespace EPR.Payment.Service.Common.Data.SeedData
{
    [ExcludeFromCodeCoverage]
    public class InitialDataSeed
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>().HasData(
               new Group { Id = 1, Type = "ProducerType", Description = "Producer Type" },
               new Group { Id = 2, Type = "ComplianceScheme", Description = "Compliance Scheme" },
               new Group { Id = 3, Type = "ProducerSubsidiaries", Description = "Producer Subsidiaries" },
               new Group { Id = 4, Type = "ComplianceSchemeSubsidiaries", Description = "Compliance Scheme Subsidiaries" },
               new Group { Id = 5, Type = "ProducerResubmission", Description = "Producer re-submitting a report" },
               new Group { Id = 6, Type = "ComplianceSchemeResubmission", Description = "Compliance Scheme re-submitting a report" }
               );

            modelBuilder.Entity<SubGroup>().HasData(
               new SubGroup { Id = 1, Type = "Large", Description = "Large producer" },
               new SubGroup { Id = 2, Type = "Small", Description = "Small producer" },
               new SubGroup { Id = 3, Type = "Registration", Description = "Registration" },
               new SubGroup { Id = 4, Type = "Online", Description = "Online Market" },
               new SubGroup { Id = 5, Type = "UpTo20", Description = "Up to 20" },
               new SubGroup { Id = 6, Type = "MoreThan20", Description = "More than 20" },
               new SubGroup { Id = 7, Type = "ReSubmitting", Description = "Re-submitting a report" },
               new SubGroup { Id = 8, Type = "LateFee", Description = "Late Fee" }
               );
           
            RegistrationFeesDataSeed.SeedRegistrationFees(modelBuilder.Entity<RegistrationFees>());
        }
    }
}
