using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPR.Payment.Service.Common.Data.TypeConfigurations.Lookups
{
    public class AccreditationFeeTypeConfiguration : IEntityTypeConfiguration<AccreditationFee>
    {
        public void Configure(EntityTypeBuilder<AccreditationFee> builder)
        {
            builder
                .ToTable("AccreditationFees", "Lookup")
                .HasKey(x => x.Id);

            builder.HasOne(x => x.Group).WithMany().HasForeignKey(x => x.GroupId);
            builder.HasOne(x => x.SubGroup).WithMany().HasForeignKey(x => x.SubGroupId);
            builder.HasOne(x => x.Regulator).WithMany().HasForeignKey(x => x.RegulatorId);

            builder.Property(x => x.TonnesUpTo)
                .IsRequired();
            builder.Property(x => x.TonnesOver)
                .IsRequired();
            builder.Property(x => x.Amount)
                .HasColumnType("decimal(19,4)")
                .IsRequired();
            builder.Property(x => x.FeesPerSite)
                .IsRequired();
            builder.Property(x => x.EffectiveFrom)
                .HasColumnType("datetime2")
                .IsRequired();
            builder.Property(x => x.EffectiveTo)
                .HasColumnType("datetime2")
                .IsRequired();
        }
    }
}
