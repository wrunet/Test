using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ModulWpfApp.Settings;

namespace ModulWpfApp.Modul
{
    public partial class ModulMyTriggers : UserControl
    {
        public ModulMyTriggers()
        {
            InitializeComponent();
            Logs.Logger.Log("ModulMyTriggers control created.");
            this.DataContext = new ModulMyTriggersViewModel();
        }

        // Для обратной совместимости и вызова из MainWindow
        public void LoadMyTriggersFromDb()
        {
            if (this.DataContext is ModulMyTriggersViewModel viewModel)
            {
                viewModel.LoadMyTriggersFromDb();
            }
        }

        // Drag&Drop обработчики
        private Point _dragStartPoint;
        private void MyTriggersItemsControl_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _dragStartPoint = e.GetPosition(null);
        }

        private void MyTriggersItemsControl_MouseMove(object sender, MouseEventArgs e)
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
                        DragDrop.DoDragDrop((ItemsControl)sender, draggedItem, DragDropEffects.Move);
                    }
                }
            }
        }

        private void MyTriggersItemsControl_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TriggerViewModel)))
            {
                var droppedData = (TriggerViewModel)e.Data.GetData(typeof(TriggerViewModel));
                var target = ((FrameworkElement)e.OriginalSource).DataContext as TriggerViewModel;
                if (droppedData != null && target != null && droppedData != target)
                {
                    var viewModel = this.DataContext as ModulMyTriggersViewModel;
                    viewModel?.MoveTrigger(droppedData, target);
                }
            }
        }

        private void MyTriggerDeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem && menuItem.DataContext is TriggerViewModel trigger)
            {
                var viewModel = this.DataContext as ModulMyTriggersViewModel;
                viewModel?.DeleteTrigger(trigger);
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
