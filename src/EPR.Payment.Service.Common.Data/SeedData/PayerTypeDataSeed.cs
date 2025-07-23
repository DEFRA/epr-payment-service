using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPR.Payment.Service.Common.Data.SeedData
{
    [ExcludeFromCodeCoverage]
    public static class PayerTypeDataSeed
    {
        public static void SeedPayerTypes(EntityTypeBuilder<PayerType> builder)
        {
            builder.HasData(
                new PayerType { Id = 1, Name = "Direct Producer" },
                new PayerType { Id = 2, Name = "Compliance Scheme" },
                new PayerType { Id = 3, Name = "Reprocessor" },
                new PayerType { Id = 4, Name = "Exporter" }
            );
        }
    }
}
