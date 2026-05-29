using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Data.Constants;
using EPR.Payment.Service.Common.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPR.Payment.Service.Common.Data.TypeConfigurations
{
    [ExcludeFromCodeCoverage]
    public class RegistrationSubmissionProducerConfiguration : IEntityTypeConfiguration<RegistrationSubmissionProducer>
    {
        public void Configure(EntityTypeBuilder<RegistrationSubmissionProducer> builder)
        {
            builder.ToTable(TableNameConstants.RegistrationSubmissionProducerTableName);

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasDefaultValueSql("NEWID()");

            builder.Property(p => p.RegistrationSubmissionDataId).IsRequired();
            builder.Property(p => p.OrganisationId).IsRequired().HasMaxLength(20);
            builder.Property(p => p.OrganisationSize).IsRequired().HasMaxLength(20);
            builder.Property(p => p.NationId).IsRequired();
            builder.Property(p => p.IsOnlineMarketplace).IsRequired();
            builder.Property(p => p.IsClosedLoopRecycling).IsRequired();
            builder.Property(p => p.IsNewJoiner).IsRequired();
            builder.Property(p => p.CreatedDate).HasColumnType("datetimeoffset").IsRequired();

            builder.HasOne(p => p.RegistrationSubmissionData)
                .WithMany(rsd => rsd.Producers)
                .HasForeignKey(p => p.RegistrationSubmissionDataId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(p => p.RegistrationSubmissionDataId);
        }
    }
}
