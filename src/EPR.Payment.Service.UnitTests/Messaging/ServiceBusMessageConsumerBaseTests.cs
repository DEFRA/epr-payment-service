using Azure.Messaging.ServiceBus;
using EPR.Payment.Service.Messaging;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.Logging;
using Moq;

namespace EPR.Payment.Service.UnitTests.Messaging
{
    [TestClass]
    public class ServiceBusMessageConsumerBaseTests
    {
        [TestMethod]
        public void Constructor_NullLogger_Throws()
        {
            Action act = () => new CapturingConsumer(null!);
            act.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public async Task ConsumeAsync_NullMessage_Throws()
        {
            var sut = new CapturingConsumer(new Mock<ILogger>().Object);

            Func<Task> act = () => sut.ConsumeAsync(null!, Mock.Of<IServiceProvider>(), CancellationToken.None);

            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [TestMethod]
        public async Task ConsumeAsync_NullScopedProvider_Throws()
        {
            var sut = new CapturingConsumer(new Mock<ILogger>().Object);
            var message = BuildMessage(new SamplePayload("hello"));

            Func<Task> act = () => sut.ConsumeAsync(message, null!, CancellationToken.None);

            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [TestMethod]
        public async Task ConsumeAsync_ValidPayload_InvokesHandleAsyncAndDoesNotWarn()
        {
            var loggerMock = new Mock<ILogger>();
            var sut = new CapturingConsumer(loggerMock.Object);
            var payload = new SamplePayload("hello");
            var message = BuildMessage(payload);

            await sut.ConsumeAsync(message, Mock.Of<IServiceProvider>(), CancellationToken.None);

            using (new AssertionScope())
            {
                sut.HandledPayloads.Should().HaveCount(1);
                sut.HandledPayloads[0].Value.Should().Be("hello");
                loggerMock.Verify(
                    x => x.Log(
                        LogLevel.Warning,
                        It.IsAny<EventId>(),
                        It.IsAny<It.IsAnyType>(),
                        It.IsAny<Exception?>(),
                        It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                    Times.Never);
            }
        }

        [TestMethod]
        public async Task ConsumeAsync_JsonNullLiteralBody_LogsWarningAndSkipsHandler()
        {
            var loggerMock = new Mock<ILogger>();
            var sut = new CapturingConsumer(loggerMock.Object);
            var message = ServiceBusModelFactory.ServiceBusReceivedMessage(body: BinaryData.FromString("null"));

            await sut.ConsumeAsync(message, Mock.Of<IServiceProvider>(), CancellationToken.None);

            using (new AssertionScope())
            {
                sut.HandledPayloads.Should().BeEmpty();
                loggerMock.Verify(
                    x => x.Log(
                        LogLevel.Warning,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("null or undeserializable")),
                        It.IsAny<Exception?>(),
                        It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                    Times.Once);
            }
        }

        [TestMethod]
        public async Task ConsumeAsync_HandleAsyncThrows_ExceptionPropagates()
        {
            var sut = new ThrowingConsumer(new Mock<ILogger>().Object);
            var message = BuildMessage(new SamplePayload("hi"));

            Func<Task> act = () => sut.ConsumeAsync(message, Mock.Of<IServiceProvider>(), CancellationToken.None);

            await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("boom");
        }

        [TestMethod]
        public void TopicAndSubscriptionConfigKeys_AreExposedByConcreteConsumer()
        {
            var sut = new CapturingConsumer(new Mock<ILogger>().Object);

            using (new AssertionScope())
            {
                sut.TopicConfigKey.Should().Be(CapturingConsumer.TopicKey);
                sut.SubscriptionConfigKey.Should().Be(CapturingConsumer.SubscriptionKey);
            }
        }

        private static ServiceBusReceivedMessage BuildMessage(SamplePayload payload)
            => ServiceBusModelFactory.ServiceBusReceivedMessage(body: BinaryData.FromObjectAsJson(payload));

        public record SamplePayload(string Value);

        private sealed class CapturingConsumer : ServiceBusMessageConsumerBase<SamplePayload>
        {
            public const string TopicKey = "ServiceBus:TestConsumer:Topic";
            public const string SubscriptionKey = "ServiceBus:TestConsumer:Subscription";

            public CapturingConsumer(ILogger logger) : base(logger)
            {
            }

            public override string TopicConfigKey => TopicKey;

            public override string SubscriptionConfigKey => SubscriptionKey;

            public List<SamplePayload> HandledPayloads { get; } = new();

            protected override Task HandleAsync(SamplePayload message, IServiceProvider scopedProvider, CancellationToken cancellationToken)
            {
                HandledPayloads.Add(message);
                return Task.CompletedTask;
            }
        }

        private sealed class ThrowingConsumer : ServiceBusMessageConsumerBase<SamplePayload>
        {
            public ThrowingConsumer(ILogger logger) : base(logger)
            {
            }

            public override string TopicConfigKey => "ServiceBus:Throwing:Topic";

            public override string SubscriptionConfigKey => "ServiceBus:Throwing:Subscription";

            protected override Task HandleAsync(SamplePayload message, IServiceProvider scopedProvider, CancellationToken cancellationToken)
                => throw new InvalidOperationException("boom");
        }
    }
}
