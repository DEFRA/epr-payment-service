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
    public class RegulatorRegistrationDecisionConsumerTests
    {
        private Mock<ILogger<RegulatorRegistrationDecisionConsumer>> _loggerMock = null!;
        private Mock<IRegulatorRegistrationDecisionHandler> _handlerMock = null!;
        private ServiceProvider _scopedProvider = null!;
        private RegulatorRegistrationDecisionConsumer _sut = null!;

        [TestInitialize]
        public void Init()
        {
            _loggerMock = new Mock<ILogger<RegulatorRegistrationDecisionConsumer>>();
            _handlerMock = new Mock<IRegulatorRegistrationDecisionHandler>();

            var services = new ServiceCollection();
            services.AddSingleton(_handlerMock.Object);
            _scopedProvider = services.BuildServiceProvider();

            _sut = new RegulatorRegistrationDecisionConsumer(_loggerMock.Object);
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
                _sut.TopicConfigKey.Should().Be("ServiceBus:RegulatorRegistrationDecisionTopicName");
                _sut.SubscriptionConfigKey.Should().Be("ServiceBus:RegulatorRegistrationDecisionSubscriptionName");
                RegulatorRegistrationDecisionConsumer.TopicConfigurationKey.Should().Be(_sut.TopicConfigKey);
                RegulatorRegistrationDecisionConsumer.SubscriptionConfigurationKey.Should().Be(_sut.SubscriptionConfigKey);
            }
        }

        [TestMethod]
        public async Task ConsumeAsync_ValidMessage_BuildsRequestAndInvokesHandler()
        {
            var messagePayload = new RegulatorRegistrationDecisionMessage(
                SubmissionId: Guid.NewGuid(),
                EventName: "AcceptedByRegulator",
                DecisionDate: new DateTime(2026, 7, 22, 10, 0, 0, DateTimeKind.Utc));
            var message = ServiceBusModelFactory.ServiceBusReceivedMessage(body: BinaryData.FromObjectAsJson(messagePayload));

            RegulatorRegistrationDecisionRequest? captured = null;
            _handlerMock
                .Setup(h => h.HandleAsync(It.IsAny<RegulatorRegistrationDecisionRequest>(), It.IsAny<CancellationToken>()))
                .Callback<RegulatorRegistrationDecisionRequest, CancellationToken>((r, _) => captured = r)
                .Returns(Task.CompletedTask);

            await _sut.ConsumeAsync(message, _scopedProvider, CancellationToken.None);

            using (new AssertionScope())
            {
                captured.Should().NotBeNull();
                captured!.SubmissionId.Should().Be(messagePayload.SubmissionId);
                captured.EventName.Should().Be(messagePayload.EventName);
                captured.DecisionDate.Should().Be(messagePayload.DecisionDate);
                _handlerMock.Verify(
                    h => h.HandleAsync(It.IsAny<RegulatorRegistrationDecisionRequest>(), It.IsAny<CancellationToken>()),
                    Times.Once);
            }
        }

        [TestMethod]
        public async Task ConsumeAsync_ValidMessage_LogsMessageReceived()
        {
            var messagePayload = new RegulatorRegistrationDecisionMessage(
                SubmissionId: Guid.NewGuid(),
                EventName: "QueriedByRegulator",
                DecisionDate: DateTime.UtcNow);
            var message = ServiceBusModelFactory.ServiceBusReceivedMessage(body: BinaryData.FromObjectAsJson(messagePayload));

            _handlerMock
                .Setup(h => h.HandleAsync(It.IsAny<RegulatorRegistrationDecisionRequest>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            await _sut.ConsumeAsync(message, _scopedProvider, CancellationToken.None);

            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Regulator registration decision message received")),
                    It.IsAny<Exception?>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [TestMethod]
        public async Task ConsumeAsync_HandlerThrows_ExceptionPropagates()
        {
            var messagePayload = new RegulatorRegistrationDecisionMessage(
                SubmissionId: Guid.NewGuid(),
                EventName: "RejectedByRegulator",
                DecisionDate: DateTime.UtcNow);
            var message = ServiceBusModelFactory.ServiceBusReceivedMessage(body: BinaryData.FromObjectAsJson(messagePayload));

            _handlerMock
                .Setup(h => h.HandleAsync(It.IsAny<RegulatorRegistrationDecisionRequest>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException("db down"));

            Func<Task> act = () => _sut.ConsumeAsync(message, _scopedProvider, CancellationToken.None);

            await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("db down");
        }
    }
}
