using System;
using Xamarin.Forms;

namespace XOCV.Converters
{
    public class InverseBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool)
                return (!(bool)value);
            throw new ArgumentException("value is not of type bool");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (!(bool)value);
        }
    }
}