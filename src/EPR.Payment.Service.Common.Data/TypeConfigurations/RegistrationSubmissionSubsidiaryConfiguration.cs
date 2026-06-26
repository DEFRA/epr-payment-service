using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Data.Constants;
using EPR.Payment.Service.Common.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPR.Payment.Service.Common.Data.TypeConfigurations
{
    [ExcludeFromCodeCoverage]
    public class RegistrationSubmissionSubsidiaryConfiguration : IEntityTypeConfiguration<RegistrationSubmissionSubsidiary>
    {
        public void Configure(EntityTypeBuilder<RegistrationSubmissionSubsidiary> builder)
        {
            builder.ToTable(TableNameConstants.RegistrationSubmissionSubsidiaryTableName, TableNameConstants.RegistrationSchemaName);

            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id).HasDefaultValueSql("NEWID()");

            builder.Property(s => s.RegistrationSubmissionProducerId).IsRequired();
            builder.Property(s => s.SubsidiaryId).IsRequired().HasMaxLength(32);
            builder.Property(s => s.IsOnlineMarketplace).IsRequired();
            builder.Property(s => s.IsClosedLoopRecycling).IsRequired();
            builder.Property(s => s.IsNewJoiner).IsRequired();
            builder.Property(s => s.CreatedDate).HasColumnType("datetimeoffset").IsRequired();

            builder.HasOne(s => s.RegistrationSubmissionProducer)
                .WithMany(p => p.Subsidiaries)
                .HasForeignKey(s => s.RegistrationSubmissionProducerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(s => s.RegistrationSubmissionProducerId);
        }
    }
}
