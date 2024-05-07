using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace EPR.Payment.Service.Common.Data.SeedData
{
    public class InitialDataSeed
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            DateTime effectiveFromDate = DateTime.ParseExact("01/04/2024", "dd/MM/yyyy", CultureInfo.InvariantCulture);
            modelBuilder.Entity<PaymentStatus>().HasData(
                new PaymentStatus { Id = Enums.Status.Initiated, Status = "Initiated" },
                new PaymentStatus { Id = Enums.Status.Success, Status = "Success" },
                new PaymentStatus { Id = Enums.Status.Failed, Status = "Failed" },
                new PaymentStatus { Id = Enums.Status.Error, Status = "Error" }
                );

            modelBuilder.Entity<AccreditationFees>().HasData(
                new AccreditationFees { Id = 1, Large = true, Regulator = "GB-ENG", Amount = 2616, EffectiveFrom = effectiveFromDate },
                new AccreditationFees { Id = 2, Large = true, Regulator = "GB-SCT", Amount = 2616, EffectiveFrom = effectiveFromDate },
                new AccreditationFees { Id = 3, Large = true, Regulator = "GB-WLS", Amount = 2616, EffectiveFrom = effectiveFromDate },
                new AccreditationFees { Id = 4, Large = true, Regulator = "GB-NIR", Amount = 2616, EffectiveFrom = effectiveFromDate },
                new AccreditationFees { Id = 5, Large = false, Regulator = "GB-ENG", Amount = 505, EffectiveFrom = effectiveFromDate },
                new AccreditationFees { Id = 6, Large = false, Regulator = "GB-SCT", Amount = 505, EffectiveFrom = effectiveFromDate },
                new AccreditationFees { Id = 7, Large = false, Regulator = "GB-WLS", Amount = 505, EffectiveFrom = effectiveFromDate },
                new AccreditationFees { Id = 8, Large = false, Regulator = "GB-NIR", Amount = 505, EffectiveFrom = effectiveFromDate }
                );
        }

    }
}
