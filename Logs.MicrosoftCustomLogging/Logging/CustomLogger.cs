namespace Logs.MicrosoftCustomLogging.Logging;

public class CustomLogger(string name, CustomLoggerConfiguration loggerConfiguration) : ILogger
{
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default;

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel >= loggerConfiguration.LogLevel;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel)) return;

        var message = formatter(state, exception);
        if (string.IsNullOrEmpty(message)) return;
        Console.WriteLine($"{logLevel} - {eventId.Id} - {name} - {message}");
        
        using StreamWriter writer = new("log.txt", append: true);
        writer.WriteLine($"{logLevel} - {eventId.Id} - {name} - {message}");
    }
}