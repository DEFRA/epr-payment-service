using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Data.Constants;
using EPR.Payment.Service.Common.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPR.Payment.Service.Common.Data.TypeConfigurations
{
    [ExcludeFromCodeCoverage]
    public class RegistrationFeeLineItemConfiguration : IEntityTypeConfiguration<RegistrationFeeLineItem>
    {
        public void Configure(EntityTypeBuilder<RegistrationFeeLineItem> builder)
        {
            builder.ToTable(TableNameConstants.RegistrationFeeLineItemTableName, TableNameConstants.RegistrationSchemaName);

            builder.HasKey(l => l.Id);
            builder.Property(l => l.Id).HasDefaultValueSql("NEWID()");

            builder.Property(l => l.RegistrationFeeSnapshotId).IsRequired();
            builder.Property(l => l.FeeTypeId).HasConversion<int>().IsRequired();
            builder.Property(l => l.FeeTypeName).IsRequired().HasMaxLength(100);
            builder.Property(l => l.UnitPrice).HasPrecision(19, 4);
            builder.Property(l => l.Quantity);
            builder.Property(l => l.Amount).HasPrecision(19, 4).IsRequired();
            builder.Property(l => l.MemberId).HasMaxLength(50);
            builder.Property(l => l.BandNumber);

            builder.HasIndex(l => new { l.RegistrationFeeSnapshotId, l.FeeTypeId });
        }
    }
}
