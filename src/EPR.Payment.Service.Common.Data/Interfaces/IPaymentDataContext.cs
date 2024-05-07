using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using Microsoft.EntityFrameworkCore;

namespace EPR.Payment.Service.Common.Data.Interfaces
{
    public interface IPaymentDataContext
    {
        DbSet<PaymentStatus> PaymentStatus { get; } 
        DbSet<DataModels.Payment> Payment { get; } 
        DbSet<AccreditationFees> AccreditationFees { get; }
    }
}
