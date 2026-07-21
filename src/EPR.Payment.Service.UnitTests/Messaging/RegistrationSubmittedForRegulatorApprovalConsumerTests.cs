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
    public class RegistrationSubmittedForRegulatorApprovalConsumerTests
    {
        private Mock<ILogger<RegistrationSubmittedForRegulatorApprovalConsumer>> _loggerMock = null!;
        private Mock<IRegistrationSubmittedForRegulatorApprovalHandler> _handlerMock = null!;
        private ServiceProvider _scopedProvider = null!;
        private RegistrationSubmittedForRegulatorApprovalConsumer _sut = null!;

        [TestInitialize]
        public void Init()
        {
            _loggerMock = new Mock<ILogger<RegistrationSubmittedForRegulatorApprovalConsumer>>();
            _handlerMock = new Mock<IRegistrationSubmittedForRegulatorApprovalHandler>();

            var services = new ServiceCollection();
            services.AddSingleton(_handlerMock.Object);
            _scopedProvider = services.BuildServiceProvider();

            _sut = new RegistrationSubmittedForRegulatorApprovalConsumer(_loggerMock.Object);
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
                _sut.TopicConfigKey.Should().Be("ServiceBus:RegistrationSubmittedForRegulatorApprovalTopicName");
                _sut.SubscriptionConfigKey.Should().Be("ServiceBus:RegistrationSubmittedForRegulatorApprovalSubscriptionName");
                RegistrationSubmittedForRegulatorApprovalConsumer.TopicConfigurationKey.Should().Be(_sut.TopicConfigKey);
                RegistrationSubmittedForRegulatorApprovalConsumer.SubscriptionConfigurationKey.Should().Be(_sut.SubscriptionConfigKey);
            }
        }

        [TestMethod]
        public async Task ConsumeAsync_ValidMessage_BuildsRequestAndInvokesHandler()
        {
            var messagePayload = new RegistrationSubmittedForRegulatorApprovalMessage(
                SubmissionId: Guid.NewGuid(),
                ApplicationReferenceNumber: "PEPR2601234",
                SubmissionDate: new DateTime(2026, 7, 20, 10, 0, 0, DateTimeKind.Utc));
            var message = ServiceBusModelFactory.ServiceBusReceivedMessage(body: BinaryData.FromObjectAsJson(messagePayload));

            RegistrationSubmittedForRegulatorApprovalRequest? captured = null;
            _handlerMock
                .Setup(h => h.HandleAsync(It.IsAny<RegistrationSubmittedForRegulatorApprovalRequest>(), It.IsAny<CancellationToken>()))
                .Callback<RegistrationSubmittedForRegulatorApprovalRequest, CancellationToken>((r, _) => captured = r)
                .Returns(Task.CompletedTask);

            await _sut.ConsumeAsync(message, _scopedProvider, CancellationToken.None);

            using (new AssertionScope())
            {
                captured.Should().NotBeNull();
                captured!.SubmissionId.Should().Be(messagePayload.SubmissionId);
                captured.ApplicationReferenceNumber.Should().Be(messagePayload.ApplicationReferenceNumber);
                captured.SubmissionDate.Should().Be(messagePayload.SubmissionDate);
                _handlerMock.Verify(
                    h => h.HandleAsync(It.IsAny<RegistrationSubmittedForRegulatorApprovalRequest>(), It.IsAny<CancellationToken>()),
                    Times.Once);
            }
        }

        [TestMethod]
        public async Task ConsumeAsync_ValidMessage_LogsMessageReceived()
        {
            var messagePayload = new RegistrationSubmittedForRegulatorApprovalMessage(
                SubmissionId: Guid.NewGuid(),
                ApplicationReferenceNumber: "PEPR2601234",
                SubmissionDate: DateTime.UtcNow);
            var message = ServiceBusModelFactory.ServiceBusReceivedMessage(body: BinaryData.FromObjectAsJson(messagePayload));

            _handlerMock
                .Setup(h => h.HandleAsync(It.IsAny<RegistrationSubmittedForRegulatorApprovalRequest>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            await _sut.ConsumeAsync(message, _scopedProvider, CancellationToken.None);

            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Registration submitted (regulator approval) message received")),
                    It.IsAny<Exception?>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [TestMethod]
        public async Task ConsumeAsync_HandlerThrows_ExceptionPropagates()
        {
            var messagePayload = new RegistrationSubmittedForRegulatorApprovalMessage(
                SubmissionId: Guid.NewGuid(),
                ApplicationReferenceNumber: "PEPR2601234",
                SubmissionDate: DateTime.UtcNow);
            var message = ServiceBusModelFactory.ServiceBusReceivedMessage(body: BinaryData.FromObjectAsJson(messagePayload));

            _handlerMock
                .Setup(h => h.HandleAsync(It.IsAny<RegistrationSubmittedForRegulatorApprovalRequest>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException("db down"));

            Func<Task> act = () => _sut.ConsumeAsync(message, _scopedProvider, CancellationToken.None);

            await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("db down");
        }
    }
}
