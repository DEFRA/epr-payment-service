using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Data.Constants;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.Data.SeedData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPR.Payment.Service.Common.Data.TypeConfigurations.Lookups
{
    [ExcludeFromCodeCoverage]
    public class RegulatorConfiguration : CommonBaseEntityConfiguration<Regulator>
    {
        /// <inheritdoc />
        public override void Configure(EntityTypeBuilder<Regulator> builder)
        {
            builder.ToTable(TableNameConstants.RegulatorTableName, SchemaNameConstants.LookupSchemaName);
            base.Configure(builder);

            RegulatorDataSeed.SeedRegulatorData(builder);
        }
    }
}
