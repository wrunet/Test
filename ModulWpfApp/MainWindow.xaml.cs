using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ModulWpfApp.Modul;
using ModulWpfApp.Settings; // Добавьте этот using для доступа к SettingsSection

namespace ModulWpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ModulWpfApp.TopMenu.TopMenu _topMenuControl;
        private ModulWpfApp.Modul.ModulMyTriggers _myTriggersControl;
        private ModulWpfApp.Settings.SettingsSection _settingsSectionControl;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
            _topMenuControl = (ModulWpfApp.TopMenu.TopMenu)this.FindName("TopMenuControl");
            _myTriggersControl = (ModulWpfApp.Modul.ModulMyTriggers)this.FindName("MyTriggersControl");
            _settingsSectionControl = (ModulWpfApp.Settings.SettingsSection)this.FindName("SettingsSection"); // Получаем ссылку на раздел "Генерация звука"
            _topMenuControl.TriggersChanged += TopMenuControl_TriggersChanged;
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _myTriggersControl.LoadMyTriggersFromDb();
            _settingsSectionControl.LoadTriggersFromDb(); // Загружаем триггеры в "Генерация звука" при запуске
        }

        private void TopMenuControl_TriggersChanged(object? sender, System.EventArgs e)
        {
            _myTriggersControl.LoadMyTriggersFromDb();
            _settingsSectionControl.LoadTriggersFromDb(); // Обновляем и "Генерация звука" при изменениях
        }

        private void MenuItem_Close_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainWindowViewModel vm)
            {
                vm.OnCloseRequested += () => this.Close();
                vm.CloseCommand.Execute(null);
            }
            else
            {
                this.Close();
            }
        }
    }
}