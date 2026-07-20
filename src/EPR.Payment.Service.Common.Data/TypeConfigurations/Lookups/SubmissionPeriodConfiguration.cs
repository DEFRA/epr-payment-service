using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Data.Constants;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.Data.SeedData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPR.Payment.Service.Common.Data.TypeConfigurations.Lookups
{
    [ExcludeFromCodeCoverage]
    public class SubmissionPeriodConfiguration : IEntityTypeConfiguration<SubmissionPeriod>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<SubmissionPeriod> builder)
        {
            builder.ToTable(TableNameConstants.SubmissionPeriodTableName, SchemaNameConstants.LookupSchemaName);

            builder.Property(p => p.WindowType).IsRequired().HasMaxLength(40);
            builder.Property(p => p.RegistrationYear).IsRequired();
            builder.Property(p => p.OpeningDate).HasColumnType("datetime2").IsRequired();
            builder.Property(p => p.DeadlineDate).HasColumnType("datetime2").IsRequired();
            builder.Property(p => p.ClosingDate).HasColumnType("datetime2").IsRequired();

            builder.HasIndex(p => new { p.WindowType, p.RegistrationYear }).IsUnique();

            SubmissionPeriodDataSeed.SeedSubmissionPeriods(builder);
        }
    }
}
