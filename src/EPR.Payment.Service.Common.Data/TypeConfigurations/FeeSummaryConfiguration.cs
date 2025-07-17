using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Data.Constants;
using EPR.Payment.Service.Common.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPR.Payment.Service.Common.Data.TypeConfigurations
{
    [ExcludeFromCodeCoverage]
    public class FeeSummaryConfiguration : IEntityTypeConfiguration<FeeSummary>
    {
        public void Configure(EntityTypeBuilder<FeeSummary> builder)
        {
            builder.ToTable(TableNameConstants.FeeSummaryTableName);

            builder.HasKey(f => f.Id);

            builder.Property(f => f.Amount).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(f => f.UnitPrice).HasColumnType("decimal(18,2)");
            builder.Property(f => f.Quantity).IsRequired();
            builder.Property(f => f.AppRefNo).IsRequired();
            builder.Property(f => f.InvoiceDate).HasColumnType("datetime2");
            builder.Property(f => f.InvoicePeriod).HasColumnType("datetime2");
            builder.Property(f => f.CreatedDate).HasColumnType("datetime2");
            builder.Property(f => f.UpdatedDate).HasColumnType("datetime2");

            builder.HasOne(f => f.FeeType)
                   .WithMany()
                   .HasForeignKey(f => f.FeeTypeId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(f => f.PayerType)
                   .WithMany()
                   .HasForeignKey(f => f.PayerTypeId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}