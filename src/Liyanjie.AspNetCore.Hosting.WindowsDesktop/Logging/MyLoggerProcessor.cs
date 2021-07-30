using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Liyanjie.AspNetCore.Hosting.WindowsDesktop.Logging
{
    class MyLoggerProcessor : IDisposable
    {
        const int maxQueuedMessages = 1024;

        readonly BlockingCollection<LogMessage> messageQueue = new(maxQueuedMessages);
        readonly Thread outputThread;

        public MyLoggerProcessor()
        {
            outputThread = new Thread(ProcessLogQueue)
            {
                IsBackground = true,
                Name = "Logger queue processing thread"
            };
            outputThread.Start();
        }

        public void EnqueueMessage(LogMessage message)
        {
            if (!messageQueue.IsAddingCompleted)
            {
                try
                {
                    messageQueue.Add(message);
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
            Program.Form.ShowLog(message);
        }

        void ProcessLogQueue()
        {
            try
            {
                foreach (var message in messageQueue.GetConsumingEnumerable())
                {
                    WriteMessage(message);
                }
            }
            catch
            {
                try
                {
                    messageQueue.CompleteAdding();
                }
                catch { }
            }
        }

        public void Dispose()
        {
            messageQueue.CompleteAdding();

            try
            {
                outputThread.Join(1500);
            }
            catch (ThreadStateException) { }
        }
    }
}
