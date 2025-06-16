using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Data.Constants;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.Data.SeedData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPR.Payment.Service.Common.Data.TypeConfigurations.Lookups
{
    [ExcludeFromCodeCoverage]
    public class PaymentMethodConfiguration : CommonBaseEntityConfiguration<PaymentMethod>
    {
        /// <inheritdoc />
        public override void Configure(EntityTypeBuilder<PaymentMethod> builder)
        {
            builder.ToTable(TableNameConstants.PaymentMethodTableName, SchemaNameConstants.LookupSchemaName);

            base.Configure(builder);

            PaymentMethodDataSeed.SeedPaymentMethodData(builder);
        }
    }
}
