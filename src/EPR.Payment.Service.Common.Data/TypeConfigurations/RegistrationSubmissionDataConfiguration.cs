using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Data.Constants;
using EPR.Payment.Service.Common.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPR.Payment.Service.Common.Data.TypeConfigurations
{
    [ExcludeFromCodeCoverage]
    public class RegistrationSubmissionDataConfiguration : IEntityTypeConfiguration<RegistrationSubmissionData>
    {
        public void Configure(EntityTypeBuilder<RegistrationSubmissionData> builder)
        {
            builder.ToTable(TableNameConstants.RegistrationSubmissionDataTableName, TableNameConstants.RegistrationSchemaName);

            builder.HasKey(r => r.Id);
            builder.Property(r => r.Id).HasDefaultValueSql("NEWID()");

            builder.Property(r => r.SubmissionId).IsRequired();
            builder.Property(r => r.RegistrationBlobName).IsRequired().HasMaxLength(100);
            builder.Property(r => r.ComplianceSchemeId);
            builder.Property(r => r.SubmissionPeriod).IsRequired();
            builder.Property(r => r.SubmissionDate).HasColumnType("datetime2").IsRequired();
            builder.Property(r => r.CreatedDate).HasColumnType("datetimeoffset").IsRequired();
            builder.Property(r => r.UpdatedDate).HasColumnType("datetimeoffset");

            builder.HasIndex(r => r.RegistrationBlobName).IsUnique();
        }
    }
}
