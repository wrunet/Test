using System.Collections.ObjectModel;
using System.Windows;
using ModulWpfApp.Settings;

namespace ModulWpfApp.Settings
{
    public class SettingsSectionViewModel
    {
        public ObservableCollection<TriggerViewModel> TriggerList { get; set; } = new ObservableCollection<TriggerViewModel>();

        public SettingsSectionViewModel()
        {
            LoadTriggersFromDb();
        }

        public void LoadTriggersFromDb()
        {
            TriggerList.Clear();
            var repo = new TriggerSettingsRepository();
            foreach (var trigger in repo.GetAllTriggers("Settings"))
                TriggerList.Add(trigger);
        }

        public void AddTrigger(string title)
        {
            TriggerList.Add(new TriggerViewModel(title));
        }

        public void EditTrigger(TriggerViewModel trigger)
        {
            var win = new TriggerSettingsWindow
            {
                TriggerWidth = trigger.Width,
                TriggerHeight = trigger.Height,
                TriggerFont = trigger.Font,
                TriggerTextColor = trigger.TextColor,
                TriggerBorderColor = trigger.BorderColor,
                TriggerBorderThickness = trigger.BorderThickness,
                TriggerCornerType = trigger.CornerType
            };
            if (win.ShowDialog() == true)
            {
                trigger.Width = win.TriggerWidth;
                trigger.Height = win.TriggerHeight;
                trigger.Font = win.TriggerFont;
                trigger.TextColor = win.TriggerTextColor;
                trigger.BorderColor = win.TriggerBorderColor;
                trigger.BorderThickness = win.TriggerBorderThickness;
                trigger.CornerType = win.TriggerCornerType;
            }
        }

        public void DeleteTrigger(TriggerViewModel trigger)
        {
            var result = MessageBox.Show($"Удалить триггер '{trigger.Title}' (ID: {trigger.Id})?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                var repo = new TriggerSettingsRepository();
                repo.DeleteTriggerById(trigger.Id);
                TriggerList.Remove(trigger);
            }
        }
    }
}
