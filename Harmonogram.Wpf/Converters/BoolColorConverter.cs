using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Harmonogram.Wpf.Converters
{
    public class BoolColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
                return Brushes.Transparent;
            return (bool)value ? Brushes.White : Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
