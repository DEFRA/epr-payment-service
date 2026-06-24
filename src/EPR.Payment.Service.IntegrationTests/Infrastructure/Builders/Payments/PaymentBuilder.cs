namespace EPR.Payment.Service.IntegrationTests.Infrastructure.Builders.Payments;

/// <summary>
/// Builds a producer organisation with an ApprovedPerson admin enrolment (Approved status).
/// Mirrors production by setting User.ExternalIdpUserId = UserId.
/// </summary>
public sealed class PaymentBuilder(TestBuilders builders)
{
    public async Task<BuiltPayment> Build()
    {
        Common.Data.DataModels.Payment payment = null!;
        await builders.WithDbContextAsync(async ctx =>
        {
            payment = await DatabaseDataGenerator.InsertRandomPayment(ctx);
        }, save: false);
        return new BuiltPayment(payment);
    }
}
