using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using Microsoft.EntityFrameworkCore;

namespace EPR.Payment.Service.Common.Data.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<PaymentStatus> PaymentStatus { get; } 
        DbSet<DataModels.Payment> Payment { get; }
        DbSet<DataModels.OnlinePayment> OnlinePayment { get; }
        DbSet<Group> Group { get; }
        DbSet<SubGroup> SubGroup { get; }
        DbSet<Regulator> Regulator { get; }
        DbSet<RegistrationFees> RegistrationFees { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
