using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Data.Constants;
using EPR.Payment.Service.Common.Data.DataModels.Migration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPR.Payment.Service.Common.Data.TypeConfigurations.Migration
{
    [ExcludeFromCodeCoverage]
    public class ExcludedSubmissionConfiguration : IEntityTypeConfiguration<ExcludedSubmission>
    {
        public void Configure(EntityTypeBuilder<ExcludedSubmission> builder)
        {
            builder.ToTable(TableNameConstants.ExcludedSubmissionTableName, TableNameConstants.MigrationSchemaName);

            builder.HasKey(e => e.SubmissionId);
            builder.Property(e => e.SubmissionId).HasMaxLength(100);
            builder.Property(e => e.Reason).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Note).HasMaxLength(1000);
            builder.Property(e => e.CreatedAt).HasColumnType("datetimeoffset").HasDefaultValueSql("SYSUTCDATETIME()");
        }
    }
}
