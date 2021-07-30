using System.Collections.Concurrent;

using Microsoft.Extensions.Logging;

namespace Liyanjie.AspNetCore.Hosting.WindowsDesktop.Logging
{
    [ProviderAlias("My")]
    class MyLoggerProvider : ILoggerProvider
    {
        readonly ConcurrentDictionary<string, MyLogger> loggers = new();
        readonly MyLoggerProcessor messageQueue;

        public MyLoggerProvider()
        {
            this.messageQueue = new MyLoggerProcessor();
        }

        public ILogger CreateLogger(string name)
        {
            return loggers.GetOrAdd(name, name => new MyLogger(name, messageQueue));
        }

        public void Dispose()
        {
            messageQueue.Dispose();
        }
    }
}
