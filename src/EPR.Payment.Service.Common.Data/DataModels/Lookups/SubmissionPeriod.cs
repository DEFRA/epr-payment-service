using System.Diagnostics.CodeAnalysis;
using EPR.Payment.Service.Common.Data.DataModels.BaseClasses;

namespace EPR.Payment.Service.Common.Data.DataModels.Lookups
{
    [ExcludeFromCodeCoverage]
    public class SubmissionPeriod : BaseEntity
    {
        public string WindowType { get; set; } = null!;

        public int RegistrationYear { get; set; }

        public DateTime OpeningDate { get; set; }

        public DateTime DeadlineDate { get; set; }

        public DateTime ClosingDate { get; set; }
    }
}
