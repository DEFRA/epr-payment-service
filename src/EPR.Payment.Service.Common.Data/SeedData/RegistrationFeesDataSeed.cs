﻿using System.Diagnostics.CodeAnalysis;
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
                            new RegistrationFees { Id = 85, GroupId = (int)Enums.Group.Exporters, SubGroupId = (int)Enums.SubGroup.Aluminium, RegulatorId = 1, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                            new RegistrationFees { Id = 86, GroupId = (int)Enums.Group.Exporters, SubGroupId = (int)Enums.SubGroup.Glass, RegulatorId = 1, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                            new RegistrationFees { Id = 87, GroupId = (int)Enums.Group.Exporters, SubGroupId = (int)Enums.SubGroup.PaperOrBoardOrFibreBasedCompositeMaterial, RegulatorId = 1, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                            new RegistrationFees { Id = 88, GroupId = (int)Enums.Group.Exporters, SubGroupId = (int)Enums.SubGroup.Plastic, RegulatorId = 1, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                            new RegistrationFees { Id = 89, GroupId = (int)Enums.Group.Exporters, SubGroupId = (int)Enums.SubGroup.Steel, RegulatorId = 1, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                            new RegistrationFees { Id = 90, GroupId = (int)Enums.Group.Exporters, SubGroupId = (int)Enums.SubGroup.Wood, RegulatorId = 1, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                            new RegistrationFees { Id = 91, GroupId = (int)Enums.Group.Exporters, SubGroupId = (int)Enums.SubGroup.Aluminium, RegulatorId = 2, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                            new RegistrationFees { Id = 92, GroupId = (int)Enums.Group.Exporters, SubGroupId = (int)Enums.SubGroup.Glass, RegulatorId = 2, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                            new RegistrationFees { Id = 93, GroupId = (int)Enums.Group.Exporters, SubGroupId = (int)Enums.SubGroup.PaperOrBoardOrFibreBasedCompositeMaterial, RegulatorId = 2, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                            new RegistrationFees { Id = 94, GroupId = (int)Enums.Group.Exporters, SubGroupId = (int)Enums.SubGroup.Plastic, RegulatorId = 2, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                            new RegistrationFees { Id = 95, GroupId = (int)Enums.Group.Exporters, SubGroupId = (int)Enums.SubGroup.Steel, RegulatorId = 2, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                            new RegistrationFees { Id = 96, GroupId = (int)Enums.Group.Exporters, SubGroupId = (int)Enums.SubGroup.Wood, RegulatorId = 2, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                            new RegistrationFees { Id = 97, GroupId = (int)Enums.Group.Exporters, SubGroupId = (int)Enums.SubGroup.Aluminium, RegulatorId = 3, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                            new RegistrationFees { Id = 98, GroupId = (int)Enums.Group.Exporters, SubGroupId = (int)Enums.SubGroup.Glass, RegulatorId = 3, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                            new RegistrationFees { Id = 99, GroupId = (int)Enums.Group.Exporters, SubGroupId = (int)Enums.SubGroup.PaperOrBoardOrFibreBasedCompositeMaterial, RegulatorId = 3, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                            new RegistrationFees { Id = 100, GroupId = (int)Enums.Group.Exporters, SubGroupId = (int)Enums.SubGroup.Plastic, RegulatorId = 3, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                            new RegistrationFees { Id = 101, GroupId = (int)Enums.Group.Exporters, SubGroupId = (int)Enums.SubGroup.Steel, RegulatorId = 3, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                            new RegistrationFees { Id = 102, GroupId = (int)Enums.Group.Exporters, SubGroupId = (int)Enums.SubGroup.Wood, RegulatorId = 3, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                            new RegistrationFees { Id = 103, GroupId = (int)Enums.Group.Exporters, SubGroupId = (int)Enums.SubGroup.Aluminium, RegulatorId = 4, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                            new RegistrationFees { Id = 104, GroupId = (int)Enums.Group.Exporters, SubGroupId = (int)Enums.SubGroup.Glass, RegulatorId = 4, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                            new RegistrationFees { Id = 105, GroupId = (int)Enums.Group.Exporters, SubGroupId = (int)Enums.SubGroup.PaperOrBoardOrFibreBasedCompositeMaterial, RegulatorId = 4, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                            new RegistrationFees { Id = 106, GroupId = (int)Enums.Group.Exporters, SubGroupId = (int)Enums.SubGroup.Plastic, RegulatorId = 4, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                            new RegistrationFees { Id = 107, GroupId = (int)Enums.Group.Exporters, SubGroupId = (int)Enums.SubGroup.Steel, RegulatorId = 4, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 },
                            new RegistrationFees { Id = 108, GroupId = (int)Enums.Group.Exporters, SubGroupId = (int)Enums.SubGroup.Wood, RegulatorId = 4, Amount = 2921, EffectiveFrom = effectiveFromDateForYear2024_25, EffectiveTo = effectiveToDateForYear2024_25 }
                            );
        }
    }
}
