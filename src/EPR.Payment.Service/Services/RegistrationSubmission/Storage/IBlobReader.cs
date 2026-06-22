namespace EPR.Payment.Service.Services.RegistrationSubmission.Storage
{
    public interface IBlobReader
    {
        Task<Stream> DownloadAsync(string blobName, CancellationToken cancellationToken);
    }
}
