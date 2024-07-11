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
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}
        public DbSet<PaymentStatus> PaymentStatus => Set<PaymentStatus>();
        public DbSet<DataModels.Payment> Payment => Set<DataModels.Payment>();
        public DbSet<InternalError> InternalError => Set<InternalError>();
        public DbSet<AdditionalRegistrationFees> AdditionalRegistrationFees => Set<AdditionalRegistrationFees>();
        public DbSet<ComplianceSchemeRegistrationFees> ComplianceSchemeRegistrationFees => Set<ComplianceSchemeRegistrationFees>();
        public DbSet<ProducerRegistrationFees> ProducerRegistrationFees => Set<ProducerRegistrationFees>();
        public DbSet<SubsidiariesRegistrationFees> SubsidiariesRegistrationFees => Set<SubsidiariesRegistrationFees>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DataModels.Payment>()
            .HasIndex(a => a.GovpayPaymentId)
            .IsUnique();

            // seed the lookup tables
            InitialDataSeed.Seed(modelBuilder);
        }
    }
}