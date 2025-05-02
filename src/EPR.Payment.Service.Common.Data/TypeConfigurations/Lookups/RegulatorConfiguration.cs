using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Data.Constants;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPR.Payment.Service.Common.Data.TypeConfigurations.Lookups
{
    [ExcludeFromCodeCoverage]
    public class RegulatorConfiguration : IEntityTypeConfiguration<Regulator>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<Regulator> builder)
        {
            builder.ToTable(TableNameConstants.RegulatorTableName, SchemaNameConstants.LookupSchemaName);

            builder.Property(p => p.Type)
                   .HasMaxLength(50);

            builder.Property(p => p.Description)
                   .HasMaxLength(255);
        }
    }
}
