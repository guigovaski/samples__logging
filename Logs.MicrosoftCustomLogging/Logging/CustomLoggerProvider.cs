using System.Collections.Concurrent;
using System.Runtime.Versioning;
using Microsoft.Extensions.Options;

namespace Logs.MicrosoftCustomLogging.Logging;

[UnsupportedOSPlatform("browser")]
[ProviderAlias("CustomFileLogger")]
public class CustomLoggerProvider : ILoggerProvider
{
    private readonly IDisposable? _onChangeToken;
    private readonly ConcurrentDictionary<string, CustomLogger> _loggers;
    private CustomLoggerConfiguration _loggerConfiguration;

    public CustomLoggerProvider(IOptionsMonitor<CustomLoggerConfiguration> loggerConfiguration)
    {
        _onChangeToken = loggerConfiguration.OnChange(updatedConfig => _loggerConfiguration = updatedConfig);
        _loggerConfiguration = loggerConfiguration.CurrentValue;
    }
    
    public ILogger CreateLogger(string categoryName) => _loggers.GetOrAdd(categoryName, name => new CustomLogger(name, _loggerConfiguration));

    private CustomLoggerConfiguration GetCurrentConfig() => _loggerConfiguration;
    
    public void Dispose()
    {
        _loggers.Clear();
        _onChangeToken?.Dispose();
    }
}