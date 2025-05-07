using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPR.Payment.Service.Common.Data.SeedData
{
    [ExcludeFromCodeCoverage]
    public static class RegulatorDataSeed
    {
        public static void SeedRegulatorData(EntityTypeBuilder<Regulator> builder)
        {
            builder.HasData(
             new Regulator { Id = 1, Type = "GB-ENG", Description = "England" },
             new Regulator { Id = 2, Type = "GB-SCT", Description = "Scotland" },
             new Regulator { Id = 3, Type = "GB-WLS", Description = "Wales" },
             new Regulator { Id = 4, Type = "GB-NIR", Description = "Northern Ireland" }
             );
        }
    }
}
