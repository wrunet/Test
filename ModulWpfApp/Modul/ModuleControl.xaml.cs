using System.Windows;
using System.Windows.Controls;

namespace ModulWpfApp.Modul
{
    public partial class ModuleControl : UserControl
    {
        public static readonly DependencyProperty ModuleTitleProperty = DependencyProperty.Register(
            "ModuleTitle", typeof(object), typeof(ModuleControl), new PropertyMetadata(null));

        public object ModuleTitle
        {
            get => GetValue(ModuleTitleProperty);
            set => SetValue(ModuleTitleProperty, value);
        }

        public ModuleControl()
        {
            InitializeComponent();
            this.DataContext = new ModuleControlViewModel();
        }
    }
}
