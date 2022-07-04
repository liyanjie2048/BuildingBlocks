using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Liyanjie.AspNetCore.Hosting.WindowsDesktop.Logging;

class MyLoggerProcessor : IDisposable
{
    const int _maxQueuedMessages = 1024;

    readonly BlockingCollection<LogMessage> _messageQueue = new(_maxQueuedMessages);
    readonly Thread _outputThread;

    public MyLoggerProcessor()
    {
        _outputThread = new Thread(ProcessLogQueue)
        {
            IsBackground = true,
            Name = "Logger queue processing thread"
        };
        _outputThread.Start();
    }

    public void EnqueueMessage(LogMessage message)
    {
        if (!_messageQueue.IsAddingCompleted)
        {
            try
            {
                _messageQueue.Add(message);
                return;
            }
            catch (InvalidOperationException) { }
        }

        try
        {
            WriteMessage(message);
        }
        catch (Exception) { }
    }

    static void WriteMessage(LogMessage message)
    {
        Program.Form?.ShowLog(message);
    }

    void ProcessLogQueue()
    {
        try
        {
            foreach (var message in _messageQueue.GetConsumingEnumerable())
            {
                WriteMessage(message);
            }
        }
        catch
        {
            try
            {
                _messageQueue.CompleteAdding();
            }
            catch { }
        }
    }

    public void Dispose()
    {
        _messageQueue.CompleteAdding();

        try
        {
            _outputThread.Join(1500);
        }
        catch (ThreadStateException) { }
    }
}
