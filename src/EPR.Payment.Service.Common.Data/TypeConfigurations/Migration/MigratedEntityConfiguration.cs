using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Data.Constants;
using EPR.Payment.Service.Common.Data.DataModels.Migration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPR.Payment.Service.Common.Data.TypeConfigurations.Migration
{
    [ExcludeFromCodeCoverage]
    public class MigratedEntityConfiguration : IEntityTypeConfiguration<MigratedEntity>
    {
        public void Configure(EntityTypeBuilder<MigratedEntity> builder)
        {
            builder.ToTable(TableNameConstants.MigratedEntityTableName, TableNameConstants.MigrationSchemaName);

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasDefaultValueSql("NEWID()");

            builder.Property(e => e.FileId).IsRequired().HasMaxLength(100);
            builder.Property(e => e.CreatedAt).HasColumnType("datetimeoffset").IsRequired();

            builder.HasOne<MigrationRun>()
                .WithMany()
                .HasForeignKey(e => e.RunId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => new { e.RunId, e.RegistrationSubmissionDataId });
        }
    }
}
