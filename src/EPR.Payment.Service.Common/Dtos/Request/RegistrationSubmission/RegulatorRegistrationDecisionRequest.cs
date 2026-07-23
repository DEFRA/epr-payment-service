using System.Diagnostics.CodeAnalysis;

namespace EPR.Payment.Service.Common.Dtos.Request.RegistrationSubmission
{
    [ExcludeFromCodeCoverage]
    public class RegulatorRegistrationDecisionRequest
    {
        public Guid SubmissionId { get; set; }

        public string EventName { get; set; } = string.Empty;

        public DateTime DecisionDate { get; set; }
    }
}
