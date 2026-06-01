namespace EPR.Payment.Service.Messaging;

public record RegistrationSubmittedMessage(
    Guid SubmissionId,
    Guid FileId,
    string FileName,
    Guid? ComplianceSchemeId,
    string SubmissionPeriod,
    DateTime SubmissionDate
);
