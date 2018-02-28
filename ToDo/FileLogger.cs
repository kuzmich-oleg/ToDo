using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace ToDo
{
    public class FileLogger : ILogger
    {
        public string FilePath { get; private set; }
        private object _lock = new object();

        public FileLogger(string path)
        {
            FilePath = path;

            lock (_lock)
            {
                File.WriteAllText(FilePath, "############Session has started############" + Environment.NewLine);
            }
        }
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (formatter != null)
            {
                lock (_lock)
                {
                    File.AppendAllText(FilePath, formatter(state, exception) + Environment.NewLine);
                }
            }
        }
    }
}
