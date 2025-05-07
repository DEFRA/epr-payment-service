using System.Globalization;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPR.Payment.Service.Common.Data.SeedData
{
    public static class RegistrationFeesDataSeed
    {
        private static readonly DateTime effectiveFromDate = DateTime.ParseExact("01/01/2024 00:00:00", "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
        private static readonly DateTime effectiveToDate = DateTime.ParseExact("31/12/2025 23:59:59", "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);

        private static readonly DateTime effectiveFromDateForYear2024_25 = DateTime.ParseExact("01/09/2024 00:00:00", "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
        private static readonly DateTime effectiveToDateForYear2024_25 = DateTime.ParseExact("31/08/2025 23:59:59", "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);

        private static readonly DateTime effectiveFromDateForYear2025_26 = DateTime.ParseExact("01/09/2025 00:00:00", "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
        private static readonly DateTime effectiveToDateForYear2025_26 = DateTime.ParseExact("31/08/2026 23:59:59", "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);

        public static void SeedRegistrationFees(EntityTypeBuilder<RegistrationFees> builder)
        {
            SeedSeedRegistrationFeesExistingData(builder);
            SeedReprocessorRegistrationFeesData(builder);
            SeedExporterRegistrationFeesData(builder);
        }

        private static void SeedSeedRegistrationFeesExistingData(EntityTypeBuilder<RegistrationFees> builder)
        {
            builder.HasData(
                            new RegistrationFees { Id = 1, GroupId = 7, SubGroupId = 1, RegulatorId = 1, Amount = 262000, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 2, GroupId = 7, SubGroupId = 1, RegulatorId = 2, Amount = 262000, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 3, GroupId = 7, SubGroupId = 1, RegulatorId = 3, Amount = 262000, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 4, GroupId = 7, SubGroupId = 1, RegulatorId = 4, Amount = 262000, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 5, GroupId = 7, SubGroupId = 2, RegulatorId = 1, Amount = 121600, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 6, GroupId = 7, SubGroupId = 2, RegulatorId = 2, Amount = 121600, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 7, GroupId = 7, SubGroupId = 2, RegulatorId = 3, Amount = 121600, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 8, GroupId = 7, SubGroupId = 2, RegulatorId = 4, Amount = 121600, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 9, GroupId = 7, SubGroupId = 4, RegulatorId = 1, Amount = 257900, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 10, GroupId = 7, SubGroupId = 4, RegulatorId = 2, Amount = 257900, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 11, GroupId = 7, SubGroupId = 4, RegulatorId = 3, Amount = 257900, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 12, GroupId = 7, SubGroupId = 4, RegulatorId = 4, Amount = 257900, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 13, GroupId = 7, SubGroupId = 1, RegulatorId = 1, Amount = 168500, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 14, GroupId = 7, SubGroupId = 1, RegulatorId = 2, Amount = 168500, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 15, GroupId = 7, SubGroupId = 1, RegulatorId = 3, Amount = 168500, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 16, GroupId = 7, SubGroupId = 1, RegulatorId = 4, Amount = 168500, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 17, GroupId = 7, SubGroupId = 2, RegulatorId = 1, Amount = 63100, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 18, GroupId = 7, SubGroupId = 2, RegulatorId = 2, Amount = 63100, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 19, GroupId = 7, SubGroupId = 2, RegulatorId = 3, Amount = 63100, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 20, GroupId = 7, SubGroupId = 2, RegulatorId = 4, Amount = 63100, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 21, GroupId = 7, SubGroupId = 3, RegulatorId = 1, Amount = 1380400, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 22, GroupId = 7, SubGroupId = 3, RegulatorId = 2, Amount = 1380400, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 23, GroupId = 7, SubGroupId = 3, RegulatorId = 3, Amount = 1380400, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 24, GroupId = 7, SubGroupId = 3, RegulatorId = 4, Amount = 1380400, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 25, GroupId = 7, SubGroupId = 4, RegulatorId = 1, Amount = 257900, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 26, GroupId = 7, SubGroupId = 4, RegulatorId = 2, Amount = 257900, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 27, GroupId = 7, SubGroupId = 4, RegulatorId = 3, Amount = 257900, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 28, GroupId = 7, SubGroupId = 4, RegulatorId = 4, Amount = 257900, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 29, GroupId = 7, SubGroupId = 5, RegulatorId = 1, Amount = 55800, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 30, GroupId = 7, SubGroupId = 5, RegulatorId = 2, Amount = 55800, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 31, GroupId = 7, SubGroupId = 5, RegulatorId = 3, Amount = 55800, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 32, GroupId = 7, SubGroupId = 5, RegulatorId = 4, Amount = 55800, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 33, GroupId = 7, SubGroupId = 6, RegulatorId = 1, Amount = 14000, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 34, GroupId = 7, SubGroupId = 6, RegulatorId = 2, Amount = 14000, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 35, GroupId = 7, SubGroupId = 6, RegulatorId = 3, Amount = 14000, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 36, GroupId = 7, SubGroupId = 6, RegulatorId = 4, Amount = 14000, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 37, GroupId = 7, SubGroupId = 5, RegulatorId = 1, Amount = 55800, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 38, GroupId = 7, SubGroupId = 5, RegulatorId = 2, Amount = 55800, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 39, GroupId = 7, SubGroupId = 5, RegulatorId = 3, Amount = 55800, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 40, GroupId = 7, SubGroupId = 5, RegulatorId = 4, Amount = 55800, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 41, GroupId = 7, SubGroupId = 6, RegulatorId = 1, Amount = 14000, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 42, GroupId = 7, SubGroupId = 6, RegulatorId = 2, Amount = 14000, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 43, GroupId = 7, SubGroupId = 6, RegulatorId = 3, Amount = 14000, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 44, GroupId = 7, SubGroupId = 6, RegulatorId = 4, Amount = 14000, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 45, GroupId = 7, SubGroupId = 7, RegulatorId = 1, Amount = 71400, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 46, GroupId = 7, SubGroupId = 7, RegulatorId = 2, Amount = 71400, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 47, GroupId = 7, SubGroupId = 7, RegulatorId = 3, Amount = 71400, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 48, GroupId = 7, SubGroupId = 7, RegulatorId = 4, Amount = 71400, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 49, GroupId = 7, SubGroupId = 8, RegulatorId = 1, Amount = 33200, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 50, GroupId = 7, SubGroupId = 8, RegulatorId = 2, Amount = 33200, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 51, GroupId = 7, SubGroupId = 8, RegulatorId = 3, Amount = 33200, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 52, GroupId = 7, SubGroupId = 8, RegulatorId = 4, Amount = 33200, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 53, GroupId = 7, SubGroupId = 8, RegulatorId = 1, Amount = 33200, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 54, GroupId = 7, SubGroupId = 8, RegulatorId = 2, Amount = 33200, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 55, GroupId = 7, SubGroupId = 8, RegulatorId = 3, Amount = 33200, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 56, GroupId = 7, SubGroupId = 8, RegulatorId = 4, Amount = 33200, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 57, GroupId = 6, SubGroupId = 7, RegulatorId = 1, Amount = 43000, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 58, GroupId = 6, SubGroupId = 7, RegulatorId = 2, Amount = 43000, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 59, GroupId = 6, SubGroupId = 7, RegulatorId = 3, Amount = 43000, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate },
                            new RegistrationFees { Id = 60, GroupId = 6, SubGroupId = 7, RegulatorId = 4, Amount = 43000, EffectiveFrom = effectiveFromDate, EffectiveTo = effectiveToDate }
                            );
        }

        private static void SeedReprocessorRegistrationFeesData(EntityTypeBuilder<RegistrationFees> builder)
        {

        }

        private static void SeedExporterRegistrationFeesData(EntityTypeBuilder<RegistrationFees> builder)
        {
            builder.HasData(
                             new RegistrationFees { Id = 61, GroupId = 7, SubGroupId = 9, RegulatorId = 1, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                             new RegistrationFees { Id = 62, GroupId = 7, SubGroupId = 10, RegulatorId = 1, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                             new RegistrationFees { Id = 63, GroupId = 7, SubGroupId = 11, RegulatorId = 1, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                             new RegistrationFees { Id = 64, GroupId = 7, SubGroupId = 12, RegulatorId = 1, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                             new RegistrationFees { Id = 65, GroupId = 7, SubGroupId = 13, RegulatorId = 1, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                             new RegistrationFees { Id = 66, GroupId = 7, SubGroupId = 14, RegulatorId = 1, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                             new RegistrationFees { Id = 67, GroupId = 7, SubGroupId = 9, RegulatorId = 1, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2025_26, EffectiveTo = effectiveToDateForYear2025_26 },
                             new RegistrationFees { Id = 68, GroupId = 7, SubGroupId = 10, RegulatorId = 1, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2025_26, EffectiveTo = effectiveToDateForYear2025_26 },
                             new RegistrationFees { Id = 69, GroupId = 7, SubGroupId = 11, RegulatorId = 1, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2025_26, EffectiveTo = effectiveToDateForYear2025_26 },
                             new RegistrationFees { Id = 70, GroupId = 7, SubGroupId = 12, RegulatorId = 1, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2025_26, EffectiveTo = effectiveToDateForYear2025_26 },
                             new RegistrationFees { Id = 71, GroupId = 7, SubGroupId = 13, RegulatorId = 1, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2025_26, EffectiveTo = effectiveToDateForYear2025_26 },
                             new RegistrationFees { Id = 72, GroupId = 7, SubGroupId = 14, RegulatorId = 1, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2025_26, EffectiveTo = effectiveToDateForYear2025_26 },
                             new RegistrationFees { Id = 73, GroupId = 7, SubGroupId = 9, RegulatorId = 2, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                             new RegistrationFees { Id = 74, GroupId = 7, SubGroupId = 10, RegulatorId = 2, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                             new RegistrationFees { Id = 75, GroupId = 7, SubGroupId = 11, RegulatorId = 2, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                             new RegistrationFees { Id = 76, GroupId = 7, SubGroupId = 12, RegulatorId = 2, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                             new RegistrationFees { Id = 77, GroupId = 7, SubGroupId = 13, RegulatorId = 2, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                             new RegistrationFees { Id = 78, GroupId = 7, SubGroupId = 14, RegulatorId = 2, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                             new RegistrationFees { Id = 79, GroupId = 7, SubGroupId = 9, RegulatorId = 2, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2025_26, EffectiveTo = effectiveToDateForYear2025_26 },
                             new RegistrationFees { Id = 80, GroupId = 7, SubGroupId = 10, RegulatorId = 2, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2025_26, EffectiveTo = effectiveToDateForYear2025_26 },
                             new RegistrationFees { Id = 81, GroupId = 7, SubGroupId = 11, RegulatorId = 2, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2025_26, EffectiveTo = effectiveToDateForYear2025_26 },
                             new RegistrationFees { Id = 82, GroupId = 7, SubGroupId = 12, RegulatorId = 2, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2025_26, EffectiveTo = effectiveToDateForYear2025_26 },
                             new RegistrationFees { Id = 83, GroupId = 7, SubGroupId = 13, RegulatorId = 2, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2025_26, EffectiveTo = effectiveToDateForYear2025_26 },
                             new RegistrationFees { Id = 84, GroupId = 7, SubGroupId = 14, RegulatorId = 2, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2025_26, EffectiveTo = effectiveToDateForYear2025_26 },
                             new RegistrationFees { Id = 85, GroupId = 7, SubGroupId = 9, RegulatorId = 3, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                             new RegistrationFees { Id = 86, GroupId = 7, SubGroupId = 10, RegulatorId = 3, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                             new RegistrationFees { Id = 87, GroupId = 7, SubGroupId = 11, RegulatorId = 3, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                             new RegistrationFees { Id = 88, GroupId = 7, SubGroupId = 12, RegulatorId = 3, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                             new RegistrationFees { Id = 89, GroupId = 7, SubGroupId = 13, RegulatorId = 3, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                             new RegistrationFees { Id = 90, GroupId = 7, SubGroupId = 14, RegulatorId = 3, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                             new RegistrationFees { Id = 91, GroupId = 7, SubGroupId = 9, RegulatorId = 3, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2025_26, EffectiveTo = effectiveToDateForYear2025_26 },
                             new RegistrationFees { Id = 92, GroupId = 7, SubGroupId = 10, RegulatorId = 3, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2025_26, EffectiveTo = effectiveToDateForYear2025_26 },
                             new RegistrationFees { Id = 93, GroupId = 7, SubGroupId = 11, RegulatorId = 3, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2025_26, EffectiveTo = effectiveToDateForYear2025_26 },
                             new RegistrationFees { Id = 94, GroupId = 7, SubGroupId = 12, RegulatorId = 3, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2025_26, EffectiveTo = effectiveToDateForYear2025_26 },
                             new RegistrationFees { Id = 95, GroupId = 7, SubGroupId = 13, RegulatorId = 3, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2025_26, EffectiveTo = effectiveToDateForYear2025_26 },
                             new RegistrationFees { Id = 96, GroupId = 7, SubGroupId = 14, RegulatorId = 3, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2025_26, EffectiveTo = effectiveToDateForYear2025_26 },
                             new RegistrationFees { Id = 97, GroupId = 7, SubGroupId = 9, RegulatorId = 4, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                             new RegistrationFees { Id = 98, GroupId = 7, SubGroupId = 10, RegulatorId = 4, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                             new RegistrationFees { Id = 99, GroupId = 7, SubGroupId = 11, RegulatorId = 4, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                             new RegistrationFees { Id = 100, GroupId = 7, SubGroupId = 12, RegulatorId = 4, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                             new RegistrationFees { Id = 101, GroupId = 7, SubGroupId = 13, RegulatorId = 4, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                             new RegistrationFees { Id = 102, GroupId = 7, SubGroupId = 14, RegulatorId = 4, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                             new RegistrationFees { Id = 103, GroupId = 7, SubGroupId = 9, RegulatorId = 4, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2025_26, EffectiveTo = effectiveToDateForYear2025_26 },
                             new RegistrationFees { Id = 104, GroupId = 7, SubGroupId = 10, RegulatorId = 4, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2025_26, EffectiveTo = effectiveToDateForYear2025_26 },
                             new RegistrationFees { Id = 105, GroupId = 7, SubGroupId = 11, RegulatorId = 4, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2025_26, EffectiveTo = effectiveToDateForYear2025_26 },
                             new RegistrationFees { Id = 106, GroupId = 7, SubGroupId = 12, RegulatorId = 4, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2025_26, EffectiveTo = effectiveToDateForYear2025_26 },
                             new RegistrationFees { Id = 107, GroupId = 7, SubGroupId = 13, RegulatorId = 4, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2025_26, EffectiveTo = effectiveToDateForYear2025_26 },
                             new RegistrationFees { Id = 108, GroupId = 7, SubGroupId = 14, RegulatorId = 4, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2025_26, EffectiveTo = effectiveToDateForYear2025_26 }
                             );
        }
    }
}
