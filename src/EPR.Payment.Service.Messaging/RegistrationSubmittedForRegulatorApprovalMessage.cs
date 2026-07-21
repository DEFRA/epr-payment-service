using System.Diagnostics.CodeAnalysis;

namespace EPR.Payment.Service.Messaging;

[ExcludeFromCodeCoverage]
public record RegistrationSubmittedForRegulatorApprovalMessage(
    Guid SubmissionId,
    string ApplicationReferenceNumber,
    DateTime SubmissionDate);
