using System.Diagnostics.CodeAnalysis;
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
            builder.ToTable(TableNameConstants.PaymentTableName);

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
                   .HasColumnType("varchar(20)");

            builder.Property(p => p.Reference)
                   .HasColumnOrder(6)
                   .HasColumnType("nvarchar(255)");

            builder.Property(p => p.Amount)
                   .HasColumnOrder(7)
                   .HasPrecision(19, 4);

            builder.Property(p => p.ReasonForPayment)
                   .HasColumnOrder(8)
                   .HasColumnType("nvarchar(255)");

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

            builder.HasOne(p => p.PaymentStatus)
                   .WithMany(p=> p.Payments)
                   .HasForeignKey(p => p.InternalStatusId);
        }
    }
}
