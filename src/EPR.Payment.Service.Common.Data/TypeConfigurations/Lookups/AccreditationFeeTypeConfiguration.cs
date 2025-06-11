using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Data.Constants;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.Data.SeedData;
using EPR.Payment.Service.Common.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPR.Payment.Service.Common.Data.TypeConfigurations.Lookups
{
    [ExcludeFromCodeCoverage]
    public class AccreditationFeeTypeConfiguration : IEntityTypeConfiguration<AccreditationFee>
    {
        public void Configure(EntityTypeBuilder<AccreditationFee> builder)
        {
            builder.ToTable(TableNameConstants.AccreditationFeesTableName, SchemaNameConstants.LookupSchemaName);

            builder.HasOne(x => x.Group).WithMany().HasForeignKey(x => x.GroupId);
            builder.HasOne(x => x.SubGroup).WithMany().HasForeignKey(x => x.SubGroupId);
            builder.HasOne(x => x.Regulator).WithMany().HasForeignKey(x => x.RegulatorId);
            builder.HasOne(x => x.TonnageBand).WithMany().HasForeignKey(x => x.TonnageBandId);

            builder.Property(x => x.TonnageBandId)
                .HasDefaultValue((int)TonnageBands.Upto500);
            
            builder.Property(x => x.Amount)
                .HasColumnType("decimal(19,4)")
                .IsRequired();
            builder.Property(x => x.FeesPerSite)
                .IsRequired()
                .HasColumnType("decimal(19,4)");
            builder.Property(x => x.EffectiveFrom)
                .HasColumnType("datetime2")
                .IsRequired();
            builder.Property(x => x.EffectiveTo)
                .HasColumnType("datetime2")
                .IsRequired();

            AccreditationFeesDataSeed.SeedAccreditationFees(builder);
        }
    }
}
