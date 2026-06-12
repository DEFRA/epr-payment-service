using System.Diagnostics.CodeAnalysis;

namespace EPR.Payment.Service.Messaging;

[ExcludeFromCodeCoverage]
public record RegistrationSubmittedMessage(
    Guid SubmissionId,
    string RegistrationBlobName,
    Guid? ComplianceSchemeId,
    string SubmissionPeriod,
    DateTime SubmissionDate
);
