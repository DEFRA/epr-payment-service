using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Constants.RegistrationFees;
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
             new Regulator { Id = 1, Type = RegulatorConstants.GBENG, Description = "England" },
             new Regulator { Id = 2, Type = RegulatorConstants.GBSCT, Description = "Scotland" },
             new Regulator { Id = 3, Type = RegulatorConstants.GBWLS, Description = "Wales" },
             new Regulator { Id = 4, Type = RegulatorConstants.GBNIR, Description = "Northern Ireland" }
             );
        }
    }
}
