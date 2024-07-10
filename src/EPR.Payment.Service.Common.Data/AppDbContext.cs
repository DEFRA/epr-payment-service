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

        public virtual DbSet<PaymentStatus> PaymentStatus => Set<PaymentStatus>();
        public virtual DbSet<DataModels.Payment> Payment => Set<DataModels.Payment>();
        public virtual DbSet<InternalError> InternalError => Set<InternalError>();
        public virtual DbSet<AdditionalRegistrationFees> AdditionalFees => Set<AdditionalRegistrationFees>();
        public virtual DbSet<ComplianceSchemeRegistrationFees> ComplianceShemeRegitrationFees => Set<ComplianceSchemeRegistrationFees>();
        public virtual DbSet<ProducerRegistrationFees> ProducerRegitrationFees => Set<ProducerRegistrationFees>();
        public virtual DbSet<SubsidiariesRegistrationFees> Subsidiaries => Set<SubsidiariesRegistrationFees>();

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