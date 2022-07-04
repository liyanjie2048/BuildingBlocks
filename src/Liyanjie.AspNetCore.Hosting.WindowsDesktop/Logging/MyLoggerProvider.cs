using System.Collections.Concurrent;

using Microsoft.Extensions.Logging;

namespace Liyanjie.AspNetCore.Hosting.WindowsDesktop.Logging;

[ProviderAlias("My")]
class MyLoggerProvider : ILoggerProvider
{
    readonly ConcurrentDictionary<string, MyLogger> _loggers = new();
    readonly MyLoggerProcessor _messageQueue;

    public MyLoggerProvider()
    {
        _messageQueue = new MyLoggerProcessor();
    }

    public ILogger CreateLogger(string name)
    {
        return _loggers.GetOrAdd(name, name => new MyLogger(name, _messageQueue));
    }

    public void Dispose()
    {
        _messageQueue.Dispose();
    }
}
