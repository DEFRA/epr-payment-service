namespace EPR.Payment.Service.IntegrationTests.Infrastructure.Builders;

/// <summary>
/// Bundle returned by <see cref="RegistrationSubmissionDataBuilder"/>.
/// </summary>
public sealed record BuiltRegistrationSubmissionData(Common.Data.DataModels.RegistrationSubmissionData Entity)
{
    public Guid Id => Entity.Id;
    public Guid SubmissionId => Entity.SubmissionId;
    public string ApplicationReferenceNumber => Entity.ApplicationReferenceNumber;
    public DateTime SubmissionDate => Entity.SubmissionDate;
}
