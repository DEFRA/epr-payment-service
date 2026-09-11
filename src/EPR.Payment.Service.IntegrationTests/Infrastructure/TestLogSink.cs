using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace EPR.Payment.Service.IntegrationTests.Infrastructure;

public sealed record CapturedLogEntry(string CategoryName, LogLevel Level, string Message, Exception? Exception);

/// <summary>
/// In-memory log capture for asserting on structured log output (e.g. the "fee snapshot created"
/// log the AC requires). Shared across the whole assembly via <see cref="ServiceFixture"/>, so
/// tests must filter by something unique to their own run (a Guid embedded in the log message is
/// the simplest) rather than assuming an empty sink at the start of the test.
/// </summary>
public sealed class TestLogSink
{
    private readonly ConcurrentQueue<CapturedLogEntry> _entries = new();

    internal void Add(CapturedLogEntry entry) => _entries.Enqueue(entry);

    public IReadOnlyList<CapturedLogEntry> EntriesContaining(string text) =>
        _entries.Where(e => e.Message.Contains(text, StringComparison.Ordinal)).ToList();
}

internal sealed class TestLoggerProvider(TestLogSink sink) : ILoggerProvider
{
    public ILogger CreateLogger(string categoryName) => new TestLogger(categoryName, sink);

    public void Dispose()
    {
    }

    private sealed class TestLogger(string categoryName, TestLogSink sink) : ILogger
    {
        public IDisposable BeginScope<TState>(TState state) where TState : notnull => NullScope.Instance;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            sink.Add(new CapturedLogEntry(categoryName, logLevel, formatter(state, exception), exception));
        }

        private sealed class NullScope : IDisposable
        {
            public static readonly NullScope Instance = new();

            public void Dispose()
            {
            }
        }
    }
}
