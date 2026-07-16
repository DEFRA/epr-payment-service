using Microsoft.Extensions.Logging;

namespace EPR.Payment.Service.Common.Logging;

public static class LoggerExtensions
{
    /// <summary>
    /// Use this in a using statement to add key-value pairs to the scope. It safely ensures that the dictionary
    /// of data is constructed using the correct Types
    /// </summary>
    public static IDisposable? AddScopedData<T>(this ILogger<T> logger, Dictionary<string, object> keyValuePairs)
    {
        return logger.BeginScope(keyValuePairs);
    }
}