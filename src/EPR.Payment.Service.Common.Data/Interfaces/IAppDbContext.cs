using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using Microsoft.EntityFrameworkCore;

namespace EPR.Payment.Service.Common.Data.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<PaymentStatus> PaymentStatus { get; } 
        DbSet<DataModels.Payment> Payment { get; }
        DbSet<AdditionalRegistrationFees> AdditionalRegistrationFees { get; }
        DbSet<ComplianceSchemeRegistrationFees> ComplianceSchemeRegistrationFees { get; }
        DbSet<ProducerRegistrationFees> ProducerRegistrationFees { get; }
        DbSet<SubsidiariesRegistrationFees> SubsidiariesRegistrationFees { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
