using EPR.Payment.Service.Common.Constants;

namespace EPR.Payment.Service.Services.Migration.RegistrationHistory
{
    /// <summary>Mirrors SubmissionEventCreateCommandHandler.TryMapRegulatorDecision in epr-pom-api-submission-status.</summary>
    public static class RegulatorDecisionMapper
    {
        public static bool TryMap(string? decision, out string eventName)
        {
            eventName = decision?.Trim().ToUpperInvariant() switch
            {
                "ACCEPTED" => RegistrationEventNames.AcceptedByRegulator,
                "REJECTED" => RegistrationEventNames.RejectedByRegulator,
                "QUERIED" => RegistrationEventNames.QueriedByRegulator,
                "CANCELLED" => RegistrationEventNames.CancelledByRegulator,
                _ => string.Empty,
            };

            return eventName.Length > 0;
        }
    }
}
