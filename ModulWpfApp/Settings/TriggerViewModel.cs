using System.ComponentModel;

namespace ModulWpfApp.Settings
{
    public class TriggerViewModel : INotifyPropertyChanged
    {
        private string _title = string.Empty;
        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(nameof(Title)); }
        }

        private string _font = "Segoe UI";
        public string Font
        {
            get => _font;
            set { _font = value; OnPropertyChanged(nameof(Font)); }
        }

        private double _width = 200;
        public double Width
        {
            get => _width;
            set { _width = value; OnPropertyChanged(nameof(Width)); }
        }

        private double _height = 80;
        public double Height
        {
            get => _height;
            set { _height = value; OnPropertyChanged(nameof(Height)); }
        }

        private System.Windows.Media.Color _textColor = System.Windows.Media.Colors.Black;
        public System.Windows.Media.Color TextColor
        {
            get => _textColor;
            set { _textColor = value; OnPropertyChanged(nameof(TextColor)); }
        }

        private System.Windows.Media.Color _borderColor = System.Windows.Media.Colors.Gray;
        public System.Windows.Media.Color BorderColor
        {
            get => _borderColor;
            set { _borderColor = value; OnPropertyChanged(nameof(BorderColor)); }
        }

        private double _borderThickness = 1;
        public double BorderThickness
        {
            get => _borderThickness;
            set { _borderThickness = value; OnPropertyChanged(nameof(BorderThickness)); }
        }

        private string _cornerType = "Закругленные";
        public string CornerType
        {
            get => _cornerType;
            set { _cornerType = value; OnPropertyChanged(nameof(CornerType)); }
        }

        public TriggerViewModel(string title)
        {
            Title = title;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
