using EPR.Payment.Service.Common.Data.Constants;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.Data.SeedData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace EPR.Payment.Service.Common.Data.TypeConfigurations.Lookups
{
    [ExcludeFromCodeCoverage]
    public class RequestorTypeConfiguration : CommonBaseEntityConfiguration<RequestorType>
    {
        /// <inheritdoc />
        public override void Configure(EntityTypeBuilder<RequestorType> builder)
        {
            builder.ToTable(TableNameConstants.RequestorTypeTableName, SchemaNameConstants.LookupSchemaName);

            base.Configure(builder);

            RequestorTypeDataSeed.SeedRequestorTypeData(builder);
        }
    }
}
