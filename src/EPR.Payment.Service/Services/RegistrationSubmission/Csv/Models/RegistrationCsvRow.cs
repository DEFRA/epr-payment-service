using System.Diagnostics.CodeAnalysis;

namespace EPR.Payment.Service.Services.RegistrationSubmission.Csv.Models
{
    [ExcludeFromCodeCoverage]
    public class RegistrationCsvRow
    {
        public string OrganisationId { get; set; } = string.Empty;

        public string SubsidiaryId { get; set; } = string.Empty;

        public string HomeNationCode { get; set; } = string.Empty;

        public string OrganisationSize { get; set; } = string.Empty;

        public string PackagingActivityOm { get; set; } = string.Empty;

        public string JoinerDate { get; set; } = string.Empty;

        public string ClosedLoopRegistration { get; set; } = string.Empty;
    }
}
