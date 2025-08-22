using System;

namespace CatalogAPI.Logging;

public class CustomLogger : ILogger
{
    readonly string _loggerName;
    readonly CustomLoggerProviderConfiguration _loggerConfig;

    public CustomLogger(string loggerName, CustomLoggerProviderConfiguration loggerConfig)
    {
        _loggerName = loggerName;
        _loggerConfig = loggerConfig;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel == _loggerConfig.LogLevel;
    }

    public IDisposable? BeginScope<TState>(TState state)
        where TState : notnull
    {
        return null;
    }

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter
    )
    {
        string message = $"{logLevel}: {eventId.Id} - {formatter(state, exception)}";

        CreateLogTextFile(message);
    }

    private void CreateLogTextFile(string message)
    {
        string logFilePath = "/Users/henrique/dotNet/dotnet9-studies/CatalogAPI/Logs/log.txt";

        using (StreamWriter sw = new StreamWriter(logFilePath, true))
        {
            try
            {
                sw.WriteLine(message);
                sw.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
