using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.Data.Interfaces;
using EPR.Payment.Service.Common.Data.SeedData;
using Microsoft.EntityFrameworkCore;

namespace EPR.Payment.Service.Common.Data
{
    public class FeePaymentDataContext : DbContext, IPaymentDataContext
    {
        public FeePaymentDataContext()
        {
        }

        public FeePaymentDataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<PaymentStatus> PaymentStatus => Set<PaymentStatus>();
        public DbSet<DataModels.Payment> Payment => Set<DataModels.Payment>();
        public DbSet<Fees> Fees => Set<Fees>();

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