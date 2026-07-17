using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EPR.Payment.Service.Common.Data.SeedData
{
    [ExcludeFromCodeCoverage]
    public static class SubmissionPeriodDataSeed
    {
        // Seeds transcribed from RegistrationPeriodPatterns in
        // epr-packaging-frontend/src/FrontendSchemeRegistration.UI/appsettings.json.
        // Time-of-day matches how IRegistrationPeriodProvider parses the JSON: midnight UTC
        // (see RegistrationPeriodProvider.ParsePatterns — hours/minutes/seconds hardcoded to 0).
        //
        // The 2026+ frontend pattern has FinalRegistrationYear = null, so the provider derives
        // future years at runtime as (currentYear - earliestOpeningDateYearOffset). With OpeningDate
        // YearOffset = -1 and today's year = 2026, the live provider generates 2026 and 2027 rows.
        // We seed the same set (2025 from the fixed pattern; 2026 and 2027 from the 2026+ pattern).
        // Additional years will be added in follow-up seed migrations as the calendar rolls over.
        private static readonly (int Id, string WindowType, int RegistrationYear, DateTime OpeningDate, DateTime DeadlineDate, DateTime ClosingDate)[] Rows =
        {
            // 2025 pattern (fixed to InitialRegistrationYear = FinalRegistrationYear = 2025)
            (1, "Cso",                 2025, new DateTime(2024, 7, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 4, 2, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 1, 0, 0, 0, DateTimeKind.Utc)),
            (2, "Direct",              2025, new DateTime(2024, 7, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 4, 2, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)),

            // 2026+ pattern, materialised for 2026
            (3, "CsoLargeProducer",    2026, new DateTime(2025, 7, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 2, 0, 0, 0, DateTimeKind.Utc), new DateTime(2027, 1, 29, 0, 0, 0, DateTimeKind.Utc)),
            (4, "CsoSmallProducer",    2026, new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 2, 0, 0, 0, DateTimeKind.Utc), new DateTime(2027, 1, 29, 0, 0, 0, DateTimeKind.Utc)),
            (5, "DirectLargeProducer", 2026, new DateTime(2025, 7, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 2, 0, 0, 0, DateTimeKind.Utc), new DateTime(2027, 1, 1, 0, 0, 0, DateTimeKind.Utc)),
            (6, "DirectSmallProducer", 2026, new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 2, 0, 0, 0, DateTimeKind.Utc), new DateTime(2027, 1, 1, 0, 0, 0, DateTimeKind.Utc)),

            // 2026+ pattern, materialised for 2027
            (7, "CsoLargeProducer",    2027, new DateTime(2026, 7, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 2, 0, 0, 0, DateTimeKind.Utc), new DateTime(2028, 1, 29, 0, 0, 0, DateTimeKind.Utc)),
            (8, "CsoSmallProducer",    2027, new DateTime(2027, 1, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2027, 4, 2, 0, 0, 0, DateTimeKind.Utc), new DateTime(2028, 1, 29, 0, 0, 0, DateTimeKind.Utc)),
            (9, "DirectLargeProducer", 2027, new DateTime(2026, 7, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 10, 2, 0, 0, 0, DateTimeKind.Utc), new DateTime(2028, 1, 1, 0, 0, 0, DateTimeKind.Utc)),
            (10, "DirectSmallProducer", 2027, new DateTime(2027, 1, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2027, 4, 2, 0, 0, 0, DateTimeKind.Utc), new DateTime(2028, 1, 1, 0, 0, 0, DateTimeKind.Utc)),
        };

        public static void SeedSubmissionPeriods(EntityTypeBuilder<SubmissionPeriod> builder)
        {
            var seedData = Rows.Select(r => new SubmissionPeriod
            {
                Id = r.Id,
                WindowType = r.WindowType,
                RegistrationYear = r.RegistrationYear,
                OpeningDate = r.OpeningDate,
                DeadlineDate = r.DeadlineDate,
                ClosingDate = r.ClosingDate,
            }).ToArray();

            builder.HasData(seedData);
        }
    }
}
