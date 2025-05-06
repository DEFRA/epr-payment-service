using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Data.Constants;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPR.Payment.Service.Common.Data.TypeConfigurations.Lookups
{
    [ExcludeFromCodeCoverage]
    public class SubGroupConfiguration : IEntityTypeConfiguration<SubGroup>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<SubGroup> builder)
        {
            builder.ToTable(TableNameConstants.SubGroupTableName, SchemaNameConstants.LookupSchemaName);

            builder.Property(p => p.Type)
                   .HasColumnType("varchar(50)");

            builder.Property(p => p.Description)
                   .HasColumnType("varchar(255)");
        }
    }
}
