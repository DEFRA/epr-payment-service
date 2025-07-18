using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Data.Constants;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.Data.SeedData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPR.Payment.Service.Common.Data.TypeConfigurations.Lookups
{
    [ExcludeFromCodeCoverage]
    public class PayerTypeConfiguration : IEntityTypeConfiguration<PayerType>
    {
        public void Configure(EntityTypeBuilder<PayerType> builder)
        {
            builder.ToTable(TableNameConstants.PayerTypesTableName, SchemaNameConstants.LookupSchemaName);
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                   .HasMaxLength(50)
                   .IsRequired();

            PayerTypeDataSeed.SeedPayerTypes(builder);
        }
    }
}