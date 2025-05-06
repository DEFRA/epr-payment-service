using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Data.Constants;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPR.Payment.Service.Common.Data.TypeConfigurations.Lookups
{
    [ExcludeFromCodeCoverage]
    public class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.ToTable(TableNameConstants.GroupTableName, SchemaNameConstants.LookupSchemaName);

            builder.Property(p => p.Type)
                   .HasColumnType("varchar(50)");

            builder.Property(p => p.Description)
                   .HasColumnType("varchar(255)");
        }
    }
}
