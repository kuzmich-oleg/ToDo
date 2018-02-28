using Microsoft.Extensions.Logging;

namespace ToDo
{
    public class FileLoggerProvider : ILoggerProvider
    {
        public string Path { get; private set; }

        public FileLoggerProvider(string path)
        {
            Path = path;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(Path);
        }

        public void Dispose() { }
    }
}
