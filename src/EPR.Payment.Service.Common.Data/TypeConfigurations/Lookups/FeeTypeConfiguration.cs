using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Data.Constants;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.Data.SeedData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPR.Payment.Service.Common.Data.TypeConfigurations.Lookups
{
    [ExcludeFromCodeCoverage]
    public class FeeTypeConfiguration : IEntityTypeConfiguration<FeeType>
    {
        public void Configure(EntityTypeBuilder<FeeType> builder)
        {
            builder.ToTable(TableNameConstants.FeeTypesTableName, SchemaNameConstants.LookupSchemaName);
            builder.HasKey(f => f.Id);
            builder.Property(f => f.Name).IsRequired();

            FeeTypeDataSeed.SeedFeeTypes(builder);
        }
    }
}
