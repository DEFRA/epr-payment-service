using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPR.Payment.Service.Common.Data.SeedData
{
    [ExcludeFromCodeCoverage]
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
        }

        private static void SeedSeedRegistrationFeesExistingData(EntityTypeBuilder<RegistrationFees> builder)
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

        private static void SeedReprocessorRegistrationFeesData(EntityTypeBuilder<RegistrationFees> builder)
        {
            builder.HasData(
                            new RegistrationFees { Id = 109, GroupId = 8, SubGroupId = 9, RegulatorId = 1, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                            new RegistrationFees { Id = 110, GroupId = 8, SubGroupId = 10, RegulatorId = 1, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                            new RegistrationFees { Id = 111, GroupId = 8, SubGroupId = 11, RegulatorId = 1, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                            new RegistrationFees { Id = 112, GroupId = 8, SubGroupId = 12, RegulatorId = 1, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                            new RegistrationFees { Id = 113, GroupId = 8, SubGroupId = 13, RegulatorId = 1, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                            new RegistrationFees { Id = 114, GroupId = 8, SubGroupId = 14, RegulatorId = 1, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                            new RegistrationFees { Id = 115, GroupId = 8, SubGroupId = 9, RegulatorId = 1, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2025_26, EffectiveTo = effectiveToDateForYear2025_26 },
                            new RegistrationFees { Id = 116, GroupId = 8, SubGroupId = 10, RegulatorId = 1, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2025_26, EffectiveTo = effectiveToDateForYear2025_26 },
                            new RegistrationFees { Id = 117, GroupId = 8, SubGroupId = 11, RegulatorId = 1, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2025_26, EffectiveTo = effectiveToDateForYear2025_26 },
                            new RegistrationFees { Id = 118, GroupId = 8, SubGroupId = 12, RegulatorId = 1, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2025_26, EffectiveTo = effectiveToDateForYear2025_26 },
                            new RegistrationFees { Id = 119, GroupId = 8, SubGroupId = 13, RegulatorId = 1, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2025_26, EffectiveTo = effectiveToDateForYear2025_26 },
                            new RegistrationFees { Id = 120, GroupId = 8, SubGroupId = 14, RegulatorId = 1, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2025_26, EffectiveTo = effectiveToDateForYear2025_26 },
                            new RegistrationFees { Id = 121, GroupId = 8, SubGroupId = 9, RegulatorId = 2, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                            new RegistrationFees { Id = 122, GroupId = 8, SubGroupId = 10, RegulatorId = 2, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                            new RegistrationFees { Id = 123, GroupId = 8, SubGroupId = 11, RegulatorId = 2, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                            new RegistrationFees { Id = 124, GroupId = 8, SubGroupId = 12, RegulatorId = 2, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                            new RegistrationFees { Id = 125, GroupId = 8, SubGroupId = 13, RegulatorId = 2, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                            new RegistrationFees { Id = 126, GroupId = 8, SubGroupId = 14, RegulatorId = 2, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                            new RegistrationFees { Id = 127, GroupId = 8, SubGroupId = 9, RegulatorId = 2, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2025_26, EffectiveTo = effectiveToDateForYear2025_26 },
                            new RegistrationFees { Id = 128, GroupId = 8, SubGroupId = 10, RegulatorId = 2, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2025_26, EffectiveTo = effectiveToDateForYear2025_26 },
                            new RegistrationFees { Id = 129, GroupId = 8, SubGroupId = 11, RegulatorId = 2, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2025_26, EffectiveTo = effectiveToDateForYear2025_26 },
                            new RegistrationFees { Id = 130, GroupId = 8, SubGroupId = 12, RegulatorId = 2, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2025_26, EffectiveTo = effectiveToDateForYear2025_26 },
                            new RegistrationFees { Id = 131, GroupId = 8, SubGroupId = 13, RegulatorId = 2, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2025_26, EffectiveTo = effectiveToDateForYear2025_26 },
                            new RegistrationFees { Id = 132, GroupId = 8, SubGroupId = 14, RegulatorId = 2, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2025_26, EffectiveTo = effectiveToDateForYear2025_26 },
                            new RegistrationFees { Id = 133, GroupId = 8, SubGroupId = 9, RegulatorId = 3, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                            new RegistrationFees { Id = 134, GroupId = 8, SubGroupId = 10, RegulatorId = 3, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                            new RegistrationFees { Id = 135, GroupId = 8, SubGroupId = 11, RegulatorId = 3, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                            new RegistrationFees { Id = 136, GroupId = 8, SubGroupId = 12, RegulatorId = 3, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                            new RegistrationFees { Id = 137, GroupId = 8, SubGroupId = 13, RegulatorId = 3, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                            new RegistrationFees { Id = 138, GroupId = 8, SubGroupId = 14, RegulatorId = 3, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                            new RegistrationFees { Id = 139, GroupId = 8, SubGroupId = 9, RegulatorId = 3, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2025_26, EffectiveTo = effectiveToDateForYear2025_26 },
                            new RegistrationFees { Id = 140, GroupId = 8, SubGroupId = 10, RegulatorId = 3, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2025_26, EffectiveTo = effectiveToDateForYear2025_26 },
                            new RegistrationFees { Id = 141, GroupId = 8, SubGroupId = 11, RegulatorId = 3, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2025_26, EffectiveTo = effectiveToDateForYear2025_26 },
                            new RegistrationFees { Id = 142, GroupId = 8, SubGroupId = 12, RegulatorId = 3, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2025_26, EffectiveTo = effectiveToDateForYear2025_26 },
                            new RegistrationFees { Id = 143, GroupId = 8, SubGroupId = 13, RegulatorId = 3, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2025_26, EffectiveTo = effectiveToDateForYear2025_26 },
                            new RegistrationFees { Id = 144, GroupId = 8, SubGroupId = 14, RegulatorId = 3, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2025_26, EffectiveTo = effectiveToDateForYear2025_26 },
                            new RegistrationFees { Id = 145, GroupId = 8, SubGroupId = 9, RegulatorId = 4, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                            new RegistrationFees { Id = 146, GroupId = 8, SubGroupId = 10, RegulatorId = 4, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                            new RegistrationFees { Id = 147, GroupId = 8, SubGroupId = 11, RegulatorId = 4, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                            new RegistrationFees { Id = 148, GroupId = 8, SubGroupId = 12, RegulatorId = 4, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                            new RegistrationFees { Id = 149, GroupId = 8, SubGroupId = 13, RegulatorId = 4, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                            new RegistrationFees { Id = 150, GroupId = 8, SubGroupId = 14, RegulatorId = 4, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                            new RegistrationFees { Id = 151, GroupId = 8, SubGroupId = 9, RegulatorId = 4, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2025_26, EffectiveTo = effectiveToDateForYear2025_26 },
                            new RegistrationFees { Id = 152, GroupId = 8, SubGroupId = 10, RegulatorId = 4, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2025_26, EffectiveTo = effectiveToDateForYear2025_26 },
                            new RegistrationFees { Id = 153, GroupId = 8, SubGroupId = 11, RegulatorId = 4, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2025_26, EffectiveTo = effectiveToDateForYear2025_26 },
                            new RegistrationFees { Id = 154, GroupId = 8, SubGroupId = 12, RegulatorId = 4, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2025_26, EffectiveTo = effectiveToDateForYear2025_26 },
                            new RegistrationFees { Id = 155, GroupId = 8, SubGroupId = 13, RegulatorId = 4, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2025_26, EffectiveTo = effectiveToDateForYear2025_26 },
                            new RegistrationFees { Id = 156, GroupId = 8, SubGroupId = 14, RegulatorId = 4, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2025_26, EffectiveTo = effectiveToDateForYear2025_26 }
                            );
        }
    }
}
