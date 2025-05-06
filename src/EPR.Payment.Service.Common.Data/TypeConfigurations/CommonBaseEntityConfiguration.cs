using EPR.Payment.Service.Common.Data.DataModels.BaseClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPR.Payment.Service.Common.Data.TypeConfigurations
{
    public class CommonBaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : CommonBaseEntity
    {
        /// <inheritdoc />
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(p => p.Type)
                  .HasColumnType("varchar(50)");

            builder.Property(p => p.Description)
                   .HasColumnType("varchar(255)");
        }

    }
}
