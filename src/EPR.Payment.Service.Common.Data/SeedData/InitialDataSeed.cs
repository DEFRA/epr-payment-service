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
            DateTime effectiveFromDate = DateTime.ParseExact("01/01/2025", "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime effectiveToDate = DateTime.ParseExact("31/12/2025", "dd/MM/yyyy", CultureInfo.InvariantCulture);

            modelBuilder.Entity<PaymentStatus>().HasData(
                new PaymentStatus { Id = Enums.Status.Initiated, Status = "Initiated" },
                new PaymentStatus { Id = Enums.Status.InProgress, Status = "InProgress" },
                new PaymentStatus { Id = Enums.Status.Success, Status = "Success" },
                new PaymentStatus { Id = Enums.Status.Failed, Status = "Failed" },
                new PaymentStatus { Id = Enums.Status.Error, Status = "Error" },
                new PaymentStatus { Id = Enums.Status.UserCancelled, Status = "UserCancelled" }
                );

            modelBuilder.Entity<Group>().HasData(
               new Group { Id = 1, Type = "ProducerType", Description = "Producer Type" },
               new Group { Id = 2, Type = "ComplianceScheme", Description = "Compliance Scheme" },
               new Group { Id = 3, Type = "ProducerSubsidiaries", Description = "Producer Subsidiaries" },
               new Group { Id = 4, Type = "ComplianceSchemeSubsidiaries", Description = "Compliance Scheme Subsidiaries" }
               );

            modelBuilder.Entity<SubGroup>().HasData(
               new SubGroup { Id = 1, Type = "Large", Description = "Large producer" },
               new SubGroup { Id = 2, Type = "Small", Description = "Small producer" },
               new SubGroup { Id = 3, Type = "Registration", Description = "Registration" },
               new SubGroup { Id = 4, Type = "Online", Description = "Online Market" },
               new SubGroup { Id = 5, Type = "UpTo20", Description = "Up to 20" },
               new SubGroup { Id = 6, Type = "MoreThan20", Description = "More than 20" }
               );

            modelBuilder.Entity<Regulator>().HasData(
               new Regulator { Id = 1, Type = "GB-ENG", Description = "England" },
               new Regulator { Id = 2, Type = "GB-SCT", Description = "Scotland" },
               new Regulator { Id = 3, Type = "GB-WLS", Description = "Wales" },
               new Regulator { Id = 4, Type = "GB-NIR", Description = "Northern Ireland" }
               );

            modelBuilder.Entity<RegistrationFees>().HasData(
                new RegistrationFees { Id = 1, GroupId = 1, SubGroupId = 1, RegulatorId = 1, Amount = 2620, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 2, GroupId = 1, SubGroupId = 1, RegulatorId = 2, Amount = 2620, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 3, GroupId = 1, SubGroupId = 1, RegulatorId = 3, Amount = 2620, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 4, GroupId = 1, SubGroupId = 1, RegulatorId = 4, Amount = 2620, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 5, GroupId = 1, SubGroupId = 2, RegulatorId = 1, Amount = 1216, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 6, GroupId = 1, SubGroupId = 2, RegulatorId = 2, Amount = 1216, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 7, GroupId = 1, SubGroupId = 2, RegulatorId = 3, Amount = 1216, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 8, GroupId = 1, SubGroupId = 2, RegulatorId = 4, Amount = 1216, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 9, GroupId = 2, SubGroupId = 1, RegulatorId = 1, Amount = 1658, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 10, GroupId = 2, SubGroupId = 1, RegulatorId = 2, Amount = 1658, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 11, GroupId = 2, SubGroupId = 1, RegulatorId = 3, Amount = 1658, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 12, GroupId = 2, SubGroupId = 1, RegulatorId = 4, Amount = 1658, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 13, GroupId = 2, SubGroupId = 2, RegulatorId = 1, Amount = 631, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 14, GroupId = 2, SubGroupId = 2, RegulatorId = 2, Amount = 631, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 15, GroupId = 2, SubGroupId = 2, RegulatorId = 3, Amount = 631, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 16, GroupId = 2, SubGroupId = 2, RegulatorId = 4, Amount = 631, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 17, GroupId = 2, SubGroupId = 3, RegulatorId = 1, Amount = 13804, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 18, GroupId = 2, SubGroupId = 3, RegulatorId = 2, Amount = 13804, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 19, GroupId = 2, SubGroupId = 3, RegulatorId = 3, Amount = 13804, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 20, GroupId = 2, SubGroupId = 3, RegulatorId = 4, Amount = 13804, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 21, GroupId = 2, SubGroupId = 4, RegulatorId = 1, Amount = 2579, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 22, GroupId = 2, SubGroupId = 4, RegulatorId = 2, Amount = 2579, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 23, GroupId = 2, SubGroupId = 4, RegulatorId = 3, Amount = 2579, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 24, GroupId = 2, SubGroupId = 4, RegulatorId = 4, Amount = 2579, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 25, GroupId = 3, SubGroupId = 5, RegulatorId = 1, Amount = 558, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 26, GroupId = 3, SubGroupId = 5, RegulatorId = 2, Amount = 558, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 27, GroupId = 3, SubGroupId = 5, RegulatorId = 3, Amount = 558, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 28, GroupId = 3, SubGroupId = 5, RegulatorId = 4, Amount = 558, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 29, GroupId = 3, SubGroupId = 6, RegulatorId = 1, Amount = 140, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 30, GroupId = 3, SubGroupId = 6, RegulatorId = 2, Amount = 140, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 31, GroupId = 3, SubGroupId = 6, RegulatorId = 3, Amount = 140, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 32, GroupId = 3, SubGroupId = 6, RegulatorId = 4, Amount = 140, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 33, GroupId = 4, SubGroupId = 5, RegulatorId = 1, Amount = 558, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 34, GroupId = 4, SubGroupId = 5, RegulatorId = 2, Amount = 558, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 35, GroupId = 4, SubGroupId = 5, RegulatorId = 3, Amount = 558, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 36, GroupId = 4, SubGroupId = 5, RegulatorId = 4, Amount = 558, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 37, GroupId = 4, SubGroupId = 6, RegulatorId = 1, Amount = 140, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 38, GroupId = 4, SubGroupId = 6, RegulatorId = 2, Amount = 140, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 39, GroupId = 4, SubGroupId = 6, RegulatorId = 3, Amount = 140, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 40, GroupId = 4, SubGroupId = 6, RegulatorId = 4, Amount = 140, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate }

                );
        }

    }
}
