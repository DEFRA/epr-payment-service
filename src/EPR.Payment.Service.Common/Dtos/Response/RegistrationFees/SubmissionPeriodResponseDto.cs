using System.Diagnostics.CodeAnalysis;

namespace EPR.Payment.Service.Common.Dtos.Response.RegistrationFees
{
    [ExcludeFromCodeCoverage]
    public class SubmissionPeriodResponseDto
    {
        public int Id { get; set; }

        public string WindowType { get; set; } = null!;

        public int RegistrationYear { get; set; }

        public DateTime OpeningDate { get; set; }

        public DateTime DeadlineDate { get; set; }

        public DateTime ClosingDate { get; set; }
    }
}
