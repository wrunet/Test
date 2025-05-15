using System.Windows;
using System.Windows.Controls;

namespace ModulWpfApp.Settings
{
    public partial class SettingsSection : UserControl
    {
        public SettingsSection()
        {
            InitializeComponent();
            this.DataContext = new SettingsSectionViewModel();
        }

        // Для обратной совместимости и вызова из MainWindow
        public void LoadTriggersFromDb()
        {
            if (this.DataContext is SettingsSectionViewModel viewModel)
            {
                viewModel.LoadTriggersFromDb();
            }
        }

        private void TriggerControl_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement control && control.DataContext is TriggerViewModel vm)
            {
                var viewModel = this.DataContext as SettingsSectionViewModel;
                viewModel?.EditTrigger(vm);
            }
        }

        private void TriggerDeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem && menuItem.DataContext is TriggerViewModel trigger)
            {
                var viewModel = this.DataContext as SettingsSectionViewModel;
                viewModel?.DeleteTrigger(trigger);
            }
        }
    }
}
