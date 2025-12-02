using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceCommonEnums = EPR.Payment.Service.Common.Enums;
using Group = EPR.Payment.Service.Common.Enums.Group;
using SubGroup = EPR.Payment.Service.Common.Enums.SubGroup;
using Regulator = EPR.Payment.Service.Common.Enums.Regulator;

namespace EPR.Payment.Service.Common.Data.SeedData
{
    [ExcludeFromCodeCoverage]
    public static class RegistrationFeesDataSeed
    {
        private static readonly DateTime effectiveFromDate = DateTime.ParseExact("01/01/2024 00:00:00", "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
        private static readonly DateTime effectiveToDate = DateTime.ParseExact("02/12/2025 12:00:00", "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);

        private static readonly DateTime effectiveFromDateForYear2024 = DateTime.ParseExact("01/09/2024 00:00:00", "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
        private static readonly DateTime effectiveToDateForYear9999 = DateTime.ParseExact("31/08/9999 23:59:59", "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);

        private static readonly List<Regulator> Regulators = [Regulator.England, Regulator.Scotland, Regulator.Wales, Regulator.NorthernIreland];

        private static readonly (int GroupId, int SubGroupId, int Amount)[] Fees2026 =
        {
            ((int)Group.ComplianceScheme, (int)SubGroup.Large, 180300),
            ((int)Group.ComplianceScheme, (int)SubGroup.Small, 69600),
            ((int)Group.ComplianceScheme, (int)SubGroup.Registration, 1470200),
            ((int)Group.ComplianceScheme, (int)SubGroup.Online, 288500),
            ((int)Group.ComplianceScheme, (int)SubGroup.LateFee, 38600),
            ((int)Group.ComplianceSchemeSubsidiaries, (int)SubGroup.UpTo20, 69000),
            ((int)Group.ComplianceSchemeSubsidiaries, (int)SubGroup.MoreThan20, 17200),
            ((int)Group.ComplianceSchemeResubmission, (int)SubGroup.ReSubmitting, 51200),
            ((int)Group.ProducerType, (int)SubGroup.Large, 284200),
            ((int)Group.ProducerType, (int)SubGroup.Small, 130300),
            ((int)Group.ProducerType, (int)SubGroup.Online, 288500),
            ((int)Group.ProducerType, (int)SubGroup.LateFee, 38600),
            ((int)Group.ProducerSubsidiaries, (int)SubGroup.UpTo20, 69000),
            ((int)Group.ProducerSubsidiaries, (int)SubGroup.MoreThan20, 17200),
            ((int)Group.ProducerResubmission, (int)SubGroup.ReSubmitting, 80700)
        };

        private static readonly (int GroupId, int SubGroupId, int Amount)[] FeesTestOne =
        {
            ((int)Group.ComplianceScheme, (int)SubGroup.Large, 180301),
            ((int)Group.ComplianceScheme, (int)SubGroup.Small, 69601),
            ((int)Group.ComplianceScheme, (int)SubGroup.Registration, 1470201),
            ((int)Group.ComplianceScheme, (int)SubGroup.Online, 288501),
            ((int)Group.ComplianceScheme, (int)SubGroup.LateFee, 38601),
            ((int)Group.ComplianceSchemeSubsidiaries, (int)SubGroup.UpTo20, 69001),
            ((int)Group.ComplianceSchemeSubsidiaries, (int)SubGroup.MoreThan20, 17201),
            ((int)Group.ComplianceSchemeResubmission, (int)SubGroup.ReSubmitting, 51201),
            ((int)Group.ProducerType, (int)SubGroup.Large, 284201),
            ((int)Group.ProducerType, (int)SubGroup.Small, 130301),
            ((int)Group.ProducerType, (int)SubGroup.Online, 288501),
            ((int)Group.ProducerType, (int)SubGroup.LateFee, 38601),
            ((int)Group.ProducerSubsidiaries, (int)SubGroup.UpTo20, 69001),
            ((int)Group.ProducerSubsidiaries, (int)SubGroup.MoreThan20, 17201),
            ((int)Group.ProducerResubmission, (int)SubGroup.ReSubmitting, 80701)
        };

        private static readonly (int GroupId, int SubGroupId, int Amount)[] FeesTestTwo =
        {
            ((int)Group.ComplianceScheme, (int)SubGroup.Large, 180302),
            ((int)Group.ComplianceScheme, (int)SubGroup.Small, 69602),
            ((int)Group.ComplianceScheme, (int)SubGroup.Registration, 1470202),
            ((int)Group.ComplianceScheme, (int)SubGroup.Online, 288502),
            ((int)Group.ComplianceScheme, (int)SubGroup.LateFee, 38602),
            ((int)Group.ComplianceSchemeSubsidiaries, (int)SubGroup.UpTo20, 69002),
            ((int)Group.ComplianceSchemeSubsidiaries, (int)SubGroup.MoreThan20, 17202),
            ((int)Group.ComplianceSchemeResubmission, (int)SubGroup.ReSubmitting, 51202),
            ((int)Group.ProducerType, (int)SubGroup.Large, 284202),
            ((int)Group.ProducerType, (int)SubGroup.Small, 130302),
            ((int)Group.ProducerType, (int)SubGroup.Online, 288502),
            ((int)Group.ProducerType, (int)SubGroup.LateFee, 38602),
            ((int)Group.ProducerSubsidiaries, (int)SubGroup.UpTo20, 69002),
            ((int)Group.ProducerSubsidiaries, (int)SubGroup.MoreThan20, 17202),
            ((int)Group.ProducerResubmission, (int)SubGroup.ReSubmitting, 80702)
        };

        private static readonly (int GroupId, int SubGroupId, int Amount)[] FeesTestThree =
        {
            ((int)Group.ComplianceScheme, (int)SubGroup.Large, 180303),
            ((int)Group.ComplianceScheme, (int)SubGroup.Small, 69603),
            ((int)Group.ComplianceScheme, (int)SubGroup.Registration, 1470203),
            ((int)Group.ComplianceScheme, (int)SubGroup.Online, 288503),
            ((int)Group.ComplianceScheme, (int)SubGroup.LateFee, 38603),
            ((int)Group.ComplianceSchemeSubsidiaries, (int)SubGroup.UpTo20, 69003),
            ((int)Group.ComplianceSchemeSubsidiaries, (int)SubGroup.MoreThan20, 17203),
            ((int)Group.ComplianceSchemeResubmission, (int)SubGroup.ReSubmitting, 51203),
            ((int)Group.ProducerType, (int)SubGroup.Large, 284203),
            ((int)Group.ProducerType, (int)SubGroup.Small, 130303),
            ((int)Group.ProducerType, (int)SubGroup.Online, 288503),
            ((int)Group.ProducerType, (int)SubGroup.LateFee, 38603),
            ((int)Group.ProducerSubsidiaries, (int)SubGroup.UpTo20, 69003),
            ((int)Group.ProducerSubsidiaries, (int)SubGroup.MoreThan20, 17203),
            ((int)Group.ProducerResubmission, (int)SubGroup.ReSubmitting, 80703)
        };

        private static readonly (int GroupId, int SubGroupId, int Amount)[] FeesTestFour =
        {
            ((int)Group.ComplianceScheme, (int)SubGroup.Large, 180304),
            ((int)Group.ComplianceScheme, (int)SubGroup.Small, 69604),
            ((int)Group.ComplianceScheme, (int)SubGroup.Registration, 1470204),
            ((int)Group.ComplianceScheme, (int)SubGroup.Online, 288504),
            ((int)Group.ComplianceScheme, (int)SubGroup.LateFee, 38604),
            ((int)Group.ComplianceSchemeSubsidiaries, (int)SubGroup.UpTo20, 69004),
            ((int)Group.ComplianceSchemeSubsidiaries, (int)SubGroup.MoreThan20, 17204),
            ((int)Group.ComplianceSchemeResubmission, (int)SubGroup.ReSubmitting, 51204),
            ((int)Group.ProducerType, (int)SubGroup.Large, 284204),
            ((int)Group.ProducerType, (int)SubGroup.Small, 130304),
            ((int)Group.ProducerType, (int)SubGroup.Online, 288504),
            ((int)Group.ProducerType, (int)SubGroup.LateFee, 38604),
            ((int)Group.ProducerSubsidiaries, (int)SubGroup.UpTo20, 69004),
            ((int)Group.ProducerSubsidiaries, (int)SubGroup.MoreThan20, 17204),
            ((int)Group.ProducerResubmission, (int)SubGroup.ReSubmitting, 80704)
        };

        public static void SeedRegistrationFees(EntityTypeBuilder<RegistrationFees> builder)
        {
            // Historical data for 2024 and 2025
            SeedRegistrationFeesExistingData(builder);
            SeedExporterRegistrationFeesData(builder);
            SeedReprocessorRegistrationFeesData(builder);

            var seedIndex = 0;
            var newRegistrationFees = new List<RegistrationFees>();

            // 2026 fees
            seedIndex = 26000000;
            AddProducerFeesForPeriod(
                newRegistrationFees,
                Fees2026,
                DateTime.SpecifyKind(new DateTime(2026, 1, 1, 0, 0, 0), DateTimeKind.Utc),
                DateTime.SpecifyKind(new DateTime(2026, 12, 31, 23, 59, 59), DateTimeKind.Utc),
                ref seedIndex);

            // test fees
            seedIndex = 99000000;

            AddProducerFeesForPeriod(
                newRegistrationFees,
                FeesTestOne,
                DateTime.SpecifyKind(new DateTime(2025, 12, 2, 8, 0, 0), DateTimeKind.Utc),
                DateTime.SpecifyKind(new DateTime(2025, 12, 2, 10, 0, 0), DateTimeKind.Utc),
                ref seedIndex);

            AddProducerFeesForPeriod(
                newRegistrationFees,
                FeesTestTwo,
                DateTime.SpecifyKind(new DateTime(2025, 12, 2, 10, 0, 0), DateTimeKind.Utc),
                DateTime.SpecifyKind(new DateTime(2025, 12, 2, 12, 0, 0), DateTimeKind.Utc),
                ref seedIndex);

            AddProducerFeesForPeriod(
                newRegistrationFees,
                FeesTestThree,
                DateTime.SpecifyKind(new DateTime(2025, 12, 2, 14, 0, 0), DateTimeKind.Utc),
                DateTime.SpecifyKind(new DateTime(2025, 12, 2, 16, 0, 0), DateTimeKind.Utc),
                ref seedIndex);

            AddProducerFeesForPeriod(
                newRegistrationFees,
                FeesTestFour,
                DateTime.SpecifyKind(new DateTime(2025, 12, 2, 16, 0, 0), DateTimeKind.Utc),
                DateTime.SpecifyKind(new DateTime(2025, 12, 2, 18, 0, 0), DateTimeKind.Utc),
                ref seedIndex);

            AddProducerFeesForPeriod(
                newRegistrationFees,
                FeesTestOne,
                DateTime.SpecifyKind(new DateTime(2025, 12, 3, 8, 0, 0), DateTimeKind.Utc),
                DateTime.SpecifyKind(new DateTime(2025, 12, 3, 10, 0, 0), DateTimeKind.Utc),
                ref seedIndex);

            AddProducerFeesForPeriod(
                newRegistrationFees,
                FeesTestTwo,
                DateTime.SpecifyKind(new DateTime(2025, 12, 3, 10, 0, 0), DateTimeKind.Utc),
                DateTime.SpecifyKind(new DateTime(2025, 12, 3, 12, 0, 0), DateTimeKind.Utc),
                ref seedIndex);

            AddProducerFeesForPeriod(
                newRegistrationFees,
                FeesTestThree,
                DateTime.SpecifyKind(new DateTime(2025, 12, 3, 14, 0, 0), DateTimeKind.Utc),
                DateTime.SpecifyKind(new DateTime(2025, 12, 3, 16, 0, 0), DateTimeKind.Utc),
                ref seedIndex);

            AddProducerFeesForPeriod(
                newRegistrationFees,
                FeesTestFour,
                DateTime.SpecifyKind(new DateTime(2025, 12, 3, 16, 0, 0), DateTimeKind.Utc),
                DateTime.SpecifyKind(new DateTime(2025, 12, 3, 18, 0, 0), DateTimeKind.Utc),
                ref seedIndex);

            AddProducerFeesForPeriod(
                newRegistrationFees,
                FeesTestOne,
                DateTime.SpecifyKind(new DateTime(2025, 12, 4, 8, 0, 0), DateTimeKind.Utc),
                DateTime.SpecifyKind(new DateTime(2025, 12, 4, 10, 0, 0), DateTimeKind.Utc),
                ref seedIndex);

            AddProducerFeesForPeriod(
                newRegistrationFees,
                FeesTestTwo,
                DateTime.SpecifyKind(new DateTime(2025, 12, 4, 10, 0, 0), DateTimeKind.Utc),
                DateTime.SpecifyKind(new DateTime(2025, 12, 4, 12, 0, 0), DateTimeKind.Utc),
                ref seedIndex);

            AddProducerFeesForPeriod(
                newRegistrationFees,
                FeesTestThree,
                DateTime.SpecifyKind(new DateTime(2025, 12, 4, 14, 0, 0), DateTimeKind.Utc),
                DateTime.SpecifyKind(new DateTime(2025, 12, 4, 16, 0, 0), DateTimeKind.Utc),
                ref seedIndex);

            AddProducerFeesForPeriod(
                newRegistrationFees,
                FeesTestFour,
                DateTime.SpecifyKind(new DateTime(2025, 12, 4, 16, 0, 0), DateTimeKind.Utc),
                DateTime.SpecifyKind(new DateTime(2025, 12, 4, 18, 0, 0), DateTimeKind.Utc),
                ref seedIndex);


            AddProducerFeesForPeriod(
                newRegistrationFees,
                FeesTestOne,
                DateTime.SpecifyKind(new DateTime(2025, 12, 5, 8, 0, 0), DateTimeKind.Utc),
                DateTime.SpecifyKind(new DateTime(2025, 12, 5, 10, 0, 0), DateTimeKind.Utc),
                ref seedIndex);

            AddProducerFeesForPeriod(
                newRegistrationFees,
                FeesTestTwo,
                DateTime.SpecifyKind(new DateTime(2025, 12, 5, 10, 0, 0), DateTimeKind.Utc),
                DateTime.SpecifyKind(new DateTime(2025, 12, 5, 12, 0, 0), DateTimeKind.Utc),
                ref seedIndex);

            AddProducerFeesForPeriod(
                newRegistrationFees,
                FeesTestThree,
                DateTime.SpecifyKind(new DateTime(2025, 12, 5, 14, 0, 0), DateTimeKind.Utc),
                DateTime.SpecifyKind(new DateTime(2025, 12, 5, 16, 0, 0), DateTimeKind.Utc),
                ref seedIndex);

            AddProducerFeesForPeriod(
                newRegistrationFees,
                FeesTestFour,
                DateTime.SpecifyKind(new DateTime(2025, 12, 5, 16, 0, 0), DateTimeKind.Utc),
                DateTime.SpecifyKind(new DateTime(2025, 12, 5, 18, 0, 0), DateTimeKind.Utc),
                ref seedIndex);

            builder.HasData(newRegistrationFees);
        }

        private static void AddProducerFeesForPeriod(
            List<RegistrationFees> target,
            (int GroupId, int SubGroupId, int Amount)[] definitions,
            DateTime effectiveFrom,
            DateTime effectiveTo,
            ref int index)
        {
            foreach (var def in definitions)
            {
                foreach (Regulator regulator in Regulators)
                {
                    target.Add(new RegistrationFees
                    {
                        Id = index++,
                        GroupId = def.GroupId,
                        SubGroupId = def.SubGroupId,
                        RegulatorId = (int)regulator,
                        Amount = def.Amount,
                        EffectiveFrom = effectiveFrom,
                        EffectiveTo = effectiveTo
                    });
                }
            }
        }

        private static void SeedRegistrationFeesExistingData(EntityTypeBuilder<RegistrationFees> builder)
        {
            builder.HasData(
                new RegistrationFees { Id = 1, GroupId = 1, SubGroupId = 1, RegulatorId = 1, Amount = 262000, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 2, GroupId = 1, SubGroupId = 1, RegulatorId = 2, Amount = 262000, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 3, GroupId = 1, SubGroupId = 1, RegulatorId = 3, Amount = 262000, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 4, GroupId = 1, SubGroupId = 1, RegulatorId = 4, Amount = 262000, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 5, GroupId = 1, SubGroupId = 2, RegulatorId = 1, Amount = 121600, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 6, GroupId = 1, SubGroupId = 2, RegulatorId = 2, Amount = 121600, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 7, GroupId = 1, SubGroupId = 2, RegulatorId = 3, Amount = 121600, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 8, GroupId = 1, SubGroupId = 2, RegulatorId = 4, Amount = 121600, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 9, GroupId = 1, SubGroupId = 4, RegulatorId = 1, Amount = 257900, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 10, GroupId = 1, SubGroupId = 4, RegulatorId = 2, Amount = 257900, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 11, GroupId = 1, SubGroupId = 4, RegulatorId = 3, Amount = 257900, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 12, GroupId = 1, SubGroupId = 4, RegulatorId = 4, Amount = 257900, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 13, GroupId = 2, SubGroupId = 1, RegulatorId = 1, Amount = 168500, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 14, GroupId = 2, SubGroupId = 1, RegulatorId = 2, Amount = 168500, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 15, GroupId = 2, SubGroupId = 1, RegulatorId = 3, Amount = 168500, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 16, GroupId = 2, SubGroupId = 1, RegulatorId = 4, Amount = 168500, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 17, GroupId = 2, SubGroupId = 2, RegulatorId = 1, Amount = 63100, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 18, GroupId = 2, SubGroupId = 2, RegulatorId = 2, Amount = 63100, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 19, GroupId = 2, SubGroupId = 2, RegulatorId = 3, Amount = 63100, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 20, GroupId = 2, SubGroupId = 2, RegulatorId = 4, Amount = 63100, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 21, GroupId = 2, SubGroupId = 3, RegulatorId = 1, Amount = 1380400, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 22, GroupId = 2, SubGroupId = 3, RegulatorId = 2, Amount = 1380400, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 23, GroupId = 2, SubGroupId = 3, RegulatorId = 3, Amount = 1380400, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 24, GroupId = 2, SubGroupId = 3, RegulatorId = 4, Amount = 1380400, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 25, GroupId = 2, SubGroupId = 4, RegulatorId = 1, Amount = 257900, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 26, GroupId = 2, SubGroupId = 4, RegulatorId = 2, Amount = 257900, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 27, GroupId = 2, SubGroupId = 4, RegulatorId = 3, Amount = 257900, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 28, GroupId = 2, SubGroupId = 4, RegulatorId = 4, Amount = 257900, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 29, GroupId = 3, SubGroupId = 5, RegulatorId = 1, Amount = 55800, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 30, GroupId = 3, SubGroupId = 5, RegulatorId = 2, Amount = 55800, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 31, GroupId = 3, SubGroupId = 5, RegulatorId = 3, Amount = 55800, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 32, GroupId = 3, SubGroupId = 5, RegulatorId = 4, Amount = 55800, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 33, GroupId = 3, SubGroupId = 6, RegulatorId = 1, Amount = 14000, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 34, GroupId = 3, SubGroupId = 6, RegulatorId = 2, Amount = 14000, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 35, GroupId = 3, SubGroupId = 6, RegulatorId = 3, Amount = 14000, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 36, GroupId = 3, SubGroupId = 6, RegulatorId = 4, Amount = 14000, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 37, GroupId = 4, SubGroupId = 5, RegulatorId = 1, Amount = 55800, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 38, GroupId = 4, SubGroupId = 5, RegulatorId = 2, Amount = 55800, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 39, GroupId = 4, SubGroupId = 5, RegulatorId = 3, Amount = 55800, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 40, GroupId = 4, SubGroupId = 5, RegulatorId = 4, Amount = 55800, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 41, GroupId = 4, SubGroupId = 6, RegulatorId = 1, Amount = 14000, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 42, GroupId = 4, SubGroupId = 6, RegulatorId = 2, Amount = 14000, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 43, GroupId = 4, SubGroupId = 6, RegulatorId = 3, Amount = 14000, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 44, GroupId = 4, SubGroupId = 6, RegulatorId = 4, Amount = 14000, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 45, GroupId = 5, SubGroupId = 7, RegulatorId = 1, Amount = 71400, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 46, GroupId = 5, SubGroupId = 7, RegulatorId = 2, Amount = 71400, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 47, GroupId = 5, SubGroupId = 7, RegulatorId = 3, Amount = 71400, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 48, GroupId = 5, SubGroupId = 7, RegulatorId = 4, Amount = 71400, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 49, GroupId = 1, SubGroupId = 8, RegulatorId = 1, Amount = 33200, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 50, GroupId = 1, SubGroupId = 8, RegulatorId = 2, Amount = 33200, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 51, GroupId = 1, SubGroupId = 8, RegulatorId = 3, Amount = 33200, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 52, GroupId = 1, SubGroupId = 8, RegulatorId = 4, Amount = 33200, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 53, GroupId = 2, SubGroupId = 8, RegulatorId = 1, Amount = 33200, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 54, GroupId = 2, SubGroupId = 8, RegulatorId = 2, Amount = 33200, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 55, GroupId = 2, SubGroupId = 8, RegulatorId = 3, Amount = 33200, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 56, GroupId = 2, SubGroupId = 8, RegulatorId = 4, Amount = 33200, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 57, GroupId = 6, SubGroupId = 7, RegulatorId = 1, Amount = 43000, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 58, GroupId = 6, SubGroupId = 7, RegulatorId = 2, Amount = 43000, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 59, GroupId = 6, SubGroupId = 7, RegulatorId = 3, Amount = 43000, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                new RegistrationFees { Id = 60, GroupId = 6, SubGroupId = 7, RegulatorId = 4, Amount = 43000, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate }
                );
        }

        private static void SeedExporterRegistrationFeesData(EntityTypeBuilder<RegistrationFees> builder)
        {
            builder.HasData(
                new RegistrationFees { Id = 61, GroupId = (int)ServiceCommonEnums.Group.Exporters, SubGroupId = (int)ServiceCommonEnums.SubGroup.Aluminium, RegulatorId = 1, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024, EffectiveTo = effectiveToDateForYear9999 },
                new RegistrationFees { Id = 62, GroupId = (int)ServiceCommonEnums.Group.Exporters, SubGroupId = (int)ServiceCommonEnums.SubGroup.Glass, RegulatorId = 1, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024, EffectiveTo = effectiveToDateForYear9999 },
                new RegistrationFees { Id = 63, GroupId = (int)ServiceCommonEnums.Group.Exporters, SubGroupId = (int)ServiceCommonEnums.SubGroup.PaperOrBoardOrFibreBasedCompositeMaterial, RegulatorId = 1, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024, EffectiveTo = effectiveToDateForYear9999 },
                new RegistrationFees { Id = 64, GroupId = (int)ServiceCommonEnums.Group.Exporters, SubGroupId = (int)ServiceCommonEnums.SubGroup.Plastic, RegulatorId = 1, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024, EffectiveTo = effectiveToDateForYear9999 },
                new RegistrationFees { Id = 65, GroupId = (int)ServiceCommonEnums.Group.Exporters, SubGroupId = (int)ServiceCommonEnums.SubGroup.Steel, RegulatorId = 1, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024, EffectiveTo = effectiveToDateForYear9999 },
                new RegistrationFees { Id = 66, GroupId = (int)ServiceCommonEnums.Group.Exporters, SubGroupId = (int)ServiceCommonEnums.SubGroup.Wood, RegulatorId = 1, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024, EffectiveTo = effectiveToDateForYear9999 },
                new RegistrationFees { Id = 67, GroupId = (int)ServiceCommonEnums.Group.Exporters, SubGroupId = (int)ServiceCommonEnums.SubGroup.Aluminium, RegulatorId = 2, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024, EffectiveTo = effectiveToDateForYear9999 },
                new RegistrationFees { Id = 68, GroupId = (int)ServiceCommonEnums.Group.Exporters, SubGroupId = (int)ServiceCommonEnums.SubGroup.Glass, RegulatorId = 2, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024, EffectiveTo = effectiveToDateForYear9999 },
                new RegistrationFees { Id = 69, GroupId = (int)ServiceCommonEnums.Group.Exporters, SubGroupId = (int)ServiceCommonEnums.SubGroup.PaperOrBoardOrFibreBasedCompositeMaterial, RegulatorId = 2, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024, EffectiveTo = effectiveToDateForYear9999 },
                new RegistrationFees { Id = 70, GroupId = (int)ServiceCommonEnums.Group.Exporters, SubGroupId = (int)ServiceCommonEnums.SubGroup.Plastic, RegulatorId = 2, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024, EffectiveTo = effectiveToDateForYear9999 },
                new RegistrationFees { Id = 71, GroupId = (int)ServiceCommonEnums.Group.Exporters, SubGroupId = (int)ServiceCommonEnums.SubGroup.Steel, RegulatorId = 2, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024, EffectiveTo = effectiveToDateForYear9999 },
                new RegistrationFees { Id = 72, GroupId = (int)ServiceCommonEnums.Group.Exporters, SubGroupId = (int)ServiceCommonEnums.SubGroup.Wood, RegulatorId = 2, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024, EffectiveTo = effectiveToDateForYear9999 },
                new RegistrationFees { Id = 73, GroupId = (int)ServiceCommonEnums.Group.Exporters, SubGroupId = (int)ServiceCommonEnums.SubGroup.Aluminium, RegulatorId = 3, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024, EffectiveTo = effectiveToDateForYear9999 },
                new RegistrationFees { Id = 74, GroupId = (int)ServiceCommonEnums.Group.Exporters, SubGroupId = (int)ServiceCommonEnums.SubGroup.Glass, RegulatorId = 3, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024, EffectiveTo = effectiveToDateForYear9999 },
                new RegistrationFees { Id = 75, GroupId = (int)ServiceCommonEnums.Group.Exporters, SubGroupId = (int)ServiceCommonEnums.SubGroup.PaperOrBoardOrFibreBasedCompositeMaterial, RegulatorId = 3, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024, EffectiveTo = effectiveToDateForYear9999 },
                new RegistrationFees { Id = 76, GroupId = (int)ServiceCommonEnums.Group.Exporters, SubGroupId = (int)ServiceCommonEnums.SubGroup.Plastic, RegulatorId = 3, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024, EffectiveTo = effectiveToDateForYear9999 },
                new RegistrationFees { Id = 77, GroupId = (int)ServiceCommonEnums.Group.Exporters, SubGroupId = (int)ServiceCommonEnums.SubGroup.Steel, RegulatorId = 3, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024, EffectiveTo = effectiveToDateForYear9999 },
                new RegistrationFees { Id = 78, GroupId = (int)ServiceCommonEnums.Group.Exporters, SubGroupId = (int)ServiceCommonEnums.SubGroup.Wood, RegulatorId = 3, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024, EffectiveTo = effectiveToDateForYear9999 },
                new RegistrationFees { Id = 79, GroupId = (int)ServiceCommonEnums.Group.Exporters, SubGroupId = (int)ServiceCommonEnums.SubGroup.Aluminium, RegulatorId = 4, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024, EffectiveTo = effectiveToDateForYear9999 },
                new RegistrationFees { Id = 80, GroupId = (int)ServiceCommonEnums.Group.Exporters, SubGroupId = (int)ServiceCommonEnums.SubGroup.Glass, RegulatorId = 4, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024, EffectiveTo = effectiveToDateForYear9999 },
                new RegistrationFees { Id = 81, GroupId = (int)ServiceCommonEnums.Group.Exporters, SubGroupId = (int)ServiceCommonEnums.SubGroup.PaperOrBoardOrFibreBasedCompositeMaterial, RegulatorId = 4, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024, EffectiveTo = effectiveToDateForYear9999 },
                new RegistrationFees { Id = 82, GroupId = (int)ServiceCommonEnums.Group.Exporters, SubGroupId = (int)ServiceCommonEnums.SubGroup.Plastic, RegulatorId = 4, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024, EffectiveTo = effectiveToDateForYear9999 },
                new RegistrationFees { Id = 83, GroupId = (int)ServiceCommonEnums.Group.Exporters, SubGroupId = (int)ServiceCommonEnums.SubGroup.Steel, RegulatorId = 4, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024, EffectiveTo = effectiveToDateForYear9999 },
                new RegistrationFees { Id = 84, GroupId = (int)ServiceCommonEnums.Group.Exporters, SubGroupId = (int)ServiceCommonEnums.SubGroup.Wood, RegulatorId = 4, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024, EffectiveTo = effectiveToDateForYear9999 }
                );
        }

        private static void SeedReprocessorRegistrationFeesData(EntityTypeBuilder<RegistrationFees> builder)
        {
            builder.HasData(
                new RegistrationFees { Id = 85, GroupId = (int)ServiceCommonEnums.Group.Reprocessors, SubGroupId = (int)ServiceCommonEnums.SubGroup.Aluminium, RegulatorId = 1, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024, EffectiveTo = effectiveToDateForYear9999 },
                new RegistrationFees { Id = 86, GroupId = (int)ServiceCommonEnums.Group.Reprocessors, SubGroupId = (int)ServiceCommonEnums.SubGroup.Glass, RegulatorId = 1, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024, EffectiveTo = effectiveToDateForYear9999 },
                new RegistrationFees { Id = 87, GroupId = (int)ServiceCommonEnums.Group.Reprocessors, SubGroupId = (int)ServiceCommonEnums.SubGroup.PaperOrBoardOrFibreBasedCompositeMaterial, RegulatorId = 1, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024, EffectiveTo = effectiveToDateForYear9999 },
                new RegistrationFees { Id = 88, GroupId = (int)ServiceCommonEnums.Group.Reprocessors, SubGroupId = (int)ServiceCommonEnums.SubGroup.Plastic, RegulatorId = 1, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024, EffectiveTo = effectiveToDateForYear9999 },
                new RegistrationFees { Id = 89, GroupId = (int)ServiceCommonEnums.Group.Reprocessors, SubGroupId = (int)ServiceCommonEnums.SubGroup.Steel, RegulatorId = 1, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024, EffectiveTo = effectiveToDateForYear9999 },
                new RegistrationFees { Id = 90, GroupId = (int)ServiceCommonEnums.Group.Reprocessors, SubGroupId = (int)ServiceCommonEnums.SubGroup.Wood, RegulatorId = 1, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024, EffectiveTo = effectiveToDateForYear9999 },
                new RegistrationFees { Id = 91, GroupId = (int)ServiceCommonEnums.Group.Reprocessors, SubGroupId = (int)ServiceCommonEnums.SubGroup.Aluminium, RegulatorId = 2, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024, EffectiveTo = effectiveToDateForYear9999 },
                new RegistrationFees { Id = 92, GroupId = (int)ServiceCommonEnums.Group.Reprocessors, SubGroupId = (int)ServiceCommonEnums.SubGroup.Glass, RegulatorId = 2, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024, EffectiveTo = effectiveToDateForYear9999 },
                new RegistrationFees { Id = 93, GroupId = (int)ServiceCommonEnums.Group.Reprocessors, SubGroupId = (int)ServiceCommonEnums.SubGroup.PaperOrBoardOrFibreBasedCompositeMaterial, RegulatorId = 2, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024, EffectiveTo = effectiveToDateForYear9999 },
                new RegistrationFees { Id = 94, GroupId = (int)ServiceCommonEnums.Group.Reprocessors, SubGroupId = (int)ServiceCommonEnums.SubGroup.Plastic, RegulatorId = 2, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024, EffectiveTo = effectiveToDateForYear9999 },
                new RegistrationFees { Id = 95, GroupId = (int)ServiceCommonEnums.Group.Reprocessors, SubGroupId = (int)ServiceCommonEnums.SubGroup.Steel, RegulatorId = 2, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024, EffectiveTo = effectiveToDateForYear9999 },
                new RegistrationFees { Id = 96, GroupId = (int)ServiceCommonEnums.Group.Reprocessors, SubGroupId = (int)ServiceCommonEnums.SubGroup.Wood, RegulatorId = 2, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024, EffectiveTo = effectiveToDateForYear9999 },
                new RegistrationFees { Id = 97, GroupId = (int)ServiceCommonEnums.Group.Reprocessors, SubGroupId = (int)ServiceCommonEnums.SubGroup.Aluminium, RegulatorId = 3, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024, EffectiveTo = effectiveToDateForYear9999 },
                new RegistrationFees { Id = 98, GroupId = (int)ServiceCommonEnums.Group.Reprocessors, SubGroupId = (int)ServiceCommonEnums.SubGroup.Glass, RegulatorId = 3, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024, EffectiveTo = effectiveToDateForYear9999 },
                new RegistrationFees { Id = 99, GroupId = (int)ServiceCommonEnums.Group.Reprocessors, SubGroupId = (int)ServiceCommonEnums.SubGroup.PaperOrBoardOrFibreBasedCompositeMaterial, RegulatorId = 3, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024, EffectiveTo = effectiveToDateForYear9999 },
                new RegistrationFees { Id = 100, GroupId = (int)ServiceCommonEnums.Group.Reprocessors, SubGroupId = (int)ServiceCommonEnums.SubGroup.Plastic, RegulatorId = 3, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024, EffectiveTo = effectiveToDateForYear9999 },
                new RegistrationFees { Id = 101, GroupId = (int)ServiceCommonEnums.Group.Reprocessors, SubGroupId = (int)ServiceCommonEnums.SubGroup.Steel, RegulatorId = 3, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024, EffectiveTo = effectiveToDateForYear9999 },
                new RegistrationFees { Id = 102, GroupId = (int)ServiceCommonEnums.Group.Reprocessors, SubGroupId = (int)ServiceCommonEnums.SubGroup.Wood, RegulatorId = 3, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024, EffectiveTo = effectiveToDateForYear9999 },
                new RegistrationFees { Id = 103, GroupId = (int)ServiceCommonEnums.Group.Reprocessors, SubGroupId = (int)ServiceCommonEnums.SubGroup.Aluminium, RegulatorId = 4, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024, EffectiveTo = effectiveToDateForYear9999 },
                new RegistrationFees { Id = 104, GroupId = (int)ServiceCommonEnums.Group.Reprocessors, SubGroupId = (int)ServiceCommonEnums.SubGroup.Glass, RegulatorId = 4, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024, EffectiveTo = effectiveToDateForYear9999 },
                new RegistrationFees { Id = 105, GroupId = (int)ServiceCommonEnums.Group.Reprocessors, SubGroupId = (int)ServiceCommonEnums.SubGroup.PaperOrBoardOrFibreBasedCompositeMaterial, RegulatorId = 4, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024, EffectiveTo = effectiveToDateForYear9999 },
                new RegistrationFees { Id = 106, GroupId = (int)ServiceCommonEnums.Group.Reprocessors, SubGroupId = (int)ServiceCommonEnums.SubGroup.Plastic, RegulatorId = 4, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024, EffectiveTo = effectiveToDateForYear9999 },
                new RegistrationFees { Id = 107, GroupId = (int)ServiceCommonEnums.Group.Reprocessors, SubGroupId = (int)ServiceCommonEnums.SubGroup.Steel, RegulatorId = 4, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024, EffectiveTo = effectiveToDateForYear9999 },
                new RegistrationFees { Id = 108, GroupId = (int)ServiceCommonEnums.Group.Reprocessors, SubGroupId = (int)ServiceCommonEnums.SubGroup.Wood, RegulatorId = 4, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024, EffectiveTo = effectiveToDateForYear9999 }
                );
        }
    }
}
