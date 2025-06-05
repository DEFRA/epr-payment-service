using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Data.Constants;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.Data.SeedData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPR.Payment.Service.Common.Data.TypeConfigurations.Lookups
{
    [ExcludeFromCodeCoverage]
    public class RegistrationFeesConfiguration : IEntityTypeConfiguration<RegistrationFees>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<RegistrationFees> builder)
        {
            builder.ToTable(TableNameConstants.RegistrationFeesTableName, SchemaNameConstants.LookupSchemaName);

            builder.HasOne(x => x.Group).WithMany().HasForeignKey(x => x.GroupId);
            builder.HasOne(x => x.SubGroup).WithMany().HasForeignKey(x => x.SubGroupId);
            builder.HasOne(x => x.Regulator).WithMany().HasForeignKey(x => x.RegulatorId);

            builder.Property(p => p.Amount)
                   .HasPrecision(19, 4);

            RegistrationFeesDataSeed.SeedRegistrationFees(builder);
        }
    }
}
