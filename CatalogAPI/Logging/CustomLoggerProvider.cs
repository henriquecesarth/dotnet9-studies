using System;
using System.Collections.Concurrent;

namespace CatalogAPI.Logging;

public class CustomLoggerProvider : ILoggerProvider
{
    readonly CustomLoggerProviderConfiguration _loggerConfig;
    readonly ConcurrentDictionary<string, CustomLogger> loggers =
        new ConcurrentDictionary<string, CustomLogger>();

    public CustomLoggerProvider(CustomLoggerProviderConfiguration loggerConfig)
    {
        _loggerConfig = loggerConfig;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return loggers.GetOrAdd(categoryName, name => new CustomLogger(name, _loggerConfig));
    }

    public void Dispose()
    {
        loggers.Clear();
    }
}
