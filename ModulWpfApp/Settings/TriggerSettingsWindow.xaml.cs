using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Linq;
using System.Windows.Controls; // Добавлено для ComboBoxItem
using Microsoft.Win32;
using Xceed.Wpf.Toolkit;

namespace ModulWpfApp.Settings
{
    public partial class TriggerSettingsWindow : Window
    {
        public double TriggerWidth { get; set; } = 200;
        public double TriggerHeight { get; set; } = 80;
        public string TriggerFont { get; set; } = "Segoe UI";
        public Color TriggerTextColor { get; set; } = Colors.Black;
        public Color TriggerBorderColor { get; set; } = Colors.Gray;
        public double TriggerBorderThickness { get; set; } = 1;
        public string TriggerCornerType { get; set; } = "Закругленные";
        public string TriggerTitle { get; set; } = string.Empty;

        public TriggerSettingsWindow()
        {
            InitializeComponent();
            Loaded += TriggerSettingsWindow_Loaded;
            SaveBtn.Click += SaveBtn_Click;
        }

        private void TriggerSettingsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            TitleBox.Text = TriggerTitle;
            WidthBox.Text = TriggerWidth.ToString();
            HeightBox.Text = TriggerHeight.ToString();
            FontBox.ItemsSource = Fonts.SystemFontFamilies;
            FontBox.SelectedItem = Fonts.SystemFontFamilies.FirstOrDefault(f => f.Source == TriggerFont) ?? Fonts.SystemFontFamilies.First();
            TextColorPicker.SelectedColor = TriggerTextColor;
            BorderColorPicker.SelectedColor = TriggerBorderColor;
            BorderThicknessBox.Text = TriggerBorderThickness.ToString();
            CornerTypeBox.SelectedIndex = TriggerCornerType == "Острые" ? 0 : 1;
        }

        private void TextColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (e.NewValue.HasValue)
            {
                TriggerTextColor = e.NewValue.Value;
            }
        }

        private void BorderColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (e.NewValue.HasValue)
            {
                TriggerBorderColor = e.NewValue.Value;
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleBox.Text?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(title))
            {
                System.Windows.MessageBox.Show("Введите название триггера", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int width = int.TryParse(WidthBox.Text, out var w) ? w : 200;
            int height = int.TryParse(HeightBox.Text, out var h) ? h : 80;
            string font = ((System.Windows.Media.FontFamily)FontBox.SelectedItem)?.Source ?? "Segoe UI";
            string textColor = TextColorPicker.SelectedColor?.ToString() ?? "#FF000000";
            string borderColor = BorderColorPicker.SelectedColor?.ToString() ?? "#FF808080";
            double borderThickness = double.TryParse(BorderThicknessBox.Text, out var bt) ? bt : 1;
            string cornerType = (CornerTypeBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Закругленные";

            var repo = new TriggerSettingsRepository();
            repo.SaveSettings(title, width, height, font, textColor, borderColor, borderThickness, cornerType);

            this.DialogResult = true;
            this.Close();
        }
    }
}
