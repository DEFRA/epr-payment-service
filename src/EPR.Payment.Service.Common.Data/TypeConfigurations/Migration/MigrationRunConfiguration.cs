using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Data.Constants;
using EPR.Payment.Service.Common.Data.DataModels.Migration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPR.Payment.Service.Common.Data.TypeConfigurations.Migration
{
    [ExcludeFromCodeCoverage]
    public class MigrationRunConfiguration : IEntityTypeConfiguration<MigrationRun>
    {
        public void Configure(EntityTypeBuilder<MigrationRun> builder)
        {
            builder.ToTable(TableNameConstants.MigrationRunTableName, TableNameConstants.MigrationSchemaName);

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasDefaultValueSql("NEWID()");

            builder.Property(e => e.RunKey).IsRequired().HasMaxLength(100);
            builder.HasIndex(e => e.RunKey).IsUnique();

            builder.Property(e => e.Status).IsRequired().HasConversion<string>().HasMaxLength(30);
            builder.Property(e => e.InstanceId).IsRequired().HasMaxLength(200);
            builder.Property(e => e.StartedAt).HasColumnType("datetimeoffset").IsRequired();
            builder.Property(e => e.StagingCompletedAt).HasColumnType("datetimeoffset");
            builder.Property(e => e.CompletedAt).HasColumnType("datetimeoffset");
        }
    }
}
