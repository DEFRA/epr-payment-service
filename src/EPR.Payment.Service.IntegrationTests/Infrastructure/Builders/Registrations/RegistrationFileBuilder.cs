namespace EPR.Payment.Service.IntegrationTests.Infrastructure.Builders.Registrations;

/// <summary>
/// Builds a registration file - stores a CSV file in blob storage
/// </summary>
public sealed class RegistrationFileBuilder(TestBuilders builders)
{
    public async Task<BuiltRegistrationFile> Build()
    {
        BuiltRegistrationFile registrationFile = null!;
        await builders.WithBlobContainer(async ctx =>
        {
            registrationFile = await RegistrationSubmissionBlobFileGenerator.StoreRandomRegistrationCsv(ctx);
        });
        return registrationFile;
    }
}
