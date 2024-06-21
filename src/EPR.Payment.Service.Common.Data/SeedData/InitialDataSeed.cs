using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace EPR.Payment.Service.Common.Data.SeedData
{
    [ExcludeFromCodeCoverage]
    public class InitialDataSeed
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            DateTime effectiveFromDate = DateTime.ParseExact("01/01/2024", "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime effectiveToDate = DateTime.ParseExact("01/01/2025", "dd/MM/yyyy", CultureInfo.InvariantCulture);

            modelBuilder.Entity<PaymentStatus>().HasData(
                new PaymentStatus { Id = Enums.Status.Initiated, Status = "Initiated" },
                new PaymentStatus { Id = Enums.Status.InProgress, Status = "InProgress" },
                new PaymentStatus { Id = Enums.Status.Success, Status = "Success" },
                new PaymentStatus { Id = Enums.Status.Failed, Status = "Failed" },
                new PaymentStatus { Id = Enums.Status.Error, Status = "Error" },
                new PaymentStatus { Id = Enums.Status.UserCancelled, Status = "UserCancelled" }
                );

            modelBuilder.Entity<ProducerRegitrationFees>().HasData(
                new ProducerRegitrationFees { Id = 1, ProducerType = "L", Description = "Large producer",  Country = "GB-ENG", Amount = 2620, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new ProducerRegitrationFees { Id = 2, ProducerType = "L", Description = "Large producer", Country = "GB-SCT", Amount = 2620, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new ProducerRegitrationFees { Id = 3, ProducerType = "L", Description = "Large producer", Country = "GB-WLS", Amount = 2620, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new ProducerRegitrationFees { Id = 4, ProducerType = "L", Description = "Large producer", Country = "GB-NIR", Amount = 2620, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new ProducerRegitrationFees { Id = 5, ProducerType = "S", Description = "Small producer", Country = "GB-ENG", Amount = 1216, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new ProducerRegitrationFees { Id = 6, ProducerType = "S", Description = "Small producer", Country = "GB-SCT", Amount = 1216, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new ProducerRegitrationFees { Id = 7, ProducerType = "S", Description = "Small producer", Country = "GB-WLS", Amount = 1216, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new ProducerRegitrationFees { Id = 8, ProducerType = "S", Description = "Small producer", Country = "GB-NIR", Amount = 1216, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate }
                );

            modelBuilder.Entity<Subsidiaries>().HasData(
                new Subsidiaries { Id = 1, MinSub = 1, MaxSub = 20, Description = "Up to 20", Country = "GB-ENG", Amount = 558, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new Subsidiaries { Id = 2, MinSub = 1, MaxSub = 20, Description = "Up to 20", Country = "GB-SCT", Amount = 558, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new Subsidiaries { Id = 3, MinSub = 1, MaxSub = 20, Description = "Up to 20", Country = "GB-WLS", Amount = 558, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new Subsidiaries { Id = 4, MinSub = 1, MaxSub = 20, Description = "Up to 20", Country = "GB-NIR", Amount = 558, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new Subsidiaries { Id = 5, MinSub = 21, MaxSub = 100, Description = "More then 20", Country = "GB-ENG", Amount = 140, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new Subsidiaries { Id = 6, MinSub = 21, MaxSub = 100, Description = "More then 20", Country = "GB-SCT", Amount = 140, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new Subsidiaries { Id = 7, MinSub = 21, MaxSub = 100, Description = "More then 20", Country = "GB-WLS", Amount = 140, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new Subsidiaries { Id = 8, MinSub = 21, MaxSub = 100, Description = "More then 20", Country = "GB-NIR", Amount = 140, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate }
                );

            modelBuilder.Entity<AdditionalFees>().HasData(
                new AdditionalFees { Id = 1, FeesSubType = "Resub", Description = "Resubmission", Country = "GB-ENG", Amount = 714, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new AdditionalFees { Id = 2, FeesSubType = "Resub", Description = "Resubmission", Country = "GB-SCT", Amount = 714, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new AdditionalFees { Id = 3, FeesSubType = "Resub", Description = "Resubmission", Country = "GB-WLS", Amount = 714, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new AdditionalFees { Id = 4, FeesSubType = "Resub", Description = "Resubmission", Country = "GB-NIR", Amount = 714, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new AdditionalFees { Id = 5, FeesSubType = "Late", Description = "Late", Country = "GB-ENG", Amount = 332, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new AdditionalFees { Id = 6, FeesSubType = "Late", Description = "Late", Country = "GB-SCT", Amount = 332, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new AdditionalFees { Id = 7, FeesSubType = "Late", Description = "Late", Country = "GB-WLS", Amount = 332, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new AdditionalFees { Id = 8, FeesSubType = "Late", Description = "Late", Country = "GB-NIR", Amount = 332, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate }
                );

            modelBuilder.Entity<ComplianceShemeRegitrationFees>().HasData(
                new ComplianceShemeRegitrationFees { Id = 1, FeesType = "Reg", Description = "Registration", Country = "GB-ENG", Amount = 13804, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new ComplianceShemeRegitrationFees { Id = 2, FeesType = "Reg", Description = "Registration", Country = "GB-SCT", Amount = 13804, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new ComplianceShemeRegitrationFees { Id = 3, FeesType = "Reg", Description = "Registration", Country = "GB-WLS", Amount = 13804, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new ComplianceShemeRegitrationFees { Id = 4, FeesType = "Reg", Description = "Registration", Country = "GB-NIR", Amount = 13804, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new ComplianceShemeRegitrationFees { Id = 5, FeesType = "L", Description = "Large Producer", Country = "GB-ENG", Amount = 1658, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new ComplianceShemeRegitrationFees { Id = 6, FeesType = "L", Description = "Large Producer", Country = "GB-SCT", Amount = 1658, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new ComplianceShemeRegitrationFees { Id = 7, FeesType = "L", Description = "Large Producer", Country = "GB-WLS", Amount = 1658, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new ComplianceShemeRegitrationFees { Id = 8, FeesType = "L", Description = "Large Producer", Country = "GB-NIR", Amount = 1658, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new ComplianceShemeRegitrationFees { Id = 9, FeesType = "S", Description = "Small Producer", Country = "GB-ENG", Amount = 631, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new ComplianceShemeRegitrationFees { Id = 10, FeesType = "S", Description = "Small Producer", Country = "GB-SCT", Amount = 631, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new ComplianceShemeRegitrationFees { Id = 11, FeesType = "S", Description = "Small Producer", Country = "GB-WLS", Amount = 631, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new ComplianceShemeRegitrationFees { Id = 12, FeesType = "S", Description = "Small Producer", Country = "GB-NIR", Amount = 631, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new ComplianceShemeRegitrationFees { Id = 13, FeesType = "On", Description = "Online Market", Country = "GB-ENG", Amount = 2579, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new ComplianceShemeRegitrationFees { Id = 14, FeesType = "On", Description = "Online Market", Country = "GB-SCT", Amount = 2579, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new ComplianceShemeRegitrationFees { Id = 15, FeesType = "On", Description = "Online Market", Country = "GB-WLS", Amount = 2579, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new ComplianceShemeRegitrationFees { Id = 16, FeesType = "On", Description = "Online Market", Country = "GB-NIR", Amount = 2579, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate }
                );

            modelBuilder.Entity<InternalError>().HasData(
                new InternalError { InternalErrorCode = "A", GovPayErrorCode = "P0030", GovPayErrorMessage = "Cancelled" },
                new InternalError { InternalErrorCode = "B", GovPayErrorCode = "P0020", GovPayErrorMessage = "Expired" },
                new InternalError { InternalErrorCode = "C", GovPayErrorCode = "P0010", GovPayErrorMessage = "Rejected" }
                );
        }

    }
}
