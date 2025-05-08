using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Data.Constants;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.Data.SeedData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPR.Payment.Service.Common.Data.TypeConfigurations.Lookups
{
    [ExcludeFromCodeCoverage]
    public class GroupConfiguration : CommonBaseEntityConfiguration<Group>
    {
        /// <inheritdoc />
        public override void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.ToTable(TableNameConstants.GroupTableName, SchemaNameConstants.LookupSchemaName);

            base.Configure(builder);

            GroupDataSeed.SeedGroupData(builder);
        }
    }
}
