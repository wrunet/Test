using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ModulWpfApp.Settings;
using System.Linq;

namespace ModulWpfApp.Modul
{
    public partial class ModulMyModules : UserControl
    {
        public ObservableCollection<TriggerViewModel> Modules { get; set; } = new ObservableCollection<TriggerViewModel>();

        public ModulMyModules()
        {
            InitializeComponent();
            LoadModulesFromDb();
            // Привязка ItemsSource делается в XAML через Binding, если нужно — раскомментируйте следующую строку:
            // ModulesItemsControl.ItemsSource = Modules;
            // Не подписываемся на события здесь, это уже делается в XAML
            // Не устанавливаем AllowDrop здесь, это уже делается в XAML
        }

        private void LoadModulesFromDb()
        {
            Modules.Clear();
            var repo = new TriggerSettingsRepository();
            foreach (var trigger in repo.GetAllTriggers())
                Modules.Add(trigger);
        }

        // Drag&Drop обработчики
        private Point _dragStartPoint;
        private void ModulesItemsControl_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _dragStartPoint = e.GetPosition(null);
        }

        private void ModulesItemsControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point position = e.GetPosition(null);
                if (Math.Abs(position.X - _dragStartPoint.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(position.Y - _dragStartPoint.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    var fe = e.OriginalSource as FrameworkElement;
                    if (fe?.DataContext is TriggerViewModel draggedItem)
                    {
                        DragDrop.DoDragDrop(ModulesItemsControl, draggedItem, DragDropEffects.Move);
                    }
                }
            }
        }

        private void ModulesItemsControl_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TriggerViewModel)))
            {
                var droppedData = (TriggerViewModel)e.Data.GetData(typeof(TriggerViewModel));
                var target = ((FrameworkElement)e.OriginalSource).DataContext as TriggerViewModel;
                if (droppedData != null && target != null && droppedData != target)
                {
                    int removedIdx = Modules.IndexOf(droppedData);
                    int targetIdx = Modules.IndexOf(target);
                    if (removedIdx >= 0 && targetIdx >= 0)
                    {
                        Modules.RemoveAt(removedIdx);
                        Modules.Insert(targetIdx, droppedData);
                    }
                }
            }
        }
    }
}
