using EPR.Payment.Service.Common.Data.DataModels;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using Microsoft.EntityFrameworkCore;

namespace EPR.Payment.Service.Common.Data.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<DataModels.Payment> Payment { get; }
        DbSet<PaymentStatus> PaymentStatus { get; }
        DbSet<DataModels.OnlinePayment> OnlinePayment { get; }
        DbSet<DataModels.OfflinePayment> OfflinePayment { get; }
        DbSet<Group> Group { get; }
        DbSet<SubGroup> SubGroup { get; }
        DbSet<Regulator> Regulator { get; }
        DbSet<RegistrationFees> RegistrationFees { get; }
        DbSet<AccreditationFee> AccreditationFees { get; }

        DbSet<FeeSummary> FeeSummaries { get; }
        DbSet<FileFeeSummaryConnection> FileFeeSummaryConnections { get; }
        DbSet<FeeType> FeeTypes { get; }
        DbSet<PayerType> PayerTypes { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
