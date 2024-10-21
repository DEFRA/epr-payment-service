using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.Data.Interfaces;
using EPR.Payment.Service.Common.Data.SeedData;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace EPR.Payment.Service.Common.Data
{
    [ExcludeFromCodeCoverage]
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext()
        {
        }
              
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<PaymentStatus> PaymentStatus => Set<PaymentStatus>();
        public DbSet<DataModels.Payment> Payment => Set<DataModels.Payment>();
        public DbSet<DataModels.OnlinePayment> OnlinePayment => Set<DataModels.OnlinePayment>();
        public DbSet<Group> Group => Set<Group>();
        public DbSet<SubGroup> SubGroup => Set<SubGroup>();
        public DbSet<Regulator> Regulator => Set<Regulator>();
        public DbSet<RegistrationFees> RegistrationFees => Set<RegistrationFees>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer();
        }
    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DataModels.Payment>()
               .ToTable("Payment");

            modelBuilder.Entity<DataModels.OnlinePayment>()
                .ToTable("OnlinePayment");

            modelBuilder.Entity<DataModels.OnlinePayment>()
            .HasIndex(a => a.GovpayPaymentId)
            .IsUnique();

            modelBuilder.Entity<DataModels.Payment>()
             .Property(x => x.ExternalPaymentId)
             .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<DataModels.Payment>()
            .HasIndex(a => a.ExternalPaymentId)
            .IsUnique();

            // seed the lookup tables
            InitialDataSeed.Seed(modelBuilder);
        }
    }
}