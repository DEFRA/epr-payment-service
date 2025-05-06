using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Data.Constants;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPR.Payment.Service.Common.Data.TypeConfigurations.Lookups
{
    [ExcludeFromCodeCoverage]
    public class SubGroupConfiguration : CommonBaseEntityConfiguration<SubGroup>
    {
        /// <inheritdoc />
        public override void Configure(EntityTypeBuilder<SubGroup> builder)
        {
            builder.ToTable(TableNameConstants.SubGroupTableName, SchemaNameConstants.LookupSchemaName);

            base.Configure(builder);
        }
    }
}
