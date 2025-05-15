using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ModulWpfApp.Modul
{
    public class ModuleControlViewModel : INotifyPropertyChanged
    {
        private string _moduleTitle = string.Empty;
        public string ModuleTitle
        {
            get => _moduleTitle;
            set { _moduleTitle = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
