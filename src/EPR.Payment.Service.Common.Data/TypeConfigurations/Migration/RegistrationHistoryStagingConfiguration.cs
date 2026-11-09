using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Data.Constants;
using EPR.Payment.Service.Common.Data.DataModels.Migration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPR.Payment.Service.Common.Data.TypeConfigurations.Migration
{
    [ExcludeFromCodeCoverage]
    public class RegistrationHistoryStagingConfiguration : IEntityTypeConfiguration<RegistrationHistoryStaging>
    {
        private const int IdLength = 100;
        private const int TextLength = 1000;

        public void Configure(EntityTypeBuilder<RegistrationHistoryStaging> builder)
        {
            builder.ToTable(TableNameConstants.RegistrationHistoryStagingTableName, TableNameConstants.MigrationSchemaName);

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).UseIdentityColumn();

            builder.Property(e => e.SubmissionId).HasMaxLength(IdLength);
            builder.Property(e => e.FileId).HasMaxLength(IdLength);
            builder.Property(e => e.BlobName).HasMaxLength(TextLength);
            builder.Property(e => e.OrganisationId).HasMaxLength(IdLength);
            builder.Property(e => e.ComplianceSchemeId).HasMaxLength(IdLength);
            builder.Property(e => e.SubmissionPeriod).HasMaxLength(TextLength);
            builder.Property(e => e.RegistrationJourney).HasMaxLength(TextLength);
            builder.Property(e => e.RegulatorNation).HasMaxLength(IdLength);
            builder.Property(e => e.Roles).HasMaxLength(TextLength);
            builder.Property(e => e.EventType).HasMaxLength(IdLength);
            builder.Property(e => e.ReplayTs).HasColumnType("datetime2");
            builder.Property(e => e.EventDate).HasColumnType("datetime2");
            builder.Property(e => e.EarliestUnselectedAppSubmittedDate).HasColumnType("datetime2");
            builder.Property(e => e.ApplicationReferenceNumber).HasMaxLength(TextLength);
            builder.Property(e => e.Decision).HasMaxLength(TextLength);

            builder.HasOne<MigrationRun>()
                .WithMany()
                .HasForeignKey(e => e.RunId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => new { e.RunId, e.SubmissionId, e.ReplayTs });
        }
    }
}
