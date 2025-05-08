using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.Data.EntityTypeConfigurations;
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
        public DbSet<DataModels.Payment> Payment => Set<DataModels.Payment>();
        public DbSet<PaymentStatus> PaymentStatus => Set<PaymentStatus>();
        public DbSet<DataModels.OnlinePayment> OnlinePayment => Set<DataModels.OnlinePayment>();
        public DbSet<DataModels.OfflinePayment> OfflinePayment => Set<DataModels.OfflinePayment>();
        public DbSet<Group> Group => Set<Group>();
        public DbSet<SubGroup> SubGroup => Set<SubGroup>();
        public DbSet<Regulator> Regulator => Set<Regulator>();
        public DbSet<RegistrationFees> RegistrationFees => Set<RegistrationFees>();
        public DbSet<AccreditationFee> AccreditationFees => Set<AccreditationFee>();

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
               .ToTable("OnlinePayment")
               .HasOne(p => p.Payment)
               .WithOne(op => op.OnlinePayment)
               .HasForeignKey<DataModels.OnlinePayment>(p => p.PaymentId);

            modelBuilder.Entity<DataModels.OfflinePayment>()
               .ToTable("OfflinePayment")
               .HasOne(p => p.Payment)
               .WithOne(op => op.OfflinePayment)
               .HasForeignKey<DataModels.OfflinePayment>(p => p.PaymentId);

            modelBuilder.Entity<DataModels.OnlinePayment>()
            .HasIndex(a => a.GovPayPaymentId)
            .IsUnique();

            modelBuilder.Entity<DataModels.Payment>()
             .Property(x => x.ExternalPaymentId)
             .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<DataModels.Payment>()
            .HasIndex(a => a.ExternalPaymentId)
            .IsUnique();

            // seed the lookup tables
            InitialDataSeed.Seed(modelBuilder);

            new AccreditationFeeTypeConfiguration().Configure(modelBuilder.Entity<AccreditationFee>());
        }
    }
}