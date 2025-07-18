using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Data.Constants;
using EPR.Payment.Service.Common.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPR.Payment.Service.Common.Data.TypeConfigurations;

[ExcludeFromCodeCoverage]
public class FileFeeSummaryConnectionConfiguration : IEntityTypeConfiguration<FileFeeSummaryConnection>
{
    public void Configure(EntityTypeBuilder<FileFeeSummaryConnection> builder)
    {
        builder.ToTable(TableNameConstants.FileFeeSummaryConnectionTableName);

        builder.HasKey(f => f.Id);

        builder.Property(f => f.FileId)
               .IsRequired();

        builder.Property(f => f.FeeSummaryId)
               .IsRequired();

        builder.HasOne(f => f.FeeSummary)
               .WithMany(f => f.FileFeeSummaryConnections)
               .HasForeignKey(f => f.FeeSummaryId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
