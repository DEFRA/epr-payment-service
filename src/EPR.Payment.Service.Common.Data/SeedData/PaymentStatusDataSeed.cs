using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPR.Payment.Service.Common.Data.SeedData
{
    [ExcludeFromCodeCoverage]
    public static class PaymentStatusDataSeed
    {
        public static void SeedPaymentStatusData(EntityTypeBuilder<PaymentStatus> builder)
        {
            builder.HasData(
               new PaymentStatus { Id = Enums.Status.Initiated, Status = "Initiated" },
               new PaymentStatus { Id = Enums.Status.InProgress, Status = "InProgress" },
               new PaymentStatus { Id = Enums.Status.Success, Status = "Success" },
               new PaymentStatus { Id = Enums.Status.Failed, Status = "Failed" },
               new PaymentStatus { Id = Enums.Status.Error, Status = "Error" },
               new PaymentStatus { Id = Enums.Status.UserCancelled, Status = "UserCancelled" }
               );
        }
    }
}
