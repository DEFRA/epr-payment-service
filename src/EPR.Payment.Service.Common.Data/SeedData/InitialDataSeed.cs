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

            modelBuilder.Entity<ProducerRegistrationFees>().HasData(
                new ProducerRegistrationFees { Id = 1, ProducerType = "L", Description = "Large producer",  Regulator = "GB-ENG", Amount = 2620, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new ProducerRegistrationFees { Id = 2, ProducerType = "L", Description = "Large producer", Regulator = "GB-SCT", Amount = 2620, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new ProducerRegistrationFees { Id = 3, ProducerType = "L", Description = "Large producer", Regulator = "GB-WLS", Amount = 2620, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new ProducerRegistrationFees { Id = 4, ProducerType = "L", Description = "Large producer", Regulator = "GB-NIR", Amount = 2620, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new ProducerRegistrationFees { Id = 5, ProducerType = "S", Description = "Small producer", Regulator = "GB-ENG", Amount = 1216, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new ProducerRegistrationFees { Id = 6, ProducerType = "S", Description = "Small producer", Regulator = "GB-SCT", Amount = 1216, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new ProducerRegistrationFees { Id = 7, ProducerType = "S", Description = "Small producer", Regulator = "GB-WLS", Amount = 1216, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new ProducerRegistrationFees { Id = 8, ProducerType = "S", Description = "Small producer", Regulator = "GB-NIR", Amount = 1216, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate }
                );

            modelBuilder.Entity<SubsidiariesRegistrationFees>().HasData(
                new SubsidiariesRegistrationFees { Id = 1, MinNumberOfSubsidiaries = 1, MaxNumberOfSubsidiaries = 20, Description = "Up to 20", Regulator = "GB-ENG", Amount = 558, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new SubsidiariesRegistrationFees { Id = 2, MinNumberOfSubsidiaries = 1, MaxNumberOfSubsidiaries = 20, Description = "Up to 20", Regulator = "GB-SCT", Amount = 558, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new SubsidiariesRegistrationFees { Id = 3, MinNumberOfSubsidiaries = 1, MaxNumberOfSubsidiaries = 20, Description = "Up to 20", Regulator = "GB-WLS", Amount = 558, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new SubsidiariesRegistrationFees { Id = 4, MinNumberOfSubsidiaries = 1, MaxNumberOfSubsidiaries = 20, Description = "Up to 20", Regulator = "GB-NIR", Amount = 558, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new SubsidiariesRegistrationFees { Id = 5, MinNumberOfSubsidiaries = 21, MaxNumberOfSubsidiaries = 100, Description = "More then 20", Regulator = "GB-ENG", Amount = 140, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new SubsidiariesRegistrationFees { Id = 6, MinNumberOfSubsidiaries = 21, MaxNumberOfSubsidiaries = 100, Description = "More then 20", Regulator = "GB-SCT", Amount = 140, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new SubsidiariesRegistrationFees { Id = 7, MinNumberOfSubsidiaries = 21, MaxNumberOfSubsidiaries = 100, Description = "More then 20", Regulator = "GB-WLS", Amount = 140, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new SubsidiariesRegistrationFees { Id = 8, MinNumberOfSubsidiaries = 21, MaxNumberOfSubsidiaries = 100, Description = "More then 20", Regulator = "GB-NIR", Amount = 140, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate }
                );

            modelBuilder.Entity<AdditionalRegistrationFees>().HasData(
                new AdditionalRegistrationFees { Id = 1, FeesSubType = "Resub", Description = "Resubmission", Regulator = "GB-ENG", Amount = 714, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new AdditionalRegistrationFees { Id = 2, FeesSubType = "Resub", Description = "Resubmission", Regulator = "GB-SCT", Amount = 714, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new AdditionalRegistrationFees { Id = 3, FeesSubType = "Resub", Description = "Resubmission", Regulator = "GB-WLS", Amount = 714, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new AdditionalRegistrationFees { Id = 4, FeesSubType = "Resub", Description = "Resubmission", Regulator = "GB-NIR", Amount = 714, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new AdditionalRegistrationFees { Id = 5, FeesSubType = "Late", Description = "Late", Regulator = "GB-ENG", Amount = 332, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new AdditionalRegistrationFees { Id = 6, FeesSubType = "Late", Description = "Late", Regulator = "GB-SCT", Amount = 332, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new AdditionalRegistrationFees { Id = 7, FeesSubType = "Late", Description = "Late", Regulator = "GB-WLS", Amount = 332, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new AdditionalRegistrationFees { Id = 8, FeesSubType = "Late", Description = "Late", Regulator = "GB-NIR", Amount = 332, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate }
                );

            modelBuilder.Entity<ComplianceSchemeRegistrationFees>().HasData(
                new ComplianceSchemeRegistrationFees { Id = 1, FeesType = "Reg", Description = "Registration", Regulator = "GB-ENG", Amount = 13804, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new ComplianceSchemeRegistrationFees { Id = 2, FeesType = "Reg", Description = "Registration", Regulator = "GB-SCT", Amount = 13804, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new ComplianceSchemeRegistrationFees { Id = 3, FeesType = "Reg", Description = "Registration", Regulator = "GB-WLS", Amount = 13804, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new ComplianceSchemeRegistrationFees { Id = 4, FeesType = "Reg", Description = "Registration", Regulator = "GB-NIR", Amount = 13804, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new ComplianceSchemeRegistrationFees { Id = 5, FeesType = "L", Description = "Large Producer", Regulator = "GB-ENG", Amount = 1658, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new ComplianceSchemeRegistrationFees { Id = 6, FeesType = "L", Description = "Large Producer", Regulator = "GB-SCT", Amount = 1658, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new ComplianceSchemeRegistrationFees { Id = 7, FeesType = "L", Description = "Large Producer", Regulator = "GB-WLS", Amount = 1658, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new ComplianceSchemeRegistrationFees { Id = 8, FeesType = "L", Description = "Large Producer", Regulator = "GB-NIR", Amount = 1658, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new ComplianceSchemeRegistrationFees { Id = 9, FeesType = "S", Description = "Small Producer", Regulator = "GB-ENG", Amount = 631, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new ComplianceSchemeRegistrationFees { Id = 10, FeesType = "S", Description = "Small Producer", Regulator = "GB-SCT", Amount = 631, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new ComplianceSchemeRegistrationFees { Id = 11, FeesType = "S", Description = "Small Producer", Regulator = "GB-WLS", Amount = 631, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new ComplianceSchemeRegistrationFees { Id = 12, FeesType = "S", Description = "Small Producer", Regulator = "GB-NIR", Amount = 631, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new ComplianceSchemeRegistrationFees { Id = 13, FeesType = "On", Description = "Online Market", Regulator = "GB-ENG", Amount = 2579, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new ComplianceSchemeRegistrationFees { Id = 14, FeesType = "On", Description = "Online Market", Regulator = "GB-SCT", Amount = 2579, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new ComplianceSchemeRegistrationFees { Id = 15, FeesType = "On", Description = "Online Market", Regulator = "GB-WLS", Amount = 2579, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new ComplianceSchemeRegistrationFees { Id = 16, FeesType = "On", Description = "Online Market", Regulator = "GB-NIR", Amount = 2579, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate }
                );

            modelBuilder.Entity<InternalError>().HasData(
                new InternalError { InternalErrorCode = "A", GovPayErrorCode = "P0030", GovPayErrorMessage = "Cancelled" },
                new InternalError { InternalErrorCode = "B", GovPayErrorCode = "P0020", GovPayErrorMessage = "Expired" },
                new InternalError { InternalErrorCode = "C", GovPayErrorCode = "P0010", GovPayErrorMessage = "Rejected" }
                );
        }

    }
}
