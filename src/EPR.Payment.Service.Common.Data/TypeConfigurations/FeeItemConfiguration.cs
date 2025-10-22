using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Data.Constants;
using EPR.Payment.Service.Common.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPR.Payment.Service.Common.Data.TypeConfigurations
{
    [ExcludeFromCodeCoverage]
    public class FeeItemConfiguration : IEntityTypeConfiguration<FeeItem>
    {
        public void Configure(EntityTypeBuilder<FeeItem> builder)
        {
            builder.ToTable(TableNameConstants.FeeItemTableName);

            builder.HasKey(f => f.Id);

            builder.Property(f => f.Amount).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(f => f.UnitPrice).HasColumnType("decimal(18,2)");
            builder.Property(f => f.Quantity).HasDefaultValue(0).IsRequired();
            builder.Property(f => f.AppRefNo).HasMaxLength(50).IsRequired();
            builder.Property(f => f.ExternalId).IsRequired();
            builder.Property(f => f.InvoiceDate).HasColumnType("datetimeoffset").IsRequired();
            builder.Property(f => f.InvoicePeriod).HasColumnType("datetimeoffset").IsRequired();
            builder.Property(f => f.PayerTypeId).IsRequired();
            builder.Property(f => f.PayerId).IsRequired();
            builder.Property(f => f.FeeTypeId).IsRequired();
            builder.Property(f => f.CreatedDate).HasColumnType("datetimeoffset").IsRequired();
            builder.Property(f => f.UpdatedDate).HasColumnType("datetimeoffset");
            builder.Property(f => f.FileId).IsRequired();

            builder.HasOne(f => f.FeeType)
                    .WithMany(ft => ft.FeeItems)
                   .HasForeignKey(f => f.FeeTypeId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(f => f.PayerType)
                    .WithMany(pt => pt.FeeItems)
                   .HasForeignKey(f => f.PayerTypeId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}