using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace ModulWpfApp.Settings
{
    public class ColorToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Color color)
                return new SolidColorBrush(color);
            return value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SolidColorBrush brush)
                return brush.Color;
            return value;
        }
    }

    public class CornerTypeToRadiusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string type)
            {
                return type == "Острые" ? new CornerRadius(0) : new CornerRadius(8);
            }
            return new CornerRadius(8);
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is CornerRadius radius)
                return radius.TopLeft == 0 ? "Острые" : "Закругленные";
            return "Закругленные";
        }
    }
}
