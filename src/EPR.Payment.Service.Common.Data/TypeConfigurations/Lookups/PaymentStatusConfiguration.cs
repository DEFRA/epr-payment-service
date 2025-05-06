using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Data.Constants;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPR.Payment.Service.Common.Data.TypeConfigurations.Lookups
{
    [ExcludeFromCodeCoverage]
    public class PaymentStatusConfiguration : IEntityTypeConfiguration<PaymentStatus>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<PaymentStatus> builder)
        {
            builder.ToTable(TableNameConstants.PaymentStatusTableName, SchemaNameConstants.LookupSchemaName);

            builder.Property(p => p.Status)
                   .HasColumnType("varchar(20)")
                   .IsRequired();
        }
    }
}
