using System.Diagnostics.CodeAnalysis;
using System.Reflection.Emit;
using EPR.Payment.Service.Common.Data.Constants;
using EPR.Payment.Service.Common.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPR.Payment.Service.Common.Data.TypeConfigurations
{
    [ExcludeFromCodeCoverage]
    public class OnlinePaymentConfiguration : IEntityTypeConfiguration<OnlinePayment>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<OnlinePayment> builder)
        {
            builder.ToTable(TableNameConstants.OnlinePaymentTableName, SchemaNameConstants.LookupSchemaName);

            builder.Property(p => p.Id)
                   .HasColumnOrder(1);

            builder.Property(p => p.PaymentId)
                   .HasColumnOrder(2);

            
            builder.Property(p => p.OrganisationId)
                   .HasColumnOrder(3);

            builder.Property(p => p.GovPayPaymentId)
                   .HasColumnOrder(4)
                   .HasMaxLength(50);

            builder.Property(p => p.GovPayStatus)
                   .HasColumnOrder(5)
                   .HasMaxLength(20);

            builder.Property(p => p.ErrorCode)
                   .HasColumnOrder(6)
                   .HasMaxLength(255);

            builder.Property(p => p.ErrorMessage)
                   .HasColumnOrder(7)
                   .HasMaxLength(255);

            builder.Property(p => p.UpdatedByOrgId)
                   .HasColumnOrder(8);

            builder.HasIndex(a => a.GovPayPaymentId)
                   .IsUnique();


            builder.HasOne(p => p.Payment)
                   .WithOne(op => op.OnlinePayment)
                   .HasForeignKey<OnlinePayment>(p => p.PaymentId);
        }
    }
}
