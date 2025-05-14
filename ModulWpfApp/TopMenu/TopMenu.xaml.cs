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

        private void MenuItem_Triggers_Click(object sender, RoutedEventArgs e)
        {
            var win = new ModulWpfApp.Settings.TriggerSettingsWindow();
            win.ShowDialog();
        }
    }
}
