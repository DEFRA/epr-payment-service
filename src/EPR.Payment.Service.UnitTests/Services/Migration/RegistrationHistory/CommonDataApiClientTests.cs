using System.Net;
using System.Text;
using EPR.Payment.Service.Services.Migration.RegistrationHistory;
using FluentAssertions;
using FluentAssertions.Execution;

namespace EPR.Payment.Service.UnitTests.Services.Migration.RegistrationHistory
{
    [TestClass]
    public class CommonDataApiClientTests
    {
        [TestMethod]
        public async Task GetRegistrationHistoryAsync_ParsesEachNdJsonLineAndSkipsBlankLines()
        {
            const string body =
                "{\"submissionId\":\"00080011-6e73-4a24-b969-88fa7a24b4bd\",\"fileId\":\"f1\",\"blobName\":\"b1\",\"eventType\":\"Submitted\",\"eventTypeOrder\":1,\"replayTs\":\"2026-03-05T10:02:36.55\",\"eventDate\":\"2026-03-05T10:02:36.55\",\"applicationReferenceNumber\":\"PEPR1\"}\n" +
                "\n" +
                "{\"submissionId\":\"00080011-6e73-4a24-b969-88fa7a24b4bd\",\"fileId\":\"f1\",\"eventType\":\"RegulatorRegistrationDecision\",\"eventTypeOrder\":3,\"replayTs\":\"2026-03-06T10:00:00\",\"eventDate\":\"2026-03-06T10:00:00\",\"decision\":\"Accepted\"}\n";
            var handler = new StubHandler(HttpStatusCode.OK, body);
            var sut = new CommonDataApiClient(new HttpClient(handler) { BaseAddress = new Uri("https://common-data/") });

            var records = new List<RegistrationHistoryRecord>();
            await foreach (var record in sut.GetRegistrationHistoryAsync(CancellationToken.None))
            {
                records.Add(record);
            }

            using (new AssertionScope())
            {
                handler.RequestedUri.Should().Be(new Uri("https://common-data/api/migration/registration-history"));
                records.Should().HaveCount(2);
                records[0].BlobName.Should().Be("b1");
                records[0].EventTypeOrder.Should().Be(1);
                records[0].ReplayTs.Should().Be(new DateTime(2026, 3, 5, 10, 2, 36, 550));
                records[0].ApplicationReferenceNumber.Should().Be("PEPR1");
                records[1].Decision.Should().Be("Accepted");
            }
        }

        [TestMethod]
        public async Task GetRegistrationHistoryAsync_NonSuccessStatus_Throws()
        {
            var sut = new CommonDataApiClient(new HttpClient(new StubHandler(HttpStatusCode.NotFound, string.Empty)) { BaseAddress = new Uri("https://common-data/") });

            Func<Task> act = async () =>
            {
                await foreach (var _ in sut.GetRegistrationHistoryAsync(CancellationToken.None))
                {
                }
            };

            await act.Should().ThrowAsync<HttpRequestException>();
        }

        [TestMethod]
        public async Task GetRegistrationHistoryAsync_NullLine_Throws()
        {
            var sut = new CommonDataApiClient(new HttpClient(new StubHandler(HttpStatusCode.OK, "null\n")) { BaseAddress = new Uri("https://common-data/") });

            Func<Task> act = async () =>
            {
                await foreach (var _ in sut.GetRegistrationHistoryAsync(CancellationToken.None))
                {
                }
            };

            await act.Should().ThrowAsync<InvalidDataException>();
        }

        [TestMethod]
        public void Constructor_NullHttpClient_Throws()
        {
            ((Action)(() => new CommonDataApiClient(null!))).Should().Throw<ArgumentNullException>();
        }

        private sealed class StubHandler(HttpStatusCode statusCode, string body) : HttpMessageHandler
        {
            public Uri? RequestedUri { get; private set; }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                RequestedUri = request.RequestUri;
                return Task.FromResult(new HttpResponseMessage(statusCode)
                {
                    Content = new StringContent(body, Encoding.UTF8, "application/x-ndjson"),
                });
            }
        }
    }
}
