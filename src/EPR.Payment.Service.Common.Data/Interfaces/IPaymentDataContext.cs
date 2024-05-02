using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using Microsoft.EntityFrameworkCore;

namespace EPR.Payment.Service.Common.Data.Interfaces
{
    public interface IPaymentDataContext
    {
        DbSet<InternalStatus> InternalStatus { get; } 
        DbSet<DataModels.Payment> Payment { get; } 
        DbSet<Fees> Fees { get; }
    }
}
