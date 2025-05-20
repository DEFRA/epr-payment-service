using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Data.Constants;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.Data.SeedData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPR.Payment.Service.Common.Data.TypeConfigurations.Lookups
{
    [ExcludeFromCodeCoverage]
    public class RegistrationFeesConfiguration : IEntityTypeConfiguration<RegistrationFees>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<RegistrationFees> builder)
        {
            builder.ToTable(TableNameConstants.RegistrationFeesTableName, SchemaNameConstants.LookupSchemaName);

            builder.Property(p => p.Amount)
                   .HasPrecision(19, 4);

            RegistrationFeesDataSeed.SeedRegistrationFees(builder);
        }
    }
}
