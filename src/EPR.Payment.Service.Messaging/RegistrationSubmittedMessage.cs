using System.Diagnostics.CodeAnalysis;

namespace EPR.Payment.Service.Messaging;

[ExcludeFromCodeCoverage]
public record RegistrationSubmittedMessage(
    Guid SubmissionId,
    Guid FileId,
    string FileName,
    Guid? ComplianceSchemeId,
    string SubmissionPeriod,
    DateTime SubmissionDate
);
