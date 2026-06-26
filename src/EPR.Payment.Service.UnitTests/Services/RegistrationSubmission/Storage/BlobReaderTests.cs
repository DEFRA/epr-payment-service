using System.Text;
using Azure;
using Azure.Storage.Blobs;
using EPR.Payment.Service.Services.RegistrationSubmission.Storage;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using StorageAccountOptions = EPR.Payment.Service.Options.StorageAccountOptions;

namespace EPR.Payment.Service.UnitTests.Services.RegistrationSubmission.Storage
{
    [TestClass]
    public class BlobReaderTests
    {
        private Mock<BlobServiceClient> _blobServiceMock = null!;
        private Mock<BlobContainerClient> _containerMock = null!;
        private Mock<BlobClient> _blobClientMock = null!;
        private BlobReader _sut = null!;

        [TestInitialize]
        public void Init()
        {
            _blobServiceMock = new Mock<BlobServiceClient>();
            _containerMock = new Mock<BlobContainerClient>();
            _blobClientMock = new Mock<BlobClient>();

            _blobServiceMock.Setup(s => s.GetBlobContainerClient("registration"))
                .Returns(_containerMock.Object);
            _containerMock.Setup(c => c.GetBlobClient(It.IsAny<string>())).Returns(_blobClientMock.Object);

            var options = Microsoft.Extensions.Options.Options.Create(new StorageAccountOptions
            {
                ConnectionString = "UseDevelopmentStorage=true",
                RegistrationContainer = "registration",
            });

            _sut = new BlobReader(_blobServiceMock.Object, options);
        }

        [TestMethod]
        public async Task DownloadAsync_ResolvesContainerAndBlobAndReturnsStream()
        {
            const string payload = "organisation_id,subsidiary_id\nORG-1,";
            _blobClientMock
                .Setup(b => b.DownloadToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .Returns<Stream, CancellationToken>((target, _) =>
                {
                    var bytes = Encoding.UTF8.GetBytes(payload);
                    target.Write(bytes, 0, bytes.Length);
                    return Task.FromResult(Mock.Of<Response>());
                });

            var result = await _sut.DownloadAsync("blob-name", CancellationToken.None);

            using var reader = new StreamReader(result);
            (await reader.ReadToEndAsync()).Should().Be(payload);

            _blobServiceMock.Verify(s => s.GetBlobContainerClient("registration"), Times.Once);
            _containerMock.Verify(c => c.GetBlobClient("blob-name"), Times.Once);
        }

        [TestMethod]
        public async Task DownloadAsync_NullOrEmptyBlobName_Throws()
        {
            Func<Task> a1 = () => _sut.DownloadAsync(null!, CancellationToken.None);
            Func<Task> a2 = () => _sut.DownloadAsync(string.Empty, CancellationToken.None);
            Func<Task> a3 = () => _sut.DownloadAsync(" ", CancellationToken.None);

            await a1.Should().ThrowAsync<ArgumentException>();
            await a2.Should().ThrowAsync<ArgumentException>();
            await a3.Should().ThrowAsync<ArgumentException>();
        }

        [TestMethod]
        public void Constructor_NullDependency_Throws()
        {
            var options = Microsoft.Extensions.Options.Options.Create(new StorageAccountOptions());
            Action a1 = () => new BlobReader(null!, options);
            Action a2 = () => new BlobReader(_blobServiceMock.Object, null!);
            a1.Should().Throw<ArgumentNullException>();
            a2.Should().Throw<ArgumentNullException>();
        }
    }
}
