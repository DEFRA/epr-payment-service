using System.Diagnostics.CodeAnalysis;
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
            builder.ToTable(TableNameConstants.OnlinePaymentTableName);

            builder.Property(p => p.Id)
                   .HasColumnOrder(1);

            builder.Property(p => p.PaymentId)
                   .HasColumnOrder(2);

            
            builder.Property(p => p.OrganisationId)
                   .HasColumnOrder(3);

            builder.Property(p => p.GovPayPaymentId)
                   .HasColumnOrder(4)
                   .HasColumnType("varchar(50)");

            builder.Property(p => p.GovPayStatus)
                   .HasColumnOrder(5)
                   .HasColumnType("varchar(20)");

            builder.Property(p => p.ErrorCode)
                   .HasColumnOrder(6)
                   .HasColumnType("varchar(255)");

            builder.Property(p => p.ErrorMessage)
                   .HasColumnOrder(7)
                   .HasColumnType("varchar(255)");

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
