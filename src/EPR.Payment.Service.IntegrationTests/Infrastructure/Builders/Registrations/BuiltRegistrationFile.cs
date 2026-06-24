namespace EPR.Payment.Service.IntegrationTests.Infrastructure.Builders.Registrations;

/// <summary>
/// Bundle returned by the <see cref="RegistrationFileBuilder"/>
/// </summary>
public sealed record BuiltRegistrationFile(Guid SubmissionId, string RegistrationBlobName, params BuiltRegistrationFileItem[] BuiltRegistrationFileItems);