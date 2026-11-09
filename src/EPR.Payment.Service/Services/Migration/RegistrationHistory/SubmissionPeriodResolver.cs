using System.Globalization;
using EPR.Payment.Service.Common.Data.DataModels.Lookups;

namespace EPR.Payment.Service.Services.Migration.RegistrationHistory
{
    /// <summary>
    /// Maps a Synapse submission period ("January to December 2026") and registration journey to a
    /// Lookup.SubmissionPeriod Id, mirroring the frontend's RegistrationApplicationService.ResolveSubmissionPeriodId.
    /// </summary>
    public static class SubmissionPeriodResolver
    {
        private const int LastSingleWindowYear = 2025;

        public static int? Resolve(
            string? submissionPeriod,
            string? registrationJourney,
            bool isComplianceScheme,
            IReadOnlyList<SubmissionPeriod> periods)
        {
            ArgumentNullException.ThrowIfNull(periods);

            var year = ParseYear(submissionPeriod);
            if (year is null)
            {
                return null;
            }

            var windowType = WindowTypeFor(year.Value, registrationJourney, isComplianceScheme);

            return periods
                .FirstOrDefault(p => p.RegistrationYear == year.Value
                                     && string.Equals(p.WindowType, windowType, StringComparison.OrdinalIgnoreCase))
                ?.Id;
        }

        public static int? ParseYear(string? submissionPeriod)
        {
            var trimmed = submissionPeriod?.Trim();
            if (string.IsNullOrEmpty(trimmed) || trimmed.Length < 4)
            {
                return null;
            }

            return int.TryParse(trimmed[^4..], NumberStyles.None, CultureInfo.InvariantCulture, out var year) ? year : null;
        }

        private static string WindowTypeFor(int year, string? registrationJourney, bool isComplianceScheme)
        {
            var scheme = isComplianceScheme ? "Cso" : "Direct";
            if (year <= LastSingleWindowYear)
            {
                return scheme;
            }

            var isSmallProducer = registrationJourney?.Contains("Small", StringComparison.OrdinalIgnoreCase) ?? false;
            return $"{scheme}{(isSmallProducer ? "Small" : "Large")}Producer";
        }
    }
}
