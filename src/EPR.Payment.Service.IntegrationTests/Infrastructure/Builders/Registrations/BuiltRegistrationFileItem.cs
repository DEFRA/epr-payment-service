namespace EPR.Payment.Service.IntegrationTests.Infrastructure.Builders.Registrations;

/// <summary>
/// Bundle returned by the <see cref="RegistrationFileBuilder"/>
/// </summary>
public sealed record BuiltRegistrationFileItem(
    string OrganisationId,
    string? SubmissionId,
    string? HomeNationCode,
    string? OrganisationSize,
    string? PackagingActivityOm,
    DateTime? JoinerDate,
    bool ClosedLoopRegistration)
{
    public override string ToString()
    {
        var closedLoop = ClosedLoopRegistration ? "YES" : "NO";
        return $"{OrganisationId},{SubmissionId},{HomeNationCode},{OrganisationSize},{PackagingActivityOm},{JoinerDate?.ToString("yyyy-MM-dd")},{closedLoop}";
    }
}