using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace ModulWpfApp.Settings
{
    public partial class SettingsSection : UserControl
    {
        public ObservableCollection<TriggerViewModel> TriggerList { get; set; } = new ObservableCollection<TriggerViewModel>();

        public SettingsSection()
        {
            InitializeComponent();
            this.DataContext = this;
            LoadTriggersFromDb();
        }

        public void LoadTriggersFromDb()
        {
            TriggerList.Clear();
            var repo = new TriggerSettingsRepository();
            foreach (var trigger in repo.GetAllTriggers())
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

        private void TriggerControl_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement control && control.DataContext is TriggerViewModel vm)
            {
                EditTrigger(vm);
            }
        }
    }
}
