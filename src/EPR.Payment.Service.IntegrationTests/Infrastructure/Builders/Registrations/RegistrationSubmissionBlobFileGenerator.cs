using Azure.Storage.Blobs;

namespace EPR.Payment.Service.IntegrationTests.Infrastructure.Builders.Registrations;

public static class RegistrationSubmissionBlobFileGenerator
{
    private static int counter;
    private static string GetNewId() => Interlocked.Increment(ref counter).ToString();
    
    private static readonly string[] CsvHeader =
        ["organisation_id,subsidiary_id,home_nation_code,organisation_size,packaging_activity_om,joiner_date,closed_loop_registration"];
    public static async Task<BuiltRegistrationFile> StoreRandomRegistrationCsv(BlobContainerClient blobContainerClient,
        Guid? existingSubmissionId)
    {
        var orgId = $"ORG{GetNewId()}";
        var subsidiaryId = $"SUB{GetNewId()}";
        var blobId = $"BLOB{GetNewId()}";
        var submissionId = existingSubmissionId ?? Guid.NewGuid();
        var blobName = $"{blobId}.csv";

        var parentOrgRegistration = new BuiltRegistrationFileItem(orgId, null, "EN", "L", "Primary", new DateTime(2024, 1, 1), true);
        var subsidiaryRegistration = new BuiltRegistrationFileItem(orgId, subsidiaryId, null, null, null, null, true);
        var builtRegistrationSubmission = new BuiltRegistrationFile(submissionId, blobName, parentOrgRegistration, subsidiaryRegistration);

        var csv = GenerateCsvContent(builtRegistrationSubmission);

        await blobContainerClient.UploadBlobAsync(blobName, BinaryData.FromString(csv));
        return builtRegistrationSubmission;
    }
    
    private static string GenerateCsvContent(BuiltRegistrationFile regFile)
    {
        var lines = CsvHeader.Concat(regFile.BuiltRegistrationFileItems.Select(i => i.ToString()));
        return string.Join("\n", lines);
    }

}
