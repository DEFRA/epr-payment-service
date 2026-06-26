using Azure.Storage.Blobs;
using EPR.Payment.Service.Options;
using Microsoft.Extensions.Options;

namespace EPR.Payment.Service.Services.RegistrationSubmission.Storage
{
    public class BlobReader : IBlobReader
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly StorageAccountOptions _options;

        public BlobReader(BlobServiceClient blobServiceClient, IOptions<StorageAccountOptions> options)
        {
            _blobServiceClient = blobServiceClient ?? throw new ArgumentNullException(nameof(blobServiceClient));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<Stream> DownloadAsync(string blobName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(blobName))
            {
                throw new ArgumentException("Blob name must be provided.", nameof(blobName));
            }

            var containerClient = _blobServiceClient.GetBlobContainerClient(_options.RegistrationContainer);
            var blobClient = containerClient.GetBlobClient(blobName);

            var memoryStream = new MemoryStream();
            await blobClient.DownloadToAsync(memoryStream, cancellationToken);
            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}
