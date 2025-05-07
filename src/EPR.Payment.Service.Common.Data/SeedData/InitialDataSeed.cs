using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace EPR.Payment.Service.Common.Data.SeedData
{
    [ExcludeFromCodeCoverage]
    public class InitialDataSeed
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            DateTime effectiveFromDate = DateTime.ParseExact("01/01/2024 00:00:00", "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
            DateTime effectiveToDate = DateTime.ParseExact("31/12/2025 23:59:59", "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);

            modelBuilder.Entity<PaymentStatus>().HasData(
                new PaymentStatus { Id = Enums.Status.Initiated, Status = "Initiated" },
                new PaymentStatus { Id = Enums.Status.InProgress, Status = "InProgress" },
                new PaymentStatus { Id = Enums.Status.Success, Status = "Success" },
                new PaymentStatus { Id = Enums.Status.Failed, Status = "Failed" },
                new PaymentStatus { Id = Enums.Status.Error, Status = "Error" },
                new PaymentStatus { Id = Enums.Status.UserCancelled, Status = "UserCancelled" }
                );

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

            modelBuilder.Entity<Regulator>().HasData(
               new Regulator { Id = 1, Type = "GB-ENG", Description = "England" },
               new Regulator { Id = 2, Type = "GB-SCT", Description = "Scotland" },
               new Regulator { Id = 3, Type = "GB-WLS", Description = "Wales" },
               new Regulator { Id = 4, Type = "GB-NIR", Description = "Northern Ireland" }
               );

            RegistrationFeesDataSeed.SeedRegistrationFees(modelBuilder.Entity<RegistrationFees>());
        }

    }
}
