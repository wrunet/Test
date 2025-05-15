using System.Windows;
using System.Windows.Controls;

namespace ModulWpfApp.TopMenu
{
    public partial class TopMenu : UserControl
    {
        public TopMenu()
        {
            InitializeComponent();
        }

        private void MenuItem_Close_Click(object sender, RoutedEventArgs e)
        {
            // Закрываем главное окно приложения
            Application.Current.MainWindow.Close();
        }

        public event EventHandler? TriggersChanged;

        private void MenuItem_Triggers_Click(object sender, RoutedEventArgs e)
        {
            var win = new ModulWpfApp.Settings.TriggerSettingsWindow();
            bool? result = win.ShowDialog();
            if (result == true)
            {
                TriggersChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void MenuItem_Logging_Click(object sender, RoutedEventArgs e)
        {
            var logWindow = new ModulWpfApp.Logs.LoggingWindow();
            logWindow.Show();
        }

        private void MenuItem_Database_Click(object sender, RoutedEventArgs e)
        {
            var dbWindow = new ModulWpfApp.Logs.DatabaseWindow();
            dbWindow.Show();
        }
    }
}
