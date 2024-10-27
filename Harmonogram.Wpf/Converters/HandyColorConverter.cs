using System.Globalization;
using System.Windows.Data;

namespace Harmonogram.Wpf.Converters
{
    public class HandyColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
                return string.Empty;

            string color = string.Concat("#", value.ToString()!.Substring(1));
            return color.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}