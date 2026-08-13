using System.Diagnostics.CodeAnalysis;

namespace EPR.Payment.Service.Messaging;

[ExcludeFromCodeCoverage]
public record RegistrationSubmittedMessage(
    Guid SubmissionId,
    string RegistrationBlobName,
    Guid? ComplianceSchemeId,
    DateTime SubmissionDate,
    int SubmissionPeriodId,
    string RegulatorNation,
    string ApplicationReferenceNumber
);
