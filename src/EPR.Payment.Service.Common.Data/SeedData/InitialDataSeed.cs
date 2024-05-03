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

            modelBuilder.Entity<Fees>().HasData(
                new Fees { Id = 1, Large = true, Regulator = "EA", Amount = 2616, EffectiveFrom = effectiveFromDate },
                new Fees { Id = 2, Large = true, Regulator = "SEPA", Amount = 2616, EffectiveFrom = effectiveFromDate },
                new Fees { Id = 3, Large = true, Regulator = "NRW", Amount = 2616, EffectiveFrom = effectiveFromDate },
                new Fees { Id = 4, Large = true, Regulator = "NIEA", Amount = 2616, EffectiveFrom = effectiveFromDate },
                new Fees { Id = 5, Large = false, Regulator = "EA", Amount = 505, EffectiveFrom = effectiveFromDate },
                new Fees { Id = 6, Large = false, Regulator = "SEPA", Amount = 505, EffectiveFrom = effectiveFromDate },
                new Fees { Id = 7, Large = false, Regulator = "NRW", Amount = 505, EffectiveFrom = effectiveFromDate },
                new Fees { Id = 8, Large = false, Regulator = "NIEA", Amount = 505, EffectiveFrom = effectiveFromDate }
                );
        }

    }
}
