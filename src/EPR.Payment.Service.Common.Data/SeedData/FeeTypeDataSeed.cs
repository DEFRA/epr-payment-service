using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPR.Payment.Service.Common.Data.SeedData
{
    [ExcludeFromCodeCoverage]
    public static class FeeTypeDataSeed
    {
        public static void SeedFeeTypes(EntityTypeBuilder<FeeType> builder)
        {
            builder.HasData(
                new FeeType { Id = 1, Name = "Producer Registration Fee" },
                new FeeType { Id = 2, Name = "Compliance Scheme Registration Fee" },
                new FeeType { Id = 3, Name = "Producer OnlineMarketPlace Fee" },
                new FeeType { Id = 4, Name = "Member Registration Fee" },
                new FeeType { Id = 5, Name = "Member Late Registration Fee" },
                new FeeType { Id = 6, Name = "UnitOMP Fee" },
                new FeeType { Id = 7, Name = "Subsidiary Fee" },
                new FeeType { Id = 8, Name = "Late Registration Fee" },
                /*new FeeType { Id = 9, Name = "Producer Resubmission Fee" },*/
                new FeeType { Id = 10, Name = "Compliance Scheme Resubmission" },
                new FeeType { Id = 11, Name = "FeePreviousPayment" }, 
                new FeeType { Id = 12, Name = "OutstandingPayment" },
                new FeeType { Id = 13, Name = "BandNumber 1" },
                new FeeType { Id = 14, Name = "BandNumber 2" },
                new FeeType { Id = 15, Name = "BandNumber 3" },
                new FeeType { Id = 16, Name = "PreviousPayment(reuse)" },
                new FeeType { Id = 17, Name = "OutstandingPayment(reuse)" },
                new FeeType { Id = 18, Name = "Member OnlineMarketPlace Fee" }
            );
        }
    }
}
