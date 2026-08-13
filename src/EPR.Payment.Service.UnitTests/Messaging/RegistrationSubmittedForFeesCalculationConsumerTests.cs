using Azure.Messaging.ServiceBus;
using EPR.Payment.Service.Common.Dtos.Request.RegistrationSubmission;
using EPR.Payment.Service.Common.Services.Interfaces.RegistrationSubmission;
using EPR.Payment.Service.Messaging;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace EPR.Payment.Service.UnitTests.Messaging
{
    [TestClass]
    public class RegistrationSubmittedForFeesCalculationConsumerTests
    {
        private Mock<ILogger<RegistrationSubmittedForFeesCalculationConsumer>> _loggerMock = null!;
        private Mock<IRegistrationSubmissionDataHandler> _handlerMock = null!;
        private ServiceProvider _scopedProvider = null!;
        private RegistrationSubmittedForFeesCalculationConsumer _sut = null!;

        [TestInitialize]
        public void Init()
        {
            _loggerMock = new Mock<ILogger<RegistrationSubmittedForFeesCalculationConsumer>>();
            _handlerMock = new Mock<IRegistrationSubmissionDataHandler>();

            var services = new ServiceCollection();
            services.AddSingleton(_handlerMock.Object);
            _scopedProvider = services.BuildServiceProvider();

            _sut = new RegistrationSubmittedForFeesCalculationConsumer(_loggerMock.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _scopedProvider.Dispose();
        }

        [TestMethod]
        public void ConfigKeys_MatchAppsettingsPath()
        {
            using (new AssertionScope())
            {
                _sut.TopicConfigKey.Should().Be("ServiceBus:RegistrationSubmittedForFeesCalculationTopicName");
                _sut.SubscriptionConfigKey.Should().Be("ServiceBus:RegistrationSubmittedForFeesCalculationSubscriptionName");
                RegistrationSubmittedForFeesCalculationConsumer.TopicConfigurationKey.Should().Be(_sut.TopicConfigKey);
                RegistrationSubmittedForFeesCalculationConsumer.SubscriptionConfigurationKey.Should().Be(_sut.SubscriptionConfigKey);
            }
        }

        [TestMethod]
        public async Task ConsumeAsync_ValidMessage_BuildsRequestAndInvokesHandler()
        {
            var messagePayload = new RegistrationSubmittedMessage(
                SubmissionId: Guid.NewGuid(),
                RegistrationBlobName: $"av-blob-{Guid.NewGuid()}",
                ComplianceSchemeId: Guid.NewGuid(),
                SubmissionDate: new DateTime(2026, 6, 15, 12, 0, 0, DateTimeKind.Utc),
                SubmissionPeriodId: 42,
                RegulatorNation: "GB-ENG",
                ApplicationReferenceNumber: "PEPR2601234");
            var message = ServiceBusModelFactory.ServiceBusReceivedMessage(body: BinaryData.FromObjectAsJson(messagePayload));

            CreateRegistrationSubmissionDataRequest? captured = null;
            _handlerMock
                .Setup(h => h.HandleAsync(It.IsAny<CreateRegistrationSubmissionDataRequest>(), It.IsAny<CancellationToken>()))
                .Callback<CreateRegistrationSubmissionDataRequest, CancellationToken>((r, _) => captured = r)
                .ReturnsAsync(Guid.NewGuid());

            await _sut.ConsumeAsync(message, _scopedProvider, CancellationToken.None);

            using (new AssertionScope())
            {
                captured.Should().NotBeNull();
                captured!.SubmissionId.Should().Be(messagePayload.SubmissionId);
                captured.RegistrationBlobName.Should().Be(messagePayload.RegistrationBlobName);
                captured.ComplianceSchemeId.Should().Be(messagePayload.ComplianceSchemeId);
                captured.SubmissionPeriodId.Should().Be(messagePayload.SubmissionPeriodId);
                captured.SubmissionDate.Should().Be(messagePayload.SubmissionDate);
                captured.RegulatorNation.Should().Be(messagePayload.RegulatorNation);
                captured.ApplicationReferenceNumber.Should().Be(messagePayload.ApplicationReferenceNumber);
                _handlerMock.Verify(
                    h => h.HandleAsync(It.IsAny<CreateRegistrationSubmissionDataRequest>(), It.IsAny<CancellationToken>()),
                    Times.Once);
            }
        }

        [TestMethod]
        public async Task ConsumeAsync_ValidMessage_LogsReceivedAndProcessed()
        {
            var messagePayload = new RegistrationSubmittedMessage(
                SubmissionId: Guid.NewGuid(),
                RegistrationBlobName: "av-blob-xyz",
                ComplianceSchemeId: null,
                SubmissionDate: DateTime.UtcNow,
                SubmissionPeriodId: 1,
                RegulatorNation: "GB-ENG",
                ApplicationReferenceNumber: "PEPR2601234");
            var message = ServiceBusModelFactory.ServiceBusReceivedMessage(body: BinaryData.FromObjectAsJson(messagePayload));

            _handlerMock
                .Setup(h => h.HandleAsync(It.IsAny<CreateRegistrationSubmissionDataRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Guid.NewGuid());

            await _sut.ConsumeAsync(message, _scopedProvider, CancellationToken.None);

            using (new AssertionScope())
            {
                _loggerMock.Verify(
                    x => x.Log(
                        LogLevel.Information,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Registration submitted (fees calculation) message received")),
                        It.IsAny<Exception?>(),
                        It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                    Times.Once);
                _loggerMock.Verify(
                    x => x.Log(
                        LogLevel.Information,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Processed registration submitted (fees calculation) message")),
                        It.IsAny<Exception?>(),
                        It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                    Times.Once);
            }
        }

        [TestMethod]
        public async Task ConsumeAsync_HandlerThrows_ExceptionPropagates()
        {
            var messagePayload = new RegistrationSubmittedMessage(
                SubmissionId: Guid.NewGuid(),
                RegistrationBlobName: "av-blob-xyz",
                ComplianceSchemeId: null,
                SubmissionDate: DateTime.UtcNow,
                SubmissionPeriodId: 1,
                RegulatorNation: "GB-ENG",
                ApplicationReferenceNumber: "PEPR2601234");
            var message = ServiceBusModelFactory.ServiceBusReceivedMessage(body: BinaryData.FromObjectAsJson(messagePayload));

            _handlerMock
                .Setup(h => h.HandleAsync(It.IsAny<CreateRegistrationSubmissionDataRequest>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException("db down"));

            Func<Task> act = () => _sut.ConsumeAsync(message, _scopedProvider, CancellationToken.None);

            await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("db down");
        }
    }
}
