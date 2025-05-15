using System.Collections.Generic;
using System.Windows;
using ModulWpfApp.Settings;

namespace ModulWpfApp.Logs
{
    public partial class DatabaseWindow : Window
    {
        public DatabaseWindow()
        {
            InitializeComponent();
            LoadDatabase();
            DeleteBtn.Click += DeleteBtn_Click;
        }

        private void LoadDatabase()
        {
            var repo = new TriggerSettingsRepository();
            var triggers = repo.GetAllTriggers();
            TriggersDataGrid.ItemsSource = triggers;
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (TriggersDataGrid.SelectedItem is TriggerViewModel trigger)
            {
                var result = MessageBox.Show($"Удалить триггер '{trigger.Title}' (ID: {trigger.Id})?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    var repo = new TriggerSettingsRepository();
                    repo.DeleteTriggerById(trigger.Id);
                    LoadDatabase();
                }
            }
            else
            {
                MessageBox.Show("Выделите строку для удаления.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
