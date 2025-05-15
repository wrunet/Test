using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using ModulWpfApp.Settings;
using System.Linq;
using System.Windows.Media;

namespace ModulWpfApp.Modul
{
    public class ModulMyTriggersViewModel
    {
        public ObservableCollection<TriggerViewModel> MyTriggers { get; set; } = new ObservableCollection<TriggerViewModel>();

        public ModulMyTriggersViewModel()
        {
            LoadMyTriggersFromDb();
        }

        public void LoadMyTriggersFromDb()
        {
            MyTriggers.Clear();
            // Здесь должна быть загрузка из БД, если потребуется
        }

        public void DeleteTrigger(TriggerViewModel trigger)
        {
            var repo = new TriggerSettingsRepository();
            repo.DeleteTriggerById(trigger.Id);
            MyTriggers.Remove(trigger);
        }

        public void MoveTrigger(TriggerViewModel draggedItem, TriggerViewModel target)
        {
            int removedIdx = MyTriggers.IndexOf(draggedItem);
            int targetIdx = MyTriggers.IndexOf(target);
            if (removedIdx >= 0 && targetIdx >= 0)
            {
                MyTriggers.RemoveAt(removedIdx);
                MyTriggers.Insert(targetIdx, draggedItem);
            }
        }

        public void HandleTriggerDoubleClick(TriggerViewModel module)
        {
            var repo = new TriggerSettingsRepository();
            var myTrigger = repo.GetAllTriggers("MyModules").FirstOrDefault(t => t.Id == module.Id);
            if (myTrigger == null)
            {
                MessageBox.Show($"Триггер не найден в базе данных!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var exists = repo.GetAllTriggers("Settings").Any(t => t.Id == myTrigger.Id);
            if (!exists)
            {
                repo.SaveSettings(myTrigger.Title, (int)myTrigger.Width, (int)myTrigger.Height, myTrigger.Font, myTrigger.TextColor.ToString(), myTrigger.BorderColor.ToString(), myTrigger.BorderThickness, myTrigger.CornerType, "Settings", myTrigger.Id);
            }
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                var settingsSection = FindChild<ModulWpfApp.Settings.SettingsSection>(mainWindow);
                settingsSection?.LoadTriggersFromDb();
            }
        }

        private static T? FindChild<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent == null) return null;
            int count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T tChild)
                    return tChild;
                var result = FindChild<T>(child);
                if (result != null)
                    return result;
            }
            return null;
        }
    }
}
