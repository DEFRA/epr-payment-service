using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Data.Constants;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.Data.SeedData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPR.Payment.Service.Common.Data.TypeConfigurations.Lookups
{
    [ExcludeFromCodeCoverage]
    public class TonnageBandConfiguration : CommonBaseEntityConfiguration<TonnageBand>
    {
        /// <inheritdoc />
        public override void Configure(EntityTypeBuilder<TonnageBand> builder)
        {
            builder.ToTable(TableNameConstants.TonnageBandTableName, SchemaNameConstants.LookupSchemaName);

            base.Configure(builder);

            TonnageBandDataSeed.SeedTonnageBandData(builder);
        }
    }
}
