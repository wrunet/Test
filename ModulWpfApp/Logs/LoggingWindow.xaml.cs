using System.Windows;

namespace ModulWpfApp.Logs
{
    public partial class LoggingWindow : Window
    {
        public LoggingWindow()
        {
            InitializeComponent();
            LogTextBox.Text = Logs.Logger.GetAllLogs();
            Logs.Logger.LogUpdated += Logger_LogUpdated;
        }

        private void Logger_LogUpdated(string logEntry)
        {
            Dispatcher.Invoke(() =>
            {
                LogTextBox.AppendText(logEntry);
                LogTextBox.ScrollToEnd();
            });
        }
    }
}
