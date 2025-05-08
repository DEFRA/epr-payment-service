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
