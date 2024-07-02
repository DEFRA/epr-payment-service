using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using Microsoft.EntityFrameworkCore;

namespace EPR.Payment.Service.Common.Data.Interfaces
{
    public interface IFeesPaymentDataContext
    {
        DbSet<PaymentStatus> PaymentStatus { get; }
        DbSet<DataModels.Payment> Payment { get; }
        DbSet<AdditionalFees> AdditionalFees { get; }
        DbSet<ComplianceShemeRegitrationFees> ComplianceShemeRegitrationFees { get; }
        DbSet<ProducerRegitrationFees> ProducerRegitrationFees { get; }
        DbSet<Subsidiaries> Subsidiaries { get; }
    }
}
