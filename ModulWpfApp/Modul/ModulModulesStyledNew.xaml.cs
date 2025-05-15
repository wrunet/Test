using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using ModulWpfApp.Settings;
using System.Windows.Input;

namespace ModulWpfApp.Modul
{
    public partial class ModulModulesStyledNew : UserControl
    {
        private Point? _dragStartPoint = null;
        private Border? _draggedModule = null;
        private Point _draggedModuleStart;

        public ModulModulesStyledNew()
        {
            InitializeComponent();
            this.DataContext = new ModulModulesStyledNewViewModel();
        }

        private void MakeModuleDraggable(Border border, Canvas canvas)
        {
            border.MouseLeftButtonDown += (s, e) =>
            {
                _draggedModule = border;
                _dragStartPoint = e.GetPosition(canvas);
                _draggedModuleStart = new Point(Canvas.GetLeft(border), Canvas.GetTop(border));
                border.CaptureMouse();
            };
            border.MouseMove += (s, e) =>
            {
                if (_draggedModule != null && _dragStartPoint != null && e.LeftButton == MouseButtonState.Pressed)
                {
                    var pos = e.GetPosition(canvas);
                    double dx = pos.X - _dragStartPoint.Value.X;
                    double dy = pos.Y - _dragStartPoint.Value.Y;
                    double newLeft = _draggedModuleStart.X + dx;
                    double newTop = _draggedModuleStart.Y + dy;
                    // Ограничения по границам
                    newLeft = Math.Max(0, Math.Min(newLeft, canvas.ActualWidth - _draggedModule.ActualWidth));
                    newTop = Math.Max(0, Math.Min(newTop, canvas.ActualHeight - _draggedModule.ActualHeight));
                    Canvas.SetLeft(_draggedModule, newLeft);
                    Canvas.SetTop(_draggedModule, newTop);
                }
            };
            border.MouseLeftButtonUp += (s, e) =>
            {
                if (_draggedModule != null)
                {
                    _draggedModule.ReleaseMouseCapture();
                    _draggedModule = null;
                    _dragStartPoint = null;
                }
            };
        }

        private static T FindChild<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent == null) return null!;
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
            return null!;
        }
    }
}
