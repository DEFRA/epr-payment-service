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
               new PaymentStatus { Id = Enums.Status.Initiated, Status = Enums.Status.Initiated.ToString() },
               new PaymentStatus { Id = Enums.Status.InProgress, Status = Enums.Status.InProgress.ToString() },
               new PaymentStatus { Id = Enums.Status.Success, Status = Enums.Status.Success.ToString() },
               new PaymentStatus { Id = Enums.Status.Failed, Status = Enums.Status.Failed.ToString() },
               new PaymentStatus { Id = Enums.Status.Error, Status = Enums.Status.Error.ToString() },
               new PaymentStatus { Id = Enums.Status.UserCancelled, Status = Enums.Status.UserCancelled.ToString() }
               );
        }
    }
}
