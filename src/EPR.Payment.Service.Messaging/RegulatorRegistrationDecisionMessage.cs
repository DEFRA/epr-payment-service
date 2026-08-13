using System.Diagnostics.CodeAnalysis;

namespace EPR.Payment.Service.Messaging;

[ExcludeFromCodeCoverage]
public record RegulatorRegistrationDecisionMessage(
    Guid SubmissionId,
    string EventName,
    DateTime DecisionDate);
