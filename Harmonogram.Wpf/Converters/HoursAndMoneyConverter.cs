using System.Globalization;
using System.Windows.Data;

namespace Harmonogram.Wpf.Converters
{
    public class HoursAndMoneyConverter : IMultiValueConverter
    {
        public object? Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length < 2 || values[0] == null || values[1] == null)
                return null;

            int hours = (int)values[0];
            double money = (double)values[1];
            return $"{hours} hrs | {money:F2} zł";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
