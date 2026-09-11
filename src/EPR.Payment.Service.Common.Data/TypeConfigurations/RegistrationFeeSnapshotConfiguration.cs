using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Data.Constants;
using EPR.Payment.Service.Common.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPR.Payment.Service.Common.Data.TypeConfigurations
{
    [ExcludeFromCodeCoverage]
    public class RegistrationFeeSnapshotConfiguration : IEntityTypeConfiguration<RegistrationFeeSnapshot>
    {
        public void Configure(EntityTypeBuilder<RegistrationFeeSnapshot> builder)
        {
            builder.ToTable(TableNameConstants.RegistrationFeeSnapshotTableName, TableNameConstants.RegistrationSchemaName);

            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id).HasDefaultValueSql("NEWID()");

            builder.Property(s => s.RegistrationSubmissionDataId).IsRequired();
            builder.Property(s => s.TotalFee).HasPrecision(19, 4).IsRequired();
            builder.Property(s => s.CreatedDate)
                   .HasColumnType("datetimeoffset")
                   .HasDefaultValueSql("SYSUTCDATETIME()")
                   .IsRequired();

            builder.HasIndex(s => s.RegistrationSubmissionDataId).IsUnique();

            builder.HasOne(s => s.RegistrationSubmissionData)
                   .WithMany()
                   .HasForeignKey(s => s.RegistrationSubmissionDataId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired();

            builder.HasMany(s => s.LineItems)
                   .WithOne(l => l.RegistrationFeeSnapshot)
                   .HasForeignKey(l => l.RegistrationFeeSnapshotId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
