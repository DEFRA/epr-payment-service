namespace EPR.Payment.Service.IntegrationTests.Infrastructure.Builders;

/// <summary>
/// Builds a producer organisation with an ApprovedPerson admin enrolment (Approved status).
/// Mirrors production by setting User.ExternalIdpUserId = UserId.
/// </summary>
public sealed class PaymentBuilder(TestBuilders builders)
{
    private string? _reference;

    /// <summary>Overrides the random default reference - useful for exercising
    /// application-reference-number matching against a <c>RegistrationSubmissionData</c> row.</summary>
    public PaymentBuilder WithReference(string reference)
    {
        _reference = reference;
        return this;
    }

    public async Task<BuiltPayment> Build()
    {
        Common.Data.DataModels.Payment payment = null!;
        await builders.WithDbContextAsync(async ctx =>
        {
            payment = await DatabaseDataGenerator.InsertRandomPayment(ctx, _reference);
        }, save: false);
        return new BuiltPayment(payment);
    }
}
