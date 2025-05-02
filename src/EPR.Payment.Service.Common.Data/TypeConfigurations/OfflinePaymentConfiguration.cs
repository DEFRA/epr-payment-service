using System.Diagnostics.CodeAnalysis;
using System.Reflection.Emit;
using EPR.Payment.Service.Common.Data.Constants;
using EPR.Payment.Service.Common.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPR.Payment.Service.Common.Data.TypeConfigurations
{
    [ExcludeFromCodeCoverage]
    public class OfflinePaymentConfiguration : IEntityTypeConfiguration<OfflinePayment>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<OfflinePayment> builder)
        {
            builder.ToTable(TableNameConstants.OfflinePaymentTableName, SchemaNameConstants.LookupSchemaName);

            builder.Property(p => p.Id)
                   .HasColumnOrder(1);

            builder.Property(p => p.PaymentId)
                   .HasColumnOrder(2);

            builder.Property(p => p.PaymentDate)
                   .HasColumnOrder(3);

            builder.Property(p => p.Comments)
                   .HasColumnOrder(4)
                   .HasMaxLength(255);

            builder.HasOne(p => p.Payment)
                   .WithOne(op => op.OfflinePayment)
                   .HasForeignKey<OfflinePayment>(p => p.PaymentId);
        }
    }
}
