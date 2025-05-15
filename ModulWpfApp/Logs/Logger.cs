using System;
using System.Text;

namespace ModulWpfApp.Logs
{
    public static class Logger
    {
        private static readonly StringBuilder _logBuilder = new StringBuilder();

        public static event Action<string>? LogUpdated;

        public static void Log(string message)
        {
            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}\n";
            _logBuilder.Append(logEntry);
            LogUpdated?.Invoke(logEntry);
        }

        public static string GetAllLogs()
        {
            return _logBuilder.ToString();
        }
    }
}
