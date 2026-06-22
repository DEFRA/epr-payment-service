using CsvHelper.Configuration;
using EPR.Payment.Service.Services.RegistrationSubmission.Csv.Models;

namespace EPR.Payment.Service.Services.RegistrationSubmission.Csv.Maps
{
    public sealed class RegistrationCsvRowMap : ClassMap<RegistrationCsvRow>
    {
        public RegistrationCsvRowMap()
        {
            Map(x => x.OrganisationId).Name("organisation_id");
            Map(x => x.SubsidiaryId).Name("subsidiary_id");
            Map(x => x.HomeNationCode).Name("home_nation_code");
            Map(x => x.OrganisationSize).Name("organisation_size").Optional();
            Map(x => x.PackagingActivityOm).Name("packaging_activity_om").Optional();
            Map(x => x.JoinerDate).Name("joiner_date").Optional();
            Map(x => x.ClosedLoopRegistration).Name("closed_loop_registration").Optional();
        }
    }
}
