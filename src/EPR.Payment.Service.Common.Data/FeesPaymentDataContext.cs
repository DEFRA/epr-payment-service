using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.Data.Interfaces;
using EPR.Payment.Service.Common.Data.SeedData;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace EPR.Payment.Service.Common.Data
{
    [ExcludeFromCodeCoverage]
    public class FeesPaymentDataContext : DbContext, IPaymentDataContext
    {
        public FeesPaymentDataContext()
        {
        }

        public FeesPaymentDataContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<PaymentStatus> PaymentStatus => Set<PaymentStatus>();
        public virtual DbSet<DataModels.Payment> Payment => Set<DataModels.Payment>();
        public virtual DbSet<AccreditationFees> AccreditationFees => Set<AccreditationFees>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DataModels.Payment>()
            .HasIndex(a => a.GovpayPaymentId)
            .IsUnique();

            // seed the lookup tables
            InitialDataSeed.Seed(modelBuilder);
        }
    }
}