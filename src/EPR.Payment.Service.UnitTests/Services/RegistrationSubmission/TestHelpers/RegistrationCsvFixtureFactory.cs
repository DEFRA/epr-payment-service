using System.Text;
using EPR.Payment.Service.Services.RegistrationSubmission.Csv.Models;

namespace EPR.Payment.Service.UnitTests.Services.RegistrationSubmission.TestHelpers
{
    internal static class RegistrationCsvFixtureFactory
    {
        public const string Headers = "organisation_id,subsidiary_id,home_nation_code,organisation_size,packaging_activity_om,joiner_date,closed_loop_registration";

        public static string BuildCsv(params RegistrationCsvRow[] rows)
        {
            var sb = new StringBuilder();
            sb.AppendLine(Headers);
            foreach (var row in rows)
            {
                sb.AppendLine($"{row.OrganisationId},{row.SubsidiaryId},{row.HomeNationCode},{row.OrganisationSize},{row.PackagingActivityOm},{row.JoinerDate},{row.ClosedLoopRegistration}");
            }
            return sb.ToString();
        }

        public static MemoryStream BuildCsvStream(params RegistrationCsvRow[] rows)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(BuildCsv(rows)));
        }

        public static RegistrationCsvRow Producer(
            string organisationId,
            string homeNationCode = "EN",
            string organisationSize = "Large",
            string packagingActivityOm = "",
            string joinerDate = "",
            string closedLoopRegistration = "") =>
            new()
            {
                OrganisationId = organisationId,
                SubsidiaryId = string.Empty,
                HomeNationCode = homeNationCode,
                OrganisationSize = organisationSize,
                PackagingActivityOm = packagingActivityOm,
                JoinerDate = joinerDate,
                ClosedLoopRegistration = closedLoopRegistration,
            };

        public static RegistrationCsvRow Subsidiary(
            string organisationId,
            string subsidiaryId,
            string packagingActivityOm = "",
            string joinerDate = "",
            string closedLoopRegistration = "") =>
            new()
            {
                OrganisationId = organisationId,
                SubsidiaryId = subsidiaryId,
                HomeNationCode = string.Empty,
                OrganisationSize = string.Empty,
                PackagingActivityOm = packagingActivityOm,
                JoinerDate = joinerDate,
                ClosedLoopRegistration = closedLoopRegistration,
            };

        public static async IAsyncEnumerable<RegistrationCsvRow> ToAsyncEnumerable(IEnumerable<RegistrationCsvRow> rows)
        {
            foreach (var row in rows)
            {
                yield return row;
            }
            await Task.CompletedTask;
        }
    }
}
