using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Data.Constants;
using EPR.Payment.Service.Common.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPR.Payment.Service.Common.Data.TypeConfigurations
{
    [ExcludeFromCodeCoverage]
    public class RegistrationSubmissionDataEventConfiguration : IEntityTypeConfiguration<RegistrationSubmissionDataEvent>
    {
        public void Configure(EntityTypeBuilder<RegistrationSubmissionDataEvent> builder)
        {
            builder.ToTable(TableNameConstants.RegistrationSubmissionDataEventsTableName, TableNameConstants.RegistrationSchemaName);

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasDefaultValueSql("NEWID()");

            builder.Property(e => e.RegistrationSubmissionDataId).IsRequired();
            builder.Property(e => e.EventName).IsRequired().HasMaxLength(100);
            builder.Property(e => e.EventDate).HasColumnType("datetime2").IsRequired();
            builder.Property(e => e.CreatedDate).HasColumnType("datetimeoffset").IsRequired();

            builder.HasOne(e => e.RegistrationSubmissionData)
                .WithMany(r => r.Events)
                .HasForeignKey(e => e.RegistrationSubmissionDataId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => new { e.RegistrationSubmissionDataId, e.EventName, e.EventDate })
                .IsUnique()
                .HasDatabaseName("IX_RegistrationSubmissionDataEvents_SubmissionData_Event_Date_Unique");
        }
    }
}
