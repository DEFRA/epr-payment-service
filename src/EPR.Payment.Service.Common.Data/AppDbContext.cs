using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using EPR.Payment.Service.Common.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

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
        public DbSet<DataModels.FeeSummary> FeeSummaries => Set<DataModels.FeeSummary>();
        public DbSet<DataModels.FileFeeSummaryConnection> FileFeeSummaryConnections => Set<DataModels.FileFeeSummaryConnection>();
        public DbSet<FeeType> FeeTypes => Set<FeeType>();
        public DbSet<PayerType> PayerTypes => Set<PayerType>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}