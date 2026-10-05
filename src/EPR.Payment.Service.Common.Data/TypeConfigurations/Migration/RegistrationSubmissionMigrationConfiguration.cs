using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Data.Constants;
using EPR.Payment.Service.Common.Data.DataModels.Migration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPR.Payment.Service.Common.Data.TypeConfigurations.Migration
{
    [ExcludeFromCodeCoverage]
    public class RegistrationSubmissionMigrationConfiguration : IEntityTypeConfiguration<RegistrationSubmissionMigration>
    {
        public void Configure(EntityTypeBuilder<RegistrationSubmissionMigration> builder)
        {
            builder.ToTable(TableNameConstants.RegistrationSubmissionMigrationTableName, TableNameConstants.MigrationSchemaName);

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasDefaultValueSql("NEWID()");

            builder.Property(e => e.SubmissionId).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Status).IsRequired().HasConversion<string>().HasMaxLength(30);
            builder.Property(e => e.Reason).HasMaxLength(50);
            builder.Property(e => e.ProcessedAt).HasColumnType("datetimeoffset");

            builder.HasOne<MigrationRun>()
                .WithMany()
                .HasForeignKey(e => e.RunId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => new { e.RunId, e.SubmissionId }).IsUnique();
            builder.HasIndex(e => new { e.RunId, e.Status });
        }
    }
}
