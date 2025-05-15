using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ModulWpfApp
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        // Пример команды для закрытия окна
        public ICommand CloseCommand { get; }

        public MainWindowViewModel()
        {
            CloseCommand = new RelayCommand(_ => OnCloseRequested?.Invoke(), _ => true);
        }

        // Событие для запроса закрытия окна
        public event Action? OnCloseRequested;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
