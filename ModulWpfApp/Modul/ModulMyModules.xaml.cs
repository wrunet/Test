using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ModulWpfApp.Modul
{
    public partial class ModulMyModules : UserControl
    {
        public ObservableCollection<string> Modules { get; set; } = new ObservableCollection<string>();

        public ModulMyModules()
        {
            InitializeComponent();
            // Модули не добавляются автоматически
            ModulesItemsControl.ItemsSource = Modules;
            ModulesItemsControl.PreviewMouseLeftButtonDown += ModulesItemsControl_PreviewMouseLeftButtonDown;
            ModulesItemsControl.MouseMove += ModulesItemsControl_MouseMove;
            ModulesItemsControl.Drop += ModulesItemsControl_Drop;
            ModulesItemsControl.AllowDrop = true;
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
                    if (fe?.DataContext is string draggedItem)
                    {
                        DragDrop.DoDragDrop(ModulesItemsControl, draggedItem, DragDropEffects.Move);
                    }
                }
            }
        }

        private void ModulesItemsControl_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(string)))
            {
                string droppedData = (string)e.Data.GetData(typeof(string));
                var target = ((FrameworkElement)e.OriginalSource).DataContext as string;
                if (!string.IsNullOrEmpty(droppedData) && target != null && droppedData != target)
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
