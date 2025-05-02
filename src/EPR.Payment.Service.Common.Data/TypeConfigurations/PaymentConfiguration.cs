using System.Diagnostics.CodeAnalysis;
using System.Reflection.Emit;
using System.Security.Principal;
using EPR.Payment.Service.Common.Data.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPR.Payment.Service.Common.Data.TypeConfigurations
{
    [ExcludeFromCodeCoverage]
    public class PaymentConfiguration : IEntityTypeConfiguration<DataModels.Payment>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<DataModels.Payment> builder)
        {
            builder.ToTable(TableNameConstants.PaymentTableName, SchemaNameConstants.LookupSchemaName);

            builder.Property(p => p.Id)
                   .HasColumnOrder(1);

            builder.Property(p => p.UserId)
                   .HasColumnOrder(2);

            builder.Property(p => p.ExternalPaymentId)
                   .HasColumnOrder(3)
                   .ValueGeneratedOnAdd();
           
            builder.Property(p => p.InternalStatusId)
                   .HasColumnOrder(4);

            builder.Property(p => p.Regulator)
                   .HasColumnOrder(5)
                   .HasMaxLength(20);

            builder.Property(p => p.Reference)
                   .HasColumnOrder(6)
                   .HasMaxLength(255);

            builder.Property(p => p.Amount)
                   .HasColumnOrder(7)
                   .HasPrecision(19, 4);

            builder.Property(p => p.ReasonForPayment)
                   .HasColumnOrder(8)
                   .HasMaxLength(255);

            builder.Property(p => p.CreatedDate)
                   .HasColumnOrder(9);

            builder.Property(p => p.UpdatedByUserId)
                   .HasColumnOrder(10);

            builder.Property(p => p.UpdatedDate)
                   .HasColumnOrder(11);

            builder.HasIndex(p => p.ExternalPaymentId)
                   .IsUnique();

            builder.Property(x => x.ExternalPaymentId)
                   .HasDefaultValueSql("NEWID()");
        }
    }
}
