using System;
using System.Text;

using Microsoft.Extensions.Logging;

namespace Liyanjie.AspNetCore.Hosting.WindowsDesktop.Logging
{
    class MyLogger : ILogger
    {
        static readonly string loglevelPadding = ": ";
        static readonly string messagePadding;
        static readonly string newLineWithMessagePadding;

        [ThreadStatic]
        static StringBuilder logBuilder;

        readonly string name;
        readonly MyLoggerProcessor processor;

        static MyLogger()
        {
            messagePadding = new string(' ', GetLogLevelString(LogLevel.Information).Length + loglevelPadding.Length);
            newLineWithMessagePadding = Environment.NewLine + messagePadding;
        }

        internal MyLogger(string name, MyLoggerProcessor processor)
        {
            this.name = name ?? throw new ArgumentNullException(nameof(name));
            this.processor = processor ?? throw new ArgumentNullException(nameof(processor)); ;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;

            if (formatter == null)
                throw new ArgumentNullException(nameof(formatter));

            var message = formatter(state, exception);

            if (!string.IsNullOrEmpty(message) || exception != null)
                WriteMessage(processor, logLevel, name, eventId.Id, message, exception);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        public IDisposable BeginScope<TState>(TState state) => NullScope.Instance;

        static void WriteMessage(
            MyLoggerProcessor queueProcessor,
            LogLevel logLevel,
            string logName,
            int eventId,
            string message,
            Exception exception)
        {
            var _logBuilder = logBuilder ?? new();
            logBuilder = null;

            var entry = CreateMessage(_logBuilder, logLevel, logName, eventId, message, exception);
            queueProcessor.EnqueueMessage(entry);

            _logBuilder.Clear();
            if (_logBuilder.Capacity > 1024)
                _logBuilder.Capacity = 1024;
            logBuilder = _logBuilder;
        }

        static LogMessage CreateMessage(
            StringBuilder logBuilder,
            LogLevel logLevel,
            string logName,
            int eventId,
            string message,
            Exception exception)
        {
            var logLevelString = GetLogLevelString(logLevel);
            logBuilder.Append(GetLogLevelString(logLevel));
            logBuilder.Append(loglevelPadding);
            logBuilder.Append(logName);
            logBuilder.Append('[');
            logBuilder.Append(eventId);
            logBuilder.Append(']');
            logBuilder.AppendLine();

            if (!string.IsNullOrEmpty(message))
            {
                logBuilder.Append(messagePadding);

                var len = logBuilder.Length;
                logBuilder.AppendLine(message);
                logBuilder.Replace(Environment.NewLine, newLineWithMessagePadding, len, message.Length);
            }

            if (exception != null)
                logBuilder.AppendLine(exception.ToString());

            return new LogMessage(
                message: logBuilder.ToString(),
                timeStamp: DateTime.Now.ToString(),
                levelString: logLevelString
            );
        }

        static string GetLogLevelString(LogLevel logLevel)
        {
            return logLevel switch
            {
                LogLevel.Trace => "trce",
                LogLevel.Debug => "dbug",
                LogLevel.Information => "info",
                LogLevel.Warning => "warn",
                LogLevel.Error => "fail",
                LogLevel.Critical => "crit",
                _ => throw new ArgumentOutOfRangeException(nameof(logLevel)),
            };
        }

        internal class NullScope : IDisposable
        {
            public static NullScope Instance { get; } = new();
            public void Dispose() { }
        }
    }
}
